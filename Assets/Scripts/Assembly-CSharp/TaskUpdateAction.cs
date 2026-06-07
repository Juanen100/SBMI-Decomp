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
		: base(null, null)
	{
	}

	public new static TaskUpdateAction FromDict(Dictionary<string, object> pData)
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
