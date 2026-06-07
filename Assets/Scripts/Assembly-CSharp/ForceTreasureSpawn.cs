using System.Collections.Generic;

public class ForceTreasureSpawn : SessionActionDefinition
{
	public const string TYPE = "force_treasure_spawn";

	private const string TARGET_SPAWNER = "persist_name";

	private const string SUCCEED_ON_FAILURE = "succeed_on_failure";

	private string targetSpawner;

	private bool? succeedOnFailure;

	public static ForceTreasureSpawn Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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
