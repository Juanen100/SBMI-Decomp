using System;
using System.Collections.Generic;

public class RandomQuestCleanupAction : PersistedTriggerableAction
{
	public const string QUEST_CLEANUP = "ru";

	private uint questId = 400000u;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	private RandomQuestCleanupAction(uint questId)
		: base("ru", Identity.Null())
	{
		this.questId = questId;
	}

	public RandomQuestCleanupAction(Quest quest)
		: this(quest.Did)
	{
	}

	public new static RandomQuestCleanupAction FromDict(Dictionary<string, object> data)
	{
		uint num = TFUtils.LoadUint(data, "did");
		return new RandomQuestCleanupAction(num);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = questId;
		return dictionary;
	}

	public override void Process(Game game)
	{
		game.questManager.DeactivateQuest(game, QuestDefinition.LastRandomQuestId);
		base.Process(game);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["quests"];
		Predicate<object> match = delegate(object b)
		{
			uint num4 = uint.Parse(((Dictionary<string, object>)b)["did"].ToString());
			return (num4 == questId) ? true : false;
		};
		list.Remove(list.Find(match));
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list2 = (List<object>)dictionary["generated_quest_definition"];
		int num = -1;
		for (int num2 = 0; num2 < list2.Count; num2++)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)list2[num2];
			uint num3 = TFUtils.LoadUint(data, "did");
			if (questId == num3)
			{
				num = num2;
			}
		}
		if (num != -1)
		{
			list2.RemoveAt(num);
		}
		base.Confirm(gameState);
	}
}
