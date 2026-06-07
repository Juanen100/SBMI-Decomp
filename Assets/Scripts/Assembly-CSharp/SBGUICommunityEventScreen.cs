using System.Collections;
using System.Diagnostics;
using MTools;
using UnityEngine;

public class SBGUICommunityEventScreen : SBGUITabbedDialog
{
	public GameObject m_pRewardPrefab;

	public GameObject[] TabOneRewardTransforms;

	public GameObject[] TabTwoRewardTransforms;

	private static string _sTab1Name;

	private static string _sTab2Name;

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

	public EventDispatcher<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> BuyButtonClickedEvent;

	private static int _nBuyRewardBuildingID;

	private static int _nBuyRewardRecipeID;

	protected override void Awake()
	{
	}

	public void SetupButtons()
	{
	}

	public Vector2 GetHardSpendButtonPosition()
	{
		return default(Vector2);
	}

	protected override void LoadCategories(Session pSession)
	{
	}

	private void UpdateCurrency()
	{
	}

	private void HideTabTwoHack()
	{
	}

	[DebuggerHidden]
	protected override IEnumerator BuildTabCoroutine(string sTabName)
	{
		return null;
	}

	private void RefreshIndividualRewardTab()
	{
	}

	private void RefreshCommunityRewardTab()
	{
	}

	private int GetNextReward(string sType)
	{
		return 0;
	}

	private int GetNextCommunityReward(string sType)
	{
		return 0;
	}

	private int GetCurrentCommunityReward(string sType)
	{
		return 0;
	}

	public bool IsBuyingReward()
	{
		return false;
	}

	public void BuyRewardCancel()
	{
	}

	public void BuyRewardConfirm(int nCost)
	{
	}

	private void BuyNextRewardButtonClick()
	{
	}

	private void HandleSoaringAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}

	private void HandleSoaringSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}
}
