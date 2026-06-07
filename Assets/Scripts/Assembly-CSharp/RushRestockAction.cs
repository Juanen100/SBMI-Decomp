using System.Collections.Generic;

public class RushRestockAction : PersistedSimulatedAction
{
	public const string RUSH_RESTOCK = "rrs";

	private Cost rushCost;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RushRestockAction(Identity id, Cost rushCost)
		: base(null, null, null)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public new static RushRestockAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
