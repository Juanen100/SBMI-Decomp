using System.Collections.Generic;

public class AutoQuestCleanupAction : PersistedTriggerableAction
{
	public const string QUEST_CLEANUP = "au";

	private uint m_uQuestId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	private AutoQuestCleanupAction(uint uQuestId)
		: base(null, null)
	{
	}

	public AutoQuestCleanupAction(Quest pQuest)
		: base(null, null)
	{
	}

	public new static AutoQuestCleanupAction FromDict(Dictionary<string, object> pData)
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

	public override void Confirm(Dictionary<string, object> pGameState)
	{
	}
}
