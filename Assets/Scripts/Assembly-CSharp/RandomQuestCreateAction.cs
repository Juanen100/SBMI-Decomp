using System.Collections.Generic;

public class RandomQuestCreateAction : PersistedTriggerableAction
{
	public const string QUEST_CREATE = "rq";

	private QuestDefinition questDef;

	private Quest quest;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RandomQuestCreateAction(QuestDefinition questDef)
		: base(null, null)
	{
	}

	public new static RandomQuestCreateAction FromDict(Dictionary<string, object> data)
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

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
