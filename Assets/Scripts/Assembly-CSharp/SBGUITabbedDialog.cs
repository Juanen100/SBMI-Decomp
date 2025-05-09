#define ASSERTS_ON
using System.Collections;
using System.Collections.Generic;

public abstract class SBGUITabbedDialog : SBGUIScreen
{
	public EventDispatcher<SBGUITabButton> TabClickedEvent = new EventDispatcher<SBGUITabButton>();

	protected Dictionary<string, SBTabCategory> categories;

	protected Dictionary<string, SBGUIElement> tabContents = new Dictionary<string, SBGUIElement>();

	protected SBGUIElement currentTab;

	protected EntityManager entityMgr;

	protected ResourceManager resourceMgr;

	protected SoundEffectManager soundEffectMgr;

	protected bool mustWaitForInfoToLoad = true;

	private bool firstTabBuilt;

	public void SetManagers(Session inSession)
	{
		session = inSession;
		entityMgr = session.TheGame.entities;
		resourceMgr = session.TheGame.resourceManager;
		soundEffectMgr = session.TheSoundEffectManager;
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
			return;
		}
		SBGUITabBar sBGUITabBar = (SBGUITabBar)FindChild("tabbar");
		sBGUITabBar.TabClick(0);
	}

	protected virtual void BuildTabForButton(SBGUITabButton tab)
	{
		TabClickedEvent.FireEvent(tab);
		string tabName = tab.category.Name;
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("name_label");
		TFUtils.Assert(tab.category.Label != null, "Tab does not contain a 'label' in catalog.json");
		if (sBGUILabel != null)
		{
			sBGUILabel.SetText(Language.Get(tab.category.Label));
		}
		if (firstTabBuilt)
		{
			soundEffectMgr.PlaySound("SwitchTab");
		}
		if (currentTab != null)
		{
			currentTab.SetActive(false);
			currentTab = null;
		}
		BuildTab(tabName);
	}

	private void BuildTab(string tabName)
	{
		SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)FindChildSessionActionId("ActivityIndicator", false);
		if (sBGUIActivityIndicator != null)
		{
			sBGUIActivityIndicator.StartActivityIndicator();
		}
		mustWaitForInfoToLoad = true;
		StopAllCoroutines();
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
		if (currentTab != null)
		{
			currentTab.SetActive(false);
		}
		TabClickedEvent.ClearListeners();
		base.Deactivate();
	}
}
