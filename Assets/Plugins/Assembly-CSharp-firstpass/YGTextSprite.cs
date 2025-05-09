using System;
using UnityEngine;
using Yarg;

public class YGTextSprite : YGAtlasSprite
{
	[Serializable]
	public class CharSprite
	{
		public Vector2 pos;

		public int character;

		public Rect coords;

		public Vector2[] uvs;

		public Vector3[] verts;

		public Color[] colors;

		public char chr
		{
			get
			{
				return (char)character;
			}
			set
			{
				character = value;
			}
		}

		public CharSprite(char _chr)
		{
			chr = _chr;
		}

		public CharSprite(char _chr, Vector2 _pos, FontAtlas.CharData data, Vector2 textureSize, Vector2 scale, Color color)
		{
			chr = _chr;
			pos = _pos;
			coords = data.size;
			Vector2 size = new Vector2(coords.width, coords.height);
			uvs = new Vector2[4];
			verts = new Vector3[4];
			colors = new Color[4];
			YGSprite.BuildVerts(size, scale, ref verts);
			YGSprite.BuildColors(color, ref colors);
			YGSprite.BuildUVs(coords, textureSize, ref uvs);
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i].x += _pos.x;
				verts[i].y += _pos.y;
			}
		}
	}

	private const string SCALAR_SYMBOL = "~";

	[HideInInspector]
	public int fontIndex;

	[HideInInspector]
	public int lineHeight;

	[HideInInspector]
	public CharSprite[] characters;

	public Bounds bounds;

	public Vector2 textSize;

	private bool dirty;

	private bool textChanged;

	public bool useKerning;

	public Vector2 center;

	public string text = "Text";

	public string localizationKey = string.Empty;

	public float textScale;

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			if (!(text == value))
			{
				text = value;
				GenerateChars();
				dirty = true;
				textChanged = true;
				base.View.RefreshEvent += AssembleMesh;
			}
		}
	}

	public string LocalizationKey
	{
		get
		{
			return localizationKey;
		}
		set
		{
			if (!(value == string.Empty) && !(localizationKey == value))
			{
				localizationKey = value;
				Text = Language.Get(localizationKey);
			}
		}
	}

	public string StripScalarDataFromString(string text, bool storeScale = true)
	{
		if (text.Contains("~"))
		{
			int num = text.IndexOf("~");
			string text2 = null;
			try
			{
				if (storeScale)
				{
					textScale = float.Parse(text.Substring(num + 1));
				}
				return text.Substring(0, num);
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
				return text;
			}
		}
		return text;
	}

	protected override void OnEnable()
	{
		YGTextureLibrary library = base.View.Library;
		if (base.GetComponent<Renderer>().sharedMaterial == null && library.fontAtlases[fontIndex].material != null)
		{
			SetMaterial(library.fontAtlases[fontIndex].material);
		}
		if (sprite == null)
		{
			Debug.LogWarning("TextSprite is null");
			sprite = new SpriteCoordinates();
		}
		if (string.IsNullOrEmpty(sprite.name))
		{
			string filename = library.fontAtlases[fontIndex].filename;
			sprite.name = filename;
		}
		if (localizationKey != string.Empty)
		{
			Text = Language.Get(localizationKey);
		}
		else
		{
			Text = text;
		}
		dirty = true;
		textChanged = true;
		base.View.RefreshEvent += AssembleMesh;
		base.OnEnable();
	}

	private void GenerateChars()
	{
		text = StripScalarDataFromString(text);
		char[] array = text.ToCharArray();
		FontAtlas fontAtlas = base.View.Library.fontAtlases[fontIndex];
		FontAtlas.CharData charOffset = GetCharOffset(' ', fontAtlas);
		characters = new CharSprite[array.Length];
		Vector2 zero = Vector2.zero;
		lineHeight = fontAtlas.info.lineHeight;
		int first = -1;
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			char c = array[i];
			if (c == '|')
			{
				c = '\n';
			}
			FontAtlas.CharData charOffset2 = GetCharOffset(c, fontAtlas);
			charOffset2.offset.Set(charOffset2.offset.x * scale.x, charOffset2.offset.y * scale.y);
			if (useKerning)
			{
				num = fontAtlas.Kerning(first, c);
				if (num != 0)
				{
					zero.x += (float)num * scale.x;
				}
			}
			if (!char.IsWhiteSpace(c))
			{
				CharSprite charSprite = new CharSprite(c, zero - charOffset2.offset, charOffset2, textureSize, scale, color);
				characters[i] = charSprite;
			}
			else
			{
				characters[i] = new CharSprite(c);
			}
			switch (c)
			{
			case '\n':
				zero.y -= lineHeight;
				zero.x = 0f;
				break;
			case '\t':
				zero.x += (float)charOffset.xadvance * scale.x * 4f;
				break;
			default:
				zero.x += (float)charOffset2.xadvance * scale.x;
				break;
			}
			first = c;
		}
		zero.y = 0f - zero.y + (float)lineHeight;
		textSize = zero;
		dirty = false;
	}

	private bool ValidateCharacters()
	{
		if (dirty || text == null || characters == null)
		{
			return false;
		}
		if (text.Length != characters.Length)
		{
			return false;
		}
		char[] array = text.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (characters[i] == null)
			{
				return false;
			}
			if (array[i] != characters[i].chr)
			{
				return false;
			}
		}
		return true;
	}

	private void BuildTextSprite()
	{
		CharSprite[] array = Array.FindAll(characters, (CharSprite x) => !char.IsWhiteSpace(x.chr));
		int num = array.Length;
		tris = new int[num * 6];
		uvs = new Vector2[num * 4];
		verts = new Vector3[num * 4];
		colors = new Color[num * 4];
		normals = YGSprite.BuildNormals(verts.Length);
		int[] array2 = YGSprite.BuildTris();
		for (int num2 = 0; num2 < num; num2++)
		{
			CharSprite charSprite = array[num2];
			int num3 = num2 * 4;
			for (int num4 = 0; num4 < 4; num4++)
			{
				verts[num3 + num4] = charSprite.verts[num4];
				uvs[num3 + num4] = charSprite.uvs[num4];
				colors[num3 + num4] = charSprite.colors[num4];
			}
			num3 = num2 * 6;
			for (int num5 = 0; num5 < 6; num5++)
			{
				tris[num3 + num5] = array2[num5];
				array2[num5] += 4;
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.colors = colors;
		mesh.triangles = tris;
		mesh.normals = normals;
		mesh.RecalculateBounds();
		sprite.SetMesh(mesh);
		bounds = mesh.bounds;
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		size = new Vector2(max.x - min.x, max.y - min.y);
		center = bounds.center;
		UnityEngine.Object.DestroyImmediate(mesh);
	}

	public override void SetSize(Vector2 s)
	{
		if (!(size == s))
		{
			size = s;
			dirty = true;
			base.View.RefreshEvent += AssembleMesh;
		}
	}

	public virtual void SetScale(Vector2 s)
	{
		if (!(scale == s))
		{
			scale = s;
			dirty = true;
			base.View.RefreshEvent += AssembleMesh;
		}
	}

	public override void SetColor(Color c)
	{
		if (dirty)
		{
			color = c;
		}
		else
		{
			base.SetColor(c);
		}
	}

	public override SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex)
	{
		LoadSprite();
		return sprite;
	}

	protected override void LoadSprite()
	{
		if (!ValidateCharacters())
		{
			GenerateChars();
		}
		BuildTextSprite();
	}

	protected override void UpdateMesh(MeshUpdate update)
	{
		if (base.meshFilter.sharedMesh != null && (textChanged || !Application.isPlaying))
		{
			base.meshFilter.sharedMesh.Clear();
			textChanged = false;
		}
		base.UpdateMesh(update);
	}

	protected override void OffsetVerts(Vector3[] verts)
	{
		Vector3 vector = center;
		vector.x -= size.x * scale.x / 2f;
		vector.y += size.y * scale.y / 2f;
		for (int i = 0; i < verts.Length; i++)
		{
			verts[i] -= vector;
		}
		base.OffsetVerts(verts);
	}

	public virtual FontAtlas.CharData GetCharOffset(char chr, FontAtlas atlas)
	{
		return atlas[chr];
	}
}
