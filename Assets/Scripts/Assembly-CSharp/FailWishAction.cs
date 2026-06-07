using System.Collections.Generic;

public class FailWishAction : PersistedSimulatedAction
{
	public const string FAIL_WISH = "fw";

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public FailWishAction(Simulated unit)
		: base(null, null, null)
	{
	}

	private FailWishAction(Identity id)
		: base(null, null, null)
	{
	}

	public new static FailWishAction FromDict(Dictionary<string, object> data)
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
