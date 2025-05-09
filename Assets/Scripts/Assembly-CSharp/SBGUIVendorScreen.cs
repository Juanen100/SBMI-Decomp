using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIVendorScreen : SBGUIScreen
{
	private const float SLOT_GAP = 8f;

	private const int SLOT_ROW_NUMBER_MAX = 4;

	private const int MAX_VENDOR_SLOTS = 12;

	private const float CURRENCY_ICON_GAP = 5f;

	public int lastSelectedSlotID;

	public SBGUIVendorSlot lastSelectedSlot;

	private SBGUIButton skipButton;

	private SBGUIElement slotMarker;

	private List<SBGUIVendorSlot> slotRefs = new List<SBGUIVendorSlot>();

	private SBGUILabel itemDescription;

	private SBGUILabel itemName;

	private SBGUILabel itemCost;

	private SBGUILabel stockLabel;

	private SBGUILabel restockTimer;

	private SBGUIAtlasImage itemIcon;

	private SBGUIAtlasImage itemCostIcon;

	private SBGUIButton buyButton;

	private SBGUIElement descriptionGroup;

	private int? descriptionIconSize;

	private int? slotIconSize;

	private int? specialSlotIconSize;

	private int? itemCostIconSize;

	private VendingDecorator vendingEntity;

	private SBGUICharacterArrowList m_pTaskCharacterList;

	public void Setup(Session session, VendorDefinition vendorDef)
	{
		base.session = session;
		Simulated selected = session.TheGame.selected;
		vendingEntity = selected.GetEntity<VendingDecorator>();
		itemDescription = (SBGUILabel)FindChild("item_description_label");
		itemName = (SBGUILabel)FindChild("item_name_label");
		itemCost = (SBGUILabel)FindChild("item_cost_label");
		itemCostIcon = (SBGUIAtlasImage)FindChild("item_cost_icon");
		itemIcon = (SBGUIAtlasImage)FindChild("item_icon");
		descriptionGroup = FindChild("description_group");
		skipButton = (SBGUIButton)FindChild("skip_button");
		restockTimer = (SBGUILabel)FindChild("restock_timer_label");
		stockLabel = (SBGUILabel)FindChild("stock_label");
		slotMarker = FindChild("slot_marker");
		buyButton = (SBGUIButton)FindChild("buy_button");
		m_pTaskCharacterList = (SBGUICharacterArrowList)FindChild("character_portrait_parent");
		int? num = descriptionIconSize;
		if (!num.HasValue)
		{
			descriptionIconSize = (int)itemIcon.Size.x;
		}
		int? num2 = itemCostIconSize;
		if (!num2.HasValue)
		{
			itemCostIconSize = (int)itemCostIcon.Size.x;
		}
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("window");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("buy_tab");
		SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)FindChild("_inset");
		List<int> backgroundColor = vendorDef.backgroundColor;
		if (vendorDef.backgroundColor != null)
		{
			sBGUIAtlasImage.SetColor(new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f));
			sBGUIAtlasImage2.SetColor(new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f));
			sBGUIAtlasImage3.SetColor(new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f));
		}
		else
		{
			TFUtils.WarningLog("VendorDefinition " + vendorDef.did + " does not have a background.color defined");
		}
		SBGUIAtlasImage sBGUIAtlasImage4 = (SBGUIAtlasImage)FindChild("title");
		sBGUIAtlasImage4.SetTextureFromAtlas(vendorDef.titleTexture);
		SBGUIAtlasImage sBGUIAtlasImage5 = (SBGUIAtlasImage)FindChild("title_icon");
		sBGUIAtlasImage5.SetTextureFromAtlas(vendorDef.titleIconTexture);
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("buy_label");
		sBGUILabel.SetText(Language.Get(vendorDef.buttonLabel));
		SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)FindChild("close");
		sBGUIPulseButton.SetTextureFromAtlas(vendorDef.cancelButtonTexture);
		CreateVendingInstanceSlots(session);
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

	private void CreateVendingInstanceSlots(Session session)
	{
		ClearVendingSlots();
		Vector3 zero = Vector3.zero;
		int num = 1;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < 12; i++)
		{
			SBGUIVendorSlot sBGUIVendorSlot = SBGUIVendorSlot.CreateVendorSlot(session, this);
			sBGUIVendorSlot.SlotID = i;
			slotRefs.Add(sBGUIVendorSlot);
			sBGUIVendorSlot.SetParent(slotMarker);
			sBGUIVendorSlot.tform.localPosition = Vector3.zero;
			int? num4 = slotIconSize;
			if (!num4.HasValue)
			{
				slotIconSize = (int)sBGUIVendorSlot.itemIcon.Size.x;
			}
			if (num > 4)
			{
				num2 -= (sBGUIVendorSlot.slotBackground.Size.y + 8f) * 0.01f;
				num3 = 0f;
				num = 1;
			}
			zero = new Vector3(num3, num2, 0f);
			sBGUIVendorSlot.tform.localPosition += zero;
			num3 += (sBGUIVendorSlot.slotBackground.Size.x + 8f) * 0.01f;
			num++;
		}
		SBGUIVendorSlot specialSlot = (SBGUIVendorSlot)FindChild("iod_slot");
		specialSlot.IsSpecial = true;
		specialSlot.SlotID = slotRefs.Count;
		specialSlot.AttachActionToButton("slot_background", delegate
		{
			session.TheSoundEffectManager.PlaySound("HighlightItem");
			HighlightSlot(session, specialSlot);
		});
		int? num5 = specialSlotIconSize;
		if (!num5.HasValue)
		{
			specialSlotIconSize = (int)specialSlot.itemIcon.Size.x;
		}
		slotRefs.Add(specialSlot);
	}

	public void UpdateVendingInstanceSlots(Session session)
	{
		Dictionary<int, VendingInstance> vendingInstances = session.TheGame.vendingManager.GetVendingInstances(vendingEntity.Id);
		VendingInstance specialInstance = session.TheGame.vendingManager.GetSpecialInstance(vendingEntity.Id);
		foreach (SBGUIVendorSlot slotRef in slotRefs)
		{
			VendingInstance value;
			vendingInstances.TryGetValue(slotRef.SlotID, out value);
			if (value == null)
			{
				value = specialInstance;
			}
			if (value == null)
			{
				slotRef.SetEmpty(true);
				continue;
			}
			VendorStock stock = session.TheGame.vendingManager.GetStock(value.StockId);
			if (slotRef.quantityLabel != null)
			{
				if (value.remaining >= 2)
				{
					slotRef.quantityCircle.SetActive(true);
					slotRef.quantityLabel.SetText(value.remaining.ToString());
				}
				else
				{
					slotRef.quantityCircle.SetActive(false);
				}
			}
			if (value.remaining <= 0)
			{
				if (value == specialInstance)
				{
					slotRef.SetEmpty(true, true);
				}
				else
				{
					slotRef.SetEmpty(true);
				}
			}
			else if (value == specialInstance)
			{
				slotRef.SetEmpty(false, true);
			}
			else
			{
				slotRef.SetEmpty(false);
			}
			if ((slotRef.SlotID == lastSelectedSlotID || slotRef == lastSelectedSlot) && !slotRef.Empty)
			{
				slotRef.SetHighlight(true);
				lastSelectedSlot = slotRef;
				lastSelectedSlotID = slotRef.SlotID;
			}
			else
			{
				slotRef.SetHighlight(false, true);
			}
			slotRef.itemIcon.SetTextureFromAtlas(stock.Icon);
			if (slotRef == slotRefs[slotRefs.Count - 1])
			{
				SBGUIAtlasImage sBGUIAtlasImage = slotRef.itemIcon;
				int? num = specialSlotIconSize;
				sBGUIAtlasImage.ScaleToMaxSize(num.Value);
			}
			else
			{
				SBGUIAtlasImage sBGUIAtlasImage2 = slotRef.itemIcon;
				int? num2 = slotIconSize;
				sBGUIAtlasImage2.ScaleToMaxSize(num2.Value);
			}
		}
		if (lastSelectedSlot != null)
		{
			VendingInstance value2;
			vendingInstances.TryGetValue(lastSelectedSlot.SlotID, out value2);
			if (value2 == null)
			{
				value2 = specialInstance;
			}
			VendorStock stock2 = session.TheGame.vendingManager.GetStock(value2.StockId);
			UpdateItemDescription(session, stock2, value2);
		}
		else
		{
			descriptionGroup.SetActive(false);
		}
	}

	public void HighlightSlot(Session session, SBGUIVendorSlot slot)
	{
		if (lastSelectedSlotID != slot.SlotID || !(lastSelectedSlot != null) || !(lastSelectedSlot == slot))
		{
			if (lastSelectedSlot != null)
			{
				lastSelectedSlot.SetHighlight(false);
			}
			lastSelectedSlot = slot;
			lastSelectedSlotID = slot.SlotID;
			if (lastSelectedSlot != null)
			{
				lastSelectedSlot.SetHighlight(true);
			}
			VendingInstance vendingInstance = session.TheGame.vendingManager.GetVendingInstance(vendingEntity.Id, slot.SlotID);
			if (vendingInstance == null)
			{
				vendingInstance = session.TheGame.vendingManager.GetSpecialInstance(vendingEntity.Id);
			}
			VendorStock stock = session.TheGame.vendingManager.GetStock(vendingInstance.StockId);
			UpdateItemDescription(session, stock, vendingInstance);
		}
	}

	public void UpdateItemDescription(Session session, VendorStock stock, VendingInstance instance)
	{
		if (!descriptionGroup.IsActive())
		{
			descriptionGroup.SetActive(true);
		}
		itemDescription.SetText(Language.Get(stock.Description));
		itemName.SetText(Language.Get(stock.Name));
		itemIcon.SetTextureFromAtlas(stock.Icon);
		SBGUIAtlasImage sBGUIAtlasImage = itemIcon;
		int? num = descriptionIconSize;
		sBGUIAtlasImage.ScaleToMaxSize(num.Value);
		stockLabel.SetText(string.Format(Language.Get("!!PREFAB_IN_STOCK"), instance.remaining));
		if (instance.Cost.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
		{
			int value = 0;
			instance.Cost.ResourceAmounts.TryGetValue(ResourceManager.SOFT_CURRENCY, out value);
			itemCost.SetText(value.ToString());
			itemCostIcon.SetTextureFromAtlas(session.TheGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
			SBGUIAtlasImage sBGUIAtlasImage2 = itemCostIcon;
			int? num2 = itemCostIconSize;
			sBGUIAtlasImage2.ScaleToMaxSize(num2.Value);
		}
		else if (instance.Cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
		{
			int value2 = 0;
			instance.Cost.ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value2);
			itemCost.SetText(value2.ToString());
			itemCostIcon.SetTextureFromAtlas(session.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture());
			SBGUIAtlasImage sBGUIAtlasImage3 = itemCostIcon;
			int? num3 = itemCostIconSize;
			sBGUIAtlasImage3.ScaleToMaxSize(num3.Value);
		}
		else
		{
			itemCost.SetText(Language.Get("!!PREFAB_FREE"));
			itemCostIcon.SetTextureFromAtlas("_blank_.png");
		}
		itemCostIcon.tform.localPosition = Vector3.zero + new Vector3((0f - ((float)itemCost.Width + 5f)) * 0.01f, 0f, 0f);
	}

	public Vector2 GetRestockRushPosition()
	{
		Vector2 result = base.View.WorldToScreen(skipButton.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	public Vector2 GetBuyButtonPosition()
	{
		Vector2 result = base.View.WorldToScreen(buyButton.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	public override void Update()
	{
		restockTimer.SetText(TFUtils.DurationToString(vendingEntity.RestockTime - TFUtils.EpochTime()));
		base.Update();
	}

	public void ClearVendingSlots()
	{
		if (slotRefs == null)
		{
			return;
		}
		int num = slotRefs.Count;
		for (int i = 0; i < num; i++)
		{
			SBGUIVendorSlot sBGUIVendorSlot = slotRefs[i];
			if (sBGUIVendorSlot.gameObject.name != "iod_slot")
			{
				if (sBGUIVendorSlot == lastSelectedSlot)
				{
					lastSelectedSlot = null;
				}
				UnityEngine.Object.Destroy(sBGUIVendorSlot.gameObject);
				slotRefs.RemoveAt(i);
				i--;
				num--;
			}
		}
	}

	public override void Deactivate()
	{
		ClearVendingSlots();
		base.Deactivate();
	}
}
