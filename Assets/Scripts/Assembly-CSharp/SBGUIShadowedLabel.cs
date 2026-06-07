public class SBGUIShadowedLabel : SBGUILabel
{
	public SBGUILabel Shadow;

	public override string Text
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public override bool SetText(string s)
	{
		return false;
	}

	protected override void Awake()
	{
	}

	protected override void SwapFont(string desiredFontName, YGTextSprite textSprite)
	{
	}
}
