using System.Collections.Generic;

public class BootyQuestCompleteDialogInputData : QuestDialogInputData
{
	public const string DIALOG_TYPE = "booty_quest_complete";

	public BootyQuestCompleteDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId)
		: base(sequenceId, "booty_quest_complete", promptData, contextData, "Dialog_QuestComplete", "Beat_QuestComplete", questId)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		BuildPersistenceDict(ref dict, "booty_quest_complete");
		return dict;
	}

	public new static BootyQuestCompleteDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint num = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			num = TFUtils.LoadUint(dict, "sequence_id");
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)dict["prompt"];
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dict["context_data"];
		uint? num2 = TFUtils.TryLoadNullableUInt(dict, "quest_id");
		return new BootyQuestCompleteDialogInputData(num, dictionary, dictionary2, num2);
	}
}
