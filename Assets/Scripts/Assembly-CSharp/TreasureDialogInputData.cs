using System.Collections.Generic;

public class TreasureDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "found_treasure";

	private const string TITLE = "title";

	private const string MESSAGE = "message";

	private const string REWARD = "reward";

	private const string SOUND = "sound";

	private string title;

	private string message;

	private Reward reward;

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

	public TreasureDialogInputData(string title, string message, Reward reward, string soundBeat)
		: base(uint.MaxValue, "found_treasure", "Dialog_FoundItem", soundBeat)
	{
		this.title = title;
		this.message = message;
		this.reward = reward;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		base.BuildPersistenceDict(ref dict, "found_treasure");
		dict["title"] = title;
		dict["message"] = message;
		dict["reward"] = reward.ToDict();
		if (base.SoundBeat != null)
		{
			dict["sound"] = base.SoundBeat;
		}
		return dict;
	}

	public new static TreasureDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "title");
		string text2 = TFUtils.LoadString(dict, "message");
		Reward reward = Reward.FromDict(TFUtils.LoadDict(dict, "reward"));
		string text3 = TFUtils.TryLoadString(dict, "sound");
		return new TreasureDialogInputData(text, text2, reward, text3);
	}
}
