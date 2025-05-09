#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class Paperdoll : IDisplayController
{
	public enum PaperdollType
	{
		Character = 0,
		Building = 1,
		Other = 2
	}

	public const int NUM_LODS = 2;

	public const int MAX_LOD = 1;

	public const int LOD_1_ORTHOGRAPHIC_SIZE = 230;

	private static string[] PaperdollTypes = new string[3] { "Materials/lod/character/", "Art/FWS_ModelAtlasCreator_Generated/Buildings/", "Materials/lod/character/" };

	private PaperdollType paperDollType = PaperdollType.Other;

	public string currentMaterialName;

	private static Shader maskShader = Shader.Find("Unlit/TransparentMask");

	private static Shader altMaskShader = Shader.Find("Custom/RGBAlphaOverlay_Mask");

	private static Shader altShader = Shader.Find("Custom/RGBAlphaOverlay");

	public PaperdollSkin dollSkin;

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

	private string defaultDisplayState = "default";

	private Vector3 displayScale = Vector3.one;

	private bool flippable = true;

	private bool displayVisible = true;

	private Color displayColor = new Color(1f, 1f, 1f, 1f);

	private float displayAlpha = 1f;

	private int levelOfDetail;

	private Shader assignedShader;

	private readonly Vector3 inverseScale = new Vector3(-1f, -1f, -1f);

	private readonly Quaternion inverseRotation = Quaternion.Euler(0f, 180f, 180f);

	private readonly Vector3 normalScale = new Vector3(1f, 1f, 1f);

	private readonly Quaternion normalRotation = Quaternion.Euler(0f, 0f, 0f);

	protected DisplayControllerFlags flags = DisplayControllerFlags.NEED_UPDATE;

	public bool Visible
	{
		get
		{
			return displayVisible;
		}
		set
		{
			displayVisible = value;
			Flags |= DisplayControllerFlags.VISIBLE_AND_VALID_STATE;
			Renderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<Renderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				componentsInChildren[i].enabled = value;
			}
		}
	}

	public bool isVisible
	{
		get
		{
			MeshRenderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<MeshRenderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				if (componentsInChildren[i].isVisible)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool IsDestroyed
	{
		get
		{
			return rootGameObject == null;
		}
	}

	public float Alpha
	{
		get
		{
			return displayAlpha;
		}
		set
		{
			displayAlpha = value;
			MeshRenderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<MeshRenderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				Color color = componentsInChildren[i].material.color;
				componentsInChildren[i].material.color = new Color(color.r, color.g, color.b, value);
			}
		}
	}

	public Color Color
	{
		get
		{
			return displayColor;
		}
		set
		{
			displayColor = value;
			MeshRenderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<MeshRenderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				componentsInChildren[i].material.color = value;
			}
		}
	}

	public string MaterialName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public QuadHitObject HitObject
	{
		get
		{
			return quadHitObject;
		}
	}

	public string DefaultDisplayState
	{
		get
		{
			return defaultDisplayState;
		}
		set
		{
			defaultDisplayState = value;
		}
	}

	public virtual string HitMeshName { get; set; }

	public virtual bool SeparateTap { get; set; }

	public Transform Transform
	{
		get
		{
			return tform;
		}
	}

	public Vector3 Position
	{
		get
		{
			return (!(tform == null)) ? tform.position : Vector3.zero;
		}
		set
		{
			if (flippable && skeletons != null && currentAnimGroup != null)
			{
				bool createdResource;
				GameObject skeleton = skeletons.GetSkeleton(GetSkeletonName(currentAnimGroup.skeletonName), false, out createdResource);
				if (ShouldFlip(value - Position, Camera.main))
				{
					skeleton.transform.localScale = inverseScale;
					skeleton.transform.localRotation = inverseRotation;
				}
				else
				{
					skeleton.transform.localScale = normalScale;
					skeleton.transform.localRotation = normalRotation;
				}
			}
			tform.position = value;
		}
	}

	public Vector3 Scale
	{
		get
		{
			Vector3 vector = displayScale;
			return (!(tform == null)) ? Vector3.Scale(tform.localScale, new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z)) : vector;
		}
		set
		{
			tform.localScale = Vector3.Scale(value, displayScale);
		}
	}

	public Vector3 BillboardScaling
	{
		get
		{
			return Scale;
		}
		set
		{
			Scale = value;
		}
	}

	public Vector3 Forward
	{
		get
		{
			return tform.forward;
		}
	}

	public Vector3 Up
	{
		get
		{
			return tform.up;
		}
	}

	public float Width
	{
		get
		{
			return quadHitObject.Width;
		}
	}

	public float Height
	{
		get
		{
			return quadHitObject.Height;
		}
	}

	public bool isPerspectiveInArt { get; set; }

	public int LevelOfDetail
	{
		get
		{
			return levelOfDetail;
		}
		set
		{
			value = ((value >= 2) ? 1 : value);
			if (levelOfDetail != value)
			{
				levelOfDetail = value;
				ApplyLevelOfDetail();
			}
		}
	}

	public int NumberOfLevelsOfDetail
	{
		get
		{
			return 2;
		}
	}

	public int MaxLevelOfDetail
	{
		get
		{
			return 1;
		}
	}

	public DisplayControllerFlags Flags
	{
		get
		{
			return flags;
		}
		set
		{
			flags = value;
		}
	}

	public Paperdoll(Vector2 center, float width, float height, Vector3 displayScale, bool flippable, PaperdollType dolltype)
	{
		quadHitObject = new QuadHitObject(center, width, height);
		animationGroupManager = new AnimationGroupManager();
		animationEventManager = new AnimationEventManager();
		TFUtils.Assert(displayScale.x > 0f && displayScale.y > 0f && displayScale.z > 0f, "Invalid display scale: contains a zero scale factor.");
		this.displayScale = displayScale;
		this.flippable = flippable;
		paperDollType = dolltype;
	}

	public Paperdoll(Paperdoll prototype, DisplayControllerManager dcm)
	{
		quadHitObject = prototype.quadHitObject;
		animationGroupManager = prototype.animationGroupManager;
		animationEventManager = prototype.animationEventManager;
		displayScale = prototype.displayScale;
		flippable = prototype.flippable;
		isPerspectiveInArt = prototype.isPerspectiveInArt;
		paperDollType = prototype.paperDollType;
		skeletons = new SkeletonCollection();
	}

	private void ApplyAnimationGroupToSkeleton(AnimationGroupManager.AnimGroup ag)
	{
		bool createdResource;
		GameObject skeleton = skeletons.GetSkeleton(GetSkeletonName(ag.skeletonName), true, out createdResource);
		if (createdResource)
		{
			skeleton.transform.parent = tform;
			Animation component = skeleton.GetComponent<Animation>();
			ag.animModel.ApplyAnimationSettings(component);
			component.cullingType = AnimationCullingType.BasedOnRenderers;
			ULRenderTextureCameraRig.SetRenderLayer(skeleton, 22);
		}
	}

	public string GetSkeletonName(string name)
	{
		return GetSkeletonName(name, null);
	}

	public string GetSkeletonName(string name, PaperdollSkin skin)
	{
		string text = name;
		if (skin == null)
		{
			skin = dollSkin;
		}
		if (dollSkin != null)
		{
			text = dollSkin.skeletonReplacement;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = name;
		}
		return text;
	}

	public void ApplyCostumeWithLOD(CostumeManager.Costume costume, int did)
	{
		if (costume == null)
		{
			dollSkin = null;
			return;
		}
		if (paperDollType == PaperdollType.Other)
		{
			dollSkin = null;
			return;
		}
		TFUtils.WarningLog("LoadingCostume: " + costume.m_sMaterial + " : " + costume.m_sSkeleton);
		Blueprint blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, did);
		if (blueprint == null)
		{
			return;
		}
		Dictionary<string, PaperdollSkin> dictionary = (Dictionary<string, PaperdollSkin>)blueprint.Invariable["costumes"];
		if (dictionary == null)
		{
			return;
		}
		PaperdollSkin value = dollSkin;
		if (!string.IsNullOrEmpty(costume.m_sSkeleton))
		{
			dictionary.TryGetValue(costume.m_sSkeleton, out value);
		}
		if (value == dollSkin)
		{
			return;
		}
		if (value != null)
		{
			Debug.LogWarning("ApplyCostumeWithLOD Skeleton: " + value.skeletonReplacement);
		}
		GameObject gameObject = null;
		bool createdResource;
		if (currentAnimGroup != null)
		{
			GameObject skeleton = skeletons.GetSkeleton(GetSkeletonName(currentAnimGroup.skeletonName), false, out createdResource);
			if (skeleton != null)
			{
				animationController.StopAnimations();
				ULRenderTextureCameraRig.SetRenderLayer(skeleton, 22);
				skeletons.Cleanse(GetSkeletonName(currentAnimGroup.skeletonName));
			}
		}
		dollSkin = value;
		animationGroupManager.ApplyToGroups(ApplyAnimationGroupToSkeleton);
		gameObject = skeletons.GetSkeleton(GetSkeletonName(currentAnimGroup.skeletonName), true, out createdResource);
		if (gameObject == null)
		{
			Debug.LogError("ApplyCostumeWithLOD: Failed to Find Costume: " + costume.m_sName);
			return;
		}
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		ULRenderTextureCameraRig.SetRenderLayer(gameObject, LayerMask.NameToLayer("Default"));
		Animation component = gameObject.GetComponent<Animation>();
		animationController.UnityAnimation = component;
		animationController.AnimationModel = currentAnimGroup.animModel;
		animationController.EnableAnimation(true);
		animationController.PlayAnimation(currentAnimationState);
		Visible = true;
		SetupAnimationEvents(component);
	}

	public bool Unload(string file)
	{
		return true;
	}

	private void ApplyMaterialLOD()
	{
		if (currentMaterialName != null)
		{
			Debug.LogWarning("M: " + currentMaterialName);
		}
		if (currentMaterialName != "default" && !string.IsNullOrEmpty(currentMaterialName))
		{
			return;
		}
		currentMaterialName = "default";
		CommonUtils.LevelOfDetail levelOfDetail = CommonUtils.TextureLod();
		if (levelOfDetail == CommonUtils.LevelOfDetail.Low)
		{
			return;
		}
		SkinnedMeshRenderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		string text = PaperdollTypes[(int)paperDollType];
		if (componentsInChildren.Length != 0)
		{
			currentMaterialName = componentsInChildren[0].name;
		}
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
			if (skinnedMeshRenderer == null)
			{
				continue;
			}
			Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
			int num2 = sharedMaterials.Length;
			Material[] array = new Material[num2];
			for (int j = 0; j <= num2 - 1; j++)
			{
				array[j] = sharedMaterials[j];
				Material material = null;
				string text2;
				if (levelOfDetail == CommonUtils.LevelOfDetail.Standard)
				{
					text2 = text + array[j].name;
					text2 = text2.TrimEnd('2');
				}
				else
				{
					text2 = text + array[j].name;
					int num3 = text2.IndexOf("_lr2");
					text2 = ((num3 >= 0) ? text2.Remove(num3, "_lr2".Length) : text2);
				}
				if (text2 == text + array[j].name)
				{
					continue;
				}
				material = Resources.Load(text2, typeof(Material)) as Material;
				if (material != null)
				{
					if (paperDollType == PaperdollType.Building)
					{
						material.shader = Shader.Find(material.shader.name);
					}
					Resources.UnloadAsset(array[j]);
					array[j] = material;
				}
				else
				{
					Debug.LogWarning("Did not find material: " + text2 + " for the material " + array[j].name + " at " + text + ". Consider making one to use higher res textures on higher end devices.");
				}
			}
			skinnedMeshRenderer.sharedMaterials = array;
		}
	}

	private void ApplyPropLOD(GameObject prop)
	{
		if (prop == null)
		{
			return;
		}
		CommonUtils.LevelOfDetail levelOfDetail = CommonUtils.TextureLod();
		if (levelOfDetail == CommonUtils.LevelOfDetail.Low)
		{
			return;
		}
		SkinnedMeshRenderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		string text = PaperdollTypes[(int)paperDollType];
		if (componentsInChildren.Length != 0)
		{
			currentMaterialName = componentsInChildren[0].name;
		}
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
			if (skinnedMeshRenderer == null)
			{
				continue;
			}
			Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
			for (int j = 0; j <= sharedMaterials.Length - 1; j++)
			{
				Material material = null;
				string text2;
				if (levelOfDetail == CommonUtils.LevelOfDetail.Standard)
				{
					text2 = text + sharedMaterials[j].name;
					text2 = text2.TrimEnd('2');
				}
				else
				{
					text2 = text + sharedMaterials[j].name;
					int num2 = text2.IndexOf("_lr2");
					text2 = ((num2 >= 0) ? text2.Remove(num2, "_lr2".Length) : text2);
				}
				if (text2 == text + sharedMaterials[j].name)
				{
					continue;
				}
				material = Resources.Load(text2, typeof(Material)) as Material;
				if (material != null)
				{
					if (paperDollType == PaperdollType.Building)
					{
						material.shader = Shader.Find(material.shader.name);
					}
					Resources.UnloadAsset(sharedMaterials[j]);
					sharedMaterials[j] = material;
				}
			}
			skinnedMeshRenderer.sharedMaterials = sharedMaterials;
		}
	}

	public IDisplayController Clone(DisplayControllerManager dcm)
	{
		Paperdoll paperdoll = new Paperdoll(this, dcm);
		paperdoll.Initialize();
		paperdoll.LevelOfDetail = 0;
		return paperdoll;
	}

	public IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		Paperdoll paperdoll = new Paperdoll(this, dcm);
		paperdoll.HitMeshName = hitMeshName;
		paperdoll.SeparateTap = separateTap;
		paperdoll.Initialize();
		paperdoll.LevelOfDetail = 0;
		return paperdoll;
	}

	public IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		IDisplayController displayController = Clone(dcm);
		displayController.Visible = true;
		return displayController;
	}

	private void Initialize()
	{
		rootGameObject = UnityGameResources.CreateEmpty("paperdoll");
		tform = rootGameObject.transform;
		animationController = new ULAnimController();
		animationGroupManager.ApplyToGroups(ApplyAnimationGroupToSkeleton);
		tform.localScale = displayScale;
		Color = new Color(1f, 1f, 1f, 1f);
		ApplyMaterialLOD();
		if (!string.IsNullOrEmpty(HitMeshName))
		{
			MeshCollider meshCollider = rootGameObject.AddComponent<MeshCollider>();
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
			meshCollider.sharedMesh = mesh;
		}
	}

	public void AddDisplayState(Dictionary<string, object> dict)
	{
		animationGroupManager.AddDisplayStateWithBlueprint(dict);
		animationEventManager.AddAnimationEventsWithBlueprint(dict);
	}

	public void Billboard(BillboardDelegate billboard)
	{
		billboard(tform, this);
	}

	public bool Intersects(Ray ray)
	{
		MeshCollider component = rootGameObject.GetComponent<MeshCollider>();
		if (component != null)
		{
			RaycastHit hitInfo;
			if (component.Raycast(ray, out hitInfo, 5000f))
			{
				return true;
			}
			return false;
		}
		return quadHitObject.Intersects(tform, ray, -quadHitObject.Center);
	}

	public string GetDisplayState()
	{
		return currentAnimationState;
	}

	public void ChangeMesh(string state, string meshName)
	{
		HitMeshName = meshName;
		if (!string.IsNullOrEmpty(HitMeshName))
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
			rootGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
		}
	}

	public void DisplayState(string state)
	{
		if (state == null)
		{
			Visible = false;
			currentAnimationState = null;
			animationController.StopAnimations();
		}
		else
		{
			if (currentAnimationState != null && currentAnimationState.Equals(state))
			{
				return;
			}
			GameObject gameObject = null;
			AnimationGroupManager.AnimGroup animGroup = null;
			animGroup = animationGroupManager.FindAnimGroup(state);
			if (animGroup == null)
			{
				state = DefaultDisplayState;
				if (currentAnimationState == state)
				{
					return;
				}
				animGroup = animationGroupManager.FindAnimGroup(state);
			}
			TFUtils.Assert(animGroup != null, string.Format("Paperdoll '{0}' display state should exist but doesn't.", DefaultDisplayState));
			bool createdResource;
			gameObject = skeletons.GetSkeleton(GetSkeletonName(animGroup.skeletonName), false, out createdResource);
			if (currentAnimGroup != null)
			{
				GameObject skeleton = skeletons.GetSkeleton(GetSkeletonName(currentAnimGroup.skeletonName), false, out createdResource);
				if (propResource)
				{
					SkeletonAnimationSetting skeletonAnimationSetting = currentAnimGroup.animModel.SkeletonSettings(currentAnimationState);
					SkeletonAnimationSetting skeletonAnimationSetting2 = animGroup.animModel.SkeletonSettings(state);
					if (skeletonAnimationSetting2.itemResource != skeletonAnimationSetting.itemResource)
					{
						RemoveProp(skeletonAnimationSetting.itemBone, skeletonAnimationSetting.itemResource, skeleton);
					}
					if (skeletonAnimationSetting2.objectResource != skeletonAnimationSetting.objectResource)
					{
						RemoveProp(skeletonAnimationSetting.objectBone, skeletonAnimationSetting.objectResource, skeleton);
					}
				}
				if (skeleton != gameObject)
				{
					animationController.StopAnimations();
					ULRenderTextureCameraRig.SetRenderLayer(skeleton, 22);
				}
			}
			ULRenderTextureCameraRig.SetRenderLayer(gameObject, LayerMask.NameToLayer("Default"));
			Animation component = gameObject.GetComponent<Animation>();
			if (currentAnimGroup != animGroup)
			{
				animationController.UnityAnimation = component;
				animationController.AnimationModel = animGroup.animModel;
				currentAnimGroup = animGroup;
			}
			animationController.EnableAnimation(true);
			animationController.PlayAnimation(state);
			currentAnimationState = state;
			if (!propResource)
			{
				SkeletonAnimationSetting skeletonAnimationSetting3 = currentAnimGroup.animModel.SkeletonSettings(currentAnimationState);
				if (skeletonAnimationSetting3.itemResource != null)
				{
					AttachPropToBoneAndOrient(skeletonAnimationSetting3.itemResource, skeletonAnimationSetting3.itemBone, gameObject, skeletonAnimationSetting3.itemScale);
				}
				if (skeletonAnimationSetting3.objectResource != null)
				{
					AttachPropToBoneAndOrient(skeletonAnimationSetting3.objectResource, skeletonAnimationSetting3.objectBone, gameObject, skeletonAnimationSetting3.objectScale);
				}
			}
			Visible = true;
			SetupAnimationEvents(component);
		}
	}

	private void AttachPropToBoneAndOrient(string propPath, string boneName, GameObject def_base_object, Vector3 scale)
	{
		Transform transform = GetBone(boneName);
		if (transform == null && boneName == null)
		{
			transform = def_base_object.transform;
		}
		GameObject gameObject = UnityGameResources.Create(propPath);
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty).Trim();
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = scale;
		gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		ApplyPropLOD(gameObject);
		propResource = true;
	}

	private void RemoveProp(string boneName, string propName, GameObject def_base_object)
	{
		Transform transform = null;
		if (boneName != null)
		{
			transform = GetBone(boneName);
		}
		if (transform == null)
		{
			if (def_base_object == null)
			{
				return;
			}
			transform = def_base_object.transform;
		}
		if (!string.IsNullOrEmpty(propName))
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (propName.EndsWith(child.name))
				{
					child = transform.Find(child.name);
					UnityEngine.Object.Destroy(child.gameObject);
					break;
				}
			}
		}
		propResource = false;
	}

	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
		element.SetTransformParent(GetBone(target));
		element.transform.localPosition = Vector3.zero;
		element.tform.localPosition = Vector3.zero;
	}

	public Transform GetBoneRecursive(Transform trans, string boneName)
	{
		if (trans.name == boneName)
		{
			return trans;
		}
		int childCount = trans.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = trans.GetChild(i);
			Transform boneRecursive = GetBoneRecursive(child, boneName);
			if (boneRecursive != null && boneRecursive.name == boneName)
			{
				return boneRecursive;
			}
		}
		return null;
	}

	public Transform GetBone(string boneName)
	{
		bool createdResource;
		GameObject skeleton = skeletons.GetSkeleton(GetSkeletonName(currentAnimGroup.skeletonName), false, out createdResource);
		return GetBoneRecursive(skeleton.transform, boneName);
	}

	public virtual void UpdateMaterialOrTexture(string material)
	{
		TFUtils.Assert(false, "UpdateMaterial(string) is not implemented and should not be called.");
	}

	public virtual void SetMaskPercentage(float pct)
	{
		pct = TFMath.ClampF(pct, 0f, 1f);
		SkinnedMeshRenderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			for (int j = 0; j <= skinnedMeshRenderer.materials.Length - 1; j++)
			{
				pct = TFMath.ClampF(pct, 0f, 1f);
				if (assignedShader == null)
				{
					Shader shader = skinnedMeshRenderer.materials[j].shader;
					Shader shader2 = maskShader;
					assignedShader = shader;
					if (CommonUtils.CheckReloadShader() && shader == altShader)
					{
						shader2 = altMaskShader;
					}
					skinnedMeshRenderer.materials[j].shader = shader2;
				}
				else if (pct == 0f)
				{
					if (assignedShader != null)
					{
						skinnedMeshRenderer.materials[j].shader = assignedShader;
						assignedShader = null;
					}
					return;
				}
				Vector2[] uv = skinnedMeshRenderer.sharedMesh.uv;
				float value = pct;
				skinnedMeshRenderer.materials[j].SetFloat("_Mask", value);
			}
		}
	}

	public static void HorizontalFlipWithDirectionAndCamera(IDisplayController dc, Vector3 direction, Camera camera)
	{
		if (!(camera == null))
		{
			Vector3 scale = dc.Scale;
			float num = Vector3.Dot(direction, camera.transform.right);
			float x = scale.x;
			float y = scale.y;
			float z = scale.z;
			if (x * (0f - num) < 0f)
			{
				dc.Scale = new Vector3(0f - x, y, z);
			}
		}
	}

	public bool ShouldFlip(Vector3 direction, Camera camera)
	{
		if (camera == null)
		{
			return false;
		}
		Vector3 scale = Scale;
		float num = Vector3.Dot(direction, camera.transform.right);
		if (flippable && scale.x * (0f - num) < 0f)
		{
			return true;
		}
		return false;
	}

	private void ApplyLevelOfDetail()
	{
		switch (levelOfDetail)
		{
		case 0:
			animationController.PlayAnimation(currentAnimationState);
			break;
		case 1:
			animationController.StopAnimations();
			break;
		default:
			TFUtils.Assert(false, string.Format("Paperdoll.ApplyLevelOfDetail(), unsupported value for level of detail = {0}", levelOfDetail));
			break;
		}
	}

	public void UpdateLOD(Camera sceneCamera)
	{
		if (LevelOfDetail != 1 && sceneCamera.orthographicSize >= 230f)
		{
			LevelOfDetail = 1;
		}
		else if (LevelOfDetail != 0 && sceneCamera.orthographicSize < 230f)
		{
			LevelOfDetail = 0;
		}
	}

	protected void SetupAnimationEvents(Animation unityAnimation)
	{
		string text = currentAnimGroup.animModel.AnimationEventsKey(currentAnimationState);
		if (text != null)
		{
			animationEventManager.Clear();
			AnimationEventData animationEventData = animationEventManager.FindAnimationEventData(text);
			animationEventData.SetupAnimationEvents(rootGameObject, unityAnimation, unityAnimation[currentAnimationState].clip, animationEventManager);
		}
	}

	public void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		if (sceneCamera != null)
		{
			UpdateLOD(sceneCamera);
		}
		animationEventManager.UpdateWithParticleSystemManager(psm);
	}

	public void Destroy()
	{
		UnityGameResources.Destroy(rootGameObject);
		animationGroupManager.CleanseAnimations(skeletons);
	}
}
