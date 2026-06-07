using System.Collections.Generic;

public class RushDebrisAction : PersistedSimulatedAction
{
	public const string RUSH_DEBRIS = "rd";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	private Cost rushCost;

	private ulong readyTime;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RushDebrisAction(Identity id, Cost rushCost, ulong readyTime)
		: base(null, null, null)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public new static RushDebrisAction FromDict(Dictionary<string, object> data)
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
