#define ASSERTS_ON
using UnityEngine;

[RequireComponent(typeof(YGSprite))]
public class SBGUIImage : SBGUIElement
{
	private YGSprite _sprite;

	protected YGSprite sprite
	{
		get
		{
			if (_sprite == null)
			{
				_sprite = GetComponent<YGSprite>();
			}
			return _sprite;
		}
		set
		{
			_sprite = value;
		}
	}

	public Vector2 Size
	{
		get
		{
			return sprite.size;
		}
		set
		{
			sprite.SetSize(value);
		}
	}

	public virtual int Width
	{
		get
		{
			return Mathf.CeilToInt(sprite.size.x);
		}
	}

	public virtual int Height
	{
		get
		{
			return Mathf.CeilToInt(sprite.size.y);
		}
	}

	protected override void Awake()
	{
		sprite = GetComponent<YGSprite>();
		base.Awake();
	}

	public static SBGUIImage Create(SBGUIElement parent, string name, string texture, Vector3 offset)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.localPosition = offset;
		SBGUIImage sBGUIImage = gameObject.AddComponent<SBGUIImage>();
		sBGUIImage.sprite = gameObject.AddComponent<YGSprite>();
		sBGUIImage.SetParent(parent);
		sBGUIImage.SetTextureFromMaterialPath(texture);
		return sBGUIImage;
	}

	public static SBGUIImage Create(SBGUIElement parent, Rect size, string asset)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIImage_{0}", SBGUIElement.InstanceID));
		SBGUIImage sBGUIImage = gameObject.AddComponent<SBGUIImage>();
		sBGUIImage.Initialize(parent, size, asset);
		return sBGUIImage;
	}

	public virtual Vector2 ScaleToMaxSize(int pixels)
	{
		Texture mainTexture = sprite.GetComponent<Renderer>().material.mainTexture;
		if (mainTexture == null)
		{
			return Vector2.zero;
		}
		Vector2 vector = new Vector2(mainTexture.width, mainTexture.height);
		float num = Mathf.Max(vector.x, vector.y);
		if (num > (float)pixels)
		{
			float num2 = (float)pixels / num;
			vector *= num2;
		}
		sprite.SetSize(vector);
		return vector;
	}

	public void SetSizeNoRebuild(Vector2 newSize)
	{
		sprite.size = newSize;
	}

	protected virtual void Initialize(SBGUIElement parent, Rect rect, string asset)
	{
		SetParent(parent);
		TFUtils.DebugLog(string.Format("SBGUIImage Initialize: {0} sprite: {1}", base.gameObject.name, asset));
		sprite = base.gameObject.AddComponent<YGSprite>();
		sprite.SetPosition((int)rect.x, (int)rect.y);
		if (asset != null)
		{
			SetTextureFromMaterialPath(asset);
		}
		if (rect.width != -1f && rect.height != -1f)
		{
			sprite.size = new Vector2(rect.width, rect.height);
		}
		sprite.ResetSize();
	}

	public void SetAlpha(float a)
	{
		sprite.SetAlpha(a);
	}

	public void SetColor(Color c)
	{
		sprite.SetColor(c);
	}

	public Rect GetWorldRect()
	{
		return sprite.GetBounds().ToRect();
	}

	public virtual void SetTextureFromTexturePath(string path)
	{
		if (!path.ToLower().StartsWith("textures/"))
		{
			TFUtils.DebugLog(string.Format("Are you sure this is a texture? '{0}'", path));
		}
		Material material = base.GetComponent<Renderer>().material;
		Texture texture = (Texture)Resources.Load(path);
		TFUtils.Assert(texture != null, "unknown texture: " + path);
		material.mainTexture = texture;
		sprite.RefreshTextureSize();
	}

	public virtual void SetMaterial(Material mat)
	{
		TFUtils.Assert(mat != null, "unknown material: " + mat.name);
		base.GetComponent<Renderer>().material = mat;
		sprite.RefreshTextureSize();
	}

	public virtual void SetTextureFromMaterialPath(string path)
	{
		TFUtils.Assert(!string.IsNullOrEmpty(path), "SetTextureFromMaterialPath: empty path!");
		Material material = null;
		material = Resources.Load(path) as Material;
		TFUtils.Assert(material != null, "unknown material: " + path);
		base.GetComponent<Renderer>().material = material;
		sprite.RefreshTextureSize();
	}

	public virtual void SetTexture(Texture t)
	{
		sprite.GetComponent<Renderer>().material.mainTexture = t;
	}

	public virtual void Unload()
	{
	}
}
