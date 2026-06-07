using System.Collections.Generic;

public class CollectRentAction : PersistedSimulatedAction
{
	public const string COLLECT_RENT = "cr";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	public const string PICKUP_TRIGGERTYPE = "RentPickup";

	public Reward reward;

	public ulong rentReadyTime;

	public ulong rentPeriod;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public CollectRentAction(Simulated building, Reward reward)
		: base(null, null, null)
	{
	}

	public CollectRentAction(Simulated building, Reward reward, ulong rentReadyTime)
		: base(null, null, null)
	{
	}

	private CollectRentAction(Identity id, Reward reward, ulong rentReadyTime)
		: base(null, null, null)
	{
	}

	public new static CollectRentAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override void AddEnvelope(ulong time, string tag)
	{
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
