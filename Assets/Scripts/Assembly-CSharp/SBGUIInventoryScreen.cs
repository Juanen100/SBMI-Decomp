using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SBGUIInventoryScreen : SBGUITabbedScrollableDialog
{
	public GameObject slotPrefab;

	public EventDispatcher<SBInventoryItem> BuildingSlotClickedEvent;

	public EventDispatcher<SBInventoryItem> MovieSlotClickedEvent;

	protected override Vector2 GetSlotSize()
	{
		return default(Vector2);
	}

	public float GetMainWindowZ()
	{
		return 0f;
	}

	protected override void LoadCategories(Session session)
	{
	}

	private void AddCategory(SBInventoryCategory category, Session session, string name, string type, string texture)
	{
	}

	[DebuggerHidden]
	protected override IEnumerator BuildTabCoroutine(string tabName)
	{
		return null;
	}

	private void LoadSlotInfo(SBTabCategory tabCategory, SBGUIElement anchor)
	{
	}

	protected override SBGUIScrollListElement MakeSlot()
	{
		return null;
	}

	protected override Rect CalculateTabContentsSize(string tabName)
	{
		return default(Rect);
	}

	private Action<SBGUIScrollListElement> SetupSlotClosure(Session session, SBGUIElement anchor, SBInventoryItem invItem, EventDispatcher<SBInventoryItem> itemClickedEvent, Vector3 offset)
	{
		return null;
	}
}
