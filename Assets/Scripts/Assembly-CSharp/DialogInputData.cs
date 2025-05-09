#define ASSERTS_ON
using System.Collections.Generic;

public abstract class DialogInputData
{
	public const string TRIGGER_TYPE_PREFIX = "dialogtrigger_";

	public const float STANDARD_BEAT_LENGTH = 1f;

	public const uint NO_ID = uint.MaxValue;

	protected const string SOUND_TO_PLAY = "sound_to_play";

	protected const string TYPE = "type";

	private uint sequenceId;

	private string type;

	protected string soundImmediate;

	protected string soundBeat;

	public uint SequenceId
	{
		get
		{
			return sequenceId;
		}
	}

	public string SoundImmediate
	{
		get
		{
			return soundImmediate;
		}
	}

	public string SoundBeat
	{
		get
		{
			return soundBeat;
		}
	}

	public DialogInputData(uint sequenceId, string type, string soundImmediate, string soundBeat)
	{
		this.sequenceId = sequenceId;
		this.type = type;
		this.soundImmediate = soundImmediate;
		this.soundBeat = soundBeat;
	}

	public static DialogInputData FromPromptDict(uint sequenceId, Dictionary<string, object> prompt, Dictionary<string, object> contextData, uint? associatedQuestId)
	{
		string text = TFUtils.LoadString(prompt, "type");
		DialogInputData dialogInputData;
		if (text.Equals("character"))
		{
			dialogInputData = new CharacterDialogInputData(sequenceId, prompt);
		}
		else if (text.Equals("quest_start"))
		{
			dialogInputData = new QuestStartDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("quest_complete"))
		{
			dialogInputData = new QuestCompleteDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("booty_quest_complete"))
		{
			dialogInputData = new BootyQuestCompleteDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("quest_line_start"))
		{
			dialogInputData = new QuestLineStartDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("quest_line_complete"))
		{
			dialogInputData = new QuestLineCompleteDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else
		{
			if (!text.Equals("found_item"))
			{
				TFUtils.Assert(false, "Unexpected prompt type:  " + text);
				return null;
			}
			dialogInputData = new FoundItemDialogInputData(sequenceId, prompt);
		}
		TFUtils.Assert(dialogInputData.type != null && dialogInputData.type != string.Empty, "Did not find a type on DialogInputData=" + dialogInputData.ToString());
		return dialogInputData;
	}

	public ITrigger CreateTrigger(ulong utcTimeStamp)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["sequence_id"] = sequenceId;
		return new Trigger("dialogtrigger_" + type, dictionary, utcTimeStamp);
	}

	public override string ToString()
	{
		return "DialogInputData(sequenceId=" + sequenceId + ", type=" + type + ")";
	}
}
