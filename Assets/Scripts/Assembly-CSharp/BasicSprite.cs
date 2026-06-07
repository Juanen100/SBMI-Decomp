using System.Collections.Generic;
using UnityEngine;

public class BasicSprite : IDisplayController
{
	public const int NUM_LODS = 1;

	public const int MAX_LOD = 0;

	private static Shader maskShader;

	private static Shader twoImageMaskShader;

	private static Shader altMaskShader;

	private static Shader altShader;

	private float ymax;

	private float ymin;

	protected Vector3 overallScale;

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

	private string defaultDisplayState;

	private Shader assignedShader;

	public Transform Transform
	{
		get
		{
			return null;
		}
	}

	public virtual Vector3 Position
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public virtual Vector3 Forward
	{
		get
		{
			return default(Vector3);
		}
	}

	public virtual Vector3 Up
	{
		get
		{
			return default(Vector3);
		}
	}

	public virtual Vector3 Scale
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public virtual Vector3 BillboardScaling
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public virtual bool Visible
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool isVisible
	{
		get
		{
			return false;
		}
	}

	public virtual int LevelOfDetail
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public virtual int NumberOfLevelsOfDetail
	{
		get
		{
			return 0;
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
			return null;
		}
	}

	public virtual string HitMeshName { get; set; }

	public virtual bool SeparateTap { get; set; }

	public virtual float Alpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public virtual Color Color
	{
		get
		{
			return default(Color);
		}
		set
		{
		}
	}

	public QuadHitObject HitObject
	{
		get
		{
			return null;
		}
	}

	public virtual string DefaultDisplayState
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	protected GameObject GameObject
	{
		get
		{
			return null;
		}
	}

	public string Name
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Material GetMaterial
	{
		get
		{
			return null;
		}
	}

	protected bool LayerRendering
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public float Width
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float Height
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public Vector2 Center
	{
		get
		{
			return default(Vector2);
		}
		set
		{
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return default(Quaternion);
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

	public bool IsDestroyed
	{
		get
		{
			return false;
		}
	}

	public bool isPerspectiveInArt { get; set; }

	public BasicSprite(string material, string texture, Vector2 center, float width, float height)
	{
	}

	public BasicSprite(string material, string texture, Vector2 center, float width, float height, QuadHitObject hitObject)
	{
	}

	public BasicSprite(BasicSprite prototype)
	{
	}

	public void Billboard(BillboardDelegate billboard)
	{
	}

	public virtual void Face(Vector3 direction, Vector3 worldUp)
	{
	}

	public virtual bool Intersects(Ray ray)
	{
		return false;
	}

	public virtual void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
	}

	public virtual void AddDisplayState(Dictionary<string, object> dict)
	{
	}

	public virtual string GetDisplayState()
	{
		return null;
	}

	public virtual IDisplayController Clone(DisplayControllerManager dcm)
	{
		return null;
	}

	public virtual IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		return null;
	}

	public virtual IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		return null;
	}

	public virtual void ChangeMesh(string state, string HitMeshName)
	{
	}

	public virtual void DisplayState(string state)
	{
	}

	public virtual void UpdateMaterialOrTexture(string material)
	{
	}

	public virtual void SetMaskPercentage(float pct)
	{
	}

	public virtual void Destroy()
	{
	}

	protected GameObject CreateQuadGameObject(string name, Material material, Rect? uvs = null, Mesh hitMesh = null)
	{
		return null;
	}

	public virtual void PublicInitialize()
	{
	}

	protected virtual void Initialize()
	{
	}

	public virtual void Resize(Vector2 center, float width, float height)
	{
	}

	public void Translate(Vector3 v)
	{
	}

	public void RotateAround(Vector3 point, Vector3 axis, float angle)
	{
	}

	public void Rotate(Vector3 v)
	{
	}

	public void ResetRotation()
	{
	}

	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
	}
}
