using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIMarketplaceScreen : SBGUITabbedScrollableDialog
{
	public class StoreImpression
	{
		public List<SBMarketOffer> m_pOffers;

		public float m_fTimeDelta;

		public StoreImpression()
		{
			m_pOffers = new List<SBMarketOffer>();
			m_fTimeDelta = 0f;
		}
	}

	private const int START_SLOTPOOL_SIZE = 6;

	public GameObject slotPrefab;

	public EventDispatcher<SBMarketOffer> OfferClickedEvent = new EventDispatcher<SBMarketOffer>();

	private List<StoreImpression> m_pStoreImpressions;

	private bool m_bImpressionScheduled;

	private int m_nStoreImpressionIndex = -1;

	private Dictionary<int, SBMarketOffer> offers;

	private SBGUIAtlasImage infoWindow;

	private int slotNameCounter;

	public override Bounds TotalBounds
	{
		get
		{
			return FindChild("window").TotalBounds;
		}
	}

	protected override SBGUIScrollListElement MakeSlot()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(slotPrefab);
		gameObject.name = "MarketplaceSlot_" + slotNameCounter++;
		SBGUIMarketplaceSlot component = gameObject.GetComponent<SBGUIMarketplaceSlot>();
		component.SetVisibilityMode(false);
		return component;
	}

	public override void Start()
	{
		m_pStoreImpressions = new List<StoreImpression>();
		region.ScrollStopEvent.AddListener(delegate
		{
			AddStoreImpression();
		});
		UpdateCallback.AddListener(UpdateStoreImpressions);
		m_nStoreImpressionIndex = -1;
		base.Start();
	}

	private void UpdateStoreImpressions(SBGUIScreen screen, Session session)
	{
		if (m_bImpressionScheduled && AddStoreImpression())
		{
			m_bImpressionScheduled = false;
		}
		if (m_nStoreImpressionIndex >= 0 && m_nStoreImpressionIndex < m_pStoreImpressions.Count)
		{
			m_pStoreImpressions[m_nStoreImpressionIndex].m_fTimeDelta += Time.deltaTime;
		}
	}

	private void AddEmptyStoreImpression()
	{
		if (m_pStoreImpressions != null)
		{
			m_pStoreImpressions.Add(new StoreImpression());
			m_nStoreImpressionIndex++;
		}
	}

	private bool AddStoreImpression()
	{
		StoreImpression storeImpression = new StoreImpression();
		List<SBGUIScrollListElement> visibleSrollListElements = GetVisibleSrollListElements();
		if (visibleSrollListElements == null)
		{
			return false;
		}
		int count = visibleSrollListElements.Count;
		for (int i = 0; i < count; i++)
		{
			if (visibleSrollListElements[i] != null && visibleSrollListElements[i] is SBGUIMarketplaceSlot)
			{
				SBGUIMarketplaceSlot sBGUIMarketplaceSlot = visibleSrollListElements[i] as SBGUIMarketplaceSlot;
				if (sBGUIMarketplaceSlot.offer != null)
				{
					storeImpression.m_pOffers.Add(sBGUIMarketplaceSlot.offer);
				}
			}
		}
		if (storeImpression.m_pOffers.Count > 0)
		{
			m_pStoreImpressions.Add(storeImpression);
			m_nStoreImpressionIndex++;
		}
		return true;
	}

	private void FlushStoreImpressions()
	{
		if (m_pStoreImpressions != null)
		{
			if (m_bImpressionScheduled)
			{
				AddStoreImpression();
				m_bImpressionScheduled = false;
			}
			AnalyticsWrapper.LogStoreImpressions(session.TheGame, m_pStoreImpressions);
			m_pStoreImpressions = new List<StoreImpression>();
			m_nStoreImpressionIndex = -1;
		}
	}

	public float GetMainWindowZ()
	{
		return FindChild("window").transform.localPosition.z;
	}

	protected override void LoadCategories(Session session)
	{
		categories = new Dictionary<string, SBTabCategory>();
		offers = new Dictionary<int, SBMarketOffer>();
		Catalog catalog = session.TheGame.catalog;
		foreach (object item in (List<object>)catalog.CatalogDict["market"])
		{
			SBMarketCategory sBMarketCategory = new SBMarketCategory((Dictionary<string, object>)item);
			categories[sBMarketCategory.Name] = sBMarketCategory;
		}
		foreach (object item2 in (List<object>)catalog.CatalogDict["offers"])
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)item2;
			if (!dictionary.ContainsKey("show_in_store") || (bool)dictionary["show_in_store"])
			{
				SBMarketOffer sBMarketOffer = new SBMarketOffer((Dictionary<string, object>)item2);
				offers[sBMarketOffer.identity] = sBMarketOffer;
			}
		}
	}

	public override void SetManagers(Session session)
	{
		base.SetManagers(session);
		infoWindow = (SBGUIAtlasImage)FindChild("info_window");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("info_message");
		if (sBGUILabel != null)
		{
			sBGUILabel.SetText(TFUtils.AssignStorePlatformText("!!MARKETLABEL_JJ_WARNING"));
		}
	}

	public void LocalizeInitialLabel()
	{
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("name_label");
		sBGUILabel.SetText(Language.Get(sBGUILabel.Text));
	}

	protected override Vector2 GetSlotSize()
	{
		YGSprite component = slotPrefab.GetComponent<YGSprite>();
		return component.size * 0.01f;
	}

	protected override void BuildTabForButton(SBGUITabButton tab)
	{
		object obj = session.CheckAsyncRequest("target_store_tab");
		if (obj != null)
		{
			string tabName = (string)obj;
			ViewTab(tabName);
		}
		else
		{
			base.BuildTabForButton(tab);
		}
	}

	protected override IEnumerator BuildTabCoroutine(string tabName)
	{
		if (!categories.ContainsKey(tabName))
		{
			TFUtils.WarningLog(string.Format("Category {0} not found in catalog", tabName));
			yield break;
		}
		if (tabName == "rmt")
		{
			infoWindow.SetActive(true);
		}
		else
		{
			infoWindow.SetActive(false);
		}
		ClearCachedSlotInfos();
		region.ClearSlotActions();
		if (!tabContents.TryGetValue(tabName, out currentTab))
		{
			SBGUIElement anchor = SBGUIElement.Create(region.Marker);
			anchor.name = string.Format(tabName);
			anchor.transform.localPosition = Vector3.zero;
			tabContents[tabName] = anchor;
			currentTab = anchor;
		}
		yield return null;
		SBTabCategory category = categories[tabName];
		LoadSlotInfo(category, currentTab);
		m_bImpressionScheduled = true;
		AnalyticsWrapper.LogShopTabOpened(session.TheGame, Catalog.ConvertTypeToDeltaDNAType(tabName));
	}

	private void LoadSlotInfo(SBTabCategory tabCategory, SBGUIElement anchor)
	{
		SBMarketCategory sBMarketCategory = (SBMarketCategory)tabCategory;
		SBMarketOffer value = null;
		int num = 0;
		int num2 = resourceMgr.Query(ResourceManager.LEVEL);
		if (sBMarketCategory.Type == "rmt")
		{
			int num3 = 0;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Dictionary<string, SoaringPurchasable> soaringProducts = session.TheGame.store.soaringProducts;
			int num4 = -100;
			foreach (KeyValuePair<string, SoaringPurchasable> item in soaringProducts)
			{
				SoaringPurchasable soaringPurchasable = soaringProducts[item.Key];
				dictionary.Clear();
				int num5;
				int num6;
				switch (soaringPurchasable.Texture)
				{
				case "JellyfishJellyShortStack_Icon.png":
					num5 = 108;
					num6 = 120;
					break;
				case "JellyfishJellyCrateTower_Icon.png":
					num5 = 56;
					num6 = 122;
					break;
				case "JellyfishJellyCrateTowers_Icon.png":
					num5 = 116;
					num6 = 94;
					break;
				case "JellyfishJellyCratePyramid_Icon.png":
					num5 = 118;
					num6 = 112;
					break;
				case "JellyfishJellyBoat_Icon.png":
					num5 = 124;
					num6 = 62;
					break;
				case "JellyfishJellyTower_Icon.png":
					num5 = 94;
					num6 = 120;
					break;
				case "CoinStack_Portrait.png":
					num5 = 128;
					num6 = 128;
					break;
				case "CoinBarrel_Portrait.png":
					num5 = 94;
					num6 = 124;
					break;
				case "CoinChest_Portrait.png":
					num5 = 122;
					num6 = 118;
					break;
				case "CoinWheelbarrel_Portrait.png":
					num5 = 126;
					num6 = 118;
					break;
				default:
					num5 = 124;
					num6 = 124;
					break;
				}
				dictionary.Add("identity", num4 - num3);
				dictionary.Add("name", soaringPurchasable.DisplayName);
				dictionary.Add("description", soaringPurchasable.Description);
				dictionary.Add("result_type", "resource");
				dictionary.Add("cost", new Dictionary<string, object>());
				dictionary.Add("data", new Dictionary<string, object> { 
				{
					soaringPurchasable.ResourceType.ToString(),
					soaringPurchasable.Amount
				} });
				dictionary.Add("code", soaringPurchasable.ProductID);
				dictionary.Add("display", new Dictionary<string, object>
				{
					{ "model_type", "sprite" },
					{ "width", num5 },
					{ "height", num6 }
				});
				dictionary.Add("display.default", new Dictionary<string, object>
				{
					{ "texture", soaringPurchasable.Texture },
					{ "name", "default" }
				});
				value = new SBMarketOffer(dictionary);
				if (value.type == null)
				{
					value.type = sBMarketCategory.Type;
				}
				region.SetupSlotActions.Insert(num3, SetupSlotClosure(anchor, value, OfferClickedEvent, GetSlotOffset(num3), false, null, resourceMgr, entityMgr, costumeMgr, session, session.TheGame.store));
				string sessionActionId = SBGUIMarketplaceSlot.GetSessionActionId(value);
				if (sessionActionIdSearchRequests.Contains(sessionActionId))
				{
					sessionActionSlotMap[sessionActionId] = num3;
				}
				num3++;
			}
			if (num3 > 0)
			{
				PostLoadRegionContentInfo(region.SetupSlotActions.Count);
				return;
			}
		}
		int num7 = -1;
		float num8 = 0f;
		object obj = session.CheckAsyncRequest("target_store_did");
		if (obj != null)
		{
			num7 = (int)obj;
		}
		int num9 = 0;
		int num10 = 0;
		int[] dids = sBMarketCategory.Dids;
		foreach (int num11 in dids)
		{
			if (!offers.TryGetValue(num11, out value))
			{
				TFUtils.WarningLog(string.Format("Offer [{0}] not found", sBMarketCategory.Dids[num]));
				continue;
			}
			if (value.microEventDID >= 0)
			{
				MicroEvent microEvent = session.TheGame.microEventManager.GetMicroEvent(value.microEventDID);
				if (microEvent == null || !microEvent.IsActive() || (value.microEventOnly && microEvent.IsCompleted()))
				{
					continue;
				}
			}
			if (value.type == null)
			{
				value.type = sBMarketCategory.Type;
			}
			if (num11 == -255)
			{
				value.type = "path";
			}
			int? minLevelToShow = null;
			bool flag = false;
			Blueprint blueprint = EntityManager.GetBlueprint(value.type, value.identity, true);
			if (blueprint != null)
			{
				int num12 = (int)blueprint.Invariable["level.minimum"];
				if (session.TheGame.buildingUnlockManager.CheckBuildingUnlock(value.identity))
				{
					flag = false;
					value.itemLocked = false;
					if (blueprint.GetInstanceLimitByLevel(num2).HasValue)
					{
						int entityCount = entityMgr.GetEntityCount(EntityTypeNamingHelper.StringToType(value.type), value.identity);
						int? instanceLimitByLevel = blueprint.GetInstanceLimitByLevel(num2);
						if (instanceLimitByLevel.HasValue && instanceLimitByLevel.Value <= entityCount)
						{
							flag = true;
							value.itemLocked = true;
						}
					}
				}
				else if (num12 > num2 || num12 == -1)
				{
					flag = true;
					value.itemLocked = true;
					minLevelToShow = num12;
				}
				else if (blueprint.GetInstanceLimitByLevel(num2).HasValue)
				{
					int entityCount2 = entityMgr.GetEntityCount(EntityTypeNamingHelper.StringToType(value.type), value.identity);
					int? instanceLimitByLevel2 = blueprint.GetInstanceLimitByLevel(num2);
					if (instanceLimitByLevel2.HasValue && instanceLimitByLevel2.Value <= entityCount2)
					{
						flag = true;
						value.itemLocked = true;
					}
				}
			}
			if (blueprint != null)
			{
				int? instanceLimitByLevel3 = blueprint.GetInstanceLimitByLevel(num2);
				if (instanceLimitByLevel3.HasValue && entityMgr.GetEntityCount(EntityTypeNamingHelper.StringToType(value.type), value.identity) >= instanceLimitByLevel3.Value && flag)
				{
					region.SetupSlotActions.Insert(num, SetupSlotClosure(anchor, value, OfferClickedEvent, GetSlotOffset(num), flag, minLevelToShow, resourceMgr, entityMgr, costumeMgr, session, session.TheGame.store));
					num++;
					goto IL_07a3;
				}
			}
			num9++;
			region.SetupSlotActions.Insert(num9 - 1, SetupSlotClosure(anchor, value, OfferClickedEvent, GetSlotOffset(num9 - 1), flag, minLevelToShow, resourceMgr, entityMgr, costumeMgr, session, session.TheGame.store));
			num++;
			goto IL_07a3;
			IL_07a3:
			string sessionActionId2 = SBGUIMarketplaceSlot.GetSessionActionId(value);
			if (sessionActionIdSearchRequests.Contains(sessionActionId2))
			{
				sessionActionSlotMap[sessionActionId2] = num;
			}
			if (num7 != -1 && num11 == num7)
			{
				num8 = 0f;
				if (num != 0)
				{
					num8 = GetSlotOffset(num - 1).x;
				}
			}
		}
		region.ResetScroll();
		if (num8 != 0f)
		{
			num8 = region.MinScroll.x - num8;
			PostLoadRegionContentInfo(region.SetupSlotActions.Count, new Vector3(num8, region.MinScroll.y, region.MinScroll.z));
		}
		else
		{
			PostLoadRegionContentInfo(region.SetupSlotActions.Count);
		}
	}

	private Action<SBGUIScrollListElement> SetupSlotClosure(SBGUIElement anchor, SBMarketOffer offer, EventDispatcher<SBMarketOffer> OfferClickedEvent, Vector2 offset, bool isDisabled, int? minLevelToShow, ResourceManager resourceMgr, EntityManager entityMgr, CostumeManager costumeMgr, Session session, RmtStore store)
	{
		return delegate(SBGUIScrollListElement slot)
		{
			((SBGUIMarketplaceSlot)slot).Setup(anchor, offer, OfferClickedEvent, offset, isDisabled, minLevelToShow, resourceMgr, entityMgr, costumeMgr, session, session.TheGame.store);
		};
	}

	protected override Rect CalculateTabContentsSize(string tabName)
	{
		SBMarketCategory sBMarketCategory = (SBMarketCategory)categories[tabName];
		return CalculateScrollRegionSize(sBMarketCategory.Dids.Length);
	}

	public override void Deactivate()
	{
		SBGUIRewardWidget.ClearWidgetPool();
		OfferClickedEvent.ClearListeners();
		FlushStoreImpressions();
		m_bImpressionScheduled = false;
		session.marketpalceActive = false;
		base.Deactivate();
	}
}
