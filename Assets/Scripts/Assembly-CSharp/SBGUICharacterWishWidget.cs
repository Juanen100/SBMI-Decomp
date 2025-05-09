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

	private GameObject m_pFullParent;

	private GameObject m_pHungryParent;

	private Simulated m_pSimulated;

	private ResidentEntity m_pEntity;

	private Session m_pSession;

	private Action m_pFeedWishAction;

	private Action m_pRushWishAction;

	private int? m_nHungerResourceID;

	private Vector2 m_pWishIconSize;

	private _eWishState m_eWishState;

	public void SetData(Session pSession, Simulated pSimulated, Action pFeedWishAction, Action pRushWishAction)
	{
		m_pSession = pSession;
		m_pSimulated = pSimulated;
		m_pFeedWishAction = pFeedWishAction;
		m_pRushWishAction = pRushWishAction;
		m_eWishState = _eWishState.eNone;
		m_nHungerResourceID = null;
		m_pEntity = m_pSimulated.GetEntity<ResidentEntity>();
		if (m_pEntity.HungerResourceId.HasValue)
		{
			m_nHungerResourceID = m_pEntity.HungerResourceId.Value;
			if (m_eWishState != _eWishState.eHungry)
			{
				m_pHungryParent.SetActive(true);
				m_pFullParent.SetActive(false);
				SetVisualsForHungerResourceID(m_nHungerResourceID.Value);
				m_pGrantWishButton.ClearClickEvents();
				AttachActionToButton(m_pGrantWishButton, m_pFeedWishAction);
				m_eWishState = _eWishState.eHungry;
			}
			return;
		}
		double num = m_pEntity.HungryAt - TFUtils.EpochTime();
		if (num < 0.0)
		{
			num = 0.0;
		}
		m_pWishProgressLabel.SetText(TFUtils.DurationToString((ulong)num));
		m_pWishProgressMeter.Progress = m_pEntity.FullnessPercentage();
		Cost cost = m_pEntity.FullnessRushCostNow();
		m_pWishRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
		if (m_eWishState != _eWishState.eFull)
		{
			m_pHungryParent.SetActive(false);
			m_pFullParent.SetActive(true);
			m_pRushWishButton.ClearClickEvents();
			AttachActionToButton(m_pRushWishButton, m_pRushWishAction);
			m_eWishState = _eWishState.eFull;
		}
	}

	public void UpdateData()
	{
		if (m_eWishState == _eWishState.eFull)
		{
			if (m_pEntity.HungerResourceId.HasValue)
			{
				m_nHungerResourceID = m_pEntity.HungerResourceId.Value;
				m_pHungryParent.SetActive(true);
				m_pFullParent.SetActive(false);
				SetVisualsForHungerResourceID(m_nHungerResourceID.Value);
				m_pGrantWishButton.ClearClickEvents();
				AttachActionToButton(m_pGrantWishButton, m_pFeedWishAction);
				m_eWishState = _eWishState.eHungry;
				return;
			}
			double num = m_pEntity.HungryAt - TFUtils.EpochTime();
			if (num < 0.0)
			{
				num = 0.0;
			}
			m_pWishProgressLabel.SetText(TFUtils.DurationToString((ulong)num));
			m_pWishProgressMeter.Progress = m_pEntity.FullnessPercentage();
			Cost cost = m_pEntity.FullnessRushCostNow();
			m_pWishRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
		}
		else if (m_pEntity.HungerResourceId.HasValue && m_pEntity.HungerResourceId.Value != m_nHungerResourceID.Value)
		{
			m_nHungerResourceID = m_pEntity.HungerResourceId.Value;
			SetVisualsForHungerResourceID(m_nHungerResourceID.Value);
			m_eWishState = _eWishState.eHungry;
		}
		if (m_nHungerResourceID.HasValue && m_pSession.TheGame.resourceManager.Resources[m_nHungerResourceID.Value].Amount <= 0)
		{
			if (m_pGrantWishButton.enabled)
			{
				m_pGrantWishButton.enabled = false;
				Color color = new Color(23f / 51f, 23f / 51f, 23f / 51f, 23f / 51f);
				m_pGrantWishButton.SetColor(color);
				m_pGrantWishButtonLabel.SetColor(color);
			}
		}
		else if (!m_pGrantWishButton.enabled)
		{
			m_pGrantWishButton.enabled = true;
			m_pGrantWishButton.SetColor(Color.white);
			m_pGrantWishButtonLabel.SetColor(new Color(0.043137256f, 0.2509804f, 0f, 255f));
		}
	}

	public Vector2 GetRushWishButtonPosition()
	{
		return m_pRushWishButton.GetScreenPosition();
	}

	private void SetVisualsForHungerResourceID(int nHungerResourceID)
	{
		Resource resource = m_pSession.TheGame.resourceManager.Resources[m_nHungerResourceID.Value];
		m_pWishIcon.SetSizeNoRebuild(m_pWishIconSize);
		m_pWishIcon.SetTextureFromAtlas(resource.GetResourceTexture(), true, false, true);
		m_pWishNameLabel.SetText(Language.Get(resource.Name));
		m_pWishFullTimeLabel.SetText(TFUtils.DurationToString((ulong)resource.FullnessTime));
		m_pWishXPRewardLabel.SetText(resource.Reward.LowestResourceValue(ResourceManager.XP).ToString());
		m_pWishCountLabel.SetText(resource.Amount.ToString());
		int num = resource.Reward.LowestResourceValue(ResourceManager.HARD_CURRENCY);
		if (num > 0)
		{
			m_pWishSoftRewardLabel.SetText(num.ToString());
			m_pCurrencyImage.SetTextureFromAtlas(m_pSession.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture());
		}
		else
		{
			num = resource.Reward.LowestResourceValue(ResourceManager.SOFT_CURRENCY);
			m_pWishSoftRewardLabel.SetText(num.ToString());
			m_pCurrencyImage.SetTextureFromAtlas(m_pSession.TheGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
		}
	}

	protected override void Awake()
	{
		m_pWishIcon = (SBGUIAtlasImage)FindChild("wish_icon");
		m_pCurrencyImage = (SBGUIAtlasImage)FindChild("soft_currency_icon");
		m_pWishNameLabel = (SBGUILabel)FindChild("wish_name_label");
		m_pWishProgressLabel = (SBGUILabel)FindChild("wish_progress_label");
		m_pWishFullTimeLabel = (SBGUILabel)FindChild("wish_full_time_label");
		m_pWishSoftRewardLabel = (SBGUILabel)FindChild("wish_soft_reward_label");
		m_pWishXPRewardLabel = (SBGUILabel)FindChild("wish_xp_reward_label");
		m_pWishRushCostLabel = (SBGUILabel)FindChild("wish_rush_cost_label");
		m_pGrantWishButtonLabel = (SBGUILabel)FindChild("grant_wish_label");
		m_pWishCountLabel = (SBGUILabel)FindChild("wish_count_label");
		m_pWishProgressMeter = (SBGUIProgressMeter)FindChild("wish_progress_meter");
		m_pGrantWishButton = (SBGUIPulseButton)FindChild("grant_wish_button");
		m_pRushWishButton = (SBGUIButton)FindChild("rush_wish_button");
		m_pFullParent = m_pRushWishButton.transform.parent.parent.gameObject;
		m_pHungryParent = m_pWishFullTimeLabel.transform.parent.gameObject;
		m_eWishState = _eWishState.eNone;
		m_pWishIconSize = m_pWishIcon.Size;
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("reward_title_label");
		sBGUILabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		base.Awake();
	}

	private void Update()
	{
		UpdateData();
	}
}
