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

	public TreasureDialogInputData(string title, string message, Reward reward, string soundBeat)
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static TreasureDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
