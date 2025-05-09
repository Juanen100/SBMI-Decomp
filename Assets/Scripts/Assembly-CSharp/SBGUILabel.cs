using UnityEngine;

[RequireComponent(typeof(YGTextAtlasSprite))]
public class SBGUILabel : SBGUIImage
{
	public YGTextSprite textSprite
	{
		get
		{
			return (YGTextSprite)base.sprite;
		}
		set
		{
			base.sprite = value;
		}
	}

	public override int Width
	{
		get
		{
			return Mathf.CeilToInt(textSprite.textSize.x);
		}
	}

	public override int Height
	{
		get
		{
			return Mathf.CeilToInt(textSprite.textSize.y);
		}
	}

	public virtual string Text
	{
		get
		{
			return textSprite.Text;
		}
		set
		{
			if (textSprite != null)
			{
				textSprite.Text = value;
			}
		}
	}

	protected override void Awake()
	{
		if (textSprite.textScale != 0f)
		{
			textSprite.SetScale(new Vector2(textSprite.textScale, textSprite.textScale));
		}
		if (Language.CurrentLanguage() == LanguageCode.RU)
		{
			SwapFont("cyrillic", textSprite);
		}
		base.Awake();
	}

	public static SBGUILabel Create(SBGUIElement parent, float x, float y, float w, float h, string text)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUILabel_{0}", SBGUIElement.InstanceID));
		Rect rect = new Rect(x, y, w, h);
		SBGUILabel sBGUILabel = gameObject.AddComponent<SBGUILabel>();
		sBGUILabel.Initialize(parent, rect, text);
		return sBGUILabel;
	}

	protected override void Initialize(SBGUIElement parent, Rect rect, string text)
	{
		SetTransformParent(parent);
		textSprite = base.gameObject.AddComponent<YGTextSprite>();
		textSprite.SetPosition((int)rect.x, (int)rect.y);
		textSprite.Text = text;
	}

	protected virtual void SwapFont(string desiredFontName, YGTextSprite textSprite)
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

	public virtual bool SetText(string s)
	{
		if (textSprite == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(s))
		{
			s = string.Empty;
		}
		if (textSprite.Text != s)
		{
			textSprite.Text = s;
			if (textSprite.textScale != 0f)
			{
				textSprite.SetScale(new Vector2(textSprite.textScale, textSprite.textScale));
			}
			return true;
		}
		return false;
	}

	public void AdjustText(SBGUIAtlasImage boundary)
	{
		string text = Language.Get(Text);
		SetText(string.Empty);
		int num = 0;
		while (num < text.Length)
		{
			Text += text[num++];
			if (!((float)Width > boundary.Size.x))
			{
				continue;
			}
			string text2 = Text;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				if (text2[num2] == ' ')
				{
					text = text.Remove(num2, 1);
					text = text.Insert(num2, "|");
					SetText(text.Substring(0, num));
					break;
				}
			}
		}
	}
}
