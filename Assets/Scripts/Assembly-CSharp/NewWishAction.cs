using System.Collections.Generic;

public class NewWishAction : PersistedSimulatedAction
{
	public const string NEW_WISH = "nw";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	public int wishProductId;

	public int? prevWishProductId;

	public ulong expiresAt;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public NewWishAction(Identity id, int wishProductId, int? prevWishProductId, ulong expiresAt)
		: base(null, null, null)
	{
	}

	public new static NewWishAction FromDict(Dictionary<string, object> data)
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
