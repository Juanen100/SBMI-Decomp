using System.Collections.Generic;

public abstract class QuestDialogInputData : PersistedDialogInputData
{
	private Dictionary<string, object> promptData;

	private Dictionary<string, object> contextData;

	private uint? questId;

	public uint? QuestId
	{
		get
		{
			return null;
		}
	}

	public Dictionary<string, object> PromptData
	{
		get
		{
			return null;
		}
	}

	public Dictionary<string, object> ContextData
	{
		get
		{
			return null;
		}
	}

	public QuestDialogInputData(uint sequenceId, string type, Dictionary<string, object> promptData, Dictionary<string, object> contextData, string soundImmediate, string soundBeat, uint? questId)
		: base(0u, null, null, null)
	{
	}

	protected override void BuildPersistenceDict(ref Dictionary<string, object> dict, string type)
	{
	}
}
