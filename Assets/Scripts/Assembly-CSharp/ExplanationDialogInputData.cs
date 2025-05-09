using System.Collections.Generic;

public class ExplanationDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "explanation";

	private const string MESSAGE = "message";

	private const string SOUND_BEAT = "soundBeat";

	private string message;

	public string Message
	{
		get
		{
			return message;
		}
	}

	public ExplanationDialogInputData(string message, string soundBeat)
		: base(uint.MaxValue, "explanation", "Dialog_Explanation", soundBeat)
	{
		this.message = message;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		BuildPersistenceDict(ref dict, "explanation");
		dict["message"] = message;
		if (base.SoundBeat != null)
		{
			dict["soundBeat"] = base.SoundBeat;
		}
		return dict;
	}

	public new static ExplanationDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "message");
		string text2 = TFUtils.TryLoadString(dict, "soundBeat");
		return new ExplanationDialogInputData(text, text2);
	}
}
