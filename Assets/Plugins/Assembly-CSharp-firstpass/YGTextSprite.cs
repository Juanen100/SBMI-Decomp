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
				return '\0';
			}
			set
			{
			}
		}

		public CharSprite(char _chr)
		{
		}

		public CharSprite(char _chr, Vector2 _pos, FontAtlas.CharData data, Vector2 textureSize, Vector2 scale, Color color)
		{
		}
	}

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

	public string text;

	public string localizationKey;

	public float textScale;

	private const string SCALAR_SYMBOL = "~";

	public string Text
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string LocalizationKey
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string StripScalarDataFromString(string text, bool storeScale = true)
	{
		return null;
	}

	protected override void OnEnable()
	{
	}

	private void GenerateChars()
	{
	}

	private bool ValidateCharacters()
	{
		return false;
	}

	private void BuildTextSprite()
	{
	}

	public override void SetSize(Vector2 s)
	{
	}

	public virtual void SetScale(Vector2 s)
	{
	}

	public override void SetColor(Color c)
	{
	}

	public override SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex)
	{
		return null;
	}

	protected override void LoadSprite()
	{
	}

	protected override void UpdateMesh(MeshUpdate update)
	{
	}

	protected override void OffsetVerts(Vector3[] verts)
	{
	}

	public virtual FontAtlas.CharData GetCharOffset(char chr, FontAtlas atlas)
	{
		return default(FontAtlas.CharData);
	}
}
