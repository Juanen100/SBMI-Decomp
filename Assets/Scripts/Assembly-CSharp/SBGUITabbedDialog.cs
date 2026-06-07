using System.Collections;
using System.Collections.Generic;

public abstract class SBGUITabbedDialog : SBGUIScreen
{
	public EventDispatcher<SBGUITabButton> TabClickedEvent;

	protected Dictionary<string, SBTabCategory> categories;

	protected Dictionary<string, SBGUIElement> tabContents;

	protected SBGUIElement currentTab;

	protected EntityManager entityMgr;

	protected ResourceManager resourceMgr;

	protected SoundEffectManager soundEffectMgr;

	protected bool mustWaitForInfoToLoad;

	private bool firstTabBuilt;

	public void SetManagers(Session inSession)
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
