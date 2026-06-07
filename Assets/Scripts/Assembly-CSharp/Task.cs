using System.Collections.Generic;

public class Task
{
	public static string _sSTART_TIME;

	public static string _sCOMPLETE_TIME;

	public static string _sTARGET_ID;

	public ulong m_ulStartTime;

	public ulong m_ulCompleteTime;

	public ulong m_ulMovingTimeStart;

	public bool m_bMovingToTarget;

	public bool m_bAtTarget;

	public string m_sTargetPrevDisplayState;

	public TaskData m_pTaskData { get; private set; }

	public Identity m_pTargetIdentity { get; private set; }

	public Task(Game pGame, Dictionary<string, object> pInvariableData, bool bIgnoreNullTaskData = false)
	{
	}

	public Task(Game pGame, int nDID, ulong ulStartTime, Identity pTargetIdentity)
	{
	}

	public void UpdateModifiableData(ulong ulStartTime, ulong ulCompleteTime)
	{
	}

	public static void UpdateModifiableDataForDict(Dictionary<string, object> pData, Task pTask)
	{
	}

	public Dictionary<string, object> GetInvariableData()
	{
		return null;
	}

	public ulong GetTimeLeft()
	{
		return 0uL;
	}

	public float GetTimeLeftPercentage()
	{
		return 0f;
	}

	public Cost RushCostNow()
	{
		return null;
	}
}
