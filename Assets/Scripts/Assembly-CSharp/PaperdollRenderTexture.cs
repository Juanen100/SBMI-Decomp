using System.Collections.Generic;
using UnityEngine;

public class PaperdollRenderTexture : BasicSprite
{
	public const int PAPERDOLL_RENDERTEXTURE_NUM_LODS = 2;

	public const int PAPERDOLL_RENDERTEXTURE_MAX_LOD = 1;

	private static string RENDERTEXTURE_SHADER;

	public const int LOD_1_ORTHOGRAPHIC_SIZE = 200;

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
			return default(Vector3);
		}
		set
		{
		}
	}

	public Vector3 CameraLookAtOffset
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public override string MaterialName
	{
		get
		{
			return null;
		}
	}

	public override Vector3 Position
	{
		set
		{
		}
	}

	public DisplayControllerManager DisplayControllerManager
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public override int NumberOfLevelsOfDetail
	{
		get
		{
			return 0;
		}
	}

	public override int MaxLevelOfDetail
	{
		get
		{
			return 0;
		}
	}

	public PaperdollRenderTexture(Vector2 center, float width, float height)
		: base(null, null, default(Vector2), 0f, 0f)
	{
	}

	public PaperdollRenderTexture(PaperdollRenderTexture prototype, DisplayControllerManager dcm)
		: base(null, null, default(Vector2), 0f, 0f)
	{
	}

	private void ApplyAnimationGroupToSkeleton(AnimationGroupManager.AnimGroup ag)
	{
	}

	protected override void Initialize()
	{
	}

	protected void CamSetup(GameObject subject, Camera cam)
	{
	}

	public override void AddDisplayState(Dictionary<string, object> dict)
	{
	}

	public override IDisplayController Clone(DisplayControllerManager dcm)
	{
		return null;
	}

	public override void DisplayState(string state)
	{
	}

	private void ParentCurrentSkeleton(Transform parent)
	{
	}

	public void UpdateLOD(Camera sceneCamera)
	{
	}

	public override void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
	}

	public override void Destroy()
	{
	}
}
