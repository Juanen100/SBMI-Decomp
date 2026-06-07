using UnityEngine;

public class SBGUIScrollBar : SBGUIElement
{
	public SBGUIScrollRegion.SCROLL_DIRECTION scrollDirection;

	public SBGUIImage scrollBar;

	public SBGUIImage thumb;

	public SBGUIScrollRegion region;

	protected override void OnEnable()
	{
	}

	public Rect GetWorldRect()
	{
		return default(Rect);
	}

	public void SetThumbSize(float percent)
	{
	}

	public void UpdateScroll(float thumbLoc)
	{
	}

	public void Reset()
	{
	}
}
