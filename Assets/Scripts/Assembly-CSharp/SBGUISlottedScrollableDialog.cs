#define ASSERTS_ON
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SBGUISlottedScrollableDialog : SBGUIScrollableDialog
{
	public int ScrollSubElementCount;

	public EventDispatcher<SBGUIElement> SlotReadyEvent = new EventDispatcher<SBGUIElement>();

	protected bool mustWaitForInfoToLoad = true;

	private TFPool<SBGUIScrollListElement> slotPool = new TFPool<SBGUIScrollListElement>();

	protected Dictionary<int, SBGUIScrollListElement> slotRefs = new Dictionary<int, SBGUIScrollListElement>();

	protected HashSet<string> sessionActionIdSearchRequests = new HashSet<string>();

	protected Dictionary<string, int> sessionActionSlotMap = new Dictionary<string, int>();

	public override void Start()
	{
		region.ScrollEvent.AddListener(UpdateSlotVisibility);
		base.Start();
	}

	public virtual void SetManagers(Session session)
	{
		base.session = session;
		SetManagers(session.TheGame.entities, session.TheGame.resourceManager, session.TheSoundEffectManager, session.TheGame.costumeManager);
	}

	public SBGUIElement FindDynamicSubElementInScrollRegionSessionActionId(string sessionActionId, bool assertOnNullScrollRegionSubComponents = true)
	{
		TFUtils.Assert(region != null, "No scroll region set for this screen");
		SBGUIElement componentInChildren = region.GetComponent<ScrollRegion>().subView.GetComponentInChildren<SBGUIElement>();
		TFUtils.Assert(!assertOnNullScrollRegionSubComponents || componentInChildren != null, "It appears that this dialog's scroll region has no ScrollRegion component! This might be a race condition that occurs when trying to Find a Dynamic Sub Element while this screen is still initializing.");
		if (componentInChildren == null)
		{
			return null;
		}
		return componentInChildren.FindChild(sessionActionId);
	}

	public virtual void FindDynamicSubElementInScrollRegionSessionActionIdAsync(string sessionActionId, Action<SBGUIElement> foundCallback)
	{
		TFUtils.Assert(foundCallback != null, "FindDynamicSubElementInScrollRegionSessionActionIdAsync requires callbacks to not be null.");
		Action<SBGUIElement> onSlotReadyEvent = null;
		onSlotReadyEvent = delegate(SBGUIElement slot)
		{
			if (slot.name.Equals(sessionActionId))
			{
				foundCallback(slot);
				SlotReadyEvent.RemoveListener(onSlotReadyEvent);
			}
		};
		SBGUIElement sBGUIElement = null;
		if (region != null && region.isReady)
		{
			sBGUIElement = FindDynamicSubElementInScrollRegionSessionActionId(sessionActionId);
		}
		if (sBGUIElement != null)
		{
			SlotReadyEvent.AddListener(onSlotReadyEvent);
			onSlotReadyEvent(sBGUIElement);
			return;
		}
		SlotReadyEvent.AddListener(onSlotReadyEvent);
		if (!sessionActionIdSearchRequests.Contains(sessionActionId))
		{
			sessionActionIdSearchRequests.Add(sessionActionId);
		}
		if (sessionActionSlotMap.ContainsKey(sessionActionId))
		{
			int num = sessionActionSlotMap[sessionActionId];
			if (region != null && region.SetupSlotActions.Count > num)
			{
				CreateSlot(num);
			}
		}
	}

	protected void PreLoadRegionContentInfo()
	{
		SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)FindChildSessionActionId("ActivityIndicator", false);
		if (sBGUIActivityIndicator != null)
		{
			sBGUIActivityIndicator.StartActivityIndicator();
		}
		mustWaitForInfoToLoad = true;
		StopAllCoroutines();
	}

	protected void PostLoadRegionContentInfo(int slotCount)
	{
		PostLoadRegionContentInfo(slotCount, Vector3.zero);
	}

	protected void PostLoadRegionContentInfo(int slotCount, Vector3 scrollPos)
	{
		SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)FindChildSessionActionId("ActivityIndicator", false);
		if (sBGUIActivityIndicator != null)
		{
			sBGUIActivityIndicator.StopActivityIndicator();
		}
		mustWaitForInfoToLoad = false;
		ResetScrolling(slotCount, scrollPos);
	}

	private void ResetScrolling(int slotCount, Vector3 scrollPos)
	{
		Rect regionSize = CalculateScrollRegionSize(slotCount);
		base.View.RefreshEvent += delegate
		{
			region.ResetScroll(regionSize);
			if (scrollPos != Vector3.zero)
			{
				if (scrollPos.sqrMagnitude > region.MaxScroll.sqrMagnitude)
				{
					region.SetScroll(region.MaxScroll);
				}
				else
				{
					region.SetScroll(scrollPos);
				}
			}
			else
			{
				region.SetScroll(region.MinScroll);
			}
		};
	}

	public override void Deactivate()
	{
		region.ClearSlotActions();
		SlotReadyEvent.ClearListeners();
		StopAllCoroutines();
		ClearCachedSlotInfos();
		base.Deactivate();
	}

	protected abstract SBGUIScrollListElement MakeSlot();

	private void CreateSlot(int i)
	{
		if (i > region.SetupSlotActions.Count)
		{
			TFUtils.Assert(false, "how are we trying to generate an out of bounds slot?");
			return;
		}
		SBGUIScrollListElement value = null;
		slotRefs.TryGetValue(i, out value);
		if (value == null)
		{
			value = slotPool.Create(MakeSlot);
			region.SetupSlotActions[i](value);
			value.transform.localPosition = GetSlotOffset(i);
			SlotReadyEvent.FireEvent(value);
			slotRefs[i] = value;
		}
	}

	protected void ClearCachedSlotInfos()
	{
		slotPool.Clear(DeactivateSlot);
		slotRefs.Clear();
		sessionActionSlotMap.Clear();
		sessionActionIdSearchRequests.Clear();
	}

	private void UpdateSlotVisibility()
	{
		if (!mustWaitForInfoToLoad)
		{
			StopAllCoroutines();
		}
		StartCoroutine(ShowSlotsAsNeeded(true));
	}

	public List<SBGUIScrollListElement> GetVisibleSrollListElements()
	{
		if (region.subViewMarker == null)
		{
			return null;
		}
		int count = region.SetupSlotActions.Count;
		float num = region.InitialMarkerPos.x - region.subViewMarker.tform.localPosition.x;
		float y = 0f;
		float x = num + region.GetComponent<ScrollRegion>().size.x * 0.01f + GetSlotSize().x;
		float y2 = region.GetComponent<ScrollRegion>().size.y * 0.01f;
		int num2 = Mathf.Max(0, GetSlotIndex(new Vector2(num, y)));
		int num3 = Mathf.Min(count - 1, GetSlotIndex(new Vector2(x, y2)));
		SBGUIScrollListElement value = null;
		List<SBGUIScrollListElement> list = new List<SBGUIScrollListElement>();
		for (int i = 0; i < count; i++)
		{
			if ((i >= num2 && i < num3) || (i == num3 && i == count - 1))
			{
				slotRefs.TryGetValue(i, out value);
				if (value != null)
				{
					list.Add(value);
				}
			}
			else if (i > num3)
			{
				break;
			}
		}
		return list;
	}

	protected IEnumerator ShowSlotsAsNeeded(bool deferProcessing)
	{
		if (region == null)
		{
			TFUtils.ErrorLog("SBGUISlottedScrollableDialog.ShowSlotsAsNeeded - region is null");
			yield return null;
		}
		int totalSlots = region.SetupSlotActions.Count;
		float startVisibleX = region.InitialMarkerPos.x - region.subViewMarker.tform.localPosition.x;
		float startVisibleY = 0f;
		float endVisibleX = startVisibleX + region.GetComponent<ScrollRegion>().size.x * 0.01f + GetSlotSize().x;
		float endVisibleY = region.GetComponent<ScrollRegion>().size.y * 0.01f;
		int startVisibleI = Mathf.Max(0, GetSlotIndex(new Vector2(startVisibleX, startVisibleY)));
		int endVisibleI = Mathf.Min(totalSlots - 1, GetSlotIndex(new Vector2(endVisibleX, endVisibleY)));
		for (int i = startVisibleI; i <= endVisibleI; i++)
		{
			CreateSlot(i);
		}
		int offscreenSlot = CheckOffscreenSelectedSlot(startVisibleI, endVisibleI);
		if (offscreenSlot != -1)
		{
			CreateSlot(offscreenSlot);
		}
		OnSlotsVisible();
		for (int j = 0; j < totalSlots; j++)
		{
			if (j < startVisibleI || j > endVisibleI)
			{
				SBGUIScrollListElement slot = null;
				slotRefs.TryGetValue(j, out slot);
				if (slot != null)
				{
					if (!SBGUI.GetInstance().CheckWhitelisted(slot))
					{
						DeactivateSlot(slot);
						slotPool.Release(slot);
						slotRefs[j] = null;
					}
					else
					{
						CreateSlot(j);
					}
				}
			}
			if (deferProcessing)
			{
				yield return null;
			}
		}
	}

	protected virtual int CheckOffscreenSelectedSlot(int visibleStart, int visibleEnd)
	{
		return -1;
	}

	protected virtual void OnSlotsVisible()
	{
		session.TheGame.sessionActionManager.RequestProcess(session.TheGame);
	}

	protected static void DeactivateSlot(SBGUIScrollListElement s)
	{
		if (s != null)
		{
			s.name = "inactive";
			s.SessionActionId = null;
			s.Deactivate();
		}
	}

	public override void ShowScrollRegion(bool visible)
	{
		if (!visible)
		{
			region.SetupSlotActions.Clear();
			SlotReadyEvent.ClearListeners();
			StopAllCoroutines();
			ClearCachedSlotInfos();
		}
		base.ShowScrollRegion(visible);
	}

	public override void OnDestroy()
	{
		slotPool.Clear(DeactivateSlot);
		slotPool.Purge(DestroySlot);
		base.OnDestroy();
	}

	public void DestroySlot(SBGUIScrollListElement elem)
	{
		if (!(elem != null))
		{
			return;
		}
		YGAtlasSprite[] componentsInChildren = elem.gameObject.GetComponentsInChildren<YGAtlasSprite>();
		YGAtlasSprite[] array = componentsInChildren;
		foreach (YGAtlasSprite yGAtlasSprite in array)
		{
			if (!string.IsNullOrEmpty(yGAtlasSprite.nonAtlasName))
			{
				base.View.Library.incrementTextureDuplicates(yGAtlasSprite.nonAtlasName);
			}
		}
		UnityGameResources.Destroy(elem.gameObject);
	}

	protected abstract Vector2 GetSlotSize();

	protected abstract int GetSlotIndex(Vector2 pos);

	protected abstract Vector2 GetSlotOffset(int index);

	protected Rect CalculateScrollRegionSize(int slotCount)
	{
		Vector2 slotOffset = GetSlotOffset(0);
		Bounds value = new Bounds(slotOffset, Vector2.zero);
		for (int i = 0; i < slotCount; i++)
		{
			Vector2 slotOffset2 = GetSlotOffset(i);
			value.Encapsulate(slotOffset2);
			value.Encapsulate(slotOffset2 + GetSlotSize());
		}
		return value.ToRect();
	}
}
