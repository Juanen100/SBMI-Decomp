using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

public class SBGUIScrollRegion : SBGUIElement
{
	public enum ANCHOR_POSITION
	{
		TOP_LEFT = 0,
		BOTTOM_LEFT = 1
	}

	public enum SCROLL_DIRECTION
	{
		VERTICAL = 0,
		HORIZONTAL = 1
	}

	public ReadyEventDispatcher ReadyEvent;

	public EventDispatcher ScrollEvent;

	public EventDispatcher ScrollStopEvent;

	public bool isReady;

	public ANCHOR_POSITION anchorPosition;

	public SCROLL_DIRECTION scrollDirection;

	public SBGUIScrollBar scrollBar;

	public Rect boundingRect;

	public Momentum momentum;

	private bool moving;

	private float movingHoverTimer;

	private bool scrollMoved;

	private const int RESET_TOUCH_SEMAPHORE = 10;

	private int movedSemaphore;

	private int touchedSemaphore;

	private ScrollRegion region;

	public SBGUIElement subViewMarker;

	private Rect contentRect;

	private Vector3 minScroll;

	private Vector3 maxScroll;

	private Vector2? scrollScreenStart;

	private Vector2 lastDelta;

	private List<Action<SBGUIScrollListElement>> createSlotActions;

	private Vector3 initialMarkerPos;

	private Vector3 currentMarkerPos;

	public SBGUIElement Marker
	{
		get
		{
			return null;
		}
	}

	public Vector3 MinScroll
	{
		get
		{
			return default(Vector3);
		}
	}

	public Vector3 MaxScroll
	{
		get
		{
			return default(Vector3);
		}
	}

	public Vector3 InitialMarkerPos
	{
		get
		{
			return default(Vector3);
		}
	}

	public List<Action<SBGUIScrollListElement>> SetupSlotActions
	{
		get
		{
			return null;
		}
	}

	public bool WasRecentlyTouched
	{
		get
		{
			return false;
		}
	}

	public override void SetVisible(bool viz)
	{
	}

	protected override void Awake()
	{
	}

	public void Update()
	{
	}

	private bool ScrollHandler(YGEvent evt)
	{
		return false;
	}

	public void ResetToMinScroll()
	{
	}

	public void ResetScroll()
	{
	}

	public void ResetScroll(Rect scrollSize)
	{
	}

	public Bounds GetTotalBounds()
	{
		return default(Bounds);
	}

	public Rect GetWorldRect()
	{
		return default(Rect);
	}

	private Vector3 ClampPosition(Vector3 pos)
	{
		return default(Vector3);
	}

	public bool DeltaScroll(Vector3 delta)
	{
		return false;
	}

	public void SetScroll(Vector3 pos)
	{
	}

	public void MatchAndRegister()
	{
	}

	private void Register()
	{
	}

	public void ClearSlotActions()
	{
	}
}
