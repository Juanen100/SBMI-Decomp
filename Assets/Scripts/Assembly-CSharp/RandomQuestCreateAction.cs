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
		: base("rq", Identity.Null())
	{
		this.questDef = questDef;
	}

	public new static RandomQuestCreateAction FromDict(Dictionary<string, object> data)
	{
		QuestDefinition questDefinition = QuestDefinition.FromDict((Dictionary<string, object>)data["questdef"]);
		return new RandomQuestCreateAction(questDefinition);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["questdef"] = questDef.ToDict(false);
		return dictionary;
	}

	public override void Process(Game game)
	{
		quest = game.questManager.AddQuestDefinition(questDef);
		quest.StartConditions = new ConditionState(game.questManager.QuestDefinitionList[quest.Did].Start.Chunks[0].Condition);
		quest.StartConditions.Hydrate(quest.StartProgress, game);
		ConditionState conditionState = new ConditionState(game.questManager.QuestDefinitionList[quest.Did].End.Chunks[0].Condition);
		quest.EndConditions.Add(conditionState);
		conditionState.Hydrate(quest.EndProgress, game);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
		quest = game.questManager.AddQuestDefinition(questDef);
		if (QuestDefinition.LastRandomQuestId < questDef.Did)
		{
			QuestDefinition.LastRandomQuestId = questDef.Did;
		}
		quest.StartConditions = new ConditionState(game.questManager.QuestDefinitionList[quest.Did].Start.Chunks[0].Condition);
		quest.StartConditions.Hydrate(quest.StartProgress, game);
		ConditionState conditionState = new ConditionState(game.questManager.QuestDefinitionList[quest.Did].End.Chunks[0].Condition);
		quest.EndConditions.Add(conditionState);
		conditionState.Hydrate(quest.EndProgress, game);
		game.questManager.RegisterQuest(game, quest);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list = (List<object>)dictionary["generated_quest_definition"];
		list.Add(questDef.ToDict(true));
		List<object> list2 = (List<object>)dictionary["quests"];
		list2.Add(quest.ToDict());
		((Dictionary<string, object>)gameState["farm"])["random_quest_id"] = QuestDefinition.LastRandomQuestId;
		base.Confirm(gameState);
	}
}
