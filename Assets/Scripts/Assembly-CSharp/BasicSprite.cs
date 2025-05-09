#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

public class BasicSprite : IDisplayController
{
	public const int NUM_LODS = 1;

	public const int MAX_LOD = 0;

	private static Shader maskShader = Shader.Find("Unlit/TransparentMask");

	private static Shader twoImageMaskShader = Shader.Find("Custom/TwoImageWithMask");

	private static Shader altMaskShader = Shader.Find("Custom/RGBAlphaOverlay_Mask");

	private static Shader altShader = Shader.Find("Custom/RGBAlphaOverlay");

	private float ymax;

	private float ymin = 1f;

	protected Vector3 overallScale = Vector3.one;

	protected Transform tform;

	protected DisplayControllerFlags flags;

	private string material;

	private string texture;

	private Vector2 center;

	private float width;

	private float height;

	private GameObject gameObject;

	private QuadHitObject quadHitObject;

	private int levelOfDetail;

	private string defaultDisplayState = "default";

	private Shader assignedShader;

	public Transform Transform
	{
		get
		{
			return tform;
		}
	}

	public virtual Vector3 Position
	{
		get
		{
			return (!(tform == null)) ? tform.position : Vector3.zero;
		}
		set
		{
			tform.position = value;
		}
	}

	public virtual Vector3 Forward
	{
		get
		{
			return tform.forward;
		}
	}

	public virtual Vector3 Up
	{
		get
		{
			return tform.up;
		}
	}

	public virtual Vector3 Scale
	{
		get
		{
			return (!(tform == null)) ? tform.localScale : Vector3.one;
		}
		set
		{
			tform.localScale = value;
		}
	}

	public virtual Vector3 BillboardScaling
	{
		get
		{
			return overallScale;
		}
		set
		{
			overallScale = value;
			Resize(center, width, height);
		}
	}

	public virtual bool Visible
	{
		get
		{
			return gameObject.GetComponent<Renderer>().enabled;
		}
		set
		{
			gameObject.GetComponent<Renderer>().enabled = value;
			gameObject.SetActiveRecursively(value);
		}
	}

	public virtual bool isVisible
	{
		get
		{
			return gameObject.GetComponent<Renderer>().isVisible;
		}
	}

	public virtual int LevelOfDetail
	{
		get
		{
			return levelOfDetail;
		}
		set
		{
			levelOfDetail = ((value >= NumberOfLevelsOfDetail) ? MaxLevelOfDetail : value);
		}
	}

	public virtual int NumberOfLevelsOfDetail
	{
		get
		{
			return 1;
		}
	}

	public virtual int MaxLevelOfDetail
	{
		get
		{
			return 0;
		}
	}

	public virtual string MaterialName
	{
		get
		{
			return (texture == null) ? material : texture;
		}
	}

	public virtual string HitMeshName { get; set; }

	public virtual bool SeparateTap { get; set; }

	public virtual float Alpha
	{
		get
		{
			if (gameObject != null && gameObject.GetComponent<Renderer>() != null && gameObject.GetComponent<Renderer>().material != null)
			{
				return gameObject.GetComponent<Renderer>().material.color.a;
			}
			return 1f;
		}
		set
		{
			Color color = gameObject.GetComponent<Renderer>().material.color;
			gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, value);
		}
	}

	public virtual Color Color
	{
		get
		{
			if (gameObject != null && gameObject.GetComponent<Renderer>() != null && gameObject.GetComponent<Renderer>().material != null)
			{
				return gameObject.GetComponent<Renderer>().material.color;
			}
			return Color.white;
		}
		set
		{
			gameObject.GetComponent<Renderer>().material.color = value;
		}
	}

	public QuadHitObject HitObject
	{
		get
		{
			return quadHitObject;
		}
	}

	public virtual string DefaultDisplayState
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

	protected GameObject GameObject
	{
		get
		{
			return gameObject;
		}
	}

	public string Name
	{
		get
		{
			return gameObject.name;
		}
		set
		{
			gameObject.name = value;
		}
	}

	public Material GetMaterial
	{
		get
		{
			return gameObject.GetComponent<Renderer>().material;
		}
	}

	protected bool LayerRendering
	{
		get
		{
			return gameObject.layer != 22;
		}
		set
		{
			int layer = ((!value) ? 22 : LayerMask.NameToLayer("Default"));
			ULRenderTextureCameraRig.SetRenderLayer(gameObject, layer);
		}
	}

	public float Width
	{
		get
		{
			return width;
		}
		set
		{
			width = value;
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
		}
	}

	public Vector2 Center
	{
		get
		{
			return center;
		}
		set
		{
			center = value;
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return tform.rotation;
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

	public bool IsDestroyed
	{
		get
		{
			return gameObject == null;
		}
	}

	public bool isPerspectiveInArt { get; set; }

	public BasicSprite(string material, string texture, Vector2 center, float width, float height)
		: this(material, texture, center, width, height, new QuadHitObject(center, width, height))
	{
	}

	public BasicSprite(string material, string texture, Vector2 center, float width, float height, QuadHitObject hitObject)
	{
		this.texture = texture;
		if (this.texture != null)
		{
			this.material = YGTextureLibrary.GetAtlasCoords(texture).atlas.name;
		}
		else
		{
			this.material = material;
		}
		this.center = center;
		this.width = width;
		this.height = height;
		quadHitObject = hitObject;
	}

	public BasicSprite(BasicSprite prototype)
	{
		material = prototype.material;
		texture = prototype.texture;
		center = prototype.center;
		width = prototype.width;
		height = prototype.height;
		quadHitObject = prototype.quadHitObject;
		isPerspectiveInArt = prototype.isPerspectiveInArt;
	}

	public void Billboard(BillboardDelegate billboard)
	{
		billboard(tform, this);
	}

	public virtual void Face(Vector3 direction, Vector3 worldUp)
	{
		tform.LookAt(tform.position + direction, worldUp);
	}

	public virtual bool Intersects(Ray ray)
	{
		MeshCollider component = gameObject.GetComponent<MeshCollider>();
		if (component != null)
		{
			RaycastHit hitInfo;
			if (component.Raycast(ray, out hitInfo, 5000f))
			{
				return true;
			}
			return false;
		}
		return quadHitObject.Intersects(tform, ray, Vector2.zero);
	}

	public virtual void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
	}

	public virtual void AddDisplayState(Dictionary<string, object> dict)
	{
		TFUtils.Assert(false, "BasicSprite.AddDisplayState(Dictionary) is not implemented and should not be called.");
	}

	public virtual string GetDisplayState()
	{
		throw new InvalidOperationException("Cannot call GetDisplayState() in BasicSprite");
	}

	public virtual IDisplayController Clone(DisplayControllerManager dcm)
	{
		BasicSprite basicSprite = new BasicSprite(this);
		basicSprite.Initialize();
		return basicSprite;
	}

	public virtual IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		HitMeshName = hitMeshName;
		SeparateTap = separateTap;
		BasicSprite basicSprite = new BasicSprite(this);
		basicSprite.Initialize();
		return basicSprite;
	}

	public virtual IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		IDisplayController displayController = Clone(dcm);
		displayController.Visible = true;
		return displayController;
	}

	public virtual void ChangeMesh(string state, string HitMeshName)
	{
		TFUtils.Assert(false, "BasicSprite.ChangeMesh(string, string) is not implemented and should not be called.");
	}

	public virtual void DisplayState(string state)
	{
		TFUtils.Assert(false, "BasicSprite.DisplayState(string) is not implemented and should not be called.");
	}

	public virtual void UpdateMaterialOrTexture(string material)
	{
		if (material == null)
		{
			TFUtils.Assert(false, "Cannot update BasicSprite to use a null material!");
			return;
		}
		int num = material.LastIndexOf('/');
		Material material2;
		Vector2[] uv;
		if (num >= 0)
		{
			material2 = TextureLibrarian.LookUp(material);
			TFUtils.Assert(material2 != null, "Could not find the material " + material);
			uv = new Vector2[4]
			{
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 0f),
				new Vector2(0f, 1f)
			};
			texture = null;
			this.material = material;
		}
		else
		{
			AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(material);
			material2 = TextureLibrarian.LookUp("Materials/lod/" + atlasCoords.atlas.name);
			Rect rect = default(Rect);
			atlasCoords.atlas.GetUVs(atlasCoords.atlasCoords, ref rect);
			uv = new Vector2[4]
			{
				new Vector2(rect.xMax, rect.yMin),
				new Vector2(rect.xMax, rect.yMax),
				new Vector2(rect.xMin, rect.yMin),
				new Vector2(rect.xMin, rect.yMax)
			};
			texture = material;
			this.material = null;
		}
		gameObject.GetComponent<Renderer>().material = material2;
		gameObject.GetComponent<MeshFilter>().mesh.uv = uv;
	}

	public virtual void SetMaskPercentage(float pct)
	{
		pct = TFMath.ClampF(pct, 0f, 1f);
		if (assignedShader == null)
		{
			Shader shader = gameObject.GetComponent<Renderer>().material.shader;
			Shader shader2 = ((!shader.name.Contains("TwoImageColorOverlay")) ? maskShader : twoImageMaskShader);
			assignedShader = shader;
			if (CommonUtils.CheckReloadShader() && shader == altShader)
			{
				shader2 = altMaskShader;
			}
			gameObject.GetComponent<Renderer>().material.shader = shader2;
		}
		else if (pct == 0f)
		{
			gameObject.GetComponent<Renderer>().material.shader = assignedShader;
			assignedShader = null;
			return;
		}
		if (ymin == 1f)
		{
			Vector2[] uv = gameObject.GetComponent<MeshFilter>().mesh.uv;
			ymin = uv[0].y;
			ymax = uv[1].y;
		}
		float value = (ymax - ymin) * pct + ymin;
		gameObject.GetComponent<Renderer>().material.SetFloat("_Mask", value);
	}

	public virtual void Destroy()
	{
		UnityGameResources.Destroy(gameObject);
		gameObject = null;
	}

	protected GameObject CreateQuadGameObject(string name, Material material, Rect? uvs = null, Mesh hitMesh = null)
	{
		TFUtils.Assert(gameObject == null, "Recreating a Basic Sprite - this will cause an untracked game object!");
		gameObject = UnityGameResources.CreateEmpty(name);
		tform = gameObject.transform;
		TFQuad.SetupQuad(gameObject, material, width, height, center, uvs, hitMesh);
		LayerRendering = true;
		if (hitMesh != null)
		{
			Vector2[] uv = gameObject.GetComponent<MeshFilter>().mesh.uv;
			ymax = 0f;
			ymin = 1f;
			for (int i = 0; i < uv.Length; i++)
			{
				if (uv[i].y > ymax)
				{
					ymax = uv[i].y;
				}
				if (uv[i].y < ymin)
				{
					ymin = uv[i].y;
				}
			}
		}
		return gameObject;
	}

	public virtual void PublicInitialize()
	{
		Initialize();
		gameObject.GetComponent<Renderer>().enabled = true;
	}

	protected virtual void Initialize()
	{
		Rect? uvs = null;
		Material material;
		if (texture == null)
		{
			material = TextureLibrarian.LookUp(this.material);
		}
		else
		{
			AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(texture);
			material = TextureLibrarian.LookUp("Materials/lod/" + atlasCoords.atlas.name);
			Rect rect = default(Rect);
			atlasCoords.atlas.GetUVs(atlasCoords.atlasCoords, ref rect);
			uvs = rect;
		}
		CreateQuadGameObject("BasicSprite", material, uvs);
		gameObject.GetComponent<Renderer>().enabled = false;
	}

	public virtual void Resize(Vector2 center, float width, float height)
	{
		if (string.IsNullOrEmpty(HitMeshName) || !(HitMeshName.Substring(HitMeshName.Length - 10, 10) == "_asset.fbx"))
		{
			TFQuad.SetupQuadMesh(gameObject.GetComponent<MeshFilter>().mesh, width * overallScale.x, height * overallScale.y, center, true);
		}
		this.width = width;
		this.height = height;
		this.center = center;
	}

	public void Translate(Vector3 v)
	{
		tform.Translate(v);
	}

	public void RotateAround(Vector3 point, Vector3 axis, float angle)
	{
		tform.RotateAround(point, axis, angle);
	}

	public void Rotate(Vector3 v)
	{
		tform.Rotate(v);
	}

	public void ResetRotation()
	{
		Billboard(SBCamera.BillboardDefinition);
	}

	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
		element.SetTransformParent(tform);
		element.transform.localPosition = Vector3.zero;
		element.tform.localPosition = Vector3.zero;
	}
}
