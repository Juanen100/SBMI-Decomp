using System;
using UnityEngine;

public class SBGUICharacterWishWidget : SBGUIElement
{
	private enum _eWishState
	{
		eFull = 0,
		eHungry = 1,
		eNone = 2
	}

	private SBGUIAtlasImage m_pWishIcon;

	private SBGUIAtlasImage m_pCurrencyImage;

	private SBGUILabel m_pWishNameLabel;

	private SBGUILabel m_pWishProgressLabel;

	private SBGUILabel m_pWishFullTimeLabel;

	private SBGUILabel m_pWishSoftRewardLabel;

	private SBGUILabel m_pWishXPRewardLabel;

	private SBGUILabel m_pWishRushCostLabel;

	private SBGUILabel m_pGrantWishButtonLabel;

	private SBGUILabel m_pWishCountLabel;

	private SBGUIProgressMeter m_pWishProgressMeter;

	private SBGUIPulseButton m_pGrantWishButton;

	private SBGUIButton m_pRushWishButton;

	private SBGUIButton m_pWatchAdButton;

	private GameObject m_pFullParent;

	private GameObject m_pHungryParent;

	private Simulated m_pSimulated;

	private ResidentEntity m_pEntity;

	private Session m_pSession;

	private Action m_pFeedWishAction;

	private Action m_pRushWishAction;

	private Action m_pOnWatchAdAction;

	private int? m_nHungerResourceID;

	private Vector2 m_pWishIconSize;

	private _eWishState m_eWishState;

	public void SetData(Session pSession, Simulated pSimulated, Action pFeedWishAction, Action pRushWishAction, Action onWatchAd)
	{
	}

	public void UpdateData()
	{
	}

	public Vector2 GetRushWishButtonPosition()
	{
		return default(Vector2);
	}

	private void SetVisualsForHungerResourceID(int nHungerResourceID)
	{
	}

	protected override void Awake()
	{
	}

	private void Update()
	{
	}
}
