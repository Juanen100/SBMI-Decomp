using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

public class SBGUIInventoryHudWidget : SBGUIElement
{
	private enum DragMode
	{
		None = 0,
		PrimedForScrolling = 1,
		Scrolling = 2,
		DraggingOut = 3
	}

	public const string SHOW = "ShowInventoryHudWidget";

	public const string RESET_SIMULATION_DRAG = "ResetSimulationDrag";

	public const string GOOD_DELIVERY_REQUEST = "GoodDeliveryRequest";

	public const string GOOD_RETURN_REQUEST = "GoodReturnRequest";

	public const string PULSE_RESOURCE_ERROR = "PulseResourceError";

	public List<SBGUIInventoryWidgetTab> Tabs;

	public SBGUIElement RowAnchor;

	public GameObject RowHideMarker;

	public float RowOffset;

	public Action<int, YGEvent> StartDragOutCallback;

	public Action<YGEvent> DragThroughCallback;

	public SBGUIElement footerAnchor;

	public YGFrameAtlasSprite backingSprite;

	private List<SBGUIInventoryWidgetRow> currentRows;

	private int currentCount;

	private float bottomHideThreshold;

	private Vector3 initialAnchorPosition;

	private bool didScrollTooHigh;

	private bool didScrollTooLow;

	private Vector2 primedEvtPosition;

	private Vector2 primedRowAnchorPositionScreen;

	private Vector2? bottomLockRowAnchorPositionScreen;

	private Action<YGEvent, int?> interactCallback;

	private int lastOpenedCookbook;

	private bool lastOpenedIsVendor;

	private const int ALL_COOKBOOKS = -1;

	private DragMode dragMode;

	private bool firstUpdateInit;

	public void Setup(Game game, CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Action<YGEvent, int?> interactCallback, float bottomHideThreshold)
	{
	}

	public void UpdateRecipes(CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr)
	{
	}

	public SBGUIInventoryWidgetRow GetNextRow(SBGUIInventoryWidgetRow rowType, int fromCookbookId, bool fromIsVendor)
	{
		return null;
	}

	public bool ActivateTab(int cookbookId, bool isVendor)
	{
		return false;
	}

	public bool ActivateAllTabs()
	{
		return false;
	}

	public void Tidy()
	{
	}

	public void TryPulseResourceError(int resourceId)
	{
	}

	public void IncrementDeductionsForTick(int resourceId)
	{
	}

	private void OnDraggedProduct(int productId, YGEvent triggeringEvent)
	{
	}

	public void CloseRows()
	{
	}

	public void OnUpdate(ResourceManager resourceMgr)
	{
	}

	private void HandleUiEvent(YGEvent evt)
	{
	}

	private void DetectScrolLimits()
	{
	}

	private bool IsTooLow()
	{
		return false;
	}

	private void EnforceScrollLimits()
	{
	}
}
