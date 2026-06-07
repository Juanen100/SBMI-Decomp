using System.Collections.Generic;

public class QuestProgressAction : QuestAction
{
	public enum ConditionType
	{
		START = 0,
		END = 1
	}

	public const string QUEST_PROGRESS = "qp";

	private ConditionType conditionType;

	private List<uint> conditionIds;

	private QuestProgressAction(uint questId, ulong? startTime, ulong? completionTime, ConditionType conditionType, List<uint> conditionIds)
		: base(null, 0u, null, null)
	{
	}

	private QuestProgressAction(uint questId, ulong? startTime, ulong? completionTime, ConditionType conditionType, ICollection<uint> conditionIds)
		: base(null, 0u, null, null)
	{
	}

	public QuestProgressAction(Quest quest, ConditionType conditionType, ICollection<uint> conditionIds)
		: base(null, 0u, null, null)
	{
	}

	public new static QuestProgressAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	private static string ConditionTypeToString(ConditionType conditionType)
	{
		return null;
	}

	private static ConditionType ConditionTypeFromString(string s)
	{
		return default(ConditionType);
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
