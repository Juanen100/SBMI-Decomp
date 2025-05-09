using System.Collections.Generic;

public class QuestCompleteDialogInputData : QuestDialogInputData
{
	public const string DIALOG_TYPE = "quest_complete";

	public QuestCompleteDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId)
		: base(sequenceId, "quest_complete", promptData, contextData, null, null, questId)
	{
		string text = "Dialog_QuestComplete";
		string text2 = "Beat_QuestComplete";
		if (promptData != null)
		{
			if (promptData.ContainsKey("voiceover"))
			{
				text2 = (string)promptData["voiceover"];
			}
			if (promptData.ContainsKey("effect"))
			{
				text = (string)promptData["effect"];
			}
		}
		soundImmediate = text;
		soundBeat = text2;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		BuildPersistenceDict(ref dict, "quest_complete");
		return dict;
	}

	public new static QuestCompleteDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint num = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			num = TFUtils.LoadUint(dict, "sequence_id");
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)dict["prompt"];
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dict["context_data"];
		uint? num2 = TFUtils.TryLoadNullableUInt(dict, "quest_id");
		return new QuestCompleteDialogInputData(num, dictionary, dictionary2, num2);
	}
}
