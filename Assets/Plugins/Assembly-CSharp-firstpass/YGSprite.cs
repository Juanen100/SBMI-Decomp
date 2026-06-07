using System;
using UnityEngine;
using Yarg;

[ExecuteInEditMode]
public class YGSprite : MonoBehaviour, ILoadable
{
	public class MeshUpdate
	{
		public bool _vertsUpdate;

		private Vector3[] _verts;

		public bool _normalsUpdate;

		private Vector3[] _normals;

		public bool _colorsUpdate;

		private Color[] _colors;

		public bool _trisUpdate;

		private int[] _tris;

		public bool _uvsUpdate;

		private Vector2[] _uvs;

		public int vertCount { get; private set; }

		public Vector3[] verts
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Vector3[] normals
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Color[] colors
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int[] tris
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Vector2[] uvs
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public MeshUpdate()
		{
		}

		public MeshUpdate(SpriteCoordinates source)
		{
		}

		public MeshUpdate(Mesh source)
		{
		}

		public void Reset()
		{
		}
	}

	public Vector2 size;

	public bool lockAspect;

	public Vector2 scale;

	public SpritePivot pivot;

	public Color color;

	private bool loaded;

	protected Vector3[] verts;

	protected Color[] colors;

	protected Vector2[] uvs;

	protected Vector3[] normals;

	protected int[] tris;

	private GUIView _view;

	private Transform _tform;

	public EventDispatcher MeshUpdateEvent;

	protected Vector2 textureSize;

	[NonSerialized]
	protected MeshFilter _meshFilter;

	protected bool init;

	protected MeshUpdate update;

	public SpritePivot Pivot
	{
		get
		{
			return default(SpritePivot);
		}
		set
		{
		}
	}

	protected GUIView View
	{
		get
		{
			return null;
		}
	}

	protected Transform tform
	{
		get
		{
			return null;
		}
	}

	public Vector3 WorldPosition
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public MeshFilter meshFilter
	{
		get
		{
			return null;
		}
	}

	public static void MeshUpdateHierarchy(GameObject root)
	{
	}

	protected virtual void OnEnable()
	{
	}

	private void UnSubscribe()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public virtual void SetPosition(int x, int y)
	{
	}

	public virtual Vector2 ResetSize()
	{
		return default(Vector2);
	}

	public virtual Vector2 PixelSnap()
	{
		return default(Vector2);
	}

	public void SetMaterial(Material mat)
	{
	}

	public void RefreshTextureSize()
	{
	}

	public virtual Bounds GetBounds()
	{
		return default(Bounds);
	}

	public virtual void SetSize(Vector2 s)
	{
	}

	public virtual void SetColor(Color c)
	{
	}

	public virtual void SetAlpha(float alpha)
	{
	}

	public static void BuildVerts(Vector2 size, Vector2 scale, ref Vector3[] verts)
	{
	}

	public static Vector3[] BuildNormals(int count)
	{
		return null;
	}

	public static void BuildColors(Color color, ref Color[] colors)
	{
	}

	public static int[] BuildTris()
	{
		return null;
	}

	public static void BuildUVs(Rect rect, Vector2 size, ref Vector2[] uvs)
	{
	}

	protected virtual void OffsetVerts(Vector3[] verts)
	{
	}

	public virtual void Load()
	{
	}

	public virtual void AssembleMesh()
	{
	}

	protected void UpdateMesh()
	{
	}

	protected virtual void UpdateMesh(MeshUpdate update)
	{
	}

	protected virtual Vector2 GetMainTextureSize(bool fromShared)
	{
		return default(Vector2);
	}
}
