using System.Collections.Generic;
using UnityEngine;

public class SBGUIAtlasButton : SBGUIButton
{
	private Dictionary<string, SBGUIAtlasImage> images;

	private string loadedTexture;

	private YGAtlasSprite atlasSprite
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public static SBGUIAtlasButton Create(SBGUIElement parent, float x, float y, float w, float h, string imageAsset)
	{
		return null;
	}

	public void SetTextureFromFound(YGTextureLibrary.FoundMaterial found)
	{
	}

	public void SetTexture(string t)
	{
	}

	protected override void Initialize(SBGUIElement parent, Rect rect, string imageAsset)
	{
	}

	public SBGUIAtlasImage AttachImage(string asset)
	{
		return null;
	}

	public void SetTextureFromAtlas(string name)
	{
	}

	public void SetTextureFromAtlas(string name, bool resize, bool resizeToTrimmed = false, bool resizeToFit = false, int scalePixel = 0)
	{
	}

	public void SetTextureFromLibrary(string name, Texture texture = null)
	{
	}

	public override Vector2 ScaleToMaxSize(int pixels)
	{
		return default(Vector2);
	}
}
