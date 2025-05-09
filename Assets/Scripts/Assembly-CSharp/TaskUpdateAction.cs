using System.Collections.Generic;

public class TaskUpdateAction : PersistedTriggerableAction
{
	public const string TASK_UPDATE = "tua";

	private Task m_pTask;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TaskUpdateAction(Task pTask)
		: base("tua", Identity.Null())
	{
		m_pTask = pTask;
	}

	public new static TaskUpdateAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		return new TaskUpdateAction(pTask);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("task_invariable", m_pTask.m_pTaskData.GetInvariableData());
		return dictionary;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		Task activeTask = pGame.taskManager.GetActiveTask(m_pTask.m_pTaskData.m_nDID);
		if (activeTask != null)
		{
			activeTask.UpdateModifiableData(m_pTask.m_ulStartTime, m_pTask.m_ulCompleteTime);
		}
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		int nDID = m_pTask.m_pTaskData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(dictionary, TaskData._sDID) == nDID)
			{
				Task.UpdateModifiableDataForDict(dictionary, m_pTask);
				break;
			}
		}
		base.Confirm(pGameState);
	}
}
