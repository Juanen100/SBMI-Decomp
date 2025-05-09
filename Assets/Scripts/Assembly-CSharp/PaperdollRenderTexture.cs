#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class PaperdollRenderTexture : BasicSprite
{
	public const int PAPERDOLL_RENDERTEXTURE_NUM_LODS = 2;

	public const int PAPERDOLL_RENDERTEXTURE_MAX_LOD = 1;

	public const int LOD_1_ORTHOGRAPHIC_SIZE = 200;

	private static string RENDERTEXTURE_SHADER = "Unlit/TransparentTint";

	private GameObject gameObjectPaperdoll;

	private Vector3 cameraOffset;

	private Vector3 cameraLookAtOffset;

	private ULAnimController animationController;

	private AnimationGroupManager animationGroupManager;

	private AnimationGroupManager.AnimGroup currentAnimGroup;

	private float animationTime;

	private string currentAnimationState;

	private DisplayControllerManager displayControllerManager;

	private ULRenderTextureBatchEntry renderTextureRig;

	public Vector3 CameraOffset
	{
		get
		{
			return cameraOffset;
		}
		set
		{
			cameraOffset = value;
		}
	}

	public Vector3 CameraLookAtOffset
	{
		get
		{
			return cameraLookAtOffset;
		}
		set
		{
			cameraLookAtOffset = value;
		}
	}

	public override string MaterialName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override Vector3 Position
	{
		set
		{
			Paperdoll.HorizontalFlipWithDirectionAndCamera(this, value - base.Position, Camera.main);
			base.Position = value;
		}
	}

	public DisplayControllerManager DisplayControllerManager
	{
		get
		{
			return displayControllerManager;
		}
		set
		{
			displayControllerManager = value;
		}
	}

	public override int NumberOfLevelsOfDetail
	{
		get
		{
			return 2;
		}
	}

	public override int MaxLevelOfDetail
	{
		get
		{
			return 1;
		}
	}

	public PaperdollRenderTexture(Vector2 center, float width, float height)
		: base(null, null, center, width, height)
	{
		animationGroupManager = new AnimationGroupManager();
		cameraOffset = new Vector3(0f, 0f, 0f);
		cameraLookAtOffset = new Vector3(0f, 0f, 0f);
	}

	public PaperdollRenderTexture(PaperdollRenderTexture prototype, DisplayControllerManager dcm)
		: base(prototype)
	{
		displayControllerManager = dcm;
		cameraOffset = prototype.cameraOffset;
		cameraLookAtOffset = prototype.cameraLookAtOffset;
		animationGroupManager = prototype.animationGroupManager;
		animationGroupManager.ApplyToGroups(ApplyAnimationGroupToSkeleton);
	}

	private void ApplyAnimationGroupToSkeleton(AnimationGroupManager.AnimGroup ag)
	{
		bool createdResource;
		GameObject skeleton = displayControllerManager.Skeletons.GetSkeleton(ag.skeletonName, true, out createdResource);
		ULRenderTextureCameraRig.SetRenderLayer(skeleton, 22);
		if (createdResource)
		{
			Animation component = skeleton.GetComponent<Animation>();
			ag.animModel.ApplyAnimationSettings(component);
		}
	}

	protected override void Initialize()
	{
		gameObjectPaperdoll = UnityGameResources.CreateEmpty("paperdollRenderTexture");
		animationController = new ULAnimController();
		gameObjectPaperdoll.transform.localPosition = new Vector3(0f, 0f, 0f);
		RenderTextureManager renderTextureManager = displayControllerManager.RenderTextureManager;
		renderTextureRig = renderTextureManager.AddGameObject(gameObjectPaperdoll, CamSetup, RENDERTEXTURE_SHADER);
		CreateQuadGameObject("PaperdollSprite", renderTextureRig.target.RMaterial);
	}

	protected void CamSetup(GameObject subject, Camera cam)
	{
		cam.transform.position = subject.transform.position + cameraOffset;
		cam.transform.LookAt(subject.transform.position + cameraLookAtOffset);
	}

	public override void AddDisplayState(Dictionary<string, object> dict)
	{
		animationGroupManager.AddDisplayStateWithBlueprint(dict);
	}

	public override IDisplayController Clone(DisplayControllerManager dcm)
	{
		PaperdollRenderTexture paperdollRenderTexture = new PaperdollRenderTexture(this, dcm);
		paperdollRenderTexture.Initialize();
		return paperdollRenderTexture;
	}

	public override void DisplayState(string state)
	{
		if (state == null)
		{
			Visible = false;
		}
		else if (currentAnimationState == null || !currentAnimationState.Equals(state))
		{
			AnimationGroupManager.AnimGroup animGroup = animationGroupManager.FindAnimGroup(state);
			if (animGroup == null)
			{
				state = DefaultDisplayState;
			}
			animGroup = animationGroupManager.FindAnimGroup(state);
			TFUtils.Assert(animGroup != null, string.Format("Paperdoll '{0}' display state should exist but doesn't.", DefaultDisplayState));
			bool createdResource;
			GameObject skeleton = displayControllerManager.Skeletons.GetSkeleton(animGroup.skeletonName, false, out createdResource);
			Animation component = skeleton.GetComponent<Animation>();
			if (currentAnimGroup != animGroup)
			{
				animationController.UnityAnimation = component;
				animationController.AnimationModel = animGroup.animModel;
				currentAnimGroup = animGroup;
			}
			currentAnimationState = state;
			animationTime = 0f;
			Visible = true;
		}
	}

	private void ParentCurrentSkeleton(Transform parent)
	{
		string skeletonName = currentAnimGroup.skeletonName;
		bool createdResource;
		GameObject skeleton = displayControllerManager.Skeletons.GetSkeleton(skeletonName, false, out createdResource);
		skeleton.transform.parent = parent;
	}

	public void UpdateLOD(Camera sceneCamera)
	{
		if (LevelOfDetail != 1 && sceneCamera.orthographicSize >= 200f)
		{
			LevelOfDetail = 1;
		}
		else if (LevelOfDetail != 0 && sceneCamera.orthographicSize < 200f)
		{
			LevelOfDetail = 0;
		}
	}

	public override void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		base.OnUpdate(sceneCamera, psm);
		UpdateLOD(sceneCamera);
		animationTime += Time.deltaTime;
		if (LevelOfDetail == 0)
		{
			ParentCurrentSkeleton(gameObjectPaperdoll.transform);
			animationController.Sample(currentAnimationState, animationTime);
			displayControllerManager.RenderTextureManager.RenderEntry(renderTextureRig);
			ParentCurrentSkeleton(null);
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		UnityGameResources.Destroy(gameObjectPaperdoll);
	}
}
