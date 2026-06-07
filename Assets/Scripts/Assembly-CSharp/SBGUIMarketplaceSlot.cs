using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUIMarketplaceSlot : SBGUIScrollListElement
{
	private delegate SBGUIElement GetChild(string key);

	private const int GAP_SIZE = 6;

	private const int MAX_SLOT_ICON_SIZE = 150;

	private const int MAX_LOCKED_SLOT_ICON_SIZE = 110;

	private const int MAX_COST_ICON_SIZE = 32;

	private const int MAX_REWARDS = 2;

	private const int REWARD_GAP_SIZE = 10;

	public SBGUIPulseButton button;

	private Color? buttonDefaultColor;

	protected SBGUIAtlasImage offerIcon;

	protected SBGUILabel offerNameLabel;

	protected SBGUILabel offerCostLabel;

	protected SBGUIAtlasImage offerCostIcon;

	protected SBGUIElement productionInfo;

	protected SBGUILabel productionTimeLabel;

	protected SBGUIElement rewardMarker;

	protected SBGUIElement ownedInfo;

	protected SBGUILabel numberOwnedLabel;

	protected SBGUILabel descriptionLabel;

	protected SBGUILabel salePercentLabel;

	public bool isDisabled;

	public int? showLevelLock;

	public SBMarketOffer offer { get; private set; }

	public void Setup(SBGUIElement parent, SBMarketOffer offer, EventDispatcher<SBMarketOffer> offerClickedEvent, Vector3 offset, bool isDisabled, int? showLevelLock, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store, bool isAd = false)
	{
	}

	public static SBGUIMarketplaceSlot Create(GameObject prefab, SBGUIElement parent, SBMarketOffer offer, EventDispatcher<SBMarketOffer> offerClickedEvent, Vector3 offset, bool isDisabled, int? showLevelLock, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store, bool isAd = false)
	{
		return null;
	}

	private void Setup(SBMarketOffer o, EventDispatcher<SBMarketOffer> offerClickedEvent, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store, bool isAd = false)
	{
	}

	public override void Deactivate()
	{
	}

	private new Dictionary<string, SBGUIElement> CacheChildren()
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator SetupCoroutine(SBMarketOffer o, EventDispatcher<SBMarketOffer> offerClickedEvent, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store, bool isAd = false)
	{
		return null;
	}

	private bool CheckCostumeUnlockCriteriaFullfilled(CostumeManager.Costume costume, Session session)
	{
		return false;
	}

	private void RemoveProductionInfo()
	{
	}

	private void RemoveOwnedInfo()
	{
	}

	private void RemoveDescriptionInfo()
	{
	}

	private void CenterBuyButtonContents()
	{
	}

	public static string GetSessionActionId(SBMarketOffer offer)
	{
		return null;
	}

	public void SetVisibilityMode(bool viz)
	{
	}
}
