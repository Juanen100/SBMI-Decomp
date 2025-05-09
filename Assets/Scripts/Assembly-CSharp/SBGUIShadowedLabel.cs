using UnityEngine;

[RequireComponent(typeof(YGTextAtlasSprite))]
public class SBGUIShadowedLabel : SBGUILabel
{
	public SBGUILabel Shadow;

	public override string Text
	{
		get
		{
			return base.textSprite.Text;
		}
		set
		{
			if (base.textSprite != null)
			{
				base.textSprite.Text = value;
				if (Shadow != null)
				{
					Shadow.SetText(value);
				}
			}
		}
	}

	public override bool SetText(string s)
	{
		if (base.textSprite == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(s))
		{
			s = string.Empty;
		}
		if (base.textSprite.Text != s)
		{
			base.textSprite.Text = s;
			Text = s;
			if (base.textSprite.textScale != 0f)
			{
				base.textSprite.SetScale(new Vector2(base.textSprite.textScale, base.textSprite.textScale));
			}
			return true;
		}
		return false;
	}

	protected override void Awake()
	{
		base.textSprite = base.gameObject.GetComponent<YGTextSprite>();
		if (base.textSprite.textScale != 0f)
		{
			base.textSprite.scale = new Vector2(base.textSprite.textScale, base.textSprite.textScale);
		}
		if (Language.CurrentLanguage() == LanguageCode.RU)
		{
			SwapFont("cyrillic", base.textSprite);
		}
	}

	protected override void SwapFont(string desiredFontName, YGTextSprite textSprite)
	{
		int size = base.View.Library.fontAtlases[textSprite.fontIndex].info.size;
		string text = desiredFontName + "-" + size + ".png";
		for (int i = 0; i < base.View.Library.fontAtlases.Length; i++)
		{
			if (base.View.Library.fontAtlases[i].filename == text)
			{
				textSprite.fontIndex = i;
				textSprite.sprite.coords = YGTextureLibrary.GetAtlasCoords(text).atlasCoords.frame;
			}
		}
		if (base.View.Library.fontAtlases[textSprite.fontIndex].filename != text)
		{
			TFUtils.ErrorLog("Could not find " + text + " in the 'Font Atlases' list in the GUIMainView");
		}
	}
}
