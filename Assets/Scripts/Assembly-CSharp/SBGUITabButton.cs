public class SBGUITabButton : SBGUIAtlasButton
{
	private const int MAX_TAB_ICON_SIZE = 58;

	private const int GAP_SIZE = -1;

	public int tabIndex;

	public SBGUITabBar parentBar;

	public SBTabCategory category;

	public virtual void Selected(bool selected)
	{
	}

	private void SetupCategory(SBTabCategory cat)
	{
	}

	public static SBGUITabButton CreateTabButton(SBGUITabBar parent, SBTabCategory category, int index)
	{
		return null;
	}
}
