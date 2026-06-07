using System.Collections.Generic;

public class MicroEvent
{
	public static string _sSTART_TIME;

	public static string _sCOMPLETE_TIME;

	public static string _sCLOSED;

	public ulong m_ulStartTime;

	public ulong? m_ulCompleteTime;

	public bool m_bIsClosed;

	public MicroEventData m_pMicroEventData { get; private set; }

	public MicroEvent(Game pGame, Dictionary<string, object> pInvariableData, bool bIgnoreNullMicroEventData = false)
	{
	}

	public MicroEvent(Game pGame, int nDID, ulong ulStartTime)
	{
	}

	public bool IsCompleted()
	{
		return false;
	}

	public bool IsActive()
	{
		return false;
	}

	public Dictionary<string, object> GetInvariableData()
	{
		return null;
	}
}
