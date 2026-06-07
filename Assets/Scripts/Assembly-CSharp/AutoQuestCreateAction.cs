using System.Collections.Generic;

public class AutoQuestCreateAction : PersistedTriggerableAction
{
	public const string QUEST_CREATE = "aq";

	private QuestDefinition m_pQuestDef;

	private Quest m_pQuest;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public AutoQuestCreateAction(QuestDefinition pQuestDef)
		: base(null, null)
	{
	}

	public new static AutoQuestCreateAction FromDict(Dictionary<string, object> pData)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Process(Game pGame)
	{
	}

	public override void Apply(Game pGame, ulong nUtcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
	}
}
