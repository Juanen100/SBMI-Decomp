using System.Collections.Generic;

public class RushBuildAction : PersistedSimulatedAction
{
	public const string RUSH_BUILD = "rb";

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

	public RushBuildAction(Identity id, Cost rushCost, ulong nextReadyTime)
		: base(null, null, null)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public new static RushBuildAction FromDict(Dictionary<string, object> data)
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
