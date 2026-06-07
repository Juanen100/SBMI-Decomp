using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SBGUICreditsScreen : SBGUIScrollableDialog
{
	public GameObject slotPrefab;

	protected static TFPool<SBGUICreditsSlot> slotPool;

	public void Setup(Session session)
	{
	}

	public void CreateUI()
	{
	}

	[DebuggerHidden]
	private IEnumerator ScrollingCredits()
	{
		return null;
	}

	public override void Deactivate()
	{
	}

	private SBGUICreditsSlot CreateCreditsSlot(Session session, SBGUIElement anchor, Vector3 offset)
	{
		return null;
	}
}
