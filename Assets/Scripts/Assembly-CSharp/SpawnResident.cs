using System.Collections.Generic;

public class SpawnResident : SessionActionDefinition
{
	public const string TYPE = "spawn_resident";

	public const string RESIDENT_ID = "resident_id";

	public const string BUILDING_ID = "building_id";

	private int? residentID;

	private int? buildingID;

	public static SpawnResident Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
	}
}
