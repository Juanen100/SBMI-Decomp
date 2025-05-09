using System.Collections.Generic;

public class SpongyGamesDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "spongy_games";

	private Dictionary<string, object> eventData;

	public Dictionary<string, object> EventData
	{
		get
		{
			return eventData;
		}
	}

	public SpongyGamesDialogInputData(Dictionary<string, object> inEventData)
		: base(uint.MaxValue, "spongy_games", null, null)
	{
		eventData = inEventData;
	}

	public SpongyGamesDialogInputData(uint unSequenceID, Dictionary<string, object> inEventData)
		: base(unSequenceID, "spongy_games", null, null)
	{
		eventData = inEventData;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = "spongy_games";
		dictionary["event_data"] = eventData;
		dictionary["sequence_id"] = base.SequenceId;
		return dictionary;
	}

	public new static SpongyGamesDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		Dictionary<string, object> inEventData = (Dictionary<string, object>)dict["event_data"];
		uint unSequenceID = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			unSequenceID = TFUtils.LoadUint(dict, "sequence_id");
		}
		return new SpongyGamesDialogInputData(unSequenceID, inEventData);
	}

	public override string ToString()
	{
		return "SpongyGamesDialogInputData(event_data=" + TFUtils.DebugDictToString(eventData) + "," + base.ToString() + ")";
	}
}
