using System.Collections.Generic;

public class TreasureUncoverAction : PersistedSimulatedAction
{
	public const string TREASURE_UNCOVER = "tu";

	private ulong completionTime;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TreasureUncoverAction(Identity id, ulong completionTime)
		: base(null, null, null)
	{
	}

	public new static TreasureUncoverAction FromDict(Dictionary<string, object> data)
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
