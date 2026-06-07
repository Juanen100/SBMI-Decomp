using System.Collections.Generic;

public class SellAction : PersistedSimulatedAction
{
	public const string SELL = "s";

	public Cost cost;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public SellAction(Identity id, Cost cost)
		: base(null, null, null)
	{
	}

	public SellAction(Simulated simulated, Cost cost)
		: base(null, null, null)
	{
	}

	public new static SellAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
