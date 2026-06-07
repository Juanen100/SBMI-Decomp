using System.Collections.Generic;

public abstract class DialogInputData
{
	public const string TRIGGER_TYPE_PREFIX = "dialogtrigger_";

	public const float STANDARD_BEAT_LENGTH = 1f;

	public const uint NO_ID = uint.MaxValue;

	private uint sequenceId;

	private string type;

	protected string soundImmediate;

	protected string soundBeat;

	protected const string SOUND_TO_PLAY = "sound_to_play";

	protected const string TYPE = "type";

	public uint SequenceId
	{
		get
		{
			return 0u;
		}
	}

	public string SoundImmediate
	{
		get
		{
			return null;
		}
	}

	public string SoundBeat
	{
		get
		{
			return null;
		}
	}

	public DialogInputData(uint sequenceId, string type, string soundImmediate, string soundBeat)
	{
	}

	public static DialogInputData FromPromptDict(uint sequenceId, Dictionary<string, object> prompt, Dictionary<string, object> contextData, uint? associatedQuestId)
	{
		return null;
	}

	public ITrigger CreateTrigger(ulong utcTimeStamp)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
