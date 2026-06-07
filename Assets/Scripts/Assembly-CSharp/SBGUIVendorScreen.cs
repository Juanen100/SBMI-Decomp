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

	private List<SBGUIVendorSlot> slotRefs;

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
	}

	public void CreateNonScrollUI(List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
	}

	private void CreateVendingInstanceSlots(Session session)
	{
	}

	public void UpdateVendingInstanceSlots(Session session)
	{
	}

	public void HighlightSlot(Session session, SBGUIVendorSlot slot)
	{
	}

	public void UpdateItemDescription(Session session, VendorStock stock, VendingInstance instance)
	{
	}

	public Vector2 GetRestockRushPosition()
	{
		return default(Vector2);
	}

	public Vector2 GetBuyButtonPosition()
	{
		return default(Vector2);
	}

	public override void Update()
	{
	}

	public void ClearVendingSlots()
	{
	}

	public override void Deactivate()
	{
	}
}
