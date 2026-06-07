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
	}

	public void SetData(Session pSession, CommunityEvent pEvent, CommunityEvent.Reward pReward, SoaringCommunityEvent.Reward pSoaringReward, bool bIsPurchasable, bool bHideCost = false)
	{
	}
}
