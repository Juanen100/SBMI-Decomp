using System.Collections.Generic;

public class MoveInDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "movein";

	private const string CHARACTER_NAME = "charactername";

	private const string BUILDING_NAME = "buildingname";

	private const string PORTRAIT_TEXTURE = "portraittexture";

	private const string SOUND_BEAT = "soundBeat";

	private string characterName;

	private string buildingName;

	private string portraitTexture;

	public string CharacterName
	{
		get
		{
			return characterName;
		}
	}

	public string BuildingName
	{
		get
		{
			return buildingName;
		}
	}

	public string PortraitTexture
	{
		get
		{
			return portraitTexture;
		}
	}

	public MoveInDialogInputData(string characterName, string buildingName, string portraitTexture, string soundBeat)
		: base(uint.MaxValue, "movein", "Dialog_Explanation", soundBeat)
	{
		this.characterName = characterName;
		this.buildingName = buildingName;
		this.portraitTexture = portraitTexture;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		BuildPersistenceDict(ref dict, "movein");
		dict["charactername"] = characterName;
		dict["buildingname"] = buildingName;
		dict["portraittexture"] = portraitTexture;
		if (base.SoundBeat != null)
		{
			dict["soundBeat"] = base.SoundBeat;
		}
		return dict;
	}

	public new static MoveInDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "charactername");
		string text2 = TFUtils.LoadString(dict, "buildingname");
		string text3 = TFUtils.LoadString(dict, "portraittexture");
		string text4 = TFUtils.TryLoadString(dict, "soundBeat");
		return new MoveInDialogInputData(text, text2, text3, text4);
	}
}
