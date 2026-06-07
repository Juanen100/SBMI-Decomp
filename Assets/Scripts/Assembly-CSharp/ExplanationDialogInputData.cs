using System.Collections.Generic;

public class ExplanationDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "explanation";

	private string message;

	private const string MESSAGE = "message";

	private const string SOUND_BEAT = "soundBeat";

	public string Message
	{
		get
		{
			return null;
		}
	}

	public ExplanationDialogInputData(string message, string soundBeat)
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static ExplanationDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
