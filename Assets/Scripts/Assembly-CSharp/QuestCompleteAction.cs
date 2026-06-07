using System.Collections.Generic;

public class QuestCompleteAction : QuestAction
{
	public const string QUEST_COMPLETE = "qc";

	private Reward reward;

	private Dictionary<string, object> buildingLabels;

	private QuestCompleteAction(uint questId, ulong? startTime, ulong? completionTime, Reward reward, Dictionary<string, object> buildingLabels)
		: base(null, 0u, null, null)
	{
	}

	public QuestCompleteAction(Quest quest, Reward reward, Dictionary<string, object> buildingLabels)
		: base(null, 0u, null, null)
	{
	}

	public new static QuestCompleteAction FromDict(Dictionary<string, object> data)
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

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}
}
