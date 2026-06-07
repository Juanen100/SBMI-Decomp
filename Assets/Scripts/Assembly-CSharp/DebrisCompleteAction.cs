using System.Collections.Generic;

public class DebrisCompleteAction : PersistedSimulatedAction
{
	public const string DEBRIS_COMPLETE = "dc";

	private Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public DebrisCompleteAction(Identity id, Reward reward)
		: base(null, null, null)
	{
	}

	public new static DebrisCompleteAction FromDict(Dictionary<string, object> data)
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

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}
}
