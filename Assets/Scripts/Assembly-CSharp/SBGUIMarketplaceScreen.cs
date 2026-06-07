using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUIMarketplaceScreen : SBGUITabbedScrollableDialog
{
	public class StoreImpression
	{
		public List<SBMarketOffer> m_pOffers;

		public float m_fTimeDelta;

		public bool Equals(StoreImpression pCompare)
		{
			return false;
		}
	}

	public GameObject slotPrefab;

	public EventDispatcher<SBMarketOffer> OfferClickedEvent;

	private List<StoreImpression> m_pStoreImpressions;

	private bool m_bImpressionScheduled;

	private StoreImpression m_pPreviousImpression;

	private int m_nStoreImpressionIndex;

	private const int START_SLOTPOOL_SIZE = 6;

	private Dictionary<int, SBMarketOffer> offers;

	private SBGUIAtlasImage infoWindow;

	private int slotNameCounter;

	public override Bounds TotalBounds
	{
		get
		{
			return default(Bounds);
		}
	}

	protected override SBGUIScrollListElement MakeSlot()
	{
		return null;
	}

	public override void Start()
	{
	}

	private void UpdateStoreImpressions(SBGUIScreen screen, Session session)
	{
	}

	private void AddEmptyStoreImpression()
	{
	}

	private bool AddStoreImpression()
	{
		return false;
	}

	private void FlushStoreImpressions()
	{
	}

	public float GetMainWindowZ()
	{
		return 0f;
	}

	protected override void LoadCategories(Session session)
	{
	}

	public override void SetManagers(Session session)
	{
	}

	public void LocalizeInitialLabel()
	{
	}

	protected override Vector2 GetSlotSize()
	{
		return default(Vector2);
	}

	protected override void BuildTabForButton(SBGUITabButton tab)
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

	private Action<SBGUIScrollListElement> SetupSlotClosure(SBGUIElement anchor, SBMarketOffer offer, EventDispatcher<SBMarketOffer> OfferClickedEvent, Vector2 offset, bool isDisabled, int? minLevelToShow, ResourceManager resourceMgr, EntityManager entityMgr, CostumeManager costumeMgr, Session session, RmtStore store, bool isAd = false)
	{
		return null;
	}

	protected override Rect CalculateTabContentsSize(string tabName)
	{
		return default(Rect);
	}

	public override void Deactivate()
	{
	}
}
