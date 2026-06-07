using System.Collections.Generic;

public class QuestStartAction : QuestAction
{
	public const string QUEST_START = "qs";

	private QuestStartAction(uint questId, ulong? startTime, ulong? completionTime)
		: base(null, 0u, null, null)
	{
	}

	public QuestStartAction(Quest quest)
		: base(null, 0u, null, null)
	{
	}

	public new static QuestStartAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}
}
