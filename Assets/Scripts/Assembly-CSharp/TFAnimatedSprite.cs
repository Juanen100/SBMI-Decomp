#define ASSERTS_ON
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TFAnimatedSprite : BasicSprite
{
	private bool shouldBeVisible;

	private bool validCurrentDisplayState;

	private string currentDisplayState;

	private SpriteAnimationModel spriteAnimationModel;

	private ULSpriteAnimController spriteAnimationController;

	public override bool Visible
	{
		get
		{
			return shouldBeVisible;
		}
		set
		{
			shouldBeVisible = value;
			UpdateVisibility();
		}
	}

	public SpriteAnimationModel SpriteAnimationModel
	{
		get
		{
			return spriteAnimationModel;
		}
		set
		{
			spriteAnimationModel = value;
			spriteAnimationController.animationModel = spriteAnimationModel;
		}
	}

	public ULAnimControllerInterface AnimController
	{
		get
		{
			return spriteAnimationController;
		}
	}

	public override string MaterialName
	{
		get
		{
			string text = spriteAnimationModel.GetTextureName(DefaultDisplayState);
			if (text == null)
			{
				text = spriteAnimationModel.GetResourceName(DefaultDisplayState);
			}
			return text;
		}
	}

	public TFAnimatedSprite(Vector2 center, float width, float height, SpriteAnimationModel animModel)
		: base(null, null, center, width, height)
	{
		spriteAnimationModel = animModel;
	}

	public TFAnimatedSprite(TFAnimatedSprite prototype)
		: base(prototype)
	{
		spriteAnimationModel = prototype.SpriteAnimationModel;
	}

	public override string GetDisplayState()
	{
		return currentDisplayState;
	}

	public override void ChangeMesh(string state, string hitMeshName)
	{
		HitMeshName = hitMeshName;
		if (!string.IsNullOrEmpty(HitMeshName))
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("RemappedMesh/" + fileNameWithoutExtension + "_asset");
			if (mesh != null)
			{
				HitMeshName = fileNameWithoutExtension + "_asset.fbx";
			}
			else
			{
				mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
			}
			base.GameObject.GetComponent<MeshFilter>().mesh = mesh;
		}
	}

	public override void DisplayState(string state)
	{
		if (state == null)
		{
			if (currentDisplayState != null)
			{
				Flags |= DisplayControllerFlags.SWITCHED_STATE;
			}
			validCurrentDisplayState = false;
			currentDisplayState = null;
		}
		else
		{
			if (currentDisplayState != null && currentDisplayState.Equals(state))
			{
				return;
			}
			if (!spriteAnimationModel.HasAnimation(state))
			{
				state = DefaultDisplayState;
			}
			TFUtils.Assert(spriteAnimationModel.HasAnimation(state), string.Format("TFAnimatedSprite '{0}' display state should exist but doesn't.", DefaultDisplayState));
			string materialName = spriteAnimationModel.GetMaterialName(state);
			if (materialName == null)
			{
				validCurrentDisplayState = false;
			}
			else
			{
				base.GameObject.GetComponent<Renderer>().material = TextureLibrarian.LookUp(materialName);
				AnimController.EnableAnimation(true);
				AnimController.PlayAnimation(state);
				validCurrentDisplayState = true;
				currentDisplayState = state;
				string animName = state;
				if (!spriteAnimationModel.HasQuadData(animName))
				{
					animName = DefaultDisplayState;
				}
				if (spriteAnimationModel.HasQuadData(animName))
				{
					int num = spriteAnimationModel.Width(animName);
					int num2 = spriteAnimationModel.Height(animName);
					if (Width != (float)num || Height != (float)num2)
					{
						Resize(new Vector2(0f, -0.5f * (float)num2), num, num2);
					}
				}
				base.Scale = spriteAnimationModel.Scale(animName);
				Flags |= DisplayControllerFlags.SWITCHED_STATE;
				if (spriteAnimationModel.CellCount(state) > 1 && spriteAnimationModel.FramesPerSecond(state) > 0)
				{
					Flags |= DisplayControllerFlags.NEED_UPDATE;
				}
			}
		}
		UpdateVisibility();
	}

	public override void AddDisplayState(Dictionary<string, object> dict)
	{
		spriteAnimationModel.AddAnimationDataWithBlueprint(dict);
	}

	public static double CalcWorldSize(double textureValue, double scaleFactor)
	{
		return textureValue * 0.1302 * scaleFactor;
	}

	public override IDisplayController Clone(DisplayControllerManager dcm)
	{
		TFAnimatedSprite tFAnimatedSprite = new TFAnimatedSprite(this);
		tFAnimatedSprite.Initialize();
		return tFAnimatedSprite;
	}

	public override IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		TFAnimatedSprite tFAnimatedSprite = new TFAnimatedSprite(this);
		tFAnimatedSprite.HitMeshName = hitMeshName;
		tFAnimatedSprite.SeparateTap = separateTap;
		tFAnimatedSprite.Initialize();
		return tFAnimatedSprite;
	}

	public override IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		IDisplayController displayController = Clone(dcm);
		displayController.DisplayState(DefaultDisplayState);
		displayController.Visible = true;
		return displayController;
	}

	private void UpdateVisibility()
	{
		if (validCurrentDisplayState && shouldBeVisible)
		{
			base.GameObject.GetComponent<Renderer>().enabled = true;
			flags |= DisplayControllerFlags.VISIBLE_AND_VALID_STATE;
		}
		else
		{
			base.GameObject.GetComponent<Renderer>().enabled = false;
			flags &= ~DisplayControllerFlags.VISIBLE_AND_VALID_STATE;
		}
	}

	public override void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		base.OnUpdate(sceneCamera, psm);
		spriteAnimationController.OnUpdate();
	}

	protected override void Initialize()
	{
		if (!string.IsNullOrEmpty(HitMeshName))
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("RemappedMesh/" + fileNameWithoutExtension + "_asset");
			GameObject gameObject;
			if (mesh != null)
			{
				HitMeshName = fileNameWithoutExtension + "_asset.fbx";
				gameObject = CreateQuadGameObject("TFAnimatedSprite", null, null, mesh);
				if (SeparateTap)
				{
					mesh = Resources.Load<GameObject>("Meshes/" + fileNameWithoutExtension).GetComponent<MeshFilter>().sharedMesh;
				}
			}
			else
			{
				mesh = Resources.Load<GameObject>("Meshes/" + fileNameWithoutExtension).GetComponent<MeshFilter>().sharedMesh;
				gameObject = CreateQuadGameObject("TFAnimatedSprite", null);
			}
			spriteAnimationController = new ULSpriteAnimController();
			spriteAnimationController.uvToVertMap = new int[4] { 3, 1, 2, 0 };
			spriteAnimationController.animationModel = spriteAnimationModel;
			spriteAnimationController.quad = gameObject.GetComponent<MeshFilter>();
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;
		}
		else
		{
			GameObject gameObject = CreateQuadGameObject("TFAnimatedSprite", null);
			spriteAnimationController = new ULSpriteAnimController();
			spriteAnimationController.uvToVertMap = new int[4] { 3, 1, 2, 0 };
			spriteAnimationController.animationModel = spriteAnimationModel;
			spriteAnimationController.quad = gameObject.GetComponent<MeshFilter>();
		}
	}

	public override void PublicInitialize()
	{
		Initialize();
	}
}
