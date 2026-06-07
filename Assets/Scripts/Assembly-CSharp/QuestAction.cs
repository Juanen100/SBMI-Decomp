using System.Collections.Generic;

public abstract class QuestAction : PersistedTriggerableAction
{
	protected uint questId;

	protected ulong? startTime;

	protected ulong? completionTime;

	public TriggerableMixin Triggerable
	{
		get
		{
			return null;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public QuestAction(string type, uint questId, ulong? startTime, ulong? completionTime)
		: base(null, null)
	{
	}

	public QuestAction(string type, Quest quest)
		: base(null, null)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Process(Game game)
	{
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
