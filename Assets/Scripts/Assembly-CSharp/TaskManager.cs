#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

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

		public TaskBlockedStatus()
		{
			m_eTaskBlockedType = _eTaskBlockedType.eNone;
			m_pBlockVars = new Dictionary<_eTaskBlockedType, int>();
		}

		public void AddBlock(_eTaskBlockedType eTaskBlockedType, int nVar)
		{
			m_eTaskBlockedType |= eTaskBlockedType;
			if (!m_pBlockVars.ContainsKey(eTaskBlockedType))
			{
				m_pBlockVars.Add(eTaskBlockedType, nVar);
			}
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

	public TaskManager()
	{
		LoadFromSpreadsheet();
		m_pActiveTasks = new Dictionary<int, Task>();
		m_pBlueprintTaskMap = new Dictionary<int, int>();
		m_pSimulatedTaskMap = new Dictionary<string, List<int>>();
		m_pTaskCompletionCounts = new Dictionary<int, int>();
	}

	public TaskData GetTaskData(int nDID, bool bDefaultActiveTaskData = false)
	{
		if (bDefaultActiveTaskData && m_pActiveTasks.ContainsKey(nDID))
		{
			return m_pActiveTasks[nDID].m_pTaskData;
		}
		if (m_pTaskDatas.ContainsKey(nDID))
		{
			return m_pTaskDatas[nDID];
		}
		return null;
	}

	public List<TaskData> GetTaskDatasForSource(int nSourceDID, bool bDefaultActiveTaskData = false)
	{
		List<TaskData> list = new List<TaskData>();
		foreach (KeyValuePair<int, TaskData> pTaskData2 in m_pTaskDatas)
		{
			if (bDefaultActiveTaskData && m_pActiveTasks.ContainsKey(pTaskData2.Key))
			{
				TaskData pTaskData = m_pActiveTasks[pTaskData2.Key].m_pTaskData;
				if (pTaskData.m_nSourceDID == nSourceDID)
				{
					list.Add(pTaskData);
				}
			}
			else
			{
				TaskData pTaskData = pTaskData2.Value;
				if (pTaskData.m_nSourceDID == nSourceDID)
				{
					list.Add(pTaskData);
				}
			}
		}
		return list;
	}

	public List<int> GetActiveSourcesForTarget(Identity sIdentity)
	{
		List<int> list = new List<int>();
		if (sIdentity == null)
		{
			return list;
		}
		foreach (KeyValuePair<int, Task> pActiveTask in m_pActiveTasks)
		{
			if (pActiveTask.Value.m_pTargetIdentity != null && pActiveTask.Value.m_pTargetIdentity.Equals(sIdentity))
			{
				list.Add(pActiveTask.Value.m_pTaskData.m_nSourceDID);
			}
		}
		return list;
	}

	public List<int> GetActiveSourcesWithMatchBonusForTarget(Simulation pSimulation, Identity sIdentity)
	{
		List<int> list = new List<int>();
		if (sIdentity == null)
		{
			return list;
		}
		foreach (KeyValuePair<int, Task> pActiveTask in m_pActiveTasks)
		{
			if (pActiveTask.Value.m_pTargetIdentity == null || !pActiveTask.Value.m_pTargetIdentity.Equals(sIdentity))
			{
				continue;
			}
			int nSourceDID = pActiveTask.Value.m_pTaskData.m_nSourceDID;
			Simulated simulated = pSimulation.FindSimulated(nSourceDID);
			if (simulated.HasEntity<ResidentEntity>())
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.MatchBonus != null)
				{
					list.Add(nSourceDID);
				}
			}
		}
		return list;
	}

	public bool IsTaskAvailable(Game pGame, int nDID, bool bDefaultActiveTaskData = false)
	{
		TaskData taskData = GetTaskData(nDID, bDefaultActiveTaskData);
		return GetTaskBlockedStatus(pGame, taskData).m_eTaskBlockedType == TaskBlockedStatus._eTaskBlockedType.eNone;
	}

	public bool IsTaskActive(int nDID)
	{
		return m_pActiveTasks.ContainsKey(nDID);
	}

	public TaskBlockedStatus GetTaskBlockedStatus(Game pGame, TaskData pTaskData, int nOverwriteSourceCostumeDID = -1)
	{
		TaskBlockedStatus taskBlockedStatus = new TaskBlockedStatus();
		if (pTaskData == null)
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eNoTask, 0);
			return taskBlockedStatus;
		}
		int nDID = pTaskData.m_nDID;
		if (IsTaskActive(nDID))
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eActive, nDID);
		}
		nDID = GetTaskCompletionCount(nDID);
		if (!pTaskData.m_bRepeatable && nDID > 0)
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eRepeatable, nDID);
		}
		Simulation simulation = pGame.simulation;
		Simulated simulated = null;
		ResidentEntity residentEntity = null;
		bool flag = false;
		nDID = pTaskData.m_nSourceDID;
		if (GetTaskingStateForSimulated(pGame.simulation, nDID, null) != _eBlueprintTaskingState.eNone)
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eSource, nDID);
		}
		simulated = simulation.FindSimulated(nDID);
		nDID = pTaskData.m_nSourceCostumeDID;
		flag = false;
		if (nDID >= 0)
		{
			if (nOverwriteSourceCostumeDID < 0)
			{
				if (simulated != null && simulated.HasEntity<ResidentEntity>())
				{
					residentEntity = simulated.GetEntity<ResidentEntity>();
					if (residentEntity != null && residentEntity.CostumeDID.HasValue && residentEntity.CostumeDID.Value == nDID)
					{
						flag = true;
					}
				}
			}
			else if (nOverwriteSourceCostumeDID == nDID)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eSourceCostume, nDID);
		}
		nDID = pTaskData.m_nTargetDID;
		if (nDID >= 0)
		{
			Identity availableSimulatedIdentity = GetAvailableSimulatedIdentity(pGame, pTaskData, nDID, true);
			if (availableSimulatedIdentity == null)
			{
				taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eTarget, nDID);
			}
		}
		nDID = pTaskData.m_nPartnerDID;
		if (nDID >= 0 && GetTaskingStateForSimulated(pGame.simulation, nDID, null) != _eBlueprintTaskingState.eNone)
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.ePartner, nDID);
		}
		flag = false;
		simulated = simulation.FindSimulated(nDID);
		nDID = pTaskData.m_nPartnerCostumeDID;
		if (nDID >= 0 && simulated != null && simulated.HasEntity<ResidentEntity>())
		{
			residentEntity = simulated.GetEntity<ResidentEntity>();
			if (residentEntity != null && residentEntity.CostumeDID.HasValue && residentEntity.CostumeDID.Value == nDID)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.ePartnerCostume, nDID);
		}
		nDID = pTaskData.m_nMinLevel;
		if (nDID > 0 && pGame.resourceManager.PlayerLevelAmount < nDID)
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eLevel, pGame.resourceManager.PlayerLevelAmount);
		}
		nDID = pTaskData.m_nMicroEventDID;
		if (nDID >= 0 && (pGame.microEventManager.GetMicroEvent(nDID) == null || !pGame.microEventManager.IsMicroEventActive(nDID)))
		{
			MicroEvent microEvent = pGame.microEventManager.GetMicroEvent(nDID);
			if (microEvent == null || !microEvent.IsActive() || (pTaskData.m_bEventOnly && microEvent.IsCompleted()))
			{
				taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eMicroEvent, nDID);
			}
		}
		nDID = pTaskData.m_nActiveQuestDID;
		if (nDID >= 0 && !pGame.questManager.IsQuestActive((uint)nDID))
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eActiveQuest, nDID);
		}
		nDID = pTaskData.m_nQuestUnlockDID;
		if (nDID >= 0 && !pGame.questManager.IsQuestCompleted((uint)nDID))
		{
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eQuestUnlock, nDID);
		}
		nDID = pTaskData.m_nQuestRelockDid;
		if (nDID >= 0 && pGame.questManager.IsQuestCompleted((uint)nDID))
		{
			nDID = pTaskData.m_nQuestReunlockDid;
			if (nDID >= 0 && !pGame.questManager.IsQuestCompleted((uint)nDID))
			{
				taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eQuestRelock, nDID);
			}
			taskBlockedStatus.AddBlock(TaskBlockedStatus._eTaskBlockedType.eQuestUnlock, nDID);
		}
		return taskBlockedStatus;
	}

	public string GetTaskBlockedStatusString(Game pGame, TaskData pTaskData, int nOverwriteSourceCostumeDID = -1)
	{
		TaskBlockedStatus taskBlockedStatus = GetTaskBlockedStatus(pGame, pTaskData, nOverwriteSourceCostumeDID);
		string text = null;
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eLevel) != TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			string text2 = text;
			text = text2 + Language.Get("!!TASK_LEVEL") + " " + pTaskData.m_nMinLevel + ", ";
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.ePartner) != TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, pTaskData.m_nPartnerDID);
			text = text + Language.Get((string)blueprint.Invariable["name"]) + ", ";
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eSourceCostume) != TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			CostumeManager.Costume costume = pGame.costumeManager.GetCostume(pTaskData.m_nSourceCostumeDID);
			text = text + Language.Get(costume.m_sName) + ", ";
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.ePartnerCostume) != TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			CostumeManager.Costume costume2 = pGame.costumeManager.GetCostume(pTaskData.m_nPartnerCostumeDID);
			text = text + Language.Get(costume2.m_sName) + ", ";
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eTarget) != TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			Blueprint blueprint2 = EntityManager.GetBlueprint(pTaskData.m_sTargetType, pTaskData.m_nTargetDID);
			text = text + Language.Get((string)blueprint2.Invariable["name"]) + ", ";
		}
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Remove(text.Length - 2);
		}
		return text;
	}

	public Task CreateActiveTask(Game pGame, int nTaskDID)
	{
		if (!IsTaskAvailable(pGame, nTaskDID))
		{
			return null;
		}
		TaskData taskData = GetTaskData(nTaskDID);
		Task task = null;
		if (taskData != null)
		{
			task = new Task(pGame, nTaskDID, TFUtils.EpochTime(), GetAvailableSimulatedIdentity(pGame, taskData, taskData.m_nTargetDID, true));
			AddActiveTask(pGame, task);
		}
		return task;
	}

	public void AddActiveTask(Game pGame, Task pTask, bool bLoading = false)
	{
		TaskData pTaskData = pTask.m_pTaskData;
		int nDID = pTaskData.m_nDID;
		if (!bLoading)
		{
			TaskBlockedStatus taskBlockedStatus = GetTaskBlockedStatus(pGame, pTaskData);
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eNoTask) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that doesn't exist");
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eActive) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that is already activated: " + nDID);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eLevel) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that does not meet the level requirement for task: " + nDID + " level: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.eLevel]);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eSource) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has a source in another task: " + nDID + " source: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.eSource]);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eTarget) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has an unavailable target for task: " + nDID + " target: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.eTarget]);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.ePartner) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has an unavailable partner for task: " + nDID + " partner: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.ePartner]);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eMicroEvent) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has a unopen micro event for task: " + nDID + " micro event: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.eMicroEvent]);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eActiveQuest) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has a inactive quest for task: " + nDID + " quest: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.eActiveQuest]);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eQuestUnlock) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has an incomplete quest for task: " + nDID + " quest: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.eQuestUnlock]);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eRepeatable) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that cannot be repeated again: " + nDID);
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskBlockedStatus._eTaskBlockedType.eQuestRelock) != TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that is blocked by questrelock: " + nDID + " quest: " + taskBlockedStatus.m_pBlockVars[TaskBlockedStatus._eTaskBlockedType.eQuestRelock]);
			}
		}
		if (m_pBlueprintTaskMap.ContainsKey(pTaskData.m_nSourceDID))
		{
			return;
		}
		m_pActiveTasks.Add(nDID, pTask);
		m_pBlueprintTaskMap.Add(pTaskData.m_nSourceDID, nDID);
		if (pTask.m_pTargetIdentity != null)
		{
			string key = pTask.m_pTargetIdentity.Describe();
			if (m_pSimulatedTaskMap.ContainsKey(key))
			{
				m_pSimulatedTaskMap[key].Add(nDID);
			}
			else
			{
				m_pSimulatedTaskMap.Add(pTask.m_pTargetIdentity.Describe(), new List<int> { nDID });
			}
		}
		if (pTaskData.m_nPartnerDID >= 0)
		{
			m_pBlueprintTaskMap.Add(pTaskData.m_nPartnerDID, nDID);
		}
	}

	public void RemoveActiveTask(int nDID)
	{
		if (m_pActiveTasks.ContainsKey(nDID))
		{
			Task task = m_pActiveTasks[nDID];
			TaskData pTaskData = task.m_pTaskData;
			m_pActiveTasks.Remove(nDID);
			m_pBlueprintTaskMap.Remove(pTaskData.m_nSourceDID);
			if (task.m_pTargetIdentity != null)
			{
				m_pSimulatedTaskMap[task.m_pTargetIdentity.Describe()].Remove(nDID);
			}
			int nPartnerDID = pTaskData.m_nPartnerDID;
			if (nPartnerDID >= 0)
			{
				m_pBlueprintTaskMap.Remove(nPartnerDID);
			}
		}
	}

	public Task GetActiveTask(int nTaskDID)
	{
		if (IsTaskActive(nTaskDID))
		{
			return m_pActiveTasks[nTaskDID];
		}
		return null;
	}

	public List<Task> GetActiveTasksForSimulated(int nSimulatedDID, Identity pIdentity, bool bIncludeReadyToCollect = true)
	{
		List<Task> activeTasksForIdentity = GetActiveTasksForIdentity(pIdentity, bIncludeReadyToCollect);
		Task activeTaskForDID = GetActiveTaskForDID(nSimulatedDID, bIncludeReadyToCollect);
		if (activeTaskForDID != null && !activeTasksForIdentity.Contains(activeTaskForDID))
		{
			activeTasksForIdentity.Add(activeTaskForDID);
		}
		return activeTasksForIdentity;
	}

	public _eBlueprintTaskingState GetTaskingStateForSimulated(Simulation pSimulation, int nDID, Identity pIdentity)
	{
		if (pSimulation.FindSimulated(nDID) == null)
		{
			return _eBlueprintTaskingState.eNotInSim;
		}
		if (!m_pBlueprintTaskMap.ContainsKey(nDID))
		{
			return _eBlueprintTaskingState.eNone;
		}
		Task task = m_pActiveTasks[m_pBlueprintTaskMap[nDID]];
		TaskData pTaskData = task.m_pTaskData;
		if (pTaskData.m_nSourceDID == nDID)
		{
			return _eBlueprintTaskingState.eSource;
		}
		if (task.m_pTargetIdentity != null && task.m_pTargetIdentity.Equals(pIdentity))
		{
			return _eBlueprintTaskingState.eTarget;
		}
		return _eBlueprintTaskingState.ePartner;
	}

	public string GetActiveDisplayStateForTarget(Identity pIdentity, out Task pTask)
	{
		List<Task> activeTasksForIdentity = GetActiveTasksForIdentity(pIdentity, false);
		pTask = null;
		int count = activeTasksForIdentity.Count;
		for (int i = 0; i < count; i++)
		{
			pTask = activeTasksForIdentity[i];
			if (pTask.m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate)
			{
				return pTask.m_pTaskData.m_sTargetDisplayState;
			}
		}
		pTask = null;
		return null;
	}

	public int GetTaskCompletionCount(int nDID)
	{
		if (m_pTaskCompletionCounts.ContainsKey(nDID))
		{
			return m_pTaskCompletionCounts[nDID];
		}
		return 0;
	}

	public void SetTaskCompletionCount(int nDID, int nCount)
	{
		if (m_pTaskCompletionCounts.ContainsKey(nDID))
		{
			m_pTaskCompletionCounts[nDID] = nCount;
		}
		else
		{
			m_pTaskCompletionCounts.Add(nDID, nCount);
		}
	}

	public void IncrementTaskCompletionCount(int nDID)
	{
		if (m_pTaskCompletionCounts.ContainsKey(nDID))
		{
			m_pTaskCompletionCounts[nDID] += 1;
		}
		else
		{
			m_pTaskCompletionCounts.Add(nDID, 1);
		}
	}

	private Task GetActiveTaskForDID(int nDID, bool bIncludeReadyToCollect)
	{
		if (m_pBlueprintTaskMap.ContainsKey(nDID) && m_pActiveTasks.ContainsKey(m_pBlueprintTaskMap[nDID]) && (bIncludeReadyToCollect || m_pActiveTasks[nDID].GetTimeLeft() != 0))
		{
			return m_pActiveTasks[m_pBlueprintTaskMap[nDID]];
		}
		return null;
	}

	public void RemoveUnsafeActiveTasks(Game pGame)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, Task> pActiveTask in m_pActiveTasks)
		{
			Task value = pActiveTask.Value;
			if (pGame.simulation.FindSimulated(value.m_pTaskData.m_nSourceDID) == null)
			{
				list.Add(value.m_pTaskData.m_nDID);
			}
			else if (value.m_pTargetIdentity != null && pGame.simulation.FindSimulated(value.m_pTargetIdentity) == null)
			{
				list.Add(value.m_pTaskData.m_nDID);
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			RemoveActiveTask(list[i]);
		}
	}

	private List<Task> GetActiveTasksForIdentity(Identity pIdentity, bool bIncludeReadyToCollect)
	{
		List<Task> list = new List<Task>();
		if (pIdentity != null)
		{
			string key = pIdentity.Describe();
			if (m_pSimulatedTaskMap.ContainsKey(key))
			{
				List<int> list2 = m_pSimulatedTaskMap[key];
				int count = list2.Count;
				for (int i = 0; i < count; i++)
				{
					if (m_pActiveTasks.ContainsKey(list2[i]) && (bIncludeReadyToCollect || m_pActiveTasks[list2[i]].GetTimeLeft() != 0))
					{
						list.Add(m_pActiveTasks[list2[i]]);
					}
				}
			}
		}
		return list;
	}

	private Identity GetAvailableSimulatedIdentity(Game pGame, TaskData pTaskData, int nSimulatedDID, bool bShuffle = false, bool bRecalculate = true)
	{
		if (!bRecalculate)
		{
			return m_pAvailableSimulatedIdentity;
		}
		if (nSimulatedDID < 0)
		{
			m_pAvailableSimulatedIdentity = null;
			return m_pAvailableSimulatedIdentity;
		}
		List<Simulated> list = pGame.simulation.FindAllSimulateds(nSimulatedDID);
		int num = list.Count;
		for (int i = 0; i < num; i++)
		{
			Simulated simulated = list[i];
			Task pTask;
			if (GetTaskingStateForSimulated(pGame.simulation, simulated.entity.DefinitionId, simulated.Id) != _eBlueprintTaskingState.eNone)
			{
				list.RemoveAt(i);
				i--;
				num--;
			}
			else if (pGame.inventory.HasItem(simulated.Id))
			{
				list.RemoveAt(i);
				i--;
				num--;
			}
			else if (pTaskData.m_eTaskType == TaskData._eTaskType.eActivate && GetActiveDisplayStateForTarget(simulated.Id, out pTask) != null)
			{
				list.RemoveAt(i);
				i--;
				num--;
			}
			else
			{
				if (!simulated.HasEntity<BuildingEntity>())
				{
					continue;
				}
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.HasDecorator<ActivatableDecorator>())
				{
					ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
					if (decorator.Activated == 0L)
					{
						list.RemoveAt(i);
						i--;
						num--;
					}
				}
			}
		}
		if (num <= 0)
		{
			m_pAvailableSimulatedIdentity = null;
			return m_pAvailableSimulatedIdentity;
		}
		if (bShuffle)
		{
			while (num > 1)
			{
				num--;
				int index = UnityEngine.Random.Range(0, num + 1);
				Simulated simulated = list[index];
				list[index] = list[num];
				list[num] = simulated;
			}
		}
		m_pAvailableSimulatedIdentity = list[0].Id;
		return m_pAvailableSimulatedIdentity;
	}

	private void LoadFromSpreadsheet()
	{
		m_pTaskDatas = new Dictionary<int, TaskData>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "Tasks";
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
			if (m_pTaskDatas.ContainsKey(intCell))
			{
				TFUtils.ErrorLog("Task Collision! DID: " + intCell);
				continue;
			}
			dictionary.Add(TaskData._sDID, intCell);
			dictionary.Add(TaskData._sSOURCE_DID, instance.GetIntCell(sheetIndex, rowIndex, "source unit did"));
			dictionary.Add(TaskData._sPARTNER_DID, -1);
			dictionary.Add(TaskData._sTARGET_DID, instance.GetIntCell(sheetIndex, rowIndex, "target did"));
			dictionary.Add(TaskData._sSOURCE_COSTUME_DID, instance.GetIntCell(sheetIndex, rowIndex, "source unit required costume"));
			dictionary.Add(TaskData._sPARTNER_COSTUME_DID, instance.GetIntCell(sheetIndex, rowIndex, "partner required costume"));
			dictionary.Add(TaskData._sMICRO_EVENT_DID, instance.GetIntCell(sheetIndex, rowIndex, "micro event did"));
			dictionary.Add(TaskData._sACTIVE_QUEST_DID, instance.GetIntCell(sheetIndex, rowIndex, "active during quest did"));
			dictionary.Add(TaskData._sQUEST_UNLOCK_DID, instance.GetIntCell(sheetIndex, rowIndex, "quest unlock did"));
			dictionary.Add(TaskData._sDURATION, instance.GetIntCell(sheetIndex, rowIndex, "duration"));
			dictionary.Add(TaskData._sMIN_LEVEL, instance.GetIntCell(sheetIndex, rowIndex, "min level"));
			dictionary.Add(TaskData._sPOS_OFFSET_TARG_X, instance.GetIntCell(sheetIndex, rowIndex, "position offset from target x"));
			dictionary.Add(TaskData._sPOS_OFFSET_TARG_Y, instance.GetIntCell(sheetIndex, rowIndex, "position offset from target y"));
			dictionary.Add(TaskData._sPARTNER_POS_OFFSET_TARG_X, instance.GetIntCell(sheetIndex, rowIndex, "partner position offset from target x"));
			dictionary.Add(TaskData._sPARTNER_POS_OFFSET_TARG_Y, instance.GetIntCell(sheetIndex, rowIndex, "partner position offset from target y"));
			dictionary.Add(TaskData._sSORT_ORDER, instance.GetIntCell(sheetIndex, rowIndex, "sort order"));
			dictionary.Add(TaskData._sHIDDEN_UNTIL_UNLOCKED, instance.GetIntCell(sheetIndex, rowIndex, "hidden until unlocked") == 1);
			dictionary.Add(TaskData._sSOURCE_FLIPPED, instance.GetIntCell(sheetIndex, rowIndex, "source facing") == 1);
			dictionary.Add(TaskData._sPARTNER_FLIPPED, instance.GetIntCell(sheetIndex, rowIndex, "partner facing") == 1);
			dictionary.Add(TaskData._sEVENT_ONLY, instance.GetIntCell(sheetIndex, rowIndex, "event only") == 1);
			dictionary.Add(TaskData._sREPEATABLE, instance.GetIntCell(sheetIndex, rowIndex, "repeatable") == 1);
			dictionary.Add(TaskData._sQUEST_RELOCK_DID, instance.GetIntCell(sheetIndex, rowIndex, "quest relock did"));
			dictionary.Add(TaskData._sQUEST_REUNLOCK_DID, instance.GetIntCell(sheetIndex, rowIndex, "quest re-unlock did"));
			dictionary.Add(TaskData._sWANDER_TIME, instance.GetFloatCell(sheetIndex, rowIndex, "wander time"));
			dictionary.Add(TaskData._sIDLE_TIME, instance.GetFloatCell(sheetIndex, rowIndex, "idle time"));
			dictionary.Add(TaskData._sMOVEMENT_SPEED, instance.GetFloatCell(sheetIndex, rowIndex, "movement speed"));
			dictionary.Add(TaskData._sNAME, instance.GetStringCell(sheetName, rowName, "name"));
			dictionary.Add(TaskData._sTARGET_TYPE, instance.GetStringCell(sheetName, rowName, "target type"));
			dictionary.Add(TaskData._sSOURCE_DISPLAY_STATE_WALK, instance.GetStringCell(sheetName, rowName, "source unit display state walk"));
			dictionary.Add(TaskData._sPARTNER_DISPLAY_STATE_WALK, instance.GetStringCell(sheetName, rowName, "partner display state walk"));
			dictionary.Add(TaskData._sSOURCE_DISPLAY_STATE_IDLE, instance.GetStringCell(sheetName, rowName, "source unit display state idle"));
			dictionary.Add(TaskData._sPARTNER_DISPLAY_STATE_IDLE, instance.GetStringCell(sheetName, rowName, "partner display state idle"));
			dictionary.Add(TaskData._sTARGET_DISPLAY_STATE, instance.GetStringCell(sheetName, rowName, "target display state"));
			dictionary.Add(TaskData._sSTART_VO, instance.GetStringCell(sheetName, rowName, "start vo"));
			dictionary.Add(TaskData._sFINISH_VO, instance.GetStringCell(sheetName, rowName, "finish vo"));
			dictionary.Add(TaskData._sSTART_SOUND, instance.GetStringCell(sheetName, rowName, "start sound"));
			dictionary.Add(TaskData._sFINISH_SOUND, instance.GetStringCell(sheetName, rowName, "finish sound"));
			dictionary.Add(TaskData._sPAYTABLE_REWARD_ICON, instance.GetStringCell(sheetName, rowName, "paytable reward icon"));
			dictionary.Add(TaskData._sREWARD, new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object>()
				},
				{
					"thought_icon",
					instance.GetStringCell(sheetIndex, rowIndex, "reward thought icon")
				}
			});
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "reward gold");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary[TaskData._sREWARD])["resources"]).Add("3", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "reward special did");
			if (intCell2 >= 0)
			{
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary[TaskData._sREWARD])["resources"]).Add(intCell2.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "reward special amount"));
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "reward xp");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary[TaskData._sREWARD])["resources"]).Add("5", intCell2);
			}
			TaskData._eTaskType eTaskType = TaskData._eTaskType.eNumTypes;
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "movement type");
			switch (stringCell)
			{
			case "enter":
				eTaskType = TaskData._eTaskType.eEnter;
				break;
			case "wander":
				eTaskType = TaskData._eTaskType.eWander;
				break;
			case "stand":
				eTaskType = TaskData._eTaskType.eStand;
				break;
			case "activate":
				eTaskType = TaskData._eTaskType.eActivate;
				break;
			}
			if (eTaskType == TaskData._eTaskType.eNumTypes)
			{
				TFUtils.Assert(false, "Unsupported movement type (" + stringCell + ") for task did (" + intCell + ").");
			}
			else
			{
				dictionary.Add(TaskData._sTASK_TYPE, (int)eTaskType);
				m_pTaskDatas.Add(intCell, new TaskData(dictionary, null));
			}
		}
	}
}
