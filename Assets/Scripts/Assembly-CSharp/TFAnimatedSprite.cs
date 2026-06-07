using System.Collections.Generic;
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
			return false;
		}
		set
		{
		}
	}

	public SpriteAnimationModel SpriteAnimationModel
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ULAnimControllerInterface AnimController
	{
		get
		{
			return null;
		}
	}

	public override string MaterialName
	{
		get
		{
			return null;
		}
	}

	public TFAnimatedSprite(Vector2 center, float width, float height, SpriteAnimationModel animModel)
		: base(null, null, default(Vector2), 0f, 0f)
	{
	}

	public TFAnimatedSprite(TFAnimatedSprite prototype)
		: base(null, null, default(Vector2), 0f, 0f)
	{
	}

	public override string GetDisplayState()
	{
		return null;
	}

	public override void ChangeMesh(string state, string hitMeshName)
	{
	}

	public override void DisplayState(string state)
	{
	}

	public override void AddDisplayState(Dictionary<string, object> dict)
	{
	}

	public static double CalcWorldSize(double textureValue, double scaleFactor)
	{
		return 0.0;
	}

	public override IDisplayController Clone(DisplayControllerManager dcm)
	{
		return null;
	}

	public override IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		return null;
	}

	public override IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		return null;
	}

	private void UpdateVisibility()
	{
	}

	public override void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
	}

	protected override void Initialize()
	{
	}

	public override void PublicInitialize()
	{
	}
}
