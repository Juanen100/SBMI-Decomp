using System;
using System.Collections.Generic;

public class TaskManager
{
	public class TaskBlockedStatus
	{
		[Flags]
		public enum _eTaskBlockedType : ulong
		{
			eNone = 0uL,
			eNoTask = 1uL,
			eActive = 2uL,
			eSource = 4uL,
			eTarget = 8uL,
			ePartner = 0x10uL,
			eLevel = 0x20uL,
			eSourceCostume = 0x40uL,
			ePartnerCostume = 0x80uL,
			eMicroEvent = 0x100uL,
			eActiveQuest = 0x200uL,
			eRepeatable = 0x400uL,
			eQuestUnlock = 0x800uL,
			eQuestRelock = 0x13E8uL
		}

		public _eTaskBlockedType m_eTaskBlockedType;

		public Dictionary<_eTaskBlockedType, int> m_pBlockVars;

		public void AddBlock(_eTaskBlockedType eTaskBlockedType, int nVar)
		{
		}
	}

	public enum _eBlueprintTaskingState
	{
		eNone = 0,
		eSource = 1,
		ePartner = 2,
		eTarget = 3,
		eNotInSim = 4,
		eNumTypes = 5
	}

	private Identity m_pAvailableSimulatedIdentity;

	private Dictionary<int, TaskData> m_pTaskDatas;

	private Dictionary<int, Task> m_pActiveTasks;

	private Dictionary<int, int> m_pBlueprintTaskMap;

	private Dictionary<string, List<int>> m_pSimulatedTaskMap;

	private Dictionary<int, int> m_pTaskCompletionCounts;

	public TaskData GetTaskData(int nDID, bool bDefaultActiveTaskData = false)
	{
		return null;
	}

	public List<TaskData> GetTaskDatasForSource(int nSourceDID, bool bDefaultActiveTaskData = false)
	{
		return null;
	}

	public List<int> GetActiveSourcesForTarget(Identity sIdentity)
	{
		return null;
	}

	public List<int> GetActiveSourcesWithMatchBonusForTarget(Simulation pSimulation, Identity sIdentity)
	{
		return null;
	}

	public bool IsTaskAvailable(Game pGame, int nDID, bool bDefaultActiveTaskData = false)
	{
		return false;
	}

	public bool IsTaskActive(int nDID)
	{
		return false;
	}

	public TaskBlockedStatus GetTaskBlockedStatus(Game pGame, TaskData pTaskData, int nOverwriteSourceCostumeDID = -1, Simulated pSimulated = null)
	{
		return null;
	}

	public string GetTaskBlockedStatusString(Game pGame, TaskData pTaskData, int nOverwriteSourceCostumeDID = -1)
	{
		return null;
	}

	public Task CreateActiveTask(Game pGame, int nTaskDID)
	{
		return null;
	}

	public void AddActiveTask(Game pGame, Task pTask, bool bLoading = false)
	{
	}

	public void RemoveActiveTask(int nDID)
	{
	}

	public Task GetActiveTask(int nTaskDID)
	{
		return null;
	}

	public List<Task> GetActiveTasksForSimulated(int nSimulatedDID, Identity pIdentity, bool bIncludeReadyToCollect = true)
	{
		return null;
	}

	public _eBlueprintTaskingState GetTaskingStateForSimulated(Simulation pSimulation, int nDID, Identity pIdentity, Simulated pSimulated)
	{
		return default(_eBlueprintTaskingState);
	}

	public string GetActiveDisplayStateForTarget(Identity pIdentity, out Task pTask)
	{
		pTask = null;
		return null;
	}

	public int GetTaskCompletionCount(int nDID)
	{
		return 0;
	}

	public void SetTaskCompletionCount(int nDID, int nCount)
	{
	}

	public void IncrementTaskCompletionCount(int nDID)
	{
	}

	private Task GetActiveTaskForDID(int nDID, bool bIncludeReadyToCollect)
	{
		return null;
	}

	public void RemoveUnsafeActiveTasks(Game pGame)
	{
	}

	private List<Task> GetActiveTasksForIdentity(Identity pIdentity, bool bIncludeReadyToCollect)
	{
		return null;
	}

	private Identity GetAvailableSimulatedIdentity(Game pGame, TaskData pTaskData, int nSimulatedDID, bool bShuffle = false, bool bRecalculate = true)
	{
		return null;
	}

	private void LoadFromSpreadsheet()
	{
	}
}
