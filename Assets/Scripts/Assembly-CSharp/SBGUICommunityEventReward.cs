using UnityEngine;

public class SBGUICommunityEventReward : SBGUIElement
{
	public SBGUIAtlasImage m_pRewardImage;

	public SBGUIAtlasImage m_pLockedImage;

	public Color m_pLockedColor;

	public SBGUILabel m_pRewardLabel;

	public GameObject m_pCurrencyGO;

	public SBGUILabel m_pValueLabel;

	public SBGUIAtlasImage m_pNextImage;

	public SBGUIAtlasImage m_pCurrencyImage;

	private Vector2 m_pRewardSize;

	protected override void Awake()
	{
		base.Awake();
		m_pRewardSize = m_pRewardImage.Size;
	}

	public void SetData(Session pSession, CommunityEvent pEvent, CommunityEvent.Reward pReward, SoaringCommunityEvent.Reward pSoaringReward, bool bIsPurchasable, bool bHideCost = false)
	{
		bool flag = pSoaringReward.m_bUnlocked || pSoaringReward.m_bAcquired;
		Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, pReward.m_nID, true);
		m_pRewardImage.SetSizeNoRebuild(m_pRewardSize);
		if (!flag && !string.IsNullOrEmpty(pReward.m_sLockedTexture))
		{
			m_pRewardImage.SetTextureFromAtlas(pReward.m_sLockedTexture, true, false, true);
			m_pRewardImage.SetColor(Color.white);
			m_pRewardLabel.SetText(string.Empty);
		}
		else
		{
			m_pRewardImage.SetTextureFromAtlas(pReward.m_sTexture, true, false, true);
			m_pRewardImage.SetColor(flag ? Color.white : m_pLockedColor);
			m_pRewardLabel.SetText(Language.Get((string)blueprint.Invariable["name"]));
		}
		m_pLockedImage.SetActive(!flag);
		m_pCurrencyImage.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pEvent.m_nValueID].GetResourceTexture());
		if (flag)
		{
			m_pLockedImage.SetActive(false);
		}
		else
		{
			m_pLockedImage.SetActive(true);
			if (pReward.m_bHideNameWhenLocked)
			{
				m_pRewardLabel.SetText(string.Empty);
			}
		}
		if (bIsPurchasable)
		{
			if (!bHideCost)
			{
				m_pCurrencyGO.SetActiveRecursively(true);
				m_pValueLabel.SetText(string.Format("{0:n0}", pSoaringReward.m_nValue));
			}
			else
			{
				m_pCurrencyGO.SetActiveRecursively(false);
				m_pNextImage.SetActive(true);
			}
		}
		else
		{
			m_pCurrencyGO.SetActiveRecursively(false);
			m_pNextImage.SetActive(false);
		}
	}
}
