using System.Collections;
using System.Collections.Generic;
using MTools;
using UnityEngine;

public class SBGUICommunityEventScreen : SBGUITabbedDialog
{
	public GameObject m_pRewardPrefab;

	public GameObject[] TabOneRewardTransforms;

	public GameObject[] TabTwoRewardTransforms;

	private static string _sTab1Name = "Tab1";

	private static string _sTab2Name = "Tab2";

	private MArray m_pGUIIndividualRewards;

	private MArray m_pGUICommunityRewards;

	private GameObject m_pBuyRewardGO;

	private SBGUILabel m_pRewardCostLabel;

	private SBGUILabel m_pRewardCostTitleLabel;

	private SBGUILabel m_pTabOneDescriptionOne;

	private GameObject m_pNextRecipeGO;

	private SBGUILabel m_pNextRecipeLabel;

	private SBGUILabel m_pNextRecipeCostLabel;

	private SBGUIAtlasImage m_pNextRecipeIconImage;

	private SBGUIAtlasImage m_pNextRecipeCostIconImage;

	private SBGUILabel m_pNextRecipeIconLabel;

	private SBGUIAtlasImage m_pSpecialCurrencyIcon;

	private SBGUILabel m_pSpecialCurrencyLabel;

	private SBGUILabel m_pHardCurrencyLabel;

	private SBGUIAtlasImage m_pLeftBannerImage;

	private SBGUIAtlasImage m_pRightBannerImage;

	private SBGUILabel m_pBannerTitle;

	private SBGUILabel m_pTabTwoDescriptionLabelOne;

	private SBGUILabel m_pTabTwoDescriptionLabelTwo;

	private SBGUIAtlasImage m_pTabTwoFooterImage;

	private SBGUILabel m_pCommunityCountLabel;

	private SBGUILabel m_pCommunityTotalLabel;

	private GameObject m_pCommunityProgressBarGO;

	private SBGUIProgressMeter m_pCommunityProgressMeter;

	private SBGUILabel m_pOfflineLabel;

	private SBGUIButton m_pNextItemButton;

	private bool m_bWaitingOnServer;

	public EventDispatcher<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> BuyButtonClickedEvent = new EventDispatcher<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward>();

	private static int _nBuyRewardBuildingID = -1;

	private static int _nBuyRewardRecipeID = -1;

	protected override void Awake()
	{
		m_pRewardCostLabel = (SBGUILabel)FindChild("get_next_item_label");
		m_pRewardCostTitleLabel = (SBGUILabel)FindChild("get_next_item_label_title");
		m_pTabOneDescriptionOne = (SBGUILabel)FindChild("tab_one_description_label_one");
		m_pBuyRewardGO = m_pRewardCostLabel.transform.parent.gameObject;
		m_pNextRecipeCostLabel = (SBGUILabel)FindChild("next_recipe_cost_label");
		m_pNextRecipeIconLabel = (SBGUILabel)FindChild("next_recipe_icon_label");
		m_pNextRecipeIconImage = (SBGUIAtlasImage)FindChild("next_recipe_icon");
		m_pNextRecipeCostIconImage = (SBGUIAtlasImage)FindChild("next_recipe_cost_icon");
		m_pNextRecipeLabel = (SBGUILabel)FindChild("next_recipe_title_label");
		m_pNextRecipeGO = m_pNextRecipeIconImage.transform.parent.gameObject;
		m_pLeftBannerImage = (SBGUIAtlasImage)FindChild("banner_image");
		m_pRightBannerImage = FindChild("banner_image_two").GetComponent<SBGUIAtlasImage>();
		m_pBannerTitle = (SBGUILabel)FindChild("banner_image_two_label");
		m_pNextItemButton = (SBGUIButton)FindChild("get_next_item_button");
		m_pSpecialCurrencyIcon = (SBGUIAtlasImage)FindChild("special_currency_icon");
		m_pSpecialCurrencyLabel = (SBGUILabel)FindChild("special_currency_label");
		m_pHardCurrencyLabel = (SBGUILabel)FindChild("hard_currency_label");
		m_pTabTwoDescriptionLabelOne = (SBGUILabel)FindChild("tab_two_description_label_one");
		m_pTabTwoDescriptionLabelTwo = (SBGUILabel)FindChild("tab_two_description_label_two");
		m_pTabTwoFooterImage = (SBGUIAtlasImage)FindChild("tab_two_footer_image");
		if (m_pTabTwoFooterImage != null)
		{
			Object.Destroy(m_pTabTwoFooterImage.gameObject);
		}
		m_pCommunityCountLabel = (SBGUILabel)FindChild("community_count_label");
		m_pCommunityTotalLabel = (SBGUILabel)FindChild("community_total_label");
		m_pCommunityProgressMeter = (SBGUIProgressMeter)FindChild("community_bar_meter");
		m_pCommunityProgressBarGO = m_pCommunityProgressMeter.transform.parent.parent.gameObject;
		m_pOfflineLabel = (SBGUILabel)FindChild("offline_label");
		base.Awake();
	}

	public void SetupButtons()
	{
		AttachActionToButton("get_next_item_button", BuyNextRewardButtonClick);
	}

	public Vector2 GetHardSpendButtonPosition()
	{
		if (m_pNextItemButton != null)
		{
			return m_pNextItemButton.GetScreenPosition();
		}
		return Vector2.zero;
	}

	protected override void LoadCategories(Session pSession)
	{
		categories = new Dictionary<string, SBTabCategory>();
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("name", _sTab1Name);
			dictionary.Add("label", _sTab1Name);
			dictionary.Add("display.material", activeEvent.m_sTabOneTexture);
			dictionary.Add("type", "buildings");
			SBTabCategory value = new SBCommunityEventCategory(dictionary);
			categories.Add("Tab1", value);
			dictionary = new Dictionary<string, object>();
			dictionary.Add("name", _sTab2Name);
			dictionary.Add("label", _sTab2Name);
			dictionary.Add("display.material", activeEvent.m_sTabTwoTexture);
			dictionary.Add("type", "buildings");
			value = new SBCommunityEventCategory(dictionary);
			categories.Add("Tab2", value);
			m_pLeftBannerImage.SetTextureFromAtlas(activeEvent.m_sLeftBannerTexture);
			m_pLeftBannerImage.ResetSize();
			m_pRightBannerImage.SetTextureFromAtlas(activeEvent.m_sRightBannerTexture);
			m_pBannerTitle.SetText(Language.Get(activeEvent.m_sRightBannerTitle));
			m_pTabOneDescriptionOne.SetText(Language.Get(activeEvent.m_sIndividualFooterText));
			m_pTabTwoDescriptionLabelOne.SetText(Language.Get(activeEvent.m_sCommunityHeaderText));
			m_pTabTwoDescriptionLabelTwo.SetText(Language.Get(activeEvent.m_sCommunityFooterText));
			m_pNextRecipeCostIconImage.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[activeEvent.m_nValueID].GetResourceTexture());
			m_pNextRecipeLabel.SetText(Language.Get("!!EVENT_NEXT_RECIPE"));
			m_pCommunityTotalLabel.SetText(Language.Get("!!EVENT_COMMUNITY_TOTAL"));
			m_pOfflineLabel.SetText(Language.Get("!!EVENT_OFFLINE"));
			m_pRewardCostTitleLabel.SetText(Language.Get("!!EVENT_BUY_ITEM_NOW"));
			if (session.TheGame.resourceManager.Resources.ContainsKey(activeEvent.m_nValueID))
			{
				m_pSpecialCurrencyIcon.SetTextureFromAtlas(session.TheGame.resourceManager.Resources[activeEvent.m_nValueID].GetResourceTexture(), true, false, true);
			}
			UpdateCurrency();
		}
	}

	private void UpdateCurrency()
	{
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent != null)
		{
			if (session.TheGame.resourceManager.Resources.ContainsKey(activeEvent.m_nValueID))
			{
				m_pSpecialCurrencyLabel.SetText(session.TheGame.resourceManager.Resources[activeEvent.m_nValueID].Amount.ToString());
			}
			m_pHardCurrencyLabel.SetText(session.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount.ToString());
		}
	}

	private void HideTabTwoHack()
	{
		m_pTabTwoDescriptionLabelOne.SetActive(false);
		m_pTabTwoDescriptionLabelTwo.SetActive(false);
		m_pCommunityCountLabel.SetActive(false);
		m_pCommunityTotalLabel.SetActive(false);
		m_pCommunityProgressBarGO.SetActive(false);
	}

	protected override IEnumerator BuildTabCoroutine(string sTabName)
	{
		if (!tabContents.TryGetValue(sTabName, out currentTab))
		{
			SBGUIElement pAnchor = SBGUIElement.Create(FindChild("window"));
			pAnchor.name = string.Format(sTabName);
			pAnchor.transform.localPosition = Vector3.zero;
			tabContents[sTabName] = pAnchor;
			currentTab = pAnchor;
			if (sTabName == _sTab1Name)
			{
				m_pTabOneDescriptionOne.transform.parent = currentTab.transform;
				m_pBuyRewardGO.transform.parent = currentTab.transform;
				m_pNextRecipeGO.transform.parent = currentTab.transform;
				HideTabTwoHack();
			}
			else
			{
				m_pTabTwoDescriptionLabelOne.transform.parent = currentTab.transform;
				m_pTabTwoDescriptionLabelTwo.transform.parent = currentTab.transform;
				m_pCommunityCountLabel.transform.parent = currentTab.transform;
				m_pCommunityTotalLabel.transform.parent = currentTab.transform;
				m_pCommunityProgressBarGO.transform.parent = currentTab.transform;
			}
		}
		CommunityEvent pEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (pEvent != null)
		{
			if (sTabName == _sTab1Name)
			{
				if (session.IsOnline())
				{
					m_pOfflineLabel.SetActive(false);
					currentTab.SetActive(true);
					HideTabTwoHack();
					RefreshIndividualRewardTab();
				}
				else
				{
					currentTab.SetActive(false);
					m_pOfflineLabel.SetActive(true);
				}
			}
			else if (session.IsOnline())
			{
				m_pOfflineLabel.SetActive(false);
				m_bWaitingOnServer = true;
				SoaringCommunityEventManager.SetValueFinished += HandleSoaringSetValueFinished;
				SBMISoaring.SetEventValue(session, int.Parse(pEvent.m_sID), session.TheGame.resourceManager.Resources[pEvent.m_nValueID].Amount);
				while (m_bWaitingOnServer)
				{
					yield return null;
				}
				SoaringCommunityEventManager.SetValueFinished -= HandleSoaringSetValueFinished;
				currentTab.SetActive(true);
				RefreshCommunityRewardTab();
			}
			else
			{
				currentTab.SetActive(false);
				m_pOfflineLabel.SetActive(true);
				m_pTabTwoDescriptionLabelTwo.SetActive(true);
			}
		}
		yield return null;
	}

	private void RefreshIndividualRewardTab()
	{
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (soaringCommunityEvent == null)
		{
			return;
		}
		if (m_pGUIIndividualRewards == null)
		{
			m_pGUIIndividualRewards = new MArray();
		}
		if (TabOneRewardTransforms == null)
		{
			return;
		}
		int num = TabOneRewardTransforms.Length;
		int num2 = m_pGUIIndividualRewards.count();
		SoaringCommunityEvent.Reward[] individualRewards = soaringCommunityEvent.IndividualRewards;
		int num3 = individualRewards.Length;
		int nextReward = GetNextReward("building");
		int nextReward2 = GetNextReward("recipe");
		int i = 0;
		int num4 = 0;
		for (; i < num3; i++)
		{
			if (num4 >= num)
			{
				break;
			}
			SoaringCommunityEvent.Reward reward = individualRewards[i];
			CommunityEvent.Reward reward2 = activeEvent.GetReward(reward.m_nID);
			if (!(reward2.m_sType == "recipe"))
			{
				SBGUICommunityEventReward sBGUICommunityEventReward;
				if (num4 < num2)
				{
					sBGUICommunityEventReward = (SBGUICommunityEventReward)m_pGUIIndividualRewards.objectAtIndex(num4);
				}
				else
				{
					TabOneRewardTransforms[num4].transform.parent = currentTab.transform;
					GameObject gameObject = (GameObject)Object.Instantiate(m_pRewardPrefab);
					Transform transform = gameObject.transform;
					transform.parent = TabOneRewardTransforms[num4].transform;
					transform.localPosition = Vector3.zero;
					sBGUICommunityEventReward = (SBGUICommunityEventReward)gameObject.GetComponent(typeof(SBGUICommunityEventReward));
				}
				sBGUICommunityEventReward.SetActive(true);
				sBGUICommunityEventReward.SetData(session, activeEvent, reward2, reward, nextReward == reward.m_nID);
				m_pGUIIndividualRewards.addObject(sBGUICommunityEventReward);
				num4++;
			}
		}
		int amount = session.TheGame.resourceManager.Resources[activeEvent.m_nValueID].Amount;
		m_pBuyRewardGO.SetActiveRecursively(nextReward != -1);
		if (nextReward != -1)
		{
			SoaringCommunityEvent.Reward reward = soaringCommunityEvent.GetReward(nextReward);
			int num5 = reward.m_nValue - amount;
			if (num5 > 0)
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				dictionary.Add(activeEvent.m_nValueID, num5);
				Dictionary<int, int> resourceAmounts = dictionary;
				num5 = session.TheGame.resourceManager.GetResourcesPackageCostInHardCurrencyValue(new Cost(resourceAmounts));
			}
			else
			{
				num5 = 0;
			}
			m_pRewardCostLabel.SetText(string.Format("{0:n0}", num5));
		}
		m_pNextRecipeGO.SetActiveRecursively(nextReward2 != -1);
		if (nextReward2 != -1)
		{
			SoaringCommunityEvent.Reward reward = soaringCommunityEvent.GetReward(nextReward2);
			string text = string.Format("{0:n0}", amount);
			string text2 = string.Format("{0:n0}", reward.m_nValue);
			m_pNextRecipeCostLabel.SetText(text + "/" + text2);
			CommunityEvent.Reward reward2 = activeEvent.GetReward(nextReward2);
			m_pNextRecipeIconImage.SetSizeNoRebuild(new Vector2(reward2.m_nWidth, reward2.m_nHeight));
			m_pNextRecipeIconImage.SetTextureFromAtlas(reward2.m_sTexture, true, false, true);
			m_pNextRecipeIconLabel.SetText(Language.Get(session.TheGame.resourceManager.Resources[nextReward2].Name));
		}
		m_pNextRecipeGO.SetActiveRecursively(nextReward2 != -1);
		if (nextReward2 != -1)
		{
			SoaringCommunityEvent.Reward reward = soaringCommunityEvent.GetReward(nextReward2);
			string text3 = string.Format("{0:n0}", amount);
			string text4 = string.Format("{0:n0}", reward.m_nValue);
			m_pNextRecipeCostLabel.SetText(text3 + "/" + text4);
			CommunityEvent.Reward reward2 = activeEvent.GetReward(nextReward2);
			m_pNextRecipeIconImage.SetSizeNoRebuild(new Vector2(reward2.m_nWidth, reward2.m_nHeight));
			m_pNextRecipeIconImage.SetTextureFromAtlas(reward2.m_sTexture, true, false, true);
			m_pNextRecipeIconLabel.SetText(Language.Get(session.TheGame.resourceManager.Resources[nextReward2].Name));
		}
		UpdateCurrency();
	}

	private void RefreshCommunityRewardTab()
	{
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (soaringCommunityEvent == null || TabTwoRewardTransforms == null)
		{
			return;
		}
		int num = TabTwoRewardTransforms.Length;
		if (m_pGUICommunityRewards == null)
		{
			m_pGUICommunityRewards = new MArray();
		}
		int num2 = m_pGUICommunityRewards.count();
		SoaringCommunityEvent.Reward[] communityRewards = soaringCommunityEvent.CommunityRewards;
		int num3 = communityRewards.Length;
		int num4 = 0;
		int nextCommunityReward = GetNextCommunityReward("building");
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int num7 = 0;
		for (; i < num3; i++)
		{
			SoaringCommunityEvent.Reward reward = communityRewards[i];
			CommunityEvent.Reward reward2 = activeEvent.GetReward(reward.m_nID);
			if (!(reward2.m_sType == "recipe"))
			{
				num5++;
				if (reward.m_nValue > num4)
				{
					num4 = reward.m_nValue;
				}
				if (num7 >= num)
				{
					break;
				}
				SBGUICommunityEventReward sBGUICommunityEventReward;
				if (num7 < num2)
				{
					sBGUICommunityEventReward = (SBGUICommunityEventReward)m_pGUICommunityRewards.objectAtIndex(num7);
				}
				else
				{
					TabTwoRewardTransforms[num7].transform.parent = currentTab.transform;
					GameObject gameObject = (GameObject)Object.Instantiate(m_pRewardPrefab);
					Transform transform = gameObject.transform;
					transform.parent = TabTwoRewardTransforms[num7].transform;
					transform.localPosition = Vector3.zero;
					sBGUICommunityEventReward = (SBGUICommunityEventReward)gameObject.GetComponent(typeof(SBGUICommunityEventReward));
				}
				if (nextCommunityReward == reward.m_nID)
				{
					num6 = num7;
				}
				sBGUICommunityEventReward.SetActive(true);
				sBGUICommunityEventReward.SetData(session, activeEvent, reward2, reward, nextCommunityReward == reward.m_nID, true);
				m_pGUICommunityRewards.addObject(sBGUICommunityEventReward);
				num7++;
			}
		}
		if (nextCommunityReward == -1)
		{
			m_pCommunityProgressMeter.Progress = 1f;
		}
		else
		{
			SoaringCommunityEvent.Reward reward = soaringCommunityEvent.GetReward(nextCommunityReward);
			float num8 = (float)num6 * (1f / (float)num5);
			int currentCommunityReward = GetCurrentCommunityReward("building");
			SoaringCommunityEvent.Reward reward3 = null;
			if (currentCommunityReward != -1)
			{
				reward3 = soaringCommunityEvent.GetReward(currentCommunityReward);
			}
			int num9;
			int num10;
			if (reward3 == null)
			{
				num9 = reward.m_nValue;
				num10 = soaringCommunityEvent.m_nCommunityValue;
			}
			else
			{
				num9 = reward.m_nValue - reward3.m_nValue;
				num10 = soaringCommunityEvent.m_nCommunityValue - reward3.m_nValue;
			}
			num8 += (float)num10 / (float)num9 * (1f / (float)num5);
			m_pCommunityProgressMeter.Progress = Mathf.Clamp01(num8);
		}
		string text = string.Format("{0:n0}", soaringCommunityEvent.m_nCommunityValue);
		string text2 = string.Format("{0:n0}", (nextCommunityReward != -1) ? soaringCommunityEvent.GetReward(nextCommunityReward).m_nValue : num4);
		m_pCommunityCountLabel.SetText(text + "/" + text2);
		if (m_pCommunityProgressMeter.Progress != 1f)
		{
			m_pTabTwoDescriptionLabelTwo.SetText(Language.Get(activeEvent.m_sCommunityFooterText));
			m_pTabTwoDescriptionLabelOne.SetActive(true);
		}
		else
		{
			m_pTabTwoDescriptionLabelTwo.SetText(Language.Get(activeEvent.m_sCommunityFooterAllUnlocksText));
			m_pTabTwoDescriptionLabelOne.SetActive(false);
		}
		UpdateCurrency();
	}

	private int GetNextReward(string sType)
	{
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (soaringCommunityEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent.Reward[] individualRewards = soaringCommunityEvent.IndividualRewards;
		int num = individualRewards.Length;
		for (int i = 0; i < num; i++)
		{
			SoaringCommunityEvent.Reward reward = individualRewards[i];
			if (!(activeEvent.GetReward(reward.m_nID).m_sType != sType) && !reward.m_bAcquired)
			{
				return reward.m_nID;
			}
		}
		return -1;
	}

	private int GetNextCommunityReward(string sType)
	{
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (soaringCommunityEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent.Reward[] communityRewards = soaringCommunityEvent.CommunityRewards;
		int num = communityRewards.Length;
		for (int i = 0; i < num; i++)
		{
			SoaringCommunityEvent.Reward reward = communityRewards[i];
			if (!(activeEvent.GetReward(reward.m_nID).m_sType != sType) && !reward.m_bAcquired)
			{
				return reward.m_nID;
			}
		}
		return -1;
	}

	private int GetCurrentCommunityReward(string sType)
	{
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (soaringCommunityEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent.Reward[] communityRewards = soaringCommunityEvent.CommunityRewards;
		int num = communityRewards.Length;
		SoaringCommunityEvent.Reward reward = null;
		for (int i = 0; i < num; i++)
		{
			SoaringCommunityEvent.Reward reward2 = communityRewards[i];
			if (activeEvent.GetReward(reward2.m_nID).m_sType != sType)
			{
				continue;
			}
			if (!reward2.m_bAcquired)
			{
				if (reward != null)
				{
					return reward.m_nID;
				}
				return -1;
			}
			reward = reward2;
		}
		return -1;
	}

	public bool IsBuyingReward()
	{
		if (_nBuyRewardBuildingID != -1 || _nBuyRewardRecipeID != -1)
		{
			return true;
		}
		return false;
	}

	public void BuyRewardCancel()
	{
		_nBuyRewardBuildingID = (_nBuyRewardRecipeID = -1);
	}

	public void BuyRewardConfirm(int nCost)
	{
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			BuyRewardCancel();
			return;
		}
		SoaringCommunityEventManager.AquireGiftFinished += HandleSoaringAquireGiftFinished;
		SBMISoaring.AquireEventGift(session, int.Parse(activeEvent.m_sID), _nBuyRewardBuildingID, nCost, true);
		if (_nBuyRewardRecipeID != -1)
		{
			SBMISoaring.AquireEventGift(session, int.Parse(activeEvent.m_sID), _nBuyRewardRecipeID, nCost, true);
		}
	}

	private void BuyNextRewardButtonClick()
	{
		if (IsBuyingReward())
		{
			return;
		}
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		_nBuyRewardBuildingID = GetNextReward("building");
		if (_nBuyRewardBuildingID != -1)
		{
			_nBuyRewardRecipeID = GetNextReward("recipe");
			SoaringCommunityEvent soaringCommunityEvent = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
			if (_nBuyRewardRecipeID != -1 && soaringCommunityEvent.GetReward(_nBuyRewardRecipeID).m_nValue != soaringCommunityEvent.GetReward(_nBuyRewardBuildingID).m_nValue)
			{
				_nBuyRewardRecipeID = -1;
			}
			if (BuyButtonClickedEvent != null)
			{
				BuyButtonClickedEvent.FireEvent(activeEvent, soaringCommunityEvent, soaringCommunityEvent.GetReward(_nBuyRewardBuildingID));
			}
		}
	}

	private void HandleSoaringAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		int num = pContext.soaringValue("giftDid");
		if (num == _nBuyRewardBuildingID)
		{
			_nBuyRewardBuildingID = -1;
			if (_nBuyRewardRecipeID == -1)
			{
				SoaringCommunityEventManager.AquireGiftFinished -= HandleSoaringAquireGiftFinished;
				RefreshIndividualRewardTab();
			}
		}
		else if (num == _nBuyRewardRecipeID)
		{
			_nBuyRewardRecipeID = -1;
			if (_nBuyRewardBuildingID == -1)
			{
				SoaringCommunityEventManager.AquireGiftFinished -= HandleSoaringAquireGiftFinished;
				RefreshIndividualRewardTab();
			}
		}
	}

	private void HandleSoaringSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		m_bWaitingOnServer = false;
	}
}
