using UnityEngine;

public class SBGUILabel : SBGUIImage
{
	public YGTextSprite textSprite
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public override int Width
	{
		get
		{
			return 0;
		}
	}

	public override int Height
	{
		get
		{
			return 0;
		}
	}

	public virtual string Text
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	protected override void Awake()
	{
	}

	public static SBGUILabel Create(SBGUIElement parent, float x, float y, float w, float h, string text)
	{
		return null;
	}

	protected override void Initialize(SBGUIElement parent, Rect rect, string text)
	{
	}

	protected virtual void SwapFont(string desiredFontName, YGTextSprite textSprite)
	{
	}

	public virtual bool SetText(string s)
	{
		return false;
	}

	public void AdjustText(SBGUIAtlasImage boundary)
	{
	}
}
