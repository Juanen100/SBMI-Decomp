using System.Collections.Generic;

public class FoundItemDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "found_item";

	protected string title;

	protected string message;

	protected string icon;

	protected const string TITLE = "title";

	protected const string MESSAGE = "message";

	protected const string ICON = "icon";

	protected const string SOUND_BEAT = "sound_beat";

	public string Title
	{
		get
		{
			return null;
		}
	}

	public string Message
	{
		get
		{
			return null;
		}
	}

	public string Icon
	{
		get
		{
			return null;
		}
	}

	public FoundItemDialogInputData(uint sequenceId, Dictionary<string, object> prompt)
		: base(0u, null, null, null)
	{
	}

	public FoundItemDialogInputData(string title, string message, string icon, string soundBeat)
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static FoundItemDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
