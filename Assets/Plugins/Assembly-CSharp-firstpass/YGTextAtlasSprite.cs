using UnityEngine;
using Yarg;

public class YGTextAtlasSprite : YGTextSprite
{
	protected override void OnEnable()
	{
		Material atlasMaterial = base.View.Library.textureAtlases[atlasIndex].GetAtlasMaterial();
		if (base.GetComponent<Renderer>().sharedMaterial != atlasMaterial)
		{
			SetMaterial(atlasMaterial);
		}
		base.OnEnable();
	}

	public override FontAtlas.CharData GetCharOffset(char chr, FontAtlas atlas)
	{
		if (sprite == null)
		{
			Debug.LogError(string.Format("sprite isn't initialized for {0}", base.gameObject.name));
			Debug.Break();
		}
		FontAtlas.CharData result = atlas[chr];
		result.size.x += sprite.coords.x;
		result.size.y += sprite.coords.y;
		if (atlas.filename.Contains("cyrillic"))
		{
			result.size.x -= 4f;
			result.size.y -= 4f;
		}
		else
		{
			result.size.x -= 1f;
			result.size.y -= 1f;
			result.size.width += 2f;
			result.size.height += 2f;
		}
		return result;
	}
}
