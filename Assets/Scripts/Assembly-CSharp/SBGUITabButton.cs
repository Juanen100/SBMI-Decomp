using UnityEngine;

public class SBGUITabButton : SBGUIAtlasButton
{
	private const int MAX_TAB_ICON_SIZE = 58;

	private const int GAP_SIZE = -1;

	public int tabIndex = -1;

	public SBGUITabBar parentBar;

	public SBTabCategory category;

	public virtual void Selected(bool selected)
	{
	}

	private void SetupCategory(SBTabCategory cat)
	{
		category = cat;
		SBGUIAtlasImage component = FindChild("icon").gameObject.GetComponent<SBGUIAtlasImage>();
		if (!string.IsNullOrEmpty(category.Texture))
		{
			component.SetTextureFromAtlas(category.Texture);
			component.ScaleToMaxSize(58);
		}
		base.ClickEvent += delegate
		{
			parentBar.TabClick(this);
		};
	}

	public static SBGUITabButton CreateTabButton(SBGUITabBar parent, SBTabCategory category, int index)
	{
		SBGUITabButton sBGUITabButton = (SBGUITabButton)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/MarketplaceTab");
		sBGUITabButton.name = string.Format("tab_{0}", category.Name);
		sBGUITabButton.SetParent(parent);
		sBGUITabButton.parentBar = parent;
		YGAtlasSprite component = sBGUITabButton.GetComponent<YGAtlasSprite>();
		float x = (component.size.x + -1f) * (float)index * 0.01f;
		sBGUITabButton.tform.localPosition = new Vector3(x, 0f, 0f);
		sBGUITabButton.SetupCategory(category);
		return sBGUITabButton;
	}
}
