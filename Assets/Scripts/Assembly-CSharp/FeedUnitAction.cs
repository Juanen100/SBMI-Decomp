using System.Collections.Generic;

public class FeedUnitAction : PersistedSimulatedAction
{
	public const string FEED_RESIDENT = "fu";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	public ulong hungerPeriod;

	public int hungerResourceId;

	public int? prevHungerResourceId;

	public Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public FeedUnitAction(Simulated unit, ulong hungerPeriod, int hungerResourceId, int? prevHungerResourceId, Reward reward)
		: base(null, null, null)
	{
	}

	private FeedUnitAction(Identity id, ulong hungerPeriod, int hungerResourceId, int? prevHungerResourceId, Reward reward)
		: base(null, null, null)
	{
	}

	public new static FeedUnitAction FromDict(Dictionary<string, object> data)
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
