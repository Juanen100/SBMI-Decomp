#define ASSERTS_ON
using System;
using System.IO;
using UnityEngine;
using Yarg;

[RequireComponent(typeof(YGAtlasSprite))]
public class SBGUIAtlasImage : SBGUIImage
{
	private string loadedTexture;

	public new static SBGUIAtlasImage Create(SBGUIElement parent, Rect size, string asset)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIAtlasImage_{0}", SBGUIElement.InstanceID));
		SBGUIAtlasImage sBGUIAtlasImage = gameObject.AddComponent<SBGUIAtlasImage>();
		sBGUIAtlasImage.Initialize(parent, size, asset);
		return sBGUIAtlasImage;
	}

	protected override void Initialize(SBGUIElement parent, Rect rect, string asset)
	{
		SetParent(parent);
		TFUtils.DebugLog(string.Format("SBGUIImage Initialize: {0} sprite: {1}", base.gameObject.name, asset));
		base.sprite = base.gameObject.AddComponent<YGAtlasSprite>();
		base.sprite.SetPosition((int)rect.x, (int)rect.y);
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		if (yGAtlasSprite.GetAtlas().useSingleTexture)
		{
			SetTextureFromLibrary(asset);
		}
		else
		{
			SetTextureFromAtlas(asset);
		}
		if (rect.width != -1f && rect.height != -1f)
		{
			base.sprite.size = new Vector2(rect.width, rect.height);
		}
		base.sprite.ResetSize();
	}

	public void SetTextureFromSearch(string path)
	{
		string fileName = Path.GetFileName(path);
		YGTextureLibrary.FoundMaterial foundMaterial = base.View.Library.FindSpriteMaterial(fileName);
		if (foundMaterial.index >= 0)
		{
			YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
			yGAtlasSprite.GetComponent<Renderer>().sharedMaterial = foundMaterial.material;
			yGAtlasSprite.sprite = new SpriteCoordinates(foundMaterial.name);
			yGAtlasSprite.atlasIndex = foundMaterial.index;
			yGAtlasSprite.Load();
		}
		else if (path.ToLower().StartsWith("texture"))
		{
			SetTextureFromTexturePath(path);
		}
		else
		{
			SetTextureFromMaterialPath(path);
		}
	}

	public override void SetTextureFromMaterialPath(string path)
	{
		base.SetTextureFromMaterialPath(path);
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		yGAtlasSprite.sprite = new SpriteCoordinates();
		yGAtlasSprite.Load();
	}

	public override void SetTextureFromTexturePath(string path)
	{
		base.SetTextureFromTexturePath(path);
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		yGAtlasSprite.sprite = new SpriteCoordinates();
		yGAtlasSprite.Load();
	}

	public void SetTextureFromLibrary(string name, Texture texture = null)
	{
		if (texture == null)
		{
			texture = base.View.Library.LoadTexture(name);
		}
		Material materialPrototype = base.View.Library.materialPrototype;
		Material material = materialPrototype;
		base.GetComponent<Renderer>().material = material;
		Color color = base.GetComponent<Renderer>().material.GetColor("_TintColor");
		if (texture != null)
		{
			color.a = 1f;
			base.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
		}
		else
		{
			color.a = 0f;
		}
		base.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		yGAtlasSprite.sprite = new SpriteCoordinates();
		yGAtlasSprite.Load();
	}

	public void SetTextureFromAtlas(string name)
	{
		SetTextureFromAtlas(name, false);
	}

	public void SetTextureFromAtlas(string name, bool resize, bool resizeToTrimmed = false, bool resizeToFit = false, bool keepSmallSize = false, bool explanationDialog = false, int scalePixel = 0)
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		if (yGAtlasSprite != null && yGAtlasSprite.sprite != null && name == yGAtlasSprite.sprite.name)
		{
			return;
		}
		YGTextureLibrary.FoundMaterial foundMaterial = base.View.Library.FindSpriteMaterial(name);
		if (foundMaterial.index >= 0)
		{
			yGAtlasSprite.sprite = new SpriteCoordinates(foundMaterial.name);
			yGAtlasSprite.sprite.coords = foundMaterial.coords.frame;
			if (yGAtlasSprite.sprite.coords.width * yGAtlasSprite.sprite.coords.height > 2500f)
			{
				keepSmallSize = false;
			}
			if (resize)
			{
				if (resizeToTrimmed)
				{
					yGAtlasSprite.size = new Vector2(yGAtlasSprite.size.x * foundMaterial.coords.spriteSize.width / foundMaterial.coords.spriteSourceSize.x, yGAtlasSprite.size.y * foundMaterial.coords.spriteSize.height / foundMaterial.coords.spriteSourceSize.y);
				}
				else if (resizeToFit && !keepSmallSize)
				{
					float num = yGAtlasSprite.size.x / foundMaterial.coords.spriteSize.width;
					float num2 = yGAtlasSprite.size.y / foundMaterial.coords.spriteSize.height;
					float num3 = 1f;
					if (num2 < num)
					{
						num = num2;
					}
					if (scalePixel > 0)
					{
						float num4 = Mathf.Max(num * foundMaterial.coords.spriteSize.width, num * foundMaterial.coords.spriteSize.height);
						num3 = ((!(num4 > (float)scalePixel)) ? ((float)scalePixel / num4) : (num4 / (float)scalePixel));
					}
					yGAtlasSprite.size = new Vector2(num * foundMaterial.coords.spriteSize.width * num3, num * foundMaterial.coords.spriteSize.height * num3);
				}
				else
				{
					yGAtlasSprite.size = new Vector2(foundMaterial.coords.spriteSize.width, foundMaterial.coords.spriteSize.height);
				}
			}
			yGAtlasSprite.atlasIndex = foundMaterial.index;
			if (yGAtlasSprite.GetAtlas().useSingleTexture && !explanationDialog)
			{
				SetTextureFromLibrary(foundMaterial.name);
				yGAtlasSprite.SetNonAtlasName(foundMaterial.name);
			}
			else
			{
				yGAtlasSprite.SetMaterial(foundMaterial.material);
				yGAtlasSprite.Load();
			}
			return;
		}
		yGAtlasSprite.sprite = new SpriteCoordinates(name);
		string text = YGTextureLibrary.ActualName(name);
		string path = "Textures/Atlases/Portraits/" + text;
		Texture2D texture2D = (Texture2D)Resources.Load(path, typeof(Texture));
		if (texture2D == null)
		{
			path = "Textures/Atlases/Obj_Portraits/" + text;
			texture2D = (Texture2D)Resources.Load(path, typeof(Texture));
			if (texture2D == null)
			{
				TFUtils.Assert(false, "Image doesn't exist in Textures/Atlases/Portraits/ or /Obj_Portraits or in atlas, inquired image:  " + name);
			}
		}
		if (yGAtlasSprite.sprite.coords.width * yGAtlasSprite.sprite.coords.height > 2500f)
		{
			keepSmallSize = false;
		}
		if (!resize)
		{
			return;
		}
		if (resizeToTrimmed)
		{
			yGAtlasSprite.size = new Vector2(yGAtlasSprite.size.x, yGAtlasSprite.size.y);
		}
		else if (resizeToFit && !keepSmallSize)
		{
			float num5 = yGAtlasSprite.size.x / (float)texture2D.width;
			float num6 = yGAtlasSprite.size.y / (float)texture2D.height;
			if (num6 < num5)
			{
				num5 = num6;
			}
			yGAtlasSprite.size = new Vector2(num5 * (float)texture2D.width, num5 * (float)texture2D.height);
		}
		else
		{
			yGAtlasSprite.size = new Vector2(texture2D.width, texture2D.height);
		}
		SetTextureFromLibrary(name, texture2D);
		yGAtlasSprite.SetNonAtlasName(name);
	}

	public virtual void ResetSize()
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		yGAtlasSprite.ResetSize();
	}

	public override Vector2 ScaleToMaxSize(int pixels)
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		if (string.IsNullOrEmpty(yGAtlasSprite.sprite.name))
		{
			return base.ScaleToMaxSize(pixels);
		}
		Vector2 vector = new Vector2(yGAtlasSprite.sprite.coords.width, yGAtlasSprite.sprite.coords.height);
		float num = Mathf.Max(vector.x, vector.y);
		if (num > (float)pixels)
		{
			float num2 = (float)pixels / num;
			vector *= num2;
		}
		base.sprite.SetSize(vector);
		return vector;
	}

	public override void SetTexture(Texture t)
	{
		throw new NotImplementedException();
	}
}
