#define ASSERTS_ON
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarg;

[RequireComponent(typeof(YGAtlasSprite))]
public class SBGUIAtlasButton : SBGUIButton
{
	private Dictionary<string, SBGUIAtlasImage> images = new Dictionary<string, SBGUIAtlasImage>();

	private string loadedTexture;

	private YGAtlasSprite atlasSprite
	{
		get
		{
			return (YGAtlasSprite)base.sprite;
		}
		set
		{
			base.sprite = value;
		}
	}

	public static SBGUIAtlasButton Create(SBGUIElement parent, float x, float y, float w, float h, string imageAsset)
	{
		GameObject gameObject = new GameObject(string.Format("SBAtlasButton_{0}", SBGUIElement.InstanceID));
		Rect rect = new Rect(x, y, w, h);
		SBGUIAtlasButton sBGUIAtlasButton = gameObject.AddComponent<SBGUIAtlasButton>();
		sBGUIAtlasButton.Initialize(parent, rect, imageAsset);
		return sBGUIAtlasButton;
	}

	public void SetTextureFromFound(YGTextureLibrary.FoundMaterial found)
	{
		atlasSprite.GetComponent<Renderer>().sharedMaterial = found.material;
		atlasSprite.sprite = new SpriteCoordinates(found.name);
		atlasSprite.sprite.coords = found.coords.frame;
		atlasSprite.atlasIndex = found.index;
		atlasSprite.Load();
	}

	public void SetTexture(string t)
	{
		YGTextureLibrary.FoundMaterial textureFromFound = base.View.Library.FindSpriteMaterial(t);
		if (textureFromFound.index >= 0)
		{
			SetTextureFromFound(textureFromFound);
		}
		else
		{
			TFUtils.Assert(false, "unknown image: " + t);
		}
	}

	protected override void Initialize(SBGUIElement parent, Rect rect, string imageAsset)
	{
		SetParent(parent);
		YG2DRectangle yG2DRectangle = base.gameObject.AddComponent<YG2DRectangle>();
		yG2DRectangle.size = new Vector2(rect.width, rect.height);
		button = base.gameObject.AddComponent<TapButton>();
		button.SetPosition((int)rect.x, (int)rect.y);
		AttachImage(imageAsset);
	}

	public SBGUIAtlasImage AttachImage(string asset)
	{
		SBGUIAtlasImage sBGUIAtlasImage = SBGUIAtlasImage.Create(this, new Rect(0f, 0f, -1f, -1f), asset);
		sBGUIAtlasImage.transform.localPosition = Vector3.zero;
		images[asset] = sBGUIAtlasImage;
		return sBGUIAtlasImage;
	}

	public void SetTextureFromAtlas(string name)
	{
		SetTextureFromAtlas(name, false);
	}

	public void SetTextureFromAtlas(string name, bool resize, bool resizeToTrimmed = false, bool resizeToFit = false, int scalePixel = 0)
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.sprite;
		if (name == yGAtlasSprite.sprite.name)
		{
			return;
		}
		YGTextureLibrary.FoundMaterial foundMaterial = base.View.Library.FindSpriteMaterial(name);
		if (foundMaterial.index >= 0)
		{
			yGAtlasSprite.sprite = new SpriteCoordinates(foundMaterial.name);
			yGAtlasSprite.sprite.coords = foundMaterial.coords.frame;
			if (resize)
			{
				if (resizeToTrimmed)
				{
					yGAtlasSprite.size = new Vector2(yGAtlasSprite.size.x * foundMaterial.coords.spriteSize.width / foundMaterial.coords.spriteSourceSize.x, yGAtlasSprite.size.y * foundMaterial.coords.spriteSize.height / foundMaterial.coords.spriteSourceSize.y);
				}
				else if (resizeToFit)
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
			if (yGAtlasSprite.GetAtlas().useSingleTexture)
			{
				SetTextureFromLibrary(foundMaterial.name);
				yGAtlasSprite.SetNonAtlasName(foundMaterial.name);
			}
			else
			{
				yGAtlasSprite.SetMaterial(foundMaterial.material);
				yGAtlasSprite.Load();
			}
			yGAtlasSprite.Load();
		}
		else if (yGAtlasSprite.GetAtlas().useSingleTexture)
		{
			yGAtlasSprite.sprite = new SpriteCoordinates(name);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
			string path = "Textures/Atlases/Obj_Portraits/" + fileNameWithoutExtension;
			Texture texture = (Texture)Resources.Load(path, typeof(Texture));
			if (texture == null)
			{
				path = "Textures/Atlases/Portraits/" + fileNameWithoutExtension;
				texture = (Texture)Resources.Load(path, typeof(Texture));
				if (texture == null)
				{
					TFUtils.Assert(false, "Image doesn't exist in Textures/Atlases/Portraits/ or in atlas, inquired image:  " + name);
				}
			}
			if (resize)
			{
				if (resizeToTrimmed)
				{
					yGAtlasSprite.size = new Vector2(yGAtlasSprite.size.x, yGAtlasSprite.size.y);
				}
				else
				{
					yGAtlasSprite.size = new Vector2(texture.width, texture.height);
				}
			}
			SetTextureFromLibrary(name, texture);
			yGAtlasSprite.SetNonAtlasName(name);
		}
		else
		{
			TFUtils.Assert(false, "unknown image: " + name);
		}
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
}
