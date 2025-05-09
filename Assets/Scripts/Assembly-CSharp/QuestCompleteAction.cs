using System.Collections.Generic;

public class QuestCompleteAction : QuestAction
{
	public const string QUEST_COMPLETE = "qc";

	private Reward reward;

	private Dictionary<string, object> buildingLabels;

	private QuestCompleteAction(uint questId, ulong? startTime, ulong? completionTime, Reward reward, Dictionary<string, object> buildingLabels)
		: base("qc", questId, startTime, completionTime)
	{
		this.reward = reward;
		this.buildingLabels = buildingLabels;
	}

	public QuestCompleteAction(Quest quest, Reward reward, Dictionary<string, object> buildingLabels)
		: this(quest.Did, quest.StartTime, quest.CompletionTime, reward, buildingLabels)
	{
	}

	public new static QuestCompleteAction FromDict(Dictionary<string, object> data)
	{
		Reward reward = Reward.FromObject(data["reward"]);
		uint num = TFUtils.LoadUint(data, "did");
		ulong? num2 = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? num3 = TFUtils.LoadNullableUlong(data, "completion_time");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)data["building_labels"];
		return new QuestCompleteAction(num, num2, num3, reward, dictionary);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = reward.ToDict();
		dictionary["building_labels"] = buildingLabels;
		return dictionary;
	}

	public override void Process(Game game)
	{
		game.communityEventManager.QuestComplete(questId);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
		game.ApplyReward(reward, GetTime());
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		base.Confirm(gameState);
		RewardManager.ApplyToGameState(reward, GetTime(), gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		data["quest_id"] = (int)questId;
	}
}
