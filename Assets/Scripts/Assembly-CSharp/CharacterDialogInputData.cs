using System.Collections.Generic;

public class CharacterDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "character";

	private List<object> promptsData;

	public List<object> PromptsData
	{
		get
		{
			return null;
		}
	}

	public CharacterDialogInputData(uint sequenceId, Dictionary<string, object> promptData)
		: base(0u, null, null, null)
	{
	}

	public CharacterDialogInputData(uint sequenceId, List<object> promptsData)
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static CharacterDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
