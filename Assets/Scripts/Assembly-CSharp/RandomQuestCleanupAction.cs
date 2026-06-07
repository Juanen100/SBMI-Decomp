using System.Collections.Generic;

public class RandomQuestCleanupAction : PersistedTriggerableAction
{
	public const string QUEST_CLEANUP = "ru";

	private uint questId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	private RandomQuestCleanupAction(uint questId)
		: base(null, null)
	{
	}

	public RandomQuestCleanupAction(Quest quest)
		: base(null, null)
	{
	}

	public new static RandomQuestCleanupAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Process(Game game)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
