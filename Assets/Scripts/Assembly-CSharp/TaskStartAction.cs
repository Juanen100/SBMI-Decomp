using System.Collections.Generic;

public class TaskStartAction : PersistedSimulatedAction
{
	public const string TASK_START = "tsa";

	private Task m_pTask;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TaskStartAction(Task pTask)
		: base("tsa", Identity.Null(), null)
	{
		m_pTask = pTask;
	}

	public new static TaskStartAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		return new TaskStartAction(pTask);
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
		pGame.taskManager.AddActiveTask(pGame, new Task(pGame, m_pTask.GetInvariableData()));
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		list.Add(m_pTask.GetInvariableData());
		base.Confirm(pGameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		base.AddMoreDataToTrigger(ref pData);
		pData.Add("task_id", m_pTask.m_pTaskData.m_nDID);
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
