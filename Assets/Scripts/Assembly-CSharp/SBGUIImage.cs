using UnityEngine;

public class SBGUIImage : SBGUIElement
{
	private YGSprite _sprite;

	protected YGSprite sprite
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Vector2 Size
	{
		get
		{
			return default(Vector2);
		}
		set
		{
		}
	}

	public virtual int Width
	{
		get
		{
			return 0;
		}
	}

	public virtual int Height
	{
		get
		{
			return 0;
		}
	}

	protected override void Awake()
	{
	}

	public static SBGUIImage Create(SBGUIElement parent, string name, string texture, Vector3 offset)
	{
		return null;
	}

	public static SBGUIImage Create(SBGUIElement parent, Rect size, string asset)
	{
		return null;
	}

	public virtual Vector2 ScaleToMaxSize(int pixels)
	{
		return default(Vector2);
	}

	public void SetSizeNoRebuild(Vector2 newSize)
	{
	}

	protected virtual void Initialize(SBGUIElement parent, Rect rect, string asset)
	{
	}

	public void SetAlpha(float a)
	{
	}

	public void SetColor(Color c)
	{
	}

	public Rect GetWorldRect()
	{
		return default(Rect);
	}

	public virtual void SetTextureFromTexturePath(string path)
	{
	}

	public virtual void SetMaterial(Material mat)
	{
	}

	public virtual void SetTextureFromMaterialPath(string path)
	{
	}

	public virtual void SetTexture(Texture t)
	{
	}

	public virtual void Unload()
	{
	}
}
