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
		SBGUICraftingSlot sBGUICraftingSlot = (SBGUICraftingSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftingSlot");
		sBGUICraftingSlot.checkMark = (SBGUIAtlasImage)sBGUICraftingSlot.FindChild("checkmark");
		sBGUICraftingSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sBGUICraftingSlot;
	}

	public static SBGUICraftingSlot Create(Session session, SBGUICraftingScreen craftingScreen, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, Action setSelected)
	{
		SBGUICraftingSlot sBGUICraftingSlot = (SBGUICraftingSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftingSlot");
		sBGUICraftingSlot.checkMark = (SBGUIAtlasImage)sBGUICraftingSlot.FindChild("checkmark");
		sBGUICraftingSlot.Setup(session, craftingScreen, anchor, cookbook, recipe, offset, setSelected);
		return sBGUICraftingSlot;
	}

	public void Setup(Session session, SBGUICraftingScreen craftingScreen, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, Action setSelected)
	{
		this.recipe = recipe;
		this.craftingScreen = craftingScreen;
		base.name = GetSessionActionId(recipe);
		SetParent(anchor);
		SetActive(true);
		base.transform.localPosition = offset;
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("icon");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("name_label");
		numberOfProduct = (SBGUILabel)FindChild("number_label");
		SBGUIAtlasButton sBGUIAtlasButton = (SBGUIAtlasButton)FindChild("slot_background");
		sBGUIAtlasButton.SessionActionId = GetSessionActionId(recipe);
		sBGUIAtlasButton.SetTextureFromAtlas(cookbook.recipeSlotTexture);
		string text = Language.Get(recipe.recipeName);
		string text2 = text;
		if (text2.Length > 12)
		{
			text2 = text2.Substring(0, 9);
			text2 += "...";
		}
		sBGUILabel.SetText(text2);
		resourceManager = session.TheGame.resourceManager;
		numberOfProduct.SetText(resourceManager.Query(recipe.productId).ToString());
		sBGUIAtlasImage.SetTextureFromAtlas(resourceManager.Resources[recipe.productId].GetResourceTexture());
		SetHighlight(false);
		AttachActionToButton("slot_background", delegate
		{
			session.TheSoundEffectManager.PlaySound("HighlightItem");
			this.craftingScreen.HighlightSlot(session, recipe);
			setSelected();
		});
		sBGUIAtlasButton.UpdateCollider();
	}

	public void SetHighlight(bool highlight)
	{
		checkMark.SetActive(highlight);
	}

	public override void Deactivate()
	{
		ClearButtonActions("slot_background");
		base.Deactivate();
	}

	public static string GetSessionActionId(CraftingRecipe recipe)
	{
		return string.Format("Slot_{0}", recipe.productId);
	}

	public void Update()
	{
		if (this != null && numberOfProduct != null && recipe != null && resourceManager != null)
		{
			numberOfProduct.SetText(resourceManager.Query(recipe.productId).ToString());
		}
	}
}
