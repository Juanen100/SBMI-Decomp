using System.Collections.Generic;

public class SBGUITabBar : SBGUIElement
{
	public SBGUIScrollRegion scrollRegion;

	public string onTexture;

	public string offTexture;

	private SBGUITabButton selected;

	private SBGUITabButton[] buttons;

	private YGTextureLibrary.FoundMaterial foundOnMat;

	private YGTextureLibrary.FoundMaterial foundOffMat;

	public EventDispatcher<SBGUITabButton> TabChangeEvent;

	public void SetupCategories(Dictionary<string, SBTabCategory> categories, Session session)
	{
	}

	protected override void OnEnable()
	{
	}

	private void Start()
	{
	}

	public void TabClick(int index)
	{
	}

	public void TabClick(SBGUITabButton button)
	{
	}

	public SBGUITabButton FindButton(string name, bool includeInactive)
	{
		return null;
	}
}
