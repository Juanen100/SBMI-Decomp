#define ASSERTS_ON
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	public void Setup(SBGUIElement parent, SBMarketOffer offer, EventDispatcher<SBMarketOffer> offerClickedEvent, Vector3 offset, bool isDisabled, int? showLevelLock, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		base.gameObject.name = GetSessionActionId(offer);
		SetActive(true);
		base.tform.localPosition = offset;
		SetParent(parent);
		this.isDisabled = isDisabled;
		this.showLevelLock = showLevelLock;
		Setup(offer, offerClickedEvent, resourceManager, entityManager, costumeManager, session, store);
	}

	public static SBGUIMarketplaceSlot Create(GameObject prefab, SBGUIElement parent, SBMarketOffer offer, EventDispatcher<SBMarketOffer> offerClickedEvent, Vector3 offset, bool isDisabled, int? showLevelLock, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(prefab);
		gameObject.name = GetSessionActionId(offer);
		TFUtils.ErrorLog("go.name: " + gameObject.name);
		SBGUIMarketplaceSlot component = gameObject.GetComponent<SBGUIMarketplaceSlot>();
		component.Setup(parent, offer, offerClickedEvent, offset, isDisabled, showLevelLock, resourceManager, entityManager, costumeManager, session, store);
		return component;
	}

	private void Setup(SBMarketOffer o, EventDispatcher<SBMarketOffer> offerClickedEvent, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		StartCoroutine(SetupCoroutine(o, offerClickedEvent, resourceManager, entityManager, costumeManager, session, store));
	}

	public override void Deactivate()
	{
		ClearButtonActions("button");
		SBGUIRewardWidget[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIRewardWidget>(true);
		SBGUIRewardWidget[] array = componentsInChildren;
		foreach (SBGUIRewardWidget sBGUIRewardWidget in array)
		{
			sBGUIRewardWidget.SetParent(null);
			sBGUIRewardWidget.gameObject.SetActiveRecursively(false);
			SBGUIRewardWidget.ReleaseRewardWidget(sBGUIRewardWidget);
		}
		VisibleEvent.ClearListeners();
		InvisibleEvent.ClearListeners();
		muted = false;
		base.Deactivate();
	}

	private new Dictionary<string, SBGUIElement> CacheChildren()
	{
		Dictionary<string, SBGUIElement> dictionary = new Dictionary<string, SBGUIElement>();
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
		SBGUIElement[] array = componentsInChildren;
		foreach (SBGUIElement sBGUIElement in array)
		{
			dictionary.Add(sBGUIElement.name, sBGUIElement);
		}
		return dictionary;
	}

	private IEnumerator SetupCoroutine(SBMarketOffer o, EventDispatcher<SBMarketOffer> offerClickedEvent, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		offer = o;
		SetVisibilityMode(false);
		Blueprint blueprint = EntityManager.GetBlueprint(offer.type, offer.identity, true);
		int currentLevel = resourceManager.Query(ResourceManager.LEVEL);
		Dictionary<string, SBGUIElement> child = CacheChildren();
		GetChild getChild = (string x) => (!child.ContainsKey(x)) ? null : child[x];
		numberOwnedLabel = (SBGUILabel)getChild("owned_num_label");
		salePercentLabel = (SBGUILabel)getChild("sale_percent_text");
		RmtProduct prod = null;
		bool rmtDisabled = false;
		if (offer.type == "rmt")
		{
			if (!Soaring.IsOnline)
			{
				SoaringDebug.Log("Store Offline: Product Info Available: " + store.receivedProductInfo + " : Ready: " + store.RmtReady, LogType.Error);
				if (store.receivedProductInfo)
				{
					SoaringInternal.instance.ClearOfflineMode();
				}
			}
			if (!Soaring.IsOnline)
			{
				rmtDisabled = true;
				isDisabled = true;
			}
			else
			{
				TFUtils.Assert(offer.innerOffer != null, "We should not have an RMT offer without an offer code: " + offer.itemName);
				if (store.RmtReady && !store.rmtProducts.TryGetValue(offer.innerOffer, out prod))
				{
					TFUtils.DebugLog("Got catalog entry " + offer.innerOffer + " without corresponding store entry");
				}
				if (prod == null)
				{
					rmtDisabled = true;
					isDisabled = true;
				}
			}
		}
		offerIcon = (SBGUIAtlasImage)getChild("icon");
		if (offer.width != 0 && offer.height != 0)
		{
			offerIcon.SetSizeNoRebuild(new Vector2(offer.width, offer.height));
		}
		else if (showLevelLock.HasValue)
		{
			offerIcon.SetSizeNoRebuild(new Vector2(110f, 110f));
		}
		else
		{
			offerIcon.SetSizeNoRebuild(new Vector2(150f, 150f));
		}
		if (!string.IsNullOrEmpty(offer.texture))
		{
			offerIcon.SetTextureFromAtlas(offer.texture);
		}
		else if (!string.IsNullOrEmpty(offer.material))
		{
			offerIcon.SetTextureFromMaterialPath(offer.material);
		}
		else if (blueprint != null)
		{
			offerIcon.SetTextureFromAtlas((string)blueprint.Invariable["portrait"], true, false, true);
		}
		offerIcon.SetActive(true);
		yield return null;
		offerNameLabel = (SBGUILabel)getChild("name_label");
		string localizedNameLabel = null;
		string localizedSaleLabel = null;
		if (blueprint != null)
		{
			localizedNameLabel = (string)blueprint.Invariable["name"];
			localizedNameLabel = Language.Get(localizedNameLabel);
			offerNameLabel.SetText(localizedNameLabel);
		}
		else
		{
			offerNameLabel.SetText(string.Format("offer {0}", offer.identity));
		}
		offerNameLabel.SetActive(true);
		yield return null;
		offerCostLabel = (SBGUILabel)getChild("price_label");
		offerCostIcon = (SBGUIAtlasImage)getChild("purchase_icon");
		button = (SBGUIPulseButton)FindChild("button");
		Color? color = buttonDefaultColor;
		if (!color.HasValue)
		{
			buttonDefaultColor = button.GetComponent<YGSprite>().color;
		}
		if (offer.buttonTexture != null)
		{
			button.SetTextureFromAtlas(offer.buttonTexture, true);
			button.SetColor(Color.white);
			button.InitializePulser(button.Size, button.Amplitude, button.Period);
			Vector3 newPos = Vector3.zero;
			if (offer.buttonTexture == "StorePurchaseButton_Halloween.png")
			{
				newPos = Vector3.zero;
			}
			button.tform.localPosition = newPos;
		}
		else
		{
			button.SetTextureFromAtlas("StorePurchaseButton.png", true);
			SBGUIPulseButton sBGUIPulseButton = button;
			Color? color2 = buttonDefaultColor;
			sBGUIPulseButton.SetColor(color2.Value);
			button.InitializePulser(button.Size, button.Amplitude, button.Period);
			button.tform.localPosition = Vector3.zero;
		}
		base.gameObject.transform.Find("sale_group").gameObject.SetActive(false);
		base.gameObject.transform.Find("new_item_group").gameObject.SetActive(false);
		base.gameObject.transform.Find("limited_item_group").gameObject.SetActive(false);
		if (!isDisabled)
		{
			base.gameObject.transform.Find("window_dimmer").gameObject.SetActiveRecursively(false);
			base.gameObject.transform.Find("lock_group").gameObject.SetActiveRecursively(false);
			base.gameObject.transform.Find("button_group").gameObject.SetActiveRecursively(true);
			base.gameObject.transform.Find("button_placed_group").gameObject.SetActiveRecursively(false);
			base.gameObject.transform.Find("button_disabled_group").gameObject.SetActiveRecursively(false);
			if (offer.isNewItem)
			{
				base.gameObject.transform.Find("new_item_group").gameObject.SetActive(true);
			}
			if (offer.isSaleItem)
			{
				base.gameObject.transform.Find("sale_group").gameObject.SetActive(true);
				if (offer.salePercent != 0f)
				{
					salePercentLabel.SetText(offer.salePercent * 100f + " %");
				}
			}
			if (offer.isLimitedItem)
			{
				base.gameObject.transform.Find("limited_item_group").gameObject.SetActive(true);
			}
			if (prod != null)
			{
				string currencySymbol = "CurrencySymbol.png";
				string locale = Language.getDeviceLocale().ToLower();
				if (locale.Equals("gb"))
				{
					currencySymbol = "CurrencySymbol_Pound.png";
				}
				else if (locale.Equals("me"))
				{
					currencySymbol = "CurrencySymbol_Peso.png";
				}
				else if (locale.Equals("ru"))
				{
					currencySymbol = null;
				}
				if (currencySymbol != null)
				{
					offerCostIcon.SetTextureFromAtlas(currencySymbol, true);
				}
				else
				{
					offerCostIcon.SetActive(false);
				}
				string localizedPrice = prod.localizedprice;
				if (localizedPrice.Contains("$"))
				{
					localizedPrice = localizedPrice.Split('$').Last();
				}
				offerCostLabel.SetText(localizedPrice);
				CenterBuyButtonContents();
			}
			else if (offer.cost.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				if (offer.cost[ResourceManager.SOFT_CURRENCY] == 0)
				{
					offerCostLabel.SetText(Language.Get("!!PREFAB_FREE"));
					offerCostIcon.SetActive(false);
				}
				else
				{
					offerCostLabel.SetText(offer.cost[ResourceManager.SOFT_CURRENCY].ToString());
					offerCostIcon.SetTextureFromAtlas(resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
					offerCostIcon.ScaleToMaxSize(32);
				}
				CenterBuyButtonContents();
			}
			else if (offer.cost.ContainsKey(ResourceManager.HARD_CURRENCY))
			{
				offerCostLabel.SetText(offer.cost[ResourceManager.HARD_CURRENCY].ToString());
				offerCostIcon.SetTextureFromAtlas(resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture());
				offerCostIcon.ScaleToMaxSize(32);
				CenterBuyButtonContents();
			}
			else
			{
				TFUtils.Assert(offer.cost.Count == 1, "Offers should always have a single resource cost, found offer id " + offer.identity + " with " + offer.cost.Count);
				offerCostLabel.SetText(offer.cost[offer.cost.Keys.First()].ToString());
				offerCostIcon.SetTextureFromAtlas(resourceManager.Resources[offer.cost.Keys.First()].GetResourceTexture());
				offerCostIcon.ScaleToMaxSize(32);
				CenterBuyButtonContents();
			}
			EnableButtons(true);
			EventDispatcher<SBMarketOffer> offerClickedEvent2 = offerClickedEvent;
			AttachActionToButton("button", delegate
			{
				offerClickedEvent2.FireEvent(offer);
			});
		}
		else
		{
			EnableButtons(false);
			base.gameObject.transform.Find("button_group").gameObject.SetActiveRecursively(false);
			if (showLevelLock.HasValue)
			{
				base.gameObject.transform.Find("button_placed_group").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.Find("button_disabled_group").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.Find("window_dimmer").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.Find("lock_group").gameObject.SetActiveRecursively(true);
			}
			else if (rmtDisabled)
			{
				base.gameObject.transform.Find("button_placed_group").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.Find("button_disabled_group").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.Find("window_dimmer").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.Find("lock_group").gameObject.SetActiveRecursively(false);
				EnableButtons(true);
				SBGUILabel label = (SBGUILabel)FindChild("button_label_disabled");
				if (store.rmtEnabled)
				{
					label.SetText(Language.Get("!!PREFAB_OFFLINE"));
					AttachActionToButton("button_disabled", delegate
					{
						TFUtils.TriggerIAPOfflineWarning();
					});
				}
				else
				{
					AttachActionToButton("button_disabled", delegate
					{
						TFUtils.TriggerIAPDisabledWarning();
					});
				}
			}
			else
			{
				base.gameObject.transform.Find("button_placed_group").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.Find("button_disabled_group").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.Find("window_dimmer").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.Find("lock_group").gameObject.SetActiveRecursively(false);
			}
		}
		yield return null;
		descriptionLabel = (SBGUILabel)getChild("description_label");
		productionInfo = getChild("makes_info");
		rewardMarker = getChild("makes_icon");
		productionTimeLabel = (SBGUILabel)getChild("makes_per_hour_label");
		ownedInfo = getChild("owned_info");
		numberOwnedLabel = (SBGUILabel)getChild("owned_num_label");
		SBGUILabel unlocksTextLabel = (SBGUILabel)getChild("unlocks_at_text");
		if (showLevelLock.HasValue)
		{
			descriptionLabel.gameObject.SetActiveRecursively(false);
			productionInfo.gameObject.SetActiveRecursively(false);
			rewardMarker.gameObject.SetActiveRecursively(false);
			productionTimeLabel.gameObject.SetActiveRecursively(false);
			ownedInfo.gameObject.SetActiveRecursively(false);
			numberOwnedLabel.gameObject.SetActiveRecursively(false);
			unlocksTextLabel.gameObject.SetActiveRecursively(true);
			if (showLevelLock == -1)
			{
				unlocksTextLabel.SetText(Language.Get("!!PREFAB_UNLOCKED_AT_QUEST"));
			}
			else
			{
				unlocksTextLabel.SetText(string.Format(Language.Get("!!PREFAB_UNLOCKED_AT"), showLevelLock.ToString()));
			}
		}
		else
		{
			descriptionLabel.gameObject.SetActiveRecursively(true);
			productionInfo.gameObject.SetActiveRecursively(true);
			rewardMarker.gameObject.SetActiveRecursively(true);
			productionTimeLabel.gameObject.SetActiveRecursively(true);
			ownedInfo.gameObject.SetActiveRecursively(true);
			numberOwnedLabel.gameObject.SetActiveRecursively(true);
			unlocksTextLabel.gameObject.SetActiveRecursively(false);
			descriptionLabel.gameObject.SetActiveRecursively(true);
			switch (offer.type)
			{
			case "building":
			case "annex":
			{
				if (blueprint.Invariable["product"] != null)
				{
					RemoveDescriptionInfo();
					RewardDefinition rewardDef = (RewardDefinition)blueprint.Invariable["product"];
					SBGUIRewardWidget.SetupRewardWidget(resourceManager, rewardDef.Summary, string.Empty, 2, rewardMarker, 10f, false, Color.white, true);
					SBGUIElement clock = getChild("clock_icon");
					clock.SetActive(true);
					ulong productionTime = (ulong)blueprint.Invariable["time.production"];
					productionTimeLabel.SetText(TFUtils.DurationToString(productionTime));
					productionTimeLabel.SetActive(true);
				}
				else
				{
					RemoveProductionInfo();
					TFUtils.Assert(offer.description != null, "Rentless building is missing a description: " + offer.itemName);
					descriptionLabel.SetText(offer.description);
				}
				int? instanceLimitByLevel = blueprint.GetInstanceLimitByLevel(currentLevel);
				if (instanceLimitByLevel.HasValue && entityManager.GetEntityCount(EntityTypeNamingHelper.StringToType(offer.type), offer.identity) >= instanceLimitByLevel.Value)
				{
					isDisabled = true;
				}
				else
				{
					numberOwnedLabel.SetText(entityManager.GetEntityCount(EntityTypeNamingHelper.StringToType(o.type), o.identity).ToString());
				}
				numberOwnedLabel.SetActive(true);
				break;
			}
			case "rmt":
			case "path":
			case "expansion":
			case "resource":
				RemoveProductionInfo();
				RemoveOwnedInfo();
				offerNameLabel.SetText(offer.itemName);
				offerNameLabel.SetActive(true);
				descriptionLabel.SetText(offer.description);
				descriptionLabel.SetActive(true);
				break;
			case "costume":
			{
				RemoveProductionInfo();
				RemoveOwnedInfo();
				offerIcon.SetTextureFromAtlas(costumeManager.GetCostume(offer.identity).m_sPortrait, true, false, true);
				offerNameLabel.SetText(Language.Get(costumeManager.GetCostume(offer.identity).m_sName));
				offerNameLabel.SetActive(true);
				descriptionLabel.SetText(offer.description);
				descriptionLabel.SetActive(true);
				if (costumeManager.IsCostumeUnlocked(offer.identity))
				{
					base.gameObject.transform.Find("button_placed_group").gameObject.SetActiveRecursively(true);
					base.gameObject.transform.Find("button_disabled_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.Find("window_dimmer").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.Find("lock_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.Find("button_group").gameObject.SetActiveRecursively(false);
				}
				else if (CheckCostumeUnlockCriteriaFullfilled(costumeManager.GetCostume(offer.identity), session) && !costumeManager.GetCostume(offer.identity).m_bLockedViaCSpanel)
				{
					base.gameObject.transform.Find("lock_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.Find("button_disabled_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.Find("button_group").gameObject.SetActiveRecursively(true);
				}
				else
				{
					base.gameObject.transform.Find("lock_group").gameObject.SetActiveRecursively(true);
					base.gameObject.transform.Find("button_disabled_group").gameObject.SetActiveRecursively(true);
					base.gameObject.transform.Find("button_group").gameObject.SetActiveRecursively(false);
					unlocksTextLabel.SetText(Language.Get("!!PREFAB_UNLOCKED_AT_QUEST"));
					descriptionLabel.SetActive(false);
				}
				SBGUILabel buttonPlacedLabel = (SBGUILabel)getChild("button_label_placed");
				buttonPlacedLabel.SetText(Language.Get("!!PREFAB_ALREADY_OWNED"));
				break;
			}
			default:
				TFUtils.Assert(false, "Unknown Offer Type " + offer.type);
				break;
			}
		}
		SetVisibilityMode(true);
		FindChild("button").UpdateCollider();
	}

	private bool CheckCostumeUnlockCriteriaFullfilled(CostumeManager.Costume costume, Session session)
	{
		bool result = false;
		if (costume.m_nUnlockLevel > 0)
		{
			result = ((session.TheGame.resourceManager.PlayerLevelAmount >= costume.m_nUnlockLevel) ? true : false);
		}
		if (costume.m_nUnlockAssetDid > 0)
		{
			result = ((session.TheGame.inventory.HasItem(costume.m_nUnlockAssetDid) || session.TheGame.simulation.FindSimulated(costume.m_nUnlockAssetDid) != null) ? true : false);
		}
		if (costume.m_nUnlockQuest1 > 0 || costume.m_nUnlockQuest2 > 0)
		{
			result = ((session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest1) || session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest2)) ? true : false);
		}
		return result;
	}

	private void RemoveProductionInfo()
	{
		productionInfo.gameObject.SetActiveRecursively(false);
	}

	private void RemoveOwnedInfo()
	{
		ownedInfo.gameObject.SetActiveRecursively(false);
	}

	private void RemoveDescriptionInfo()
	{
		descriptionLabel.gameObject.SetActiveRecursively(false);
	}

	private void CenterBuyButtonContents()
	{
		offerCostLabel.tform.localPosition = new Vector3(0f, 0f, -0.1f);
		Vector3 localPosition = offerCostLabel.tform.localPosition;
		localPosition.x -= ((float)offerCostLabel.Width / 2f + 3f) * 0.01f;
		offerCostIcon.tform.localPosition = localPosition;
	}

	public static string GetSessionActionId(SBMarketOffer offer)
	{
		return string.Format("Slot_{0}", offer.identity);
	}

	public void SetVisibilityMode(bool viz)
	{
		SBGUIElement[] componentsInChildren = GetComponentsInChildren<SBGUIElement>(true);
		foreach (SBGUIElement sBGUIElement in componentsInChildren)
		{
			if (sBGUIElement != this)
			{
				sBGUIElement.SetVisible(viz);
			}
		}
	}
}
