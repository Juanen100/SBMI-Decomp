using System.Collections.Generic;

public class TaskRushAction : PersistedTriggerableAction
{
	public const string TASK_RUSH = "tra";

	private Task m_pTask;

	private Cost m_pCost;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TaskRushAction(Task pTask, Cost pCost)
		: base("tra", Identity.Null())
	{
		m_pTask = pTask;
		m_pCost = pCost;
	}

	public new static TaskRushAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		Cost pCost = Cost.FromDict((Dictionary<string, object>)pData["cost"]);
		return new TaskRushAction(pTask, pCost);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("task_invariable", m_pTask.m_pTaskData.GetInvariableData());
		dictionary.Add("cost", m_pCost.ToDict());
		return dictionary;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		Task activeTask = pGame.taskManager.GetActiveTask(m_pTask.m_pTaskData.m_nDID);
		if (activeTask != null)
		{
			activeTask.m_ulCompleteTime = ulUtcNow;
		}
		pGame.resourceManager.Apply(m_pCost, pGame);
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		int nDID = m_pTask.m_pTaskData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(dictionary, TaskData._sDID) == nDID && dictionary.ContainsKey(Task._sCOMPLETE_TIME))
			{
				dictionary[Task._sCOMPLETE_TIME] = TFUtils.EpochTime();
			}
		}
		ResourceManager.ApplyCostToGameState(m_pCost, pGameState);
		base.Confirm(pGameState);
	}
}
