using System.Collections.Generic;

public class DebrisStartAction : PersistedSimulatedAction
{
	public const string DEBRIS_START = "ds";

	private ulong completionTime;

	private Cost cost;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public DebrisStartAction(Identity target, ulong completeTime, Cost cost)
		: base(null, null, null)
	{
	}

	public new static DebrisStartAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
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
