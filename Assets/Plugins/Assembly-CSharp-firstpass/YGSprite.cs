using System;
using UnityEngine;
using Yarg;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
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
				return _verts;
			}
			set
			{
				if (value == null)
				{
					Debug.LogError("Null verts sent to MeshUpdate");
					return;
				}
				_verts = value;
				_vertsUpdate = true;
				vertCount = value.Length;
			}
		}

		public Vector3[] normals
		{
			get
			{
				return _normals;
			}
			set
			{
				_normals = value;
				_normalsUpdate = true;
			}
		}

		public Color[] colors
		{
			get
			{
				return _colors;
			}
			set
			{
				_colors = value;
				_colorsUpdate = true;
			}
		}

		public int[] tris
		{
			get
			{
				return _tris;
			}
			set
			{
				_tris = value;
				_trisUpdate = true;
			}
		}

		public Vector2[] uvs
		{
			get
			{
				return _uvs;
			}
			set
			{
				_uvs = value;
				_uvsUpdate = true;
			}
		}

		public MeshUpdate()
		{
		}

		public MeshUpdate(SpriteCoordinates source)
		{
			verts = source.verts;
			normals = source.normals;
			colors = source.color;
			tris = source.tris;
			uvs = source.uvs;
		}

		public MeshUpdate(Mesh source)
		{
			verts = source.vertices;
			normals = source.normals;
			colors = source.colors;
			tris = source.triangles;
			uvs = source.uv;
		}

		public void Reset()
		{
			_vertsUpdate = false;
			_normalsUpdate = false;
			_colorsUpdate = false;
			_trisUpdate = false;
			_uvsUpdate = false;
		}
	}

	public Vector2 size = new Vector2(64f, 64f);

	public bool lockAspect = true;

	public Vector2 scale = Vector2.one;

	public SpritePivot pivot = SpritePivot.MiddleCenter;

	public Color color = new Color(1f, 1f, 1f, 0.5f);

	private bool loaded;

	protected Vector3[] verts = new Vector3[4];

	protected Color[] colors = new Color[4];

	protected Vector2[] uvs = new Vector2[4];

	protected Vector3[] normals = BuildNormals(4);

	protected int[] tris = BuildTris();

	private GUIView _view;

	private Transform _tform;

	public EventDispatcher MeshUpdateEvent = new EventDispatcher();

	protected Vector2 textureSize = Vector2.one;

	[NonSerialized]
	protected MeshFilter _meshFilter;

	protected bool init;

	protected MeshUpdate update = new MeshUpdate();

	public SpritePivot Pivot
	{
		get
		{
			return pivot;
		}
		set
		{
			pivot = value;
			View.RefreshEvent += AssembleMesh;
		}
	}

	protected GUIView View
	{
		get
		{
			if (_view == null)
			{
				_view = GUIView.GetParentView(tform);
			}
			return _view;
		}
	}

	protected Transform tform
	{
		get
		{
			return (!(_tform != null)) ? (_tform = base.transform) : _tform;
		}
	}

	public Vector3 WorldPosition
	{
		get
		{
			return tform.position;
		}
		set
		{
			if (!(value == tform.position))
			{
				tform.position = value;
				MeshUpdateHierarchy(base.gameObject);
			}
		}
	}

	public MeshFilter meshFilter
	{
		get
		{
			if (_meshFilter == null)
			{
				_meshFilter = base.gameObject.GetComponent<MeshFilter>();
				if (_meshFilter == null)
				{
					_meshFilter = base.gameObject.AddComponent<MeshFilter>();
					base.GetComponent<Renderer>().castShadows = false;
					base.GetComponent<Renderer>().receiveShadows = false;
				}
				UnityEngine.Object.DestroyImmediate(_meshFilter.mesh);
				_meshFilter.mesh = new Mesh();
			}
			return _meshFilter;
		}
	}

	public static void MeshUpdateHierarchy(GameObject root)
	{
		YGSprite[] componentsInChildren = root.GetComponentsInChildren<YGSprite>();
		YGSprite[] array = componentsInChildren;
		foreach (YGSprite yGSprite in array)
		{
			yGSprite.MeshUpdateEvent.FireEvent();
		}
	}

	protected virtual void OnEnable()
	{
		if (base.GetComponent<Renderer>().sharedMaterial != null)
		{
			textureSize = GetMainTextureSize(true);
		}
		if (!loaded)
		{
			View.RefreshEvent += Load;
		}
	}

	private void UnSubscribe()
	{
		GUIView view = View;
		view.RefreshEvent -= Load;
		view.RefreshEvent -= AssembleMesh;
		View.RefreshEvent -= UpdateMesh;
	}

	protected virtual void OnDisable()
	{
		UnSubscribe();
		_view = null;
	}

	protected virtual void OnDestroy()
	{
		if (base.transform.parent != null)
		{
			UnSubscribe();
		}
		UnityEngine.Object.Destroy(meshFilter.sharedMesh);
	}

	public virtual void SetPosition(int x, int y)
	{
		Vector3 position = View.PixelsToWorld(new Vector2(x, y));
		tform.position = position;
	}

	public virtual Vector2 ResetSize()
	{
		if (base.GetComponent<Renderer>().sharedMaterial == null)
		{
			return Vector2.zero;
		}
		textureSize = GetMainTextureSize(true);
		size.Set(textureSize.x, textureSize.y);
		AssembleMesh();
		return size;
	}

	public virtual Vector2 PixelSnap()
	{
		Vector3 position = tform.position;
		position.x = (float)Mathf.RoundToInt(position.x / 0.01f) * 0.01f;
		position.y = (float)Mathf.RoundToInt(position.y / 0.01f) * 0.01f;
		position.z = (float)Mathf.RoundToInt(position.z / 0.01f) * 0.01f;
		tform.position = position;
		size.x = Mathf.RoundToInt(size.x);
		size.y = Mathf.RoundToInt(size.y);
		AssembleMesh();
		return size;
	}

	public void SetMaterial(Material mat)
	{
		base.GetComponent<Renderer>().sharedMaterial = mat;
		textureSize = GetMainTextureSize(true);
	}

	public void RefreshTextureSize()
	{
		textureSize = GetMainTextureSize(false);
	}

	public virtual Bounds GetBounds()
	{
		return base.GetComponent<Renderer>().bounds;
	}

	public virtual void SetSize(Vector2 s)
	{
		size = s;
		BuildVerts(size, scale, ref verts);
		update.verts = verts;
		UpdateMesh();
		View.RefreshEvent += UpdateMesh;
	}

	public virtual void SetColor(Color c)
	{
		color = c;
		BuildColors(color, ref colors);
		update.colors = colors;
		View.RefreshEvent += UpdateMesh;
	}

	public virtual void SetAlpha(float alpha)
	{
		Color color = this.color;
		if (color.a != alpha)
		{
			color.a = alpha;
			SetColor(color);
		}
	}

	public static void BuildVerts(Vector2 size, Vector2 scale, ref Vector3[] verts)
	{
		size.x *= scale.x;
		size.y *= scale.y;
		verts[0].Set(0f, 0f, 0f);
		verts[1].Set(size.x, 0f, 0f);
		verts[2].Set(0f, 0f - size.y, 0f);
		verts[3].Set(size.x, 0f - size.y, 0f);
	}

	public static Vector3[] BuildNormals(int count)
	{
		Vector3[] array = new Vector3[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = -Vector3.forward;
		}
		return array;
	}

	public static void BuildColors(Color color, ref Color[] colors)
	{
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = color;
		}
	}

	public static int[] BuildTris()
	{
		return new int[6] { 1, 3, 2, 1, 2, 0 };
	}

	public static void BuildUVs(Rect rect, Vector2 size, ref Vector2[] uvs)
	{
		uvs[0].Set(rect.xMin / size.x, 1f - rect.yMin / size.y);
		uvs[1].Set(rect.xMax / size.x, 1f - rect.yMin / size.y);
		uvs[2].Set(rect.xMin / size.x, 1f - rect.yMax / size.y);
		uvs[3].Set(rect.xMax / size.x, 1f - rect.yMax / size.y);
	}

	protected virtual void OffsetVerts(Vector3[] verts)
	{
		for (int i = 0; i < verts.Length; i++)
		{
			verts[i] *= 0.01f;
			switch (pivot)
			{
			case SpritePivot.LowerCenter:
			case SpritePivot.MiddleCenter:
			case SpritePivot.UpperCenter:
				verts[i].x -= size.x * 0.01f * 0.5f * scale.x;
				break;
			case SpritePivot.LowerRight:
			case SpritePivot.MiddleRight:
			case SpritePivot.UpperRight:
				verts[i].x -= size.x * 0.01f * scale.x;
				break;
			}
			switch (pivot)
			{
			case SpritePivot.MiddleCenter:
			case SpritePivot.MiddleLeft:
			case SpritePivot.MiddleRight:
				verts[i].y += size.y * 0.01f * 0.5f * scale.y;
				break;
			case SpritePivot.LowerCenter:
			case SpritePivot.LowerLeft:
			case SpritePivot.LowerRight:
				verts[i].y += size.y * 0.01f * scale.y;
				break;
			}
		}
	}

	public virtual void Load()
	{
		loaded = true;
		AssembleMesh();
	}

	public virtual void AssembleMesh()
	{
		update.Reset();
		BuildUVs(new Rect(0f, 0f, textureSize.x, textureSize.y), textureSize, ref uvs);
		BuildVerts(size, scale, ref verts);
		BuildColors(color, ref colors);
		update.verts = verts;
		update.normals = normals;
		update.colors = colors;
		update.tris = tris;
		update.uvs = uvs;
		UpdateMesh(update);
	}

	protected void UpdateMesh()
	{
		UpdateMesh(update);
	}

	protected virtual void UpdateMesh(MeshUpdate update)
	{
		if (this == null)
		{
			return;
		}
		Mesh mesh = null;
		mesh = ((!(meshFilter.sharedMesh == null)) ? meshFilter.sharedMesh : new Mesh());
		if (update._vertsUpdate)
		{
			OffsetVerts(update.verts);
			try
			{
				mesh.vertices = update.verts;
				mesh.RecalculateBounds();
			}
			catch
			{
				Debug.Log(string.Format("{0} : {1}", base.gameObject.name, mesh == null));
				throw;
			}
		}
		if (update._uvsUpdate)
		{
			if (update.uvs.Length != mesh.vertices.Length)
			{
				return;
			}
			mesh.uv = update.uvs;
		}
		if (update._trisUpdate)
		{
			mesh.triangles = update.tris;
		}
		if (update._normalsUpdate)
		{
			if (update.normals.Length != mesh.vertices.Length)
			{
				return;
			}
			mesh.normals = update.normals;
		}
		if (update._colorsUpdate)
		{
			if (update.colors.Length != mesh.vertices.Length)
			{
				return;
			}
			mesh.colors = update.colors;
		}
		meshFilter.mesh = mesh;
		update.Reset();
		MeshUpdateEvent.FireEvent();
	}

	protected virtual Vector2 GetMainTextureSize(bool fromShared)
	{
		if (fromShared)
		{
			if (base.GetComponent<Renderer>().sharedMaterial != null && base.GetComponent<Renderer>().sharedMaterial.mainTexture != null)
			{
				return new Vector2(base.GetComponent<Renderer>().sharedMaterial.mainTexture.width, base.GetComponent<Renderer>().sharedMaterial.mainTexture.height);
			}
			return Vector2.zero;
		}
		if (base.GetComponent<Renderer>().material != null && base.GetComponent<Renderer>().material.mainTexture != null)
		{
			return new Vector2(base.GetComponent<Renderer>().material.mainTexture.width, base.GetComponent<Renderer>().material.mainTexture.height);
		}
		return Vector2.zero;
	}
}
