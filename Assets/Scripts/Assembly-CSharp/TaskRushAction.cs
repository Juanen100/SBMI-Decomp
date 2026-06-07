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
		: base(null, null)
	{
	}

	public new static TaskRushAction FromDict(Dictionary<string, object> pData)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
	}
}
