using System.Collections.Generic;

public class QuestStartDialogInputData : QuestDialogInputData
{
	public const string DIALOG_TYPE = "quest_start";

	public QuestStartDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId)
		: base(0u, null, null, null, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static QuestStartDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
