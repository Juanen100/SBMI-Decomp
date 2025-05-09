using System.Collections.Generic;

public class PickupDropAction : PersistedSimulatedAction
{
	public const string PICKUP_DROP = "pd";

	public const int INVALID_INT = -1;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public PickupDropAction(Identity id, Identity dropID)
		: base("pd", id, typeof(PickupDropAction).ToString())
	{
		base.dropID = dropID;
	}

	public new static PickupDropAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Identity identity = new Identity((string)data["dropID"]);
		return new PickupDropAction(id, identity);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["dropID"] = dropID.Describe();
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.dropManager.RemovePickupTrigger(dropID);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ItemDropManager.RemovePickupTriggerFromGameState(gameState, dropID.Describe());
		base.Confirm(gameState);
	}
}
