#define ASSERTS_ON
using System;
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
		: base("qp", questId, startTime, completionTime)
	{
		this.conditionType = conditionType;
		this.conditionIds = conditionIds;
	}

	private QuestProgressAction(uint questId, ulong? startTime, ulong? completionTime, ConditionType conditionType, ICollection<uint> conditionIds)
		: base("qp", questId, startTime, completionTime)
	{
		this.conditionType = conditionType;
		this.conditionIds = new List<uint>();
		foreach (uint conditionId in conditionIds)
		{
			this.conditionIds.Add(conditionId);
		}
	}

	public QuestProgressAction(Quest quest, ConditionType conditionType, ICollection<uint> conditionIds)
		: this(quest.Did, quest.StartTime, quest.CompletionTime, conditionType, conditionIds)
	{
	}

	public new static QuestProgressAction FromDict(Dictionary<string, object> data)
	{
		ConditionType conditionType = ConditionTypeFromString(TFUtils.LoadString(data, "condition_type"));
		List<uint> list = TFUtils.LoadList<uint>(data, "condition_ids");
		uint num = TFUtils.LoadUint(data, "did");
		ulong? num2 = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? num3 = TFUtils.LoadNullableUlong(data, "completion_time");
		return new QuestProgressAction(num, num2, num3, conditionType, list);
	}

	private static string ConditionTypeToString(ConditionType conditionType)
	{
		switch (conditionType)
		{
		case ConditionType.START:
			return "s";
		case ConditionType.END:
			return "e";
		default:
			throw new ArgumentException("Unrecognized condition type:  " + conditionType);
		}
	}

	private static ConditionType ConditionTypeFromString(string s)
	{
		switch (s)
		{
		case "s":
			return ConditionType.START;
		case "e":
			return ConditionType.END;
		default:
			throw new ArgumentException("Unrecognized condition type string:  " + s);
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["condition_type"] = ConditionTypeToString(conditionType);
		dictionary["condition_ids"] = TFUtils.CloneAndCastList<uint, object>(conditionIds);
		return dictionary;
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
			if (conditionType == ConditionType.START)
			{
				quest.StartProgress = new ConditionalProgress(conditionIds);
			}
			else if (conditionType == ConditionType.END)
			{
				quest.EndProgress = new ConditionalProgress(conditionIds);
			}
			game.questManager.RegisterQuest(game, quest);
		}
		else
		{
			TFUtils.WarningLog("QuestProgressAction.Apply - Quest " + questId + " does not exist in the questList.");
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
			if (num2 != questId)
			{
				continue;
			}
			num++;
			if (num == 1)
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[i];
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dictionary2["conditions"];
				if (conditionType == ConditionType.START)
				{
					dictionary3["met_start_condition_ids"] = TFUtils.CloneAndCastList<uint, object>(conditionIds);
				}
				if (conditionType == ConditionType.END)
				{
					dictionary3["met_end_condition_ids"] = TFUtils.CloneAndCastList<uint, object>(conditionIds);
				}
			}
		}
		if (num == 0)
		{
			Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
			Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
			TFUtils.Assert(conditionType == ConditionType.START, "Can't process End Conditions - No Quest Starting progress has been made yet.");
			dictionary5["met_start_condition_ids"] = TFUtils.CloneAndCastList<uint, object>(conditionIds);
			dictionary5["met_end_condition_ids"] = new List<object>();
			dictionary4["did"] = questId;
			dictionary4["start_time"] = null;
			dictionary4["conditions"] = dictionary5;
			list.Add(dictionary4);
		}
		TFUtils.Assert(num <= 1, "Too many quests match did " + questId);
		base.Confirm(gameState);
	}
}
