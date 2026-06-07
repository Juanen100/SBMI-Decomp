using System;
using UnityEngine;

public class SBGUICraftingSlot : SBGUIScrollListElement
{
	public const int GAP_SIZE = 6;

	public SBGUIAtlasImage checkMark;

	private SBGUICraftingScreen craftingScreen;

	private SBGUILabel numberOfProduct;

	private ResourceManager resourceManager;

	public CraftingRecipe recipe { get; protected set; }

	public static SBGUICraftingSlot MakeCraftingSlot()
	{
		return null;
	}

	public static SBGUICraftingSlot Create(Session session, SBGUICraftingScreen craftingScreen, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, Action setSelected)
	{
		return null;
	}

	public void Setup(Session session, SBGUICraftingScreen craftingScreen, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, Action setSelected)
	{
	}

	public void SetHighlight(bool highlight)
	{
	}

	public override void Deactivate()
	{
	}

	public static string GetSessionActionId(CraftingRecipe recipe)
	{
		return null;
	}

	public void Update()
	{
	}
}
