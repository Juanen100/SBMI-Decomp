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
		: base("rra", Identity.Null())
	{
		this.redemptionOffer = redemptionOffer;
		this.reward = reward;
	}

	public new static ReceiveRewardAction FromDict(Dictionary<string, object> data)
	{
		string text = ((!data.ContainsKey("redemption_offer")) ? string.Empty : TFUtils.LoadString(data, "redemption_offer"));
		Reward reward = Reward.FromObject(data["reward"]);
		return new ReceiveRewardAction(reward, text);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = reward.ToDict();
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.ApplyReward(reward, GetTime());
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		RewardManager.ApplyToGameState(reward, GetTime(), gameState);
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data.Add("redemption_id", redemptionOffer);
		reward.AddDataToTrigger(ref data);
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger("rra", AddMoreDataToTrigger);
	}
}
