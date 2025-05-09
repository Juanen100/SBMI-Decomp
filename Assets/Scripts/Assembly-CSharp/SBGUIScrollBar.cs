#define ASSERTS_ON
using UnityEngine;

public class SBGUIScrollBar : SBGUIElement
{
	public SBGUIScrollRegion.SCROLL_DIRECTION scrollDirection = SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL;

	public SBGUIImage scrollBar;

	public SBGUIImage thumb;

	public SBGUIScrollRegion region;

	protected override void OnEnable()
	{
		TFUtils.Assert(region != null, "Scrollbar must have an associated SBGUIScrollRegion");
		base.OnEnable();
	}

	public Rect GetWorldRect()
	{
		return scrollBar.GetWorldRect();
	}

	public void SetThumbSize(float percent)
	{
		Vector2 size = thumb.Size;
		if (scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL)
		{
			size.x = scrollBar.Size.x * percent;
		}
		else if (scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.VERTICAL)
		{
			size.y = scrollBar.Size.y * percent;
		}
		thumb.Size = size;
	}

	public void UpdateScroll(float thumbLoc)
	{
		Vector3 position = thumb.tform.position;
		Rect worldRect = GetWorldRect();
		if (scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL)
		{
			position.x = worldRect.xMin + Mathf.Lerp(0f, worldRect.width, thumbLoc);
		}
		else if (scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.VERTICAL)
		{
			position.y = worldRect.yMax - Mathf.Lerp(0f, worldRect.height, thumbLoc);
		}
		thumb.tform.position = position;
	}

	public void Reset()
	{
		UpdateScroll(0f);
	}
}
