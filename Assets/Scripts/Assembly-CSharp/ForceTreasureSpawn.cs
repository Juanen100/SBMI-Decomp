#define ASSERTS_ON
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
		ForceTreasureSpawn forceTreasureSpawn = new ForceTreasureSpawn();
		forceTreasureSpawn.Parse(data, id, startConditions, originatedFromQuest);
		return forceTreasureSpawn;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		targetSpawner = TFUtils.LoadString(data, "persist_name");
		succeedOnFailure = TFUtils.TryLoadBool(data, "succeed_on_failure");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["persist_name"] = targetSpawner;
		dictionary["succeed_on_failure"] = succeedOnFailure;
		return dictionary;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		TreasureSpawner treasureSpawner = session.TheGame.treasureManager.FindTreasureSpawner(targetSpawner);
		TFUtils.Assert(treasureSpawner != null, "Failed to find the treasure spawner: " + targetSpawner);
		if ((treasureSpawner != null && treasureSpawner.PlaceTreasure()) || (succeedOnFailure.HasValue && succeedOnFailure.Value))
		{
			action.MarkSucceeded();
		}
		else
		{
			action.MarkFailed();
		}
	}
}
