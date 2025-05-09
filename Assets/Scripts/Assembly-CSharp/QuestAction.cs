#define ASSERTS_ON
using System.Collections.Generic;

public abstract class QuestAction : PersistedTriggerableAction
{
	protected uint questId;

	protected ulong? startTime;

	protected ulong? completionTime;

	public TriggerableMixin Triggerable
	{
		get
		{
			return triggerable;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public QuestAction(string type, uint questId, ulong? startTime, ulong? completionTime)
		: base(type, Identity.Null())
	{
		this.questId = questId;
		this.startTime = startTime;
		this.completionTime = completionTime;
	}

	public QuestAction(string type, Quest quest)
		: base(type, Identity.Null())
	{
		questId = quest.Did;
		startTime = quest.StartTime;
		completionTime = quest.CompletionTime;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dest = base.ToDict();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = questId;
		dictionary["start_time"] = TFUtils.NullableToObject(startTime);
		dictionary["completion_time"] = TFUtils.NullableToObject(completionTime);
		return TFUtils.ConcatenateDictionaryInPlace(dest, dictionary);
	}

	public override void Process(Game game)
	{
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Quest quest = game.questManager.GetQuest(questId);
		if (quest != null)
		{
			if (startTime.HasValue)
			{
				quest.Start(startTime.Value);
			}
			if (completionTime.HasValue)
			{
				quest.Complete(completionTime.Value);
			}
			game.questManager.RegisterQuest(game, quest);
		}
		else
		{
			TFUtils.WarningLog("QuestAction.Apply - Quest " + questId + " does not exist in the questList.");
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list = (List<object>)dictionary["quests"];
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)list[i];
			uint num2 = TFUtils.LoadUint(data, "did");
			if (num2 == questId)
			{
				num++;
				if (num == 1)
				{
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[i];
					dictionary2["start_time"] = startTime;
					dictionary2["completion_time"] = completionTime;
					dictionary2["reminded"] = true;
				}
			}
		}
		if (num == 0)
		{
			TFUtils.Assert(false, "Missing a quest " + questId + " - should be generated in QuestProgressAction");
		}
		else if (num > 1)
		{
			TFUtils.Assert(false, "Too many quests match did " + questId);
		}
		base.Confirm(gameState);
	}

	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["quest_id"] = (int)questId;
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
