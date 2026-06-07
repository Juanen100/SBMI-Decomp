using System.Collections.Generic;

public class SpongyGamesDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "spongy_games";

	private Dictionary<string, object> eventData;

	public Dictionary<string, object> EventData
	{
		get
		{
			return null;
		}
	}

	public SpongyGamesDialogInputData(Dictionary<string, object> inEventData)
		: base(0u, null, null, null)
	{
	}

	public SpongyGamesDialogInputData(uint unSequenceID, Dictionary<string, object> inEventData)
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static SpongyGamesDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
