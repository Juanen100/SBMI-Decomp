using System.Collections.Generic;

public class SpawnWanderer : SessionActionDefinition
{
	public const string TYPE = "spawn_wanderer";

	public const string WANDERER_ID = "id";

	private int? wandererID;

	public static SpawnWanderer Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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
