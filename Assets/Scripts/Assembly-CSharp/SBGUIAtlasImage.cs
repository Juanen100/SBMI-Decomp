using UnityEngine;

public class SBGUIAtlasImage : SBGUIImage
{
	private string loadedTexture;

	public new static SBGUIAtlasImage Create(SBGUIElement parent, Rect size, string asset)
	{
		return null;
	}

	protected override void Initialize(SBGUIElement parent, Rect rect, string asset)
	{
	}

	public void SetTextureFromSearch(string path)
	{
	}

	public override void SetTextureFromMaterialPath(string path)
	{
	}

	public override void SetTextureFromTexturePath(string path)
	{
	}

	public void SetTextureFromLibrary(string name, Texture texture = null)
	{
	}

	public void SetTextureFromAtlas(string name)
	{
	}

	public void SetTextureFromAtlas(string name, bool resize, bool resizeToTrimmed = false, bool resizeToFit = false, bool keepSmallSize = false, bool explanationDialog = false, int scalePixel = 0)
	{
	}

	public virtual void ResetSize()
	{
	}

	public override Vector2 ScaleToMaxSize(int pixels)
	{
		return default(Vector2);
	}

	public override void SetTexture(Texture t)
	{
	}
}
