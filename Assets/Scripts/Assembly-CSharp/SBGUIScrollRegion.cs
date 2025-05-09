using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

[RequireComponent(typeof(ScrollRegion))]
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

	private const int RESET_TOUCH_SEMAPHORE = 10;

	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	public EventDispatcher ScrollEvent = new EventDispatcher();

	public EventDispatcher ScrollStopEvent = new EventDispatcher();

	public bool isReady;

	public ANCHOR_POSITION anchorPosition = ANCHOR_POSITION.BOTTOM_LEFT;

	public SCROLL_DIRECTION scrollDirection = SCROLL_DIRECTION.HORIZONTAL;

	public SBGUIScrollBar scrollBar;

	public Rect boundingRect;

	public Momentum momentum;

	private bool moving;

	private int movedSemaphore;

	private int touchedSemaphore = 10;

	private ScrollRegion region;

	public SBGUIElement subViewMarker;

	private Rect contentRect;

	private Vector3 minScroll;

	private Vector3 maxScroll;

	private Vector2? scrollScreenStart;

	private Vector2 lastDelta;

	private List<Action<SBGUIScrollListElement>> createSlotActions = new List<Action<SBGUIScrollListElement>>();

	private Vector3 initialMarkerPos;

	private Vector3 currentMarkerPos;

	public SBGUIElement Marker
	{
		get
		{
			return subViewMarker;
		}
	}

	public Vector3 MinScroll
	{
		get
		{
			return minScroll;
		}
	}

	public Vector3 MaxScroll
	{
		get
		{
			return maxScroll;
		}
	}

	public Vector3 InitialMarkerPos
	{
		get
		{
			return initialMarkerPos;
		}
	}

	public List<Action<SBGUIScrollListElement>> SetupSlotActions
	{
		get
		{
			return createSlotActions;
		}
	}

	public bool WasRecentlyTouched
	{
		get
		{
			return touchedSemaphore > 0;
		}
	}

	public override void SetVisible(bool viz)
	{
		region.Visible = viz;
	}

	protected override void Awake()
	{
		region = GetComponent<ScrollRegion>();
		region.ScrollEvent.AddListener(ScrollHandler);
		if (scrollBar != null)
		{
			scrollBar.scrollDirection = scrollDirection;
		}
		momentum = new Momentum(8);
		moving = false;
		base.Awake();
	}

	public void Update()
	{
		if (subViewMarker != null && momentum != null)
		{
			momentum.TrackForSmoothing(subViewMarker.tform.position);
		}
		if (movedSemaphore <= 0 && moving && momentum != null)
		{
			momentum.ApplyFriction(0.85f);
			Vector2 vector = new Vector2(momentum.Velocity.x, 0f - momentum.Velocity.y);
			float num = vector.SqrMagnitude();
			if ((double)num <= 0.0001)
			{
				Vector2? vector2 = scrollScreenStart;
				if (!vector2.HasValue)
				{
					moving = false;
					ScrollStopEvent.FireEvent();
				}
			}
			else
			{
				DeltaScroll(vector);
			}
		}
		movedSemaphore--;
		if (touchedSemaphore <= 0)
		{
			scrollScreenStart = null;
		}
		touchedSemaphore--;
	}

	private bool ScrollHandler(YGEvent evt)
	{
		touchedSemaphore = 10;
		switch (evt.type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			SoundEffectManager.CreateSoundEffectManager().PlaySound("scrolling");
			scrollScreenStart = evt.position;
			lastDelta = Vector2.zero;
			momentum.ClearTrackPositions();
			break;
		case YGEvent.TYPE.TOUCH_MOVE:
		{
			if (!scrollScreenStart.HasValue)
			{
				scrollScreenStart = evt.position;
				lastDelta = Vector2.zero;
			}
			Vector2 vector2 = (evt.position - scrollScreenStart.Value) * 0.01f;
			vector2 = new Vector2(vector2.x, 0f - vector2.y);
			DeltaScroll(vector2 - lastDelta);
			momentum.CalculateSmoothVelocity();
			lastDelta = vector2;
			movedSemaphore = 1;
			moving = true;
			break;
		}
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
			if (scrollScreenStart.HasValue)
			{
				Vector2 vector = (evt.position - scrollScreenStart.Value) * 0.01f;
				vector = new Vector2(vector.x, 0f - vector.y);
				DeltaScroll(vector - lastDelta);
				momentum.CalculateSmoothVelocity();
			}
			scrollScreenStart = null;
			lastDelta = Vector2.zero;
			movedSemaphore = 1;
			touchedSemaphore = 0;
			break;
		case YGEvent.TYPE.RESET:
			scrollScreenStart = null;
			lastDelta = Vector2.zero;
			break;
		}
		return false;
	}

	public void ResetToMinScroll()
	{
		moving = false;
		SetScroll(minScroll);
	}

	public void ResetScroll()
	{
		if (createSlotActions.Count > 0)
		{
			ResetScroll(boundingRect);
			return;
		}
		Bounds totalBounds = GetTotalBounds();
		ResetScroll(totalBounds.ToRect());
	}

	public void ResetScroll(Rect scrollSize)
	{
		moving = false;
		float thumbSize = 1f;
		minScroll = (maxScroll = InitialMarkerPos);
		if (scrollBar != null && scrollSize.width == 0f && scrollSize.height == 0f)
		{
			scrollBar.SetThumbSize(thumbSize);
			scrollBar.Reset();
			return;
		}
		Rect worldRect = region.GetWorldRect();
		contentRect = scrollSize;
		if (scrollDirection == SCROLL_DIRECTION.HORIZONTAL)
		{
			if (contentRect.width > worldRect.width)
			{
				maxScroll.x -= contentRect.width - worldRect.width;
				maxScroll.y = minScroll.y;
				maxScroll.z = minScroll.z;
				thumbSize = Mathf.Clamp01(worldRect.width / contentRect.width);
			}
		}
		else if (scrollDirection == SCROLL_DIRECTION.VERTICAL && contentRect.height > worldRect.height)
		{
			maxScroll.y += contentRect.height - worldRect.height;
			maxScroll.x = minScroll.x;
			maxScroll.z = minScroll.z;
			thumbSize = Mathf.Clamp01(worldRect.height / contentRect.height);
		}
		if (scrollBar != null)
		{
			scrollBar.SetThumbSize(thumbSize);
			scrollBar.Reset();
		}
		base.View.RefreshEvent += base.ReregisterColliders;
	}

	public Bounds GetTotalBounds()
	{
		return region.GetTotalBounds();
	}

	public Rect GetWorldRect()
	{
		return region.GetWorldRect();
	}

	private Vector3 ClampPosition(Vector3 pos)
	{
		pos.x = Mathf.Clamp(pos.x, Mathf.Min(minScroll.x, maxScroll.x), Mathf.Max(minScroll.x, maxScroll.x));
		pos.y = Mathf.Clamp(pos.y, Mathf.Min(minScroll.y, maxScroll.y), Mathf.Max(minScroll.y, maxScroll.y));
		return pos;
	}

	public bool DeltaScroll(Vector3 delta)
	{
		if (delta.sqrMagnitude <= 1E-06f)
		{
			return false;
		}
		if (subViewMarker == null)
		{
			return false;
		}
		Vector3 position = subViewMarker.tform.position;
		Vector3 vector = position;
		float value = 0f;
		if (scrollDirection == SCROLL_DIRECTION.VERTICAL)
		{
			delta.x = 0f;
			vector = ClampPosition(vector - delta);
			value = (vector.y - initialMarkerPos.y) / contentRect.height;
		}
		else if (scrollDirection == SCROLL_DIRECTION.HORIZONTAL)
		{
			delta.y = 0f;
			delta.x *= -1f;
			vector = ClampPosition(vector - delta);
			value = (initialMarkerPos.x - vector.x) / contentRect.width;
		}
		if (vector == position)
		{
			return false;
		}
		if (scrollBar != null)
		{
			scrollBar.UpdateScroll(Mathf.Clamp01(value));
		}
		SetScroll(vector);
		return true;
	}

	public void SetScroll(Vector3 pos)
	{
		subViewMarker.tform.position = pos;
		YG2DBody[] componentsInChildren = subViewMarker.gameObject.GetComponentsInChildren<YG2DBody>();
		YG2DBody[] array = componentsInChildren;
		foreach (YG2DBody yG2DBody in array)
		{
			yG2DBody.MatchTransform3D();
		}
		ScrollEvent.FireEvent();
	}

	public void MatchAndRegister()
	{
		region.MatchSubView();
		region.ReadyEvent.AddListener(Register);
	}

	private void Register()
	{
		Vector3 pos = base.View.WorldToScreen(base.tform.position);
		initialMarkerPos = region.ScreenToWorld(pos);
		initialMarkerPos.z += base.tform.position.z - base.View.transform.position.z;
		subViewMarker = SBGUIElement.Create();
		subViewMarker.name = string.Format("{0}_marker_{1}", base.gameObject.name, (uint)GetInstanceID());
		subViewMarker.tform.parent = region.SubViewTform;
		if (anchorPosition == ANCHOR_POSITION.TOP_LEFT)
		{
			initialMarkerPos.y += region.size.y * 0.01f;
		}
		subViewMarker.tform.position = initialMarkerPos;
		subViewMarker.MuteButtons(muted);
		ReadyEvent.FireEvent();
		isReady = true;
	}

	public void ClearSlotActions()
	{
		createSlotActions.Clear();
	}
}
