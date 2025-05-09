#define ASSERTS_ON
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SBGUITabbedScrollableDialog : SBGUISlottedScrollableDialog
{
	public EventDispatcher<SBGUITabButton> TabClickedEvent = new EventDispatcher<SBGUITabButton>();

	protected Dictionary<string, SBTabCategory> categories;

	protected Dictionary<string, SBGUIElement> tabContents = new Dictionary<string, SBGUIElement>();

	protected SBGUIElement currentTab;

	private bool firstTabBuilt;

	public override void SetManagers(Session session)
	{
		base.SetManagers(session);
		LoadCategories(session);
		SetupTabCategories();
	}

	public void SetupTabCategories()
	{
		SBGUITabBar sBGUITabBar = (SBGUITabBar)FindChild("tabbar");
		sBGUITabBar.TabChangeEvent.AddListener(BuildTabForButton);
		sBGUITabBar.SetupCategories(categories, session);
	}

	protected abstract void LoadCategories(Session session);

	public void ViewTab(string tabName)
	{
		SBGUITabButton sBGUITabButton = (SBGUITabButton)FindChild(string.Format("tab_{0}", tabName));
		SBGUITabBar sBGUITabBar = (SBGUITabBar)FindChild("tabbar");
		sBGUITabBar.TabClick(sBGUITabButton);
		BuildTabForButton(sBGUITabButton);
	}

	public void ViewCurrentTab()
	{
		if (currentTab != null)
		{
			ViewTab(currentTab.name);
		}
	}

	protected abstract Rect CalculateTabContentsSize(string tabName);

	protected override int GetSlotIndex(Vector2 pos)
	{
		return Mathf.FloorToInt(pos.x / GetSlotSize().x);
	}

	protected override Vector2 GetSlotOffset(int index)
	{
		return new Vector2(GetSlotSize().x * (float)index, 0f);
	}

	protected virtual void BuildTabForButton(SBGUITabButton tab)
	{
		TabClickedEvent.FireEvent(tab);
		string tabName = tab.category.Name;
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("name_label");
		TFUtils.Assert(tab.category.Label != null, "Tab does not contain a 'label' in catalog.json");
		sBGUILabel.SetText(Language.Get(tab.category.Label));
		if (firstTabBuilt)
		{
			soundEffectMgr.PlaySound("SwitchTab");
		}
		if (currentTab != null)
		{
			ClearCachedSlotInfos();
			region.ClearSlotActions();
			currentTab = null;
		}
		BuildTab(tabName);
	}

	private void BuildTab(string tabName)
	{
		ClearCachedSlotInfos();
		PreLoadRegionContentInfo();
		StartCoroutine(BuildTabCoroutine(tabName));
	}

	protected abstract IEnumerator BuildTabCoroutine(string tabName);

	public override SBGUIElement FindDynamicSubElementSessionActionId(string sessionActionId, bool includeInactive)
	{
		SBGUITabBar sBGUITabBar = (SBGUITabBar)FindChild("tabbar");
		SBGUITabButton sBGUITabButton = sBGUITabBar.FindButton(sessionActionId, includeInactive);
		TFUtils.Assert(sBGUITabButton != null, string.Format("Trying to find dynamic sub element({0}), but that element does not exist on this element({1}). Could not find child named({2}) on tabbar({3}).", sessionActionId, ToString(), sessionActionId, sBGUITabBar.ToString()));
		return sBGUITabButton;
	}

	public override void Deactivate()
	{
		TabClickedEvent.ClearListeners();
		base.Deactivate();
	}
}
