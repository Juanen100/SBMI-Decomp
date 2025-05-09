using System;
using MTools;

public class SoaringCommunityEventManager
{
	private MDictionary m_pEvents;

	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> SetValueFinished;

	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> GetValueFinished;

	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> AquireGiftFinished;

	public SoaringCommunityEventManager()
	{
		m_pEvents = new MDictionary();
	}

	public SoaringCommunityEvent GetEvent(string sEventID)
	{
		if (m_pEvents == null || string.IsNullOrEmpty(sEventID))
		{
			return null;
		}
		return (SoaringCommunityEvent)m_pEvents.objectWithKey(sEventID);
	}

	public void _HandleSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string sEventID = pContext.soaringValue("eventDid").ToString();
		if (bSuccess)
		{
			SoaringCommunityEvent soaringCommunityEvent = GetEvent(sEventID);
			if (soaringCommunityEvent == null)
			{
				AddEvent(sEventID, pData);
			}
			else
			{
				soaringCommunityEvent.SetData(sEventID, pData);
			}
		}
		else if (pError.ErrorCode == 404)
		{
			RemoveEvent(sEventID);
		}
		if (SoaringCommunityEventManager.SetValueFinished != null)
		{
			SoaringCommunityEventManager.SetValueFinished(bSuccess, pError, pData, pContext);
		}
	}

	public void _HandleGetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string sEventID = pContext.soaringValue("eventDid").ToString();
		if (bSuccess)
		{
			SoaringCommunityEvent soaringCommunityEvent = GetEvent(sEventID);
			if (soaringCommunityEvent == null)
			{
				AddEvent(sEventID, pData);
			}
			else
			{
				soaringCommunityEvent.SetData(sEventID, pData);
			}
		}
		else if (pError.ErrorCode == 404)
		{
			RemoveEvent(sEventID);
		}
		if (SoaringCommunityEventManager.GetValueFinished != null)
		{
			SoaringCommunityEventManager.GetValueFinished(bSuccess, pError, pData, pContext);
		}
	}

	public void _HandleAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string sEventID = pContext.soaringValue("eventDid").ToString();
		int nID = pContext.soaringValue("giftDid");
		if (bSuccess)
		{
			SoaringCommunityEvent soaringCommunityEvent = GetEvent(sEventID);
			if (soaringCommunityEvent != null)
			{
				SoaringCommunityEvent.Reward reward = soaringCommunityEvent.GetReward(nID);
				if (reward != null)
				{
					reward._SetAquired(true);
				}
			}
		}
		else if (pError.ErrorCode == 404)
		{
			RemoveEvent(sEventID);
		}
		if (SoaringCommunityEventManager.AquireGiftFinished != null)
		{
			SoaringCommunityEventManager.AquireGiftFinished(bSuccess, pError, pData, pContext);
		}
	}

	private void AddEvent(string sEventID, SoaringDictionary pData)
	{
		string text = "Default";
		SoaringCommunityEvent soaringCommunityEvent = null;
		switch (text)
		{
		case "Default":
			soaringCommunityEvent = new SoaringCommunityEvent(sEventID, pData);
			break;
		}
		if (m_pEvents.containsKey(soaringCommunityEvent.m_sID))
		{
			RemoveEvent(soaringCommunityEvent.m_sID);
		}
		m_pEvents.addValue(soaringCommunityEvent, soaringCommunityEvent.m_sID);
	}

	private void RemoveEvent(string sEventID)
	{
		if (m_pEvents != null && !string.IsNullOrEmpty(sEventID))
		{
			m_pEvents.removeObjectWithKey(sEventID);
		}
	}
}
