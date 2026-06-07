using System.Collections.Generic;

public class MoveInDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "movein";

	private string characterName;

	private string buildingName;

	private string portraitTexture;

	private const string CHARACTER_NAME = "charactername";

	private const string BUILDING_NAME = "buildingname";

	private const string PORTRAIT_TEXTURE = "portraittexture";

	private const string SOUND_BEAT = "soundBeat";

	public string CharacterName
	{
		get
		{
			return null;
		}
	}

	public string BuildingName
	{
		get
		{
			return null;
		}
	}

	public string PortraitTexture
	{
		get
		{
			return null;
		}
	}

	public MoveInDialogInputData(string characterName, string buildingName, string portraitTexture, string soundBeat)
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static MoveInDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
