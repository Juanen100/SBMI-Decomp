#define ASSERTS_ON
using System.Collections.Generic;

public class MicroEvent
{
	public static string _sSTART_TIME = "start_time";

	public static string _sCOMPLETE_TIME = "complete_time";

	public static string _sCLOSED = "closed";

	public ulong m_ulStartTime;

	public ulong? m_ulCompleteTime;

	public bool m_bIsClosed;

	public MicroEventData m_pMicroEventData { get; private set; }

	public MicroEvent(Game pGame, Dictionary<string, object> pInvariableData, bool bIgnoreNullMicroEventData = false)
	{
		if (!pInvariableData.ContainsKey(MicroEventData._sDID))
		{
			TFUtils.Assert(false, "MicroEvent | Invariable Data does not contain key: " + MicroEventData._sDID);
		}
		int num = TFUtils.LoadInt(pInvariableData, MicroEventData._sDID);
		m_ulStartTime = TFUtils.LoadUlong(pInvariableData, _sSTART_TIME);
		m_ulCompleteTime = TFUtils.TryLoadNullableUlong(pInvariableData, _sCOMPLETE_TIME);
		m_bIsClosed = TFUtils.LoadBool(pInvariableData, _sCLOSED);
		m_pMicroEventData = null;
		if (pGame != null)
		{
			m_pMicroEventData = pGame.microEventManager.GetMicroEventData(num);
		}
		if (m_pMicroEventData == null)
		{
			if (!bIgnoreNullMicroEventData)
			{
				TFUtils.Assert(false, "MicroEvent | Cannot find micro event data for did(invariable): " + num);
			}
			else
			{
				m_pMicroEventData = new MicroEventData(null, pInvariableData);
			}
		}
		else
		{
			m_pMicroEventData = new MicroEventData(m_pMicroEventData.ToDict(), pInvariableData);
		}
	}

	public MicroEvent(Game pGame, int nDID, ulong ulStartTime)
	{
		m_pMicroEventData = pGame.microEventManager.GetMicroEventData(nDID);
		if (m_pMicroEventData == null)
		{
			TFUtils.Assert(false, "MicroEvent | Cannot find micro event data for did: " + nDID);
		}
		m_ulStartTime = ulStartTime;
		m_ulCompleteTime = null;
		m_bIsClosed = false;
	}

	public bool IsCompleted()
	{
		return m_ulCompleteTime.HasValue;
	}

	public bool IsActive()
	{
		if (m_pMicroEventData.m_bClosedEvent)
		{
			return !m_bIsClosed;
		}
		return true;
	}

	public Dictionary<string, object> GetInvariableData()
	{
		Dictionary<string, object> invariableData = m_pMicroEventData.GetInvariableData();
		invariableData.Add(_sSTART_TIME, m_ulStartTime);
		invariableData.Add(_sCLOSED, m_bIsClosed);
		if (m_ulCompleteTime.HasValue)
		{
			invariableData.Add(_sCOMPLETE_TIME, m_ulCompleteTime.Value);
		}
		return invariableData;
	}
}
