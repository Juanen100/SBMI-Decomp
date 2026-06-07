using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SBGUITabbedScrollableDialog : SBGUISlottedScrollableDialog
{
	public EventDispatcher<SBGUITabButton> TabClickedEvent;

	protected Dictionary<string, SBTabCategory> categories;

	protected Dictionary<string, SBGUIElement> tabContents;

	protected SBGUIElement currentTab;

	private bool firstTabBuilt;

	public override void SetManagers(Session session)
	{
	}

	public void SetupTabCategories()
	{
	}

	protected abstract void LoadCategories(Session session);

	public void ViewTab(string tabName)
	{
	}

	public void ViewCurrentTab()
	{
	}

	protected abstract Rect CalculateTabContentsSize(string tabName);

	protected override int GetSlotIndex(Vector2 pos)
	{
		return 0;
	}

	protected override Vector2 GetSlotOffset(int index)
	{
		return default(Vector2);
	}

	protected virtual void BuildTabForButton(SBGUITabButton tab)
	{
	}

	private void BuildTab(string tabName)
	{
	}

	protected abstract IEnumerator BuildTabCoroutine(string tabName);

	public override SBGUIElement FindDynamicSubElementSessionActionId(string sessionActionId, bool includeInactive)
	{
		return null;
	}

	public override void Deactivate()
	{
	}
}
