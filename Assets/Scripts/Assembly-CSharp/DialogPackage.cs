#define ASSERTS_ON
using System.Collections.Generic;

public class DialogPackage
{
	private uint did;

	private Dictionary<string, object> data;

	public uint Did
	{
		get
		{
			return did;
		}
	}

	public Dictionary<string, object> Data
	{
		get
		{
			return data;
		}
	}

	public DialogPackage(Dictionary<string, object> data)
	{
		this.data = data;
		did = TFUtils.LoadUint(data, "did");
	}

	public List<DialogInputData> GetDialogInputsInSequence(uint sequenceId, Dictionary<string, object> contextData, uint? associatedQuestId)
	{
		List<DialogInputData> list = new List<DialogInputData>();
		List<object> promptsInSequence = GetPromptsInSequence(sequenceId);
		TFUtils.Assert(promptsInSequence != null, "Found no prompts in dialog sequence! SequenceId=" + sequenceId);
		foreach (Dictionary<string, object> item in promptsInSequence)
		{
			list.Add(DialogInputData.FromPromptDict(sequenceId, item, contextData, associatedQuestId));
		}
		return list;
	}

	private List<object> GetPromptsInSequence(uint sequenceId)
	{
		List<object> list = (List<object>)data["sequences"];
		foreach (Dictionary<string, object> item in list)
		{
			uint num = TFUtils.LoadUint(item, "id");
			if (num == sequenceId)
			{
				return (List<object>)item["prompts"];
			}
		}
		return null;
	}
}
