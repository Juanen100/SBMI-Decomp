using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

public class SBGUIInventoryWidgetTab : SBGUIButton
{
	public delegate SBGUIInventoryWidgetRow GetNewRow(SBGUIInventoryWidgetRow tabRow, int fromCookbookId, bool fromIsVendor);

	public delegate void CloseRows();

	public delegate bool OpenAllTabs();

	public int CookbookId;

	public bool isVendor;

	public SBGUIInventoryWidgetRow Row;

	public Mesh RowMesh;

	private List<int> productsMade;

	private GetNewRow nextRowDelegate;

	private CloseRows closeRowsCallback;

	private OpenAllTabs openAllTabsCallback;

	private Action<bool> openRows;

	private bool rowsVisible;

	public void Setup(Game game, CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback, GetNewRow nextRowDelegate, CloseRows closeRowsCallback, OpenAllTabs openAllTabsCallback)
	{
	}

	public bool ActivateTab(bool closeExisting)
	{
		return false;
	}

	public bool TryActivateTab(int cookbookId, bool isVendor, bool closeExisting)
	{
		return false;
	}

	public void UpdateRecipes(CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr)
	{
	}

	public void Close()
	{
	}

	private void Open(ResourceManager resourceMgr, CraftingManager craftMgr, SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback)
	{
	}

	private void InternalClose()
	{
	}

	public override void MockClick()
	{
	}
}
