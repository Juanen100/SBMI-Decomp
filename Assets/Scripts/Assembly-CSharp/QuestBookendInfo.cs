#define ASSERTS_ON
using System.Collections.Generic;

public class QuestBookendInfo
{
	public class ChunkConditions
	{
		public LoadableCondition Condition;

		public string Name;

		public string Icon;

		public ChunkConditions(LoadableCondition condition, string name, string icon)
		{
			Condition = condition;
			Name = name;
			Icon = icon;
		}
	}

	private const string DIALOG_SEQUENCE_ID = "dialog_sequence_id";

	private const string POSTPONE = "postpone";

	private const string ARRAY = "array";

	private const string CONDITIONS = "conditions";

	public List<ChunkConditions> Chunks;

	public uint? DialogSequenceId;

	public float Postpone;

	public static QuestBookendInfo FromDict(Dictionary<string, object> data, bool chunkQuest, bool autoQuest)
	{
		QuestBookendInfo questBookendInfo = new QuestBookendInfo();
		questBookendInfo.Chunks = new List<ChunkConditions>();
		if (chunkQuest)
		{
			List<object> orCreateList = TFUtils.GetOrCreateList<object>(data, "array");
			foreach (object item3 in orCreateList)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)item3;
				LoadableCondition condition = (LoadableCondition)ConditionFactory.FromDict((Dictionary<string, object>)dictionary["conditions"]);
				string text = TFUtils.TryLoadString(dictionary, "name");
				string text2 = TFUtils.TryLoadString(dictionary, "icon");
				ChunkConditions item = new ChunkConditions(condition, (text != null) ? text : string.Empty, (text2 != null) ? text2 : string.Empty);
				questBookendInfo.Chunks.Add(item);
			}
			if (!autoQuest)
			{
				TFUtils.Assert(questBookendInfo.Chunks.Count > 1, "Chunk quests need to have at least 2 items");
			}
		}
		else
		{
			TFUtils.Assert(data.ContainsKey("conditions"), "This bookend should have a condition. Is this an array by mistake?");
			ChunkConditions item2 = new ChunkConditions((LoadableCondition)ConditionFactory.FromDict((Dictionary<string, object>)data["conditions"]), string.Empty, string.Empty);
			questBookendInfo.Chunks.Add(item2);
		}
		questBookendInfo.DialogSequenceId = ((!data.ContainsKey("dialog_sequence_id")) ? ((uint?)0u) : TFUtils.LoadNullableUInt(data, "dialog_sequence_id"));
		questBookendInfo.Postpone = ((!data.ContainsKey("postpone")) ? 0f : TFUtils.LoadFloat(data, "postpone"));
		return questBookendInfo;
	}

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (Chunks.Count == 1)
		{
			dictionary["conditions"] = Chunks[0].Condition.ToDict();
		}
		else
		{
			List<object> list = new List<object>();
			foreach (ChunkConditions chunk in Chunks)
			{
				list.Add(new Dictionary<string, object>
				{
					{
						"conditions",
						chunk.Condition.ToDict()
					},
					{ "name", chunk.Name },
					{ "icon", chunk.Icon }
				});
			}
			dictionary["array"] = list;
		}
		dictionary["dialog_sequence_id"] = DialogSequenceId;
		dictionary["postpone"] = Postpone;
		return dictionary;
	}
}
