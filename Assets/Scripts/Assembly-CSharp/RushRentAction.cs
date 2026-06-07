using System.Collections.Generic;

public class RushRentAction : PersistedSimulatedAction
{
	public const string RUSH_RENT = "rr";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	private Cost rushCost;

	private ulong rentReadyTime;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RushRentAction(Identity id, Cost rushCost, ulong nextRentReadyTime)
		: base(null, null, null)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public new static RushRentAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void AddEnvelope(ulong time, string tag)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}
}
