using System.Collections.Generic;
using UnityEngine;

internal class Paperdoll : IDisplayController
{
	public enum PaperdollType
	{
		Character = 0,
		Building = 1,
		Other = 2
	}

	private static string[] PaperdollTypes;

	private PaperdollType paperDollType;

	public const int NUM_LODS = 2;

	public const int MAX_LOD = 1;

	public string currentMaterialName;

	private static Shader maskShader;

	private static Shader altMaskShader;

	private static Shader altShader;

	public PaperdollSkin dollSkin;

	public const int LOD_1_ORTHOGRAPHIC_SIZE = 230;

	protected Transform tform;

	private ULAnimController animationController;

	private AnimationGroupManager animationGroupManager;

	private AnimationEventManager animationEventManager;

	private SkeletonCollection skeletons;

	private string currentAnimationState;

	private AnimationGroupManager.AnimGroup currentAnimGroup;

	private GameObject rootGameObject;

	private bool propResource;

	private QuadHitObject quadHitObject;

	private string defaultDisplayState;

	private Vector3 displayScale;

	private bool flippable;

	private bool displayVisible;

	private Color displayColor;

	private float displayAlpha;

	private int levelOfDetail;

	private Shader assignedShader;

	private readonly Vector3 inverseScale;

	private readonly Quaternion inverseRotation;

	private readonly Vector3 normalScale;

	private readonly Quaternion normalRotation;

	protected DisplayControllerFlags flags;

	public bool Visible
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool isVisible
	{
		get
		{
			return false;
		}
	}

	public bool IsDestroyed
	{
		get
		{
			return false;
		}
	}

	public float Alpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public Color Color
	{
		get
		{
			return default(Color);
		}
		set
		{
		}
	}

	public string MaterialName
	{
		get
		{
			return null;
		}
	}

	public QuadHitObject HitObject
	{
		get
		{
			return null;
		}
	}

	public string DefaultDisplayState
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual string HitMeshName { get; set; }

	public virtual bool SeparateTap { get; set; }

	public Transform Transform
	{
		get
		{
			return null;
		}
	}

	public Vector3 Position
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public Vector3 Scale
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public Vector3 BillboardScaling
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public Vector3 Forward
	{
		get
		{
			return default(Vector3);
		}
	}

	public Vector3 Up
	{
		get
		{
			return default(Vector3);
		}
	}

	public float Width
	{
		get
		{
			return 0f;
		}
	}

	public float Height
	{
		get
		{
			return 0f;
		}
	}

	public bool isPerspectiveInArt { get; set; }

	public int LevelOfDetail
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public int NumberOfLevelsOfDetail
	{
		get
		{
			return 0;
		}
	}

	public int MaxLevelOfDetail
	{
		get
		{
			return 0;
		}
	}

	public DisplayControllerFlags Flags
	{
		get
		{
			return default(DisplayControllerFlags);
		}
		set
		{
		}
	}

	public Paperdoll(Vector2 center, float width, float height, Vector3 displayScale, bool flippable, PaperdollType dolltype)
	{
	}

	public Paperdoll(Paperdoll prototype, DisplayControllerManager dcm)
	{
	}

	private void ApplyAnimationGroupToSkeleton(AnimationGroupManager.AnimGroup ag)
	{
	}

	public string GetSkeletonName(string name)
	{
		return null;
	}

	public string GetSkeletonName(string name, PaperdollSkin skin)
	{
		return null;
	}

	public void ApplyCostumeWithLOD(CostumeManager.Costume costume, int did)
	{
	}

	public bool Unload(string file)
	{
		return false;
	}

	private void ApplyMaterialLOD()
	{
	}

	private void ApplyPropLOD(GameObject prop)
	{
	}

	public IDisplayController Clone(DisplayControllerManager dcm)
	{
		return null;
	}

	public IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		return null;
	}

	public IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		return null;
	}

	private void Initialize()
	{
	}

	public void AddDisplayState(Dictionary<string, object> dict)
	{
	}

	public void Billboard(BillboardDelegate billboard)
	{
	}

	public bool Intersects(Ray ray)
	{
		return false;
	}

	public string GetDisplayState()
	{
		return null;
	}

	public void ChangeMesh(string state, string meshName)
	{
	}

	public void DisplayState(string state)
	{
	}

	private void CheckAnimationExists(string state, Animation targetAnimation, SkeletonAnimationSetting animSettings)
	{
	}

	private void AttachPropToBoneAndOrient(string propPath, string boneName, GameObject def_base_object, Vector3 scale)
	{
	}

	private void RemoveProp(string boneName, string propName, GameObject def_base_object)
	{
	}

	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
	}

	public Transform GetBoneRecursive(Transform trans, string boneName)
	{
		return null;
	}

	public Transform GetBone(string boneName)
	{
		return null;
	}

	public virtual void UpdateMaterialOrTexture(string material)
	{
	}

	public virtual void SetMaskPercentage(float pct)
	{
	}

	public static void HorizontalFlipWithDirectionAndCamera(IDisplayController dc, Vector3 direction, Camera camera)
	{
	}

	public bool ShouldFlip(Vector3 direction, Camera camera)
	{
		return false;
	}

	private void ApplyLevelOfDetail()
	{
	}

	public void UpdateLOD(Camera sceneCamera)
	{
	}

	protected void SetupAnimationEvents(Animation unityAnimation)
	{
	}

	public void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
	}

	public void Destroy()
	{
	}
}
