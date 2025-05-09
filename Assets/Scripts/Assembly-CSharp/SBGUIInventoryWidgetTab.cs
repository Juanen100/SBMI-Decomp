#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarg;

[RequireComponent(typeof(TapButton))]
[RequireComponent(typeof(YG2DRectangle))]
public class SBGUIInventoryWidgetTab : SBGUIButton
{
	public delegate SBGUIInventoryWidgetRow GetNewRow(SBGUIInventoryWidgetRow tabRow, int fromCookbookId, bool fromIsVendor);

	public delegate void CloseRows();

	public delegate bool OpenAllTabs();

	public int CookbookId;

	public bool isVendor;

	public SBGUIInventoryWidgetRow Row;

	public Mesh RowMesh;

	private List<int> productsMade = new List<int>();

	private GetNewRow nextRowDelegate;

	private CloseRows closeRowsCallback;

	private OpenAllTabs openAllTabsCallback;

	private Action<bool> openRows;

	private bool rowsVisible;

	public void Setup(Game game, CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback, GetNewRow nextRowDelegate, CloseRows closeRowsCallback, OpenAllTabs openAllTabsCallback)
	{
		TFUtils.Assert(this != null, "This Inventory Widget Tab Unity Object has not yet been able to set itself up.");
		this.nextRowDelegate = nextRowDelegate;
		this.closeRowsCallback = closeRowsCallback;
		this.openAllTabsCallback = openAllTabsCallback;
		openRows = delegate(bool closeFirst)
		{
			if (closeFirst)
			{
				InternalClose();
			}
			Open(resourceMgr, craftMgr, sfxMgr, onUiEventCallback, onDragCallback);
		};
		Action value = delegate
		{
			if (rowsVisible)
			{
				sfxMgr.PlaySound("CloseIngredientWidget");
				SBGUIStandardScreen.userClosedWishList = true;
				InternalClose();
				AnalyticsWrapper.LogUIInteraction(game, "ui_hide_wishes", "button", "tap");
			}
			else
			{
				sfxMgr.PlaySound("OpenIngredientWidget");
				SBGUIStandardScreen.userClosedWishList = false;
				openAllTabsCallback();
				AnalyticsWrapper.LogUIInteraction(game, "ui_display_wishes", "button", "tap");
			}
		};
		button.TapEvent.AddListener(value);
		UpdateRecipes(craftMgr, vendingMgr, resourceMgr);
	}

	public bool ActivateTab(bool closeExisting)
	{
		bool result = !rowsVisible;
		openRows(closeExisting);
		return result;
	}

	public bool TryActivateTab(int cookbookId, bool isVendor, bool closeExisting)
	{
		if (CookbookId == cookbookId && isVendor == this.isVendor)
		{
			ActivateTab(closeExisting);
		}
		return false;
	}

	public void UpdateRecipes(CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr)
	{
		productsMade.Clear();
		int[] array = null;
		if (isVendor)
		{
			VendorDefinition vendorDefinition = vendingMgr.GetVendorDefinition(CookbookId);
			if (vendorDefinition != null)
			{
				List<int> generalStock = vendorDefinition.generalStock;
				generalStock.AddRange(vendorDefinition.specialStock);
				array = craftMgr.UnlockedRecipesCopy.Intersect(generalStock).ToArray();
			}
		}
		else
		{
			array = craftMgr.UnlockedRecipesCopy.Intersect(craftMgr.GetCookbookById(CookbookId).GetRecipes()).ToArray();
		}
		if (array == null)
		{
			return;
		}
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			int productId = craftMgr.GetRecipeById(array[i]).productId;
			if (!productsMade.Contains(productId))
			{
				productsMade.Add(productId);
			}
		}
	}

	public void Close()
	{
		rowsVisible = false;
	}

	private void Open(ResourceManager resourceMgr, CraftingManager craftMgr, SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback)
	{
		TFUtils.Assert(resourceMgr != null, "Must call Setup before opening!");
		List<int> consumables = resourceMgr.ConsumableProducts(craftMgr);
		List<int> list = productsMade.Where((int pid) => consumables.Contains(pid)).ToList();
		foreach (int item in list)
		{
			SBGUIInventoryWidgetRow sBGUIInventoryWidgetRow = nextRowDelegate(Row, CookbookId, isVendor);
			string resourceTexture = resourceMgr.Resources[item].GetResourceTexture();
			TFUtils.Assert(resourceTexture != null && resourceTexture != string.Empty, "Did not find texture for resource with resourceId=" + item);
			sBGUIInventoryWidgetRow.Initialize(sfxMgr, onUiEventCallback, onDragCallback, resourceTexture);
			sBGUIInventoryWidgetRow.SetProductToTrack(item);
		}
		rowsVisible = true;
	}

	private void InternalClose()
	{
		closeRowsCallback();
		rowsVisible = false;
	}

	public override void MockClick()
	{
		openAllTabsCallback();
	}
}
