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
		: base(null, null, null)
	{
	}

	public new static TaskCompleteAction FromDict(Dictionary<string, object> pData)
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
