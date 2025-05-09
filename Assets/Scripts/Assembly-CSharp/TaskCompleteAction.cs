using System.Collections.Generic;

public class TaskCompleteAction : PersistedSimulatedAction
{
	public const string TASK_COMPLETE = "tca";

	public const string PICKUP_TRIGGERTYPE = "TaskPickup";

	private Task m_pTask;

	private Reward m_pReward;

	private int m_nTaskCompletionCount;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TaskCompleteAction(Identity ID, Task pTask, Reward pReward, int nTaskCompletionCount)
		: base("tca", ID, "TaskPickup")
	{
		m_pTask = pTask;
		m_pReward = pReward;
		m_nTaskCompletionCount = nTaskCompletionCount;
	}

	public new static TaskCompleteAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		Reward pReward = Reward.FromObject(pData["reward"]);
		Identity iD = new Identity((string)pData["target"]);
		int? num = TFUtils.TryLoadInt(pData, "task_completion_count");
		TaskCompleteAction taskCompleteAction = new TaskCompleteAction(iD, pTask, pReward, num.HasValue ? num.Value : 0);
		taskCompleteAction.DropTargetDataFromDict(pData);
		return taskCompleteAction;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("task_invariable", m_pTask.m_pTaskData.GetInvariableData());
		dictionary.Add("reward", m_pReward.ToDict());
		dictionary.Add("task_completion_count", m_nTaskCompletionCount);
		DropTargetDataToDict(dictionary);
		return dictionary;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		int nDID = m_pTask.m_pTaskData.m_nDID;
		pGame.taskManager.RemoveActiveTask(nDID);
		pGame.ApplyReward(m_pReward, ulUtcNow);
		AddPickup(pGame.simulation);
		pGame.taskManager.SetTaskCompletionCount(nDID, m_nTaskCompletionCount);
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		int nDID = m_pTask.m_pTaskData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> d = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(d, TaskData._sDID) == nDID)
			{
				list.RemoveAt(i);
				break;
			}
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)((Dictionary<string, object>)pGameState["farm"])["task_completion_counts"];
		string key = nDID.ToString();
		if (dictionary.ContainsKey(key))
		{
			dictionary[key] = m_nTaskCompletionCount;
		}
		else
		{
			dictionary.Add(key, m_nTaskCompletionCount);
		}
		RewardManager.ApplyToGameState(m_pReward, GetTime(), pGameState);
		AddPickupToGameState(pGameState);
		base.Confirm(pGameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		base.AddMoreDataToTrigger(ref pData);
		m_pReward.AddDataToTrigger(ref pData);
		pData.Add("task_id", m_pTask.m_pTaskData.m_nDID);
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
