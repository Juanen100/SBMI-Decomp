using System.Collections.Generic;
using UnityEngine;

public class DeferredDisplayController : IDisplayController
{
	private float alpha;

	private Color color;

	private Vector3 position;

	private Vector3 scale;

	private Vector3 billboardScale;

	private int levelOfDetail;

	private string defaultDisplayState;

	private DisplayControllerFlags flags;

	private bool perspectiveInArt;

	private BillboardDelegate billboardDelegate;

	private IDisplayController idc;

	private IDisplayController source;

	private DisplayControllerManager dcm;

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

	public bool isPerspectiveInArt
	{
		get
		{
			return false;
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

	public Transform Transform
	{
		get
		{
			return null;
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

	public bool isVisible
	{
		get
		{
			return false;
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

	private IDisplayController InternalDisplayController
	{
		get
		{
			return null;
		}
	}

	public DeferredDisplayController(IDisplayController source, DisplayControllerManager dcm)
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

	public string GetDisplayState()
	{
		return null;
	}

	public void AddDisplayState(Dictionary<string, object> dict)
	{
	}

	public bool Intersects(Ray ray)
	{
		return false;
	}

	public void ChangeMesh(string state, string meshName)
	{
	}

	public void DisplayState(string state)
	{
	}

	public void UpdateMaterialOrTexture(string material)
	{
	}

	public void SetMaskPercentage(float pct)
	{
	}

	public void Billboard(BillboardDelegate billboard)
	{
	}

	public void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
	}

	public void Destroy()
	{
	}

	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
	}
}
