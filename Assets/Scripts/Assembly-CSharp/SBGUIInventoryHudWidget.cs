#define ASSERTS_ON
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

	private const int ALL_COOKBOOKS = -1;

	public List<SBGUIInventoryWidgetTab> Tabs;

	public SBGUIElement RowAnchor;

	public GameObject RowHideMarker;

	public float RowOffset;

	public Action<int, YGEvent> StartDragOutCallback;

	public Action<YGEvent> DragThroughCallback;

	public SBGUIElement footerAnchor;

	public YGFrameAtlasSprite backingSprite;

	private List<SBGUIInventoryWidgetRow> currentRows = new List<SBGUIInventoryWidgetRow>();

	private int currentCount;

	private float bottomHideThreshold;

	private Vector3 initialAnchorPosition;

	private bool didScrollTooHigh;

	private bool didScrollTooLow;

	private Vector2 primedEvtPosition = Vector2.zero;

	private Vector2 primedRowAnchorPositionScreen = Vector2.zero;

	private Vector2? bottomLockRowAnchorPositionScreen;

	private Action<YGEvent, int?> interactCallback;

	private int lastOpenedCookbook = 1;

	private bool lastOpenedIsVendor;

	private DragMode dragMode;

	private bool firstUpdateInit;

	public void Setup(Game game, CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Action<YGEvent, int?> interactCallback, float bottomHideThreshold)
	{
		this.bottomHideThreshold = bottomHideThreshold;
		foreach (SBGUIInventoryWidgetTab tab in Tabs)
		{
			tab.Setup(game, craftMgr, vendingMgr, resourceMgr, sfxMgr, HandleUiEvent, OnDraggedProduct, GetNextRow, CloseRows, ActivateAllTabs);
		}
		TFUtils.Assert(RowAnchor != null, "Must specify a Row Anchor for this element. this=" + ToString());
		TFUtils.Assert(RowHideMarker != null, "Must specify a Row Hide Marker for this element. this=" + ToString());
		initialAnchorPosition = RowAnchor.transform.localPosition;
		this.interactCallback = interactCallback;
		craftMgr.UnlockedEvent.AddListener(delegate
		{
			UpdateRecipes(craftMgr, vendingMgr, resourceMgr);
		});
		firstUpdateInit = true;
	}

	public void UpdateRecipes(CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr)
	{
		CloseRows();
		foreach (SBGUIInventoryWidgetTab tab in Tabs)
		{
			tab.UpdateRecipes(craftMgr, vendingMgr, resourceMgr);
		}
	}

	public SBGUIInventoryWidgetRow GetNextRow(SBGUIInventoryWidgetRow rowType, int fromCookbookId, bool fromIsVendor)
	{
		Vector3 vector = Vector3.zero;
		if (currentCount == 0)
		{
			lastOpenedCookbook = fromCookbookId;
			lastOpenedIsVendor = fromIsVendor;
		}
		else if (currentCount <= currentRows.Count)
		{
			SBGUIInventoryWidgetRow sBGUIInventoryWidgetRow = currentRows[currentCount - 1];
			vector = sBGUIInventoryWidgetRow.WorldPosition + new Vector3(0f, 0f - RowOffset, 0f) - RowAnchor.WorldPosition;
		}
		SBGUIInventoryWidgetRow sBGUIInventoryWidgetRow2 = null;
		if (currentCount >= currentRows.Count)
		{
			sBGUIInventoryWidgetRow2 = (SBGUIInventoryWidgetRow)UnityEngine.Object.Instantiate(rowType);
			sBGUIInventoryWidgetRow2.SetParent(RowAnchor);
			sBGUIInventoryWidgetRow2.WorldPosition = RowAnchor.WorldPosition + vector;
			currentRows.Add(sBGUIInventoryWidgetRow2);
			initialAnchorPosition = RowAnchor.transform.localPosition;
		}
		else
		{
			sBGUIInventoryWidgetRow2 = currentRows[currentCount];
			if (sBGUIInventoryWidgetRow2.GetType().Equals(rowType.GetType()))
			{
				sBGUIInventoryWidgetRow2.SetActive(true);
			}
			else
			{
				currentRows[currentCount] = (SBGUIInventoryWidgetRow)UnityEngine.Object.Instantiate(rowType);
				currentRows[currentCount].SetParent(RowAnchor);
				currentRows[currentCount].WorldPosition = sBGUIInventoryWidgetRow2.WorldPosition;
				UnityEngine.Object.Destroy(sBGUIInventoryWidgetRow2);
				sBGUIInventoryWidgetRow2 = currentRows[currentCount];
			}
		}
		interactCallback(null, null);
		currentCount++;
		return sBGUIInventoryWidgetRow2;
	}

	public bool ActivateTab(int cookbookId, bool isVendor)
	{
		bool flag = currentCount > 0;
		bool flag2 = false;
		if (flag && cookbookId != lastOpenedCookbook && isVendor != lastOpenedIsVendor)
		{
			CloseRows();
		}
		if (!flag || (cookbookId != lastOpenedCookbook && isVendor != lastOpenedIsVendor))
		{
			foreach (SBGUIInventoryWidgetTab tab in Tabs)
			{
				flag2 |= tab.TryActivateTab(cookbookId, isVendor, true);
			}
		}
		lastOpenedCookbook = cookbookId;
		lastOpenedIsVendor = isVendor;
		return !flag && flag2;
	}

	public bool ActivateAllTabs()
	{
		bool flag = false;
		if (IsActive())
		{
			if (currentCount <= 0)
			{
				foreach (SBGUIInventoryWidgetTab tab in Tabs)
				{
					flag |= tab.ActivateTab(false);
				}
			}
			lastOpenedCookbook = 1;
			lastOpenedIsVendor = false;
			footerAnchor.SetActive(true);
			backingSprite.gameObject.SetActiveRecursively(true);
		}
		return flag;
	}

	public void Tidy()
	{
		dragMode = DragMode.None;
		primedEvtPosition = Vector2.zero;
		primedRowAnchorPositionScreen = Vector2.zero;
		bottomLockRowAnchorPositionScreen = null;
	}

	public void TryPulseResourceError(int resourceId)
	{
		SBGUIInventoryWidgetRow sBGUIInventoryWidgetRow = currentRows.Find((SBGUIInventoryWidgetRow i) => i.Product == resourceId);
		if (sBGUIInventoryWidgetRow != null)
		{
			sBGUIInventoryWidgetRow.PulseError();
		}
	}

	public void IncrementDeductionsForTick(int resourceId)
	{
		if (currentCount > 0)
		{
			SBGUIInventoryWidgetRow sBGUIInventoryWidgetRow = currentRows.Find((SBGUIInventoryWidgetRow i) => i.Product == resourceId);
			if (sBGUIInventoryWidgetRow == null)
			{
				Debug.LogError("Could not find row with productID(" + resourceId + ")");
			}
			else
			{
				sBGUIInventoryWidgetRow.IncrementDeductionsForTick();
			}
		}
	}

	private void OnDraggedProduct(int productId, YGEvent triggeringEvent)
	{
		TFUtils.Assert(StartDragOutCallback != null, "Must assign a dragout callback before reacting to dragout!");
		dragMode = DragMode.DraggingOut;
		StartDragOutCallback(productId, triggeringEvent);
		interactCallback(null, productId);
	}

	public void CloseRows()
	{
		if (currentRows.Count == 0)
		{
			return;
		}
		foreach (SBGUIInventoryWidgetTab tab in Tabs)
		{
			tab.Close();
		}
		foreach (SBGUIInventoryWidgetRow currentRow in currentRows)
		{
			currentRow.SetActive(false);
		}
		RowAnchor.transform.localPosition = initialAnchorPosition;
		currentCount = 0;
		interactCallback(null, null);
		footerAnchor.SetActive(false);
		backingSprite.gameObject.SetActiveRecursively(false);
	}

	public void OnUpdate(ResourceManager resourceMgr)
	{
		if (firstUpdateInit)
		{
			Vector3 position = footerAnchor.tform.position;
			Vector2 screenPosition = footerAnchor.GetScreenPosition();
			Vector3 vector = new Vector3(screenPosition.x, bottomHideThreshold, footerAnchor.tform.position.z);
			footerAnchor.SetScreenPosition(vector);
			firstUpdateInit = false;
			backingSprite.transform.position = (position + footerAnchor.tform.position) * 0.5f;
			backingSprite.transform.Translate(new Vector3(0f, 0f, 0.5f), Space.World);
			backingSprite.size.y = Math.Abs(screenPosition.y - footerAnchor.GetScreenPosition().y);
			backingSprite.size.y *= 2f / GUIView.ResolutionScaleFactor();
			backingSprite.SetSize(backingSprite.size);
		}
		if (currentCount > 0 && IsActive())
		{
			float y = GetScreenPosition().y;
			int count = currentRows.Count;
			for (int i = 0; i < count; i++)
			{
				currentRows[i].OnUpdate(resourceMgr, y, bottomHideThreshold);
			}
		}
	}

	private void HandleUiEvent(YGEvent evt)
	{
		if (evt.type == YGEvent.TYPE.TOUCH_BEGIN)
		{
			primedRowAnchorPositionScreen = RowAnchor.GetScreenPosition();
			primedEvtPosition = evt.position;
			if (dragMode != DragMode.DraggingOut)
			{
				dragMode = DragMode.PrimedForScrolling;
			}
		}
		else if (evt.type == YGEvent.TYPE.TOUCH_END || evt.type == YGEvent.TYPE.TOUCH_CANCEL)
		{
			if (DragThroughCallback != null)
			{
				DragThroughCallback(evt);
			}
			Tidy();
		}
		Vector2 vector = primedEvtPosition - evt.position;
		vector.x = 0f;
		if (dragMode == DragMode.PrimedForScrolling && Math.Abs(vector.y) >= 2f)
		{
			dragMode = DragMode.Scrolling;
		}
		if (dragMode == DragMode.Scrolling)
		{
			RowAnchor.SetScreenPosition(primedRowAnchorPositionScreen + vector);
			EnforceScrollLimits();
		}
		if (dragMode == DragMode.DraggingOut && DragThroughCallback != null)
		{
			DragThroughCallback(evt);
		}
		interactCallback(evt, null);
	}

	private void DetectScrolLimits()
	{
		if (currentRows.Count == 0)
		{
			didScrollTooHigh = false;
			didScrollTooLow = false;
		}
		else if (RowAnchor.transform.localPosition.y <= initialAnchorPosition.y)
		{
			didScrollTooHigh = true;
			didScrollTooLow = false;
		}
		else
		{
			didScrollTooHigh = false;
			didScrollTooLow = IsTooLow();
		}
	}

	private bool IsTooLow()
	{
		float y = currentRows[currentRows.Count - 1].GetScreenPosition().y;
		return y < (float)(Screen.height / 2);
	}

	private void EnforceScrollLimits()
	{
		DetectScrolLimits();
		if (didScrollTooHigh)
		{
			RowAnchor.transform.localPosition = initialAnchorPosition;
		}
		else if (didScrollTooLow)
		{
			if (bottomLockRowAnchorPositionScreen.HasValue)
			{
				RowAnchor.SetPosition(bottomLockRowAnchorPositionScreen.Value);
			}
			else
			{
				while (IsTooLow())
				{
					Vector3 vector = new Vector3(0f, (float)(-Screen.height) * 0.001f, 0f);
					RowAnchor.transform.position += vector;
					if (RowAnchor.transform.localPosition.y <= initialAnchorPosition.y)
					{
						RowAnchor.transform.localPosition = initialAnchorPosition;
						break;
					}
				}
				bottomLockRowAnchorPositionScreen = RowAnchor.GetScreenPosition();
			}
			RowAnchor.transform.localPosition = new Vector3(RowAnchor.transform.localPosition.x, RowAnchor.transform.localPosition.y, initialAnchorPosition.z);
		}
		UpdateColliderTransforms();
	}
}
