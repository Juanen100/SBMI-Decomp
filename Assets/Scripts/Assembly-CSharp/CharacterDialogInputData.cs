using System.Collections.Generic;

public class CharacterDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "character";

	private List<object> promptsData;

	public List<object> PromptsData
	{
		get
		{
			return promptsData;
		}
	}

	public CharacterDialogInputData(uint sequenceId, Dictionary<string, object> promptData)
		: this(sequenceId, new List<object> { promptData })
	{
	}

	public CharacterDialogInputData(uint sequenceId, List<object> promptsData)
		: base(sequenceId, "character", null, null)
	{
		this.promptsData = promptsData;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		BuildPersistenceDict(ref dict, "character");
		dict["sequence_id"] = base.SequenceId;
		dict["prompts"] = promptsData;
		return dict;
	}

	public new static CharacterDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint num = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			num = TFUtils.LoadUint(dict, "sequence_id");
		}
		List<object> list = (List<object>)dict["prompts"];
		return new CharacterDialogInputData(num, list);
	}

	public override string ToString()
	{
		return "CharacterDialogInputData(prompts=" + TFUtils.DebugListToString(promptsData) + "," + base.ToString() + ")";
	}
}
