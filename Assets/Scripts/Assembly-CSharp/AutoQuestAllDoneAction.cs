using System.Collections.Generic;

public class AutoQuestAllDoneAction : PersistedTriggerableAction
{
	public const string AUTO_QUEST_ALL_DONE = "aqad";

	protected uint questId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public AutoQuestAllDoneAction(uint questId)
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

	protected void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
