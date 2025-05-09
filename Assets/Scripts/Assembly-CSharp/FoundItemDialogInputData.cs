using System.Collections.Generic;

public class FoundItemDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "found_item";

	protected const string TITLE = "title";

	protected const string MESSAGE = "message";

	protected const string ICON = "icon";

	protected const string SOUND_BEAT = "sound_beat";

	protected string title;

	protected string message;

	protected string icon;

	public string Title
	{
		get
		{
			return title;
		}
	}

	public string Message
	{
		get
		{
			return message;
		}
	}

	public string Icon
	{
		get
		{
			return icon;
		}
	}

	public FoundItemDialogInputData(uint sequenceId, Dictionary<string, object> prompt)
		: base(sequenceId, "found_item", "Dialog_FoundItem", null)
	{
		if (prompt.ContainsKey("title"))
		{
			title = Language.Get((string)prompt["title"]);
		}
		if (prompt.ContainsKey("body"))
		{
			message = Language.Get((string)prompt["body"]);
		}
		if (prompt.ContainsKey("effect"))
		{
			soundBeat = (string)prompt["effect"];
		}
		if (prompt.ContainsKey("icon"))
		{
			icon = (string)prompt["icon"];
		}
	}

	public FoundItemDialogInputData(string title, string message, string icon, string soundBeat)
		: base(uint.MaxValue, "found_item", "Dialog_FoundItem", soundBeat)
	{
		this.title = title;
		this.message = message;
		this.icon = icon;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		base.BuildPersistenceDict(ref dict, "found_item");
		dict["title"] = title;
		dict["message"] = message;
		dict["icon"] = icon;
		if (base.SoundBeat != null)
		{
			dict["sound_beat"] = base.SoundBeat;
		}
		return dict;
	}

	public new static FoundItemDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "title");
		string text2 = TFUtils.LoadString(dict, "message");
		string text3 = TFUtils.LoadString(dict, "icon");
		string text4 = TFUtils.TryLoadString(dict, "sound_beat");
		return new FoundItemDialogInputData(text, text2, text3, text4);
	}
}
