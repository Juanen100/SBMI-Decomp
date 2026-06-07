using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class SBGUISlottedScrollableDialog : SBGUIScrollableDialog
{
	public int ScrollSubElementCount;

	public EventDispatcher<SBGUIElement> SlotReadyEvent;

	public static Action OnSafeToClose;

	protected bool mustWaitForInfoToLoad;

	private TFPool<SBGUIScrollListElement> slotPool;

	protected Dictionary<int, SBGUIScrollListElement> slotRefs;

	protected HashSet<string> sessionActionIdSearchRequests;

	protected Dictionary<string, int> sessionActionSlotMap;

	public override void Start()
	{
	}

	public virtual void SetManagers(Session session)
	{
	}

	public SBGUIElement FindDynamicSubElementInScrollRegionSessionActionId(string sessionActionId, bool assertOnNullScrollRegionSubComponents = true)
	{
		return null;
	}

	public virtual void FindDynamicSubElementInScrollRegionSessionActionIdAsync(string sessionActionId, Action<SBGUIElement> foundCallback)
	{
	}

	protected void PreLoadRegionContentInfo()
	{
	}

	protected void PostLoadRegionContentInfo(int slotCount)
	{
	}

	protected void PostLoadRegionContentInfo(int slotCount, Vector3 scrollPos)
	{
	}

	private void ResetScrolling(int slotCount, Vector3 scrollPos)
	{
	}

	public override void Deactivate()
	{
	}

	protected abstract SBGUIScrollListElement MakeSlot();

	private void CreateSlot(int i)
	{
	}

	protected void ClearCachedSlotInfos()
	{
	}

	private void UpdateSlotVisibility()
	{
	}

	public List<SBGUIScrollListElement> GetVisibleSrollListElements()
	{
		return null;
	}

	[DebuggerHidden]
	protected IEnumerator ShowSlotsAsNeeded(bool deferProcessing)
	{
		return null;
	}

	protected virtual int CheckOffscreenSelectedSlot(int visibleStart, int visibleEnd)
	{
		return 0;
	}

	protected virtual void OnSlotsVisible()
	{
	}

	protected static void DeactivateSlot(SBGUIScrollListElement s)
	{
	}

	public override void ShowScrollRegion(bool visible)
	{
	}

	public override void OnDestroy()
	{
	}

	public void DestroySlot(SBGUIScrollListElement elem)
	{
	}

	protected abstract Vector2 GetSlotSize();

	protected abstract int GetSlotIndex(Vector2 pos);

	protected abstract Vector2 GetSlotOffset(int index);

	protected Rect CalculateScrollRegionSize(int slotCount)
	{
		return default(Rect);
	}
}
