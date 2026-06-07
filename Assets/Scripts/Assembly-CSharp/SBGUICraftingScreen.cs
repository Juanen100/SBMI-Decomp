using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUICraftingScreen : SBGUISlottedScrollableDialog
{
	public GameObject rowPrefab;

	public EventDispatcher<CraftingRecipe> MakeRecipeClickedEvent;

	public SBGUICraftingSlot selectedSlot;

	public SBGUIAtlasButton closeButton;

	private static TFPool<SBGUIProductionSlot> prodSlotPool;

	private SBGUICraftingRecipeDialog recipeDialog;

	private List<ProductionSlotShell> productionSlotShells;

	private Action<int> rushHandler;

	private Action<int> watchADHandler;

	private CraftingRecipe highlightedRecipe;

	private int highlightedSlot;

	private int currentCookbook;

	private SBGUILabel makeButtonLabel;

	private Dictionary<int, float> lastSelectedByCookbook;

	private SBGUICharacterArrowList m_pTaskCharacterList;

	private const int NUM_ROWS = 2;

	private const int MAX_SLOTS = 7;

	private const float SLOT_DISPLACEMENT = 0.955f;

	private const float GAP_SIZE = 0.06f;

	private bool isAdAvailable;

	public void Setup(Session session, CraftingCookbook cookbook, Action<int> rushHandler, int productionSlots, Action<int> watchADHandler)
	{
	}

	public void CreateNonScrollUI(List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
	}

	public void CreateUI(CraftingCookbook cookbook, CraftingRecipe highlightedRecipe, int unlockedSlots, int maxSlots, Action<CraftingRecipe> setSelected)
	{
	}

	public void HighlightSlot(Session session, CraftingRecipe recipe)
	{
	}

	protected override void OnSlotsVisible()
	{
	}

	protected override int GetSlotIndex(Vector2 pos)
	{
		return 0;
	}

	protected override Vector2 GetSlotOffset(int index)
	{
		return default(Vector2);
	}

	protected override Vector2 GetSlotSize()
	{
		return default(Vector2);
	}

	private void LoadRecipes(List<int> recipes, Session session, CraftingCookbook cookbook, SBGUIElement anchor, Action<CraftingRecipe> setSelected)
	{
	}

	public override void Deactivate()
	{
	}

	public void ForceCycleProdSlots()
	{
	}

	public override void Update()
	{
	}

	public void UpdateResources(Session session)
	{
	}

	public Vector2 GetHardSpendButtonPositionForSlot(int slotId)
	{
		return default(Vector2);
	}

	private void UpdateProductionSlots()
	{
	}

	protected override SBGUIScrollListElement MakeSlot()
	{
		return null;
	}

	private void SetupProductionSlots(Session session, int availableSlots, int unlockableSlots)
	{
	}

	public override void OnDestroy()
	{
	}

	private Action<SBGUIScrollListElement> SetupSlotClosure(Session session, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, int slotId, Action<CraftingRecipe> setSelected)
	{
		return null;
	}

	public Vector2 GetHardSpendPosition()
	{
		return default(Vector2);
	}
}
