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
		: base(null, null, null)
	{
	}

	public new static TaskStartAction FromDict(Dictionary<string, object> pData)
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

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return null;
	}
}
