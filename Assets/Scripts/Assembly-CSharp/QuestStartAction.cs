using System.Collections.Generic;

public class QuestStartAction : QuestAction
{
	public const string QUEST_START = "qs";

	private QuestStartAction(uint questId, ulong? startTime, ulong? completionTime)
		: base("qs", questId, startTime, completionTime)
	{
	}

	public QuestStartAction(Quest quest)
		: this(quest.Did, quest.StartTime, quest.CompletionTime)
	{
	}

	public new static QuestStartAction FromDict(Dictionary<string, object> data)
	{
		uint num = TFUtils.LoadUint(data, "did");
		ulong? num2 = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? num3 = TFUtils.LoadNullableUlong(data, "completion_time");
		return new QuestStartAction(num, num2, num3);
	}
}
