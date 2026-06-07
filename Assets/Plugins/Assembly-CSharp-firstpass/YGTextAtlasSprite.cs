using Yarg;

public class YGTextAtlasSprite : YGTextSprite
{
	protected override void OnEnable()
	{
	}

	public override FontAtlas.CharData GetCharOffset(char chr, FontAtlas atlas)
	{
		return default(FontAtlas.CharData);
	}
}
