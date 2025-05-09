#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SBGUICraftingScreen : SBGUISlottedScrollableDialog
{
	private const int NUM_ROWS = 2;

	private const int MAX_SLOTS = 7;

	private const float SLOT_DISPLACEMENT = 0.955f;

	private const float GAP_SIZE = 0.06f;

	public GameObject rowPrefab;

	public EventDispatcher<CraftingRecipe> MakeRecipeClickedEvent = new EventDispatcher<CraftingRecipe>();

	public SBGUICraftingSlot selectedSlot;

	public SBGUIAtlasButton closeButton;

	private static TFPool<SBGUIProductionSlot> prodSlotPool = new TFPool<SBGUIProductionSlot>();

	private SBGUICraftingRecipeDialog recipeDialog;

	private List<ProductionSlotShell> productionSlotShells;

	private Action<int> rushHandler;

	private CraftingRecipe highlightedRecipe;

	private int highlightedSlot;

	private int currentCookbook;

	private SBGUILabel makeButtonLabel;

	private Dictionary<int, float> lastSelectedByCookbook = new Dictionary<int, float>();

	private SBGUICharacterArrowList m_pTaskCharacterList;

	public void Setup(Session session, CraftingCookbook cookbook, Action<int> rushHandler, int productionSlots)
	{
		TFUtils.Assert(session != null, "There should be a session. Found null.");
		base.session = session;
		SessionActionId = cookbook.sessionActionId;
		this.rushHandler = rushHandler;
		closeButton = FindChild("close").GetComponent<SBGUIAtlasButton>();
		TFUtils.Assert(closeButton != null, "Could not find child closeButton button on crafting screen!");
		closeButton.SessionActionId = cookbook.sessionActionId + "_Close_Button";
		recipeDialog = (SBGUICraftingRecipeDialog)FindChild("recipe_dialog");
		TFUtils.Assert(recipeDialog != null, "Couldn't find recipe_dialog");
		recipeDialog.Init();
		makeButtonLabel = (SBGUILabel)FindChild("button_label");
		m_pTaskCharacterList = (SBGUICharacterArrowList)FindChild("character_portrait_parent");
	}

	public void CreateNonScrollUI(List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		List<SBGUIArrowList.ListItemData> list = new List<SBGUIArrowList.ListItemData>();
		int count = pTaskCharacterDIDs.Count;
		List<int> list2 = new List<int>();
		if (count <= 0)
		{
			m_pTaskCharacterList.SetActive(false);
			return;
		}
		m_pTaskCharacterList.SetActive(true);
		for (int i = 0; i < count; i++)
		{
			Simulated simulated = session.TheGame.simulation.FindSimulated(pTaskCharacterDIDs[i]);
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			list.Add(new SBGUIArrowList.ListItemData(entity.DefinitionId, entity.QuestReminderIcon));
			List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(entity.DefinitionId, null);
			if (activeTasksForSimulated != null && activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].m_bMovingToTarget)
			{
				list2.Add(entity.DefinitionId);
			}
		}
		m_pTaskCharacterList.SetData(session, list, (list.Count > 0) ? list[0].m_nID : 0, list2, null, pTaskCharacterClicked);
	}

	public void CreateUI(CraftingCookbook cookbook, CraftingRecipe highlightedRecipe, int unlockedSlots, int maxSlots, Action<CraftingRecipe> setSelected)
	{
		PreLoadRegionContentInfo();
		SessionActionId = cookbook.sessionActionId;
		closeButton.SessionActionId = cookbook.sessionActionId + "_Close_Button";
		SetupProductionSlots(session, unlockedSlots, maxSlots);
		List<int> unsortedList = session.TheGame.craftManager.UnlockedRecipesCopy.Intersect(cookbook.GetRecipes()).ToList();
		unsortedList = session.TheGame.resourceManager.SortRecipesByProductGroup(session.TheGame.craftManager, unsortedList);
		Action<CraftingRecipe> value = delegate
		{
			UpdateProductionSlots();
		};
		MakeRecipeClickedEvent.ClearListeners();
		MakeRecipeClickedEvent.AddListener(value);
		UpdateProductionSlots();
		if (this.highlightedRecipe != highlightedRecipe)
		{
			this.highlightedRecipe = highlightedRecipe;
		}
		currentCookbook = cookbook.identity;
		LoadRecipes(unsortedList, session, cookbook, region.Marker, setSelected);
	}

	public void HighlightSlot(Session session, CraftingRecipe recipe)
	{
		highlightedRecipe = recipe;
		if (recipe == null)
		{
			recipeDialog.Deselect();
			return;
		}
		SBGUICraftingSlot sBGUICraftingSlot = (SBGUICraftingSlot)region.subViewMarker.FindChild(SBGUICraftingSlot.GetSessionActionId(recipe));
		if (selectedSlot != null)
		{
			selectedSlot.SetHighlight(false);
		}
		selectedSlot = sBGUICraftingSlot;
		if (selectedSlot != null)
		{
			selectedSlot.SetHighlight(true);
			lastSelectedByCookbook[currentCookbook] = selectedSlot.transform.localPosition.x;
		}
		recipeDialog.Setup(recipe, session.TheGame.resourceManager);
	}

	protected override void OnSlotsVisible()
	{
		HighlightSlot(session, highlightedRecipe);
		base.OnSlotsVisible();
	}

	protected override int GetSlotIndex(Vector2 pos)
	{
		float num = GetSlotSize().x + 0.06f;
		float num2 = 0f - num;
		int num3 = Mathf.FloorToInt(pos.x * 2f / num + num2);
		if (pos.y > 0f)
		{
			num3++;
		}
		return num3;
	}

	protected override Vector2 GetSlotOffset(int index)
	{
		Vector2 slotSize = GetSlotSize();
		return new Vector2(slotSize.x * ((float)(index / 2) + 0.06f), -1f * (slotSize.y * (float)(index % 2)));
	}

	protected override Vector2 GetSlotSize()
	{
		SBGUICraftingSlot component = rowPrefab.GetComponent<SBGUICraftingSlot>();
		SBGUIAtlasButton sBGUIAtlasButton = (SBGUIAtlasButton)component.FindChild("slot_background");
		return sBGUIAtlasButton.Size * 0.01f;
	}

	private void LoadRecipes(List<int> recipes, Session session, CraftingCookbook cookbook, SBGUIElement anchor, Action<CraftingRecipe> setSelected)
	{
		int num = 0;
		region.SetupSlotActions.Clear();
		CraftingRecipe craftingRecipe = null;
		List<int> list = new List<int>();
		foreach (int recipe in recipes)
		{
			if (list.Contains(recipe))
			{
				continue;
			}
			list.Add(recipe);
			craftingRecipe = session.TheGame.craftManager.GetRecipeById(recipe);
			if (craftingRecipe == null)
			{
				TFUtils.WarningLog("null CraftingRecipe");
				continue;
			}
			Action<CraftingRecipe> setSelected2 = delegate(CraftingRecipe recipeToSelect)
			{
				setSelected(recipeToSelect);
			};
			region.SetupSlotActions.Insert(num, SetupSlotClosure(session, anchor, cookbook, craftingRecipe, GetSlotOffset(num), num, setSelected2));
			string sessionActionId = SBGUICraftingSlot.GetSessionActionId(craftingRecipe);
			if (sessionActionIdSearchRequests.Contains(sessionActionId))
			{
				sessionActionSlotMap[sessionActionId] = num;
			}
			num++;
		}
		Vector3 scrollPos = Vector3.zero;
		if (lastSelectedByCookbook.ContainsKey(cookbook.identity))
		{
			float num2 = lastSelectedByCookbook[cookbook.identity];
			float num3 = num2 - region.InitialMarkerPos.x;
			scrollPos = new Vector3(0f - num3, region.InitialMarkerPos.y, region.InitialMarkerPos.z);
		}
		PostLoadRegionContentInfo(region.SetupSlotActions.Count, scrollPos);
	}

	public override void Deactivate()
	{
		ClearButtonActions("accept_button");
		prodSlotPool.Clear(delegate(SBGUIProductionSlot slot)
		{
			slot.Deactivate();
		});
		productionSlotShells = null;
		selectedSlot = null;
		recipeDialog.Deactivate();
		MakeRecipeClickedEvent.ClearListeners();
		base.Deactivate();
	}

	public void ForceCycleProdSlots()
	{
		prodSlotPool.Clear(delegate(SBGUIProductionSlot slot)
		{
			slot.Deactivate();
		});
		productionSlotShells = null;
	}

	public override void Update()
	{
		base.Update();
		UpdateProductionSlots();
	}

	public void UpdateResources(Session session)
	{
		if (selectedSlot != null && recipeDialog != null)
		{
			recipeDialog.Setup(selectedSlot.recipe, session.TheGame.resourceManager);
		}
		UpdateProductionSlots();
	}

	public Vector2 GetHardSpendButtonPositionForSlot(int slotId)
	{
		return productionSlotShells.Find((ProductionSlotShell shell) => shell.SlotId == slotId).Position;
	}

	private void UpdateProductionSlots()
	{
		if (session == null || session.TheGame == null || productionSlotShells == null)
		{
			return;
		}
		TFUtils.Assert(session.TheGame.selected != null, "There should be a selected entity. Found null.");
		BuildingEntity entity = session.TheGame.selected.GetEntity<BuildingEntity>();
		TFUtils.Assert(entity.CanCraft, "Should not be viewing production slots on a non-craftable building");
		if (entity.ShuntsCrafting)
		{
			List<Entity> annexes = entity.Annexes;
			int i;
			for (i = 0; i < annexes.Count; i++)
			{
				productionSlotShells[i].UpdateInfo(entity, i, rushHandler, session.TheGame);
			}
			for (; i < productionSlotShells.Count; i++)
			{
				productionSlotShells[i].UpdateInfo(null, 0, null, null);
			}
		}
		else
		{
			for (int j = 0; j < productionSlotShells.Count; j++)
			{
				productionSlotShells[j].UpdateInfo(entity, j, rushHandler, session.TheGame);
			}
		}
	}

	protected override SBGUIScrollListElement MakeSlot()
	{
		return SBGUICraftingSlot.MakeCraftingSlot();
	}

	private void SetupProductionSlots(Session session, int availableSlots, int unlockableSlots)
	{
		SBGUIElement sBGUIElement = FindChild("production_slots");
		SBGUIElement parent = sBGUIElement.FindChild("anchor");
		int definitionId = session.TheGame.selected.entity.DefinitionId;
		productionSlotShells = new List<ProductionSlotShell>(Math.Min(availableSlots, unlockableSlots));
		TFUtils.Assert(availableSlots <= 7, "We do not have screen space for more than " + 7 + " crafting production slots");
		float num = 0f;
		for (int i = 0; i < 7; i++)
		{
			SBGUIProductionSlot sBGUIProductionSlot = prodSlotPool.Create(SBGUIProductionSlot.Create);
			sBGUIProductionSlot.SetParent(parent);
			ProductionSlotShell item = ((i >= availableSlots) ? ((i >= unlockableSlots) ? ((ProductionSlotShell)new ProductionSlotShellUnavailable(sBGUIProductionSlot, i)) : ((ProductionSlotShell)new ProductionSlotShellLocked(sBGUIProductionSlot, session.TheGame.craftManager.GetSlotExpandCost(definitionId, i), i, session.TheGame))) : new ProductionSlotShellAvailable(sBGUIProductionSlot, i));
			productionSlotShells.Add(item);
			sBGUIProductionSlot.transform.localPosition += new Vector3(num, 0f, 0f);
			num += 0.955f;
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	private Action<SBGUIScrollListElement> SetupSlotClosure(Session session, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, int slotId, Action<CraftingRecipe> setSelected)
	{
		Action action = delegate
		{
			setSelected(recipe);
		};
		return delegate(SBGUIScrollListElement slot)
		{
			((SBGUICraftingSlot)slot).Setup(session, this, anchor, cookbook, recipe, offset, action);
		};
	}

	public Vector2 GetHardSpendPosition()
	{
		Vector2 result = base.View.WorldToScreen(makeButtonLabel.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}
}
