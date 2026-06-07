using System;
using System.Collections.Generic;
using MTools;

public class CommunityEventManager
{
	public static Dictionary<string, object> _pEventStatusDialogData;

	public static string _sSpongyGamesEventID;

	public static string _sSpongyGamesLastDayEventID;

	public static string _sChrismas14EventID;

	public static int _nColiseumDID;

	public static string _sValentines15EventID;

	private Action dialogNeededCallback;

	private MDictionary m_pCommunityEventDefinitions;

	private const int m_nUpdateEventValueTimeLimit = 300;

	private float m_fUpdateEventValueTimer;

	private const int m_nUpdateEventMinWaitTime = 10;

	private float m_fPreviousValueUpdateTime;

	private float m_fPreviousRewardUpdateTime;

	private float m_fPreviousBannerPingTime;

	private Session m_pSession;

	public Action DialogNeededCallback
	{
		set
		{
		}
	}

	public CommunityEventManager(Session pSession)
	{
	}

	public Session GetSession()
	{
		return null;
	}

	public void DialogNeeded()
	{
	}

	public void QuestComplete(uint nQuestID)
	{
	}

	private void HandleSoaringAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}

	private void HandleSoaringCallFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}

	public CommunityEvent GetActiveEvent()
	{
		return null;
	}

	public CommunityEvent[] GetEvents()
	{
		return null;
	}

	public void OnUpdate(Session pSession)
	{
	}

	private void CheckValueUpdate()
	{
	}

	private void CheckClaimRewardUpdate()
	{
	}

	private void CheckEventBannerPing()
	{
	}

	private void UpdateValuesToSoaring()
	{
	}

	private void UpdateValueToSoaring(CommunityEvent pEvent)
	{
	}

	private void AquireEventGift(CommunityEvent pEvent, SoaringCommunityEvent.Reward pSoaringReward)
	{
	}

	private void LoadCommunityEventsFromSpreadsheets()
	{
	}
}
