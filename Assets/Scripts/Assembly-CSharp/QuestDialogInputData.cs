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
			return questId;
		}
	}

	public Dictionary<string, object> PromptData
	{
		get
		{
			return promptData;
		}
	}

	public Dictionary<string, object> ContextData
	{
		get
		{
			return contextData;
		}
	}

	public QuestDialogInputData(uint sequenceId, string type, Dictionary<string, object> promptData, Dictionary<string, object> contextData, string soundImmediate, string soundBeat, uint? questId)
		: base(sequenceId, type, soundImmediate, soundBeat)
	{
		this.promptData = promptData;
		this.contextData = contextData;
		this.questId = questId;
	}

	protected override void BuildPersistenceDict(ref Dictionary<string, object> dict, string type)
	{
		base.BuildPersistenceDict(ref dict, type);
		dict["sequence_id"] = base.SequenceId;
		dict["prompt"] = promptData;
		dict["context_data"] = contextData;
		if (questId.HasValue)
		{
			dict["quest_id"] = questId.Value;
		}
	}
}
