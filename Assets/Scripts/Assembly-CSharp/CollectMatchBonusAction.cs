using System.Collections.Generic;

public class CollectMatchBonusAction : PersistedSimulatedAction
{
	public const string COLLECT_MATCH_BONUS = "cmb";

	public const string PICKUP_TRIGGERTYPE = "BonusPickup";

	public Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public CollectMatchBonusAction(Identity id, Reward reward)
		: base("cmb", id, "BonusPickup")
	{
		this.reward = reward;
	}

	public new static CollectMatchBonusAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromDict(TFUtils.LoadDict(data, "match_bonus"));
		CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(id, reward);
		collectMatchBonusAction.DropTargetDataFromDict(data);
		return collectMatchBonusAction;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["match_bonus"] = reward.ToDict();
		DropTargetDataToDict(dictionary);
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.MatchBonus = null;
		simulated.EnterInitialState(EntityManager.ResidentActions["start_wander"], game.simulation);
		game.ApplyReward(reward, GetTime());
		AddPickup(game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, target);
		unitGameState["match_bonus"] = null;
		RewardManager.ApplyToGameState(reward, GetTime(), gameState);
		AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		reward.AddDataToTrigger(ref data);
	}
}
