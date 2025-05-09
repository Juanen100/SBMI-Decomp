using System.Collections;
using UnityEngine;

public class SBGUICreditsScreen : SBGUIScrollableDialog
{
	public GameObject slotPrefab;

	protected static TFPool<SBGUICreditsSlot> slotPool = new TFPool<SBGUICreditsSlot>();

	public void Setup(Session session)
	{
		base.session = session;
	}

	public void CreateUI()
	{
		SBGUICreditsSlot component = slotPrefab.GetComponent<SBGUICreditsSlot>();
		SBGUIImage sBGUIImage = (SBGUIImage)component.FindChild("slot_boundary");
		Vector2 vector = sBGUIImage.Size * 0.01f;
		Rect scrollSize = new Rect(0f, 0f, vector.x, vector.y);
		region.ResetScroll(scrollSize);
		region.ResetToMinScroll();
		CreateCreditsSlot(session, region.Marker, Vector3.zero);
		StartCoroutine(ScrollingCredits());
	}

	private IEnumerator ScrollingCredits()
	{
		yield return null;
		bool keepScrolling = true;
		while (keepScrolling)
		{
			if (region.WasRecentlyTouched)
			{
				keepScrolling = false;
			}
			region.momentum.TrackForSmoothing(region.subViewMarker.tform.position + new Vector3(0f, 0.005f, 0f));
			region.momentum.CalculateSmoothVelocity();
			yield return null;
		}
	}

	public override void Deactivate()
	{
		StopCoroutine("ScrollingCredits");
		slotPool.Clear(delegate(SBGUICreditsSlot slot)
		{
			slot.Deactivate();
		});
		base.Deactivate();
	}

	private SBGUICreditsSlot CreateCreditsSlot(Session session, SBGUIElement anchor, Vector3 offset)
	{
		SBGUICreditsSlot sBGUICreditsSlot = slotPool.Create(SBGUICreditsSlot.MakeCreditsSlot);
		sBGUICreditsSlot.SetActive(true);
		sBGUICreditsSlot.Setup(session, this, anchor, offset);
		return sBGUICreditsSlot;
	}
}
