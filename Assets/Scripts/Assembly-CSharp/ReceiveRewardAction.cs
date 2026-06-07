using System.Collections.Generic;

public class ReceiveRewardAction : PersistedTriggerableAction
{
	public const string RECEIVE_REWARD = "rra";

	public Reward reward;

	public string redemptionOffer;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public ReceiveRewardAction(Reward reward, string redemptionOffer)
		: base(null, null)
	{
	}

	public new static ReceiveRewardAction FromDict(Dictionary<string, object> data)
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

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
