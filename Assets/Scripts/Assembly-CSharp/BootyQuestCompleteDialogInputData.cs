using System.Collections.Generic;

public class BootyQuestCompleteDialogInputData : QuestDialogInputData
{
	public const string DIALOG_TYPE = "booty_quest_complete";

	public BootyQuestCompleteDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId)
		: base(0u, null, null, null, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static BootyQuestCompleteDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
