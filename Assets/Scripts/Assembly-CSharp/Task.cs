#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class Task
{
	public static string _sSTART_TIME = "start_time";

	public static string _sCOMPLETE_TIME = "complete_time";

	public static string _sTARGET_ID = "target_id";

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
		if (!pInvariableData.ContainsKey(TaskData._sDID))
		{
			TFUtils.Assert(false, "Task | Invariable Data does not contain key: " + TaskData._sDID);
		}
		if (!pInvariableData.ContainsKey(_sSTART_TIME))
		{
			TFUtils.Assert(false, "Task | Invariable Data does not contain key: " + _sSTART_TIME);
		}
		int num = TFUtils.LoadInt(pInvariableData, TaskData._sDID);
		m_ulStartTime = TFUtils.LoadUlong(pInvariableData, _sSTART_TIME);
		m_ulCompleteTime = TFUtils.LoadUlong(pInvariableData, _sCOMPLETE_TIME);
		m_bMovingToTarget = false;
		m_bAtTarget = false;
		m_sTargetPrevDisplayState = null;
		m_ulMovingTimeStart = 0uL;
		if (pInvariableData.ContainsKey(_sTARGET_ID))
		{
			m_pTargetIdentity = new Identity(TFUtils.LoadString(pInvariableData, _sTARGET_ID));
		}
		m_pTaskData = null;
		if (pGame != null)
		{
			m_pTaskData = pGame.taskManager.GetTaskData(num);
		}
		if (m_pTaskData == null)
		{
			if (!bIgnoreNullTaskData)
			{
				TFUtils.Assert(false, "Task | Cannot find task data for did(invariable): " + num);
			}
			else
			{
				m_pTaskData = new TaskData(null, pInvariableData);
			}
		}
		else
		{
			m_pTaskData = new TaskData(m_pTaskData.ToDict(), pInvariableData);
		}
	}

	public Task(Game pGame, int nDID, ulong ulStartTime, Identity pTargetIdentity)
	{
		m_pTaskData = pGame.taskManager.GetTaskData(nDID);
		if (m_pTaskData == null)
		{
			TFUtils.Assert(false, "Task | Cannot find task data for did: " + nDID);
		}
		m_ulStartTime = ulStartTime;
		m_ulCompleteTime = ulStartTime + (ulong)m_pTaskData.m_nDuration;
		m_pTargetIdentity = pTargetIdentity;
		m_bMovingToTarget = false;
		m_bAtTarget = false;
		m_sTargetPrevDisplayState = null;
		m_ulMovingTimeStart = 0uL;
	}

	public void UpdateModifiableData(ulong ulStartTime, ulong ulCompleteTime)
	{
		m_ulStartTime = ulStartTime;
		m_ulCompleteTime = ulCompleteTime;
	}

	public static void UpdateModifiableDataForDict(Dictionary<string, object> pData, Task pTask)
	{
		if (pData.ContainsKey(_sSTART_TIME))
		{
			pData[_sSTART_TIME] = pTask.m_ulStartTime;
		}
		else
		{
			pData.Add(_sSTART_TIME, pTask.m_ulStartTime);
		}
		if (pData.ContainsKey(_sCOMPLETE_TIME))
		{
			pData[_sCOMPLETE_TIME] = pTask.m_ulCompleteTime;
		}
		else
		{
			pData.Add(_sCOMPLETE_TIME, pTask.m_ulCompleteTime);
		}
	}

	public Dictionary<string, object> GetInvariableData()
	{
		Dictionary<string, object> invariableData = m_pTaskData.GetInvariableData();
		invariableData.Add(_sSTART_TIME, m_ulStartTime);
		invariableData.Add(_sCOMPLETE_TIME, m_ulCompleteTime);
		if (m_pTargetIdentity != null)
		{
			invariableData.Add(_sTARGET_ID, m_pTargetIdentity.Describe());
		}
		return invariableData;
	}

	public ulong GetTimeLeft()
	{
		ulong result = 0uL;
		ulong num = TFUtils.EpochTime();
		ulong num2 = 0uL;
		if (m_bMovingToTarget && m_ulMovingTimeStart > m_ulStartTime)
		{
			num2 = num - m_ulMovingTimeStart;
		}
		if (m_ulCompleteTime + num2 > num)
		{
			result = m_ulCompleteTime + num2 - num;
		}
		return result;
	}

	public float GetTimeLeftPercentage()
	{
		ulong timeLeft = GetTimeLeft();
		ulong num = 0uL;
		if (m_bMovingToTarget && m_ulMovingTimeStart > m_ulStartTime)
		{
			num = TFUtils.EpochTime() - m_ulMovingTimeStart;
		}
		ulong num2 = m_ulCompleteTime + num - m_ulStartTime;
		return Mathf.Clamp01(1f - (float)((double)timeLeft / (double)num2));
	}

	public Cost RushCostNow()
	{
		return ResourceManager.CalculateTaskRushCost(GetTimeLeft());
	}
}
