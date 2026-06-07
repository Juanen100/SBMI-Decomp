using System.Collections.Generic;

public class DialogPackage
{
	private uint did;

	private Dictionary<string, object> data;

	public uint Did
	{
		get
		{
			return 0u;
		}
	}

	public Dictionary<string, object> Data
	{
		get
		{
			return null;
		}
	}

	public DialogPackage(Dictionary<string, object> data)
	{
	}

	public List<DialogInputData> GetDialogInputsInSequence(uint sequenceId, Dictionary<string, object> contextData, uint? associatedQuestId)
	{
		return null;
	}

	private List<object> GetPromptsInSequence(uint sequenceId)
	{
		return null;
	}
}
