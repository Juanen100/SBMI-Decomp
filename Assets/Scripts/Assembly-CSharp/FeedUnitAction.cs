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
			return true;
		}
	}

	public FeedUnitAction(Simulated unit, ulong hungerPeriod, int hungerResourceId, int? prevHungerResourceId, Reward reward)
		: this(unit.Id, hungerPeriod, hungerResourceId, prevHungerResourceId, reward)
	{
	}

	private FeedUnitAction(Identity id, ulong hungerPeriod, int hungerResourceId, int? prevHungerResourceId, Reward reward)
		: base("fu", id, typeof(FeedUnitAction).ToString())
	{
		this.hungerPeriod = hungerPeriod;
		this.hungerResourceId = hungerResourceId;
		this.reward = reward;
		this.prevHungerResourceId = prevHungerResourceId;
	}

	public new static FeedUnitAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		ulong num = TFUtils.LoadUlong(data, "hungerPeriod");
		int num2 = TFUtils.LoadInt(data, "hungerResourceId");
		int? num3 = TFUtils.TryLoadInt(data, "prevHungerResourceId");
		Reward reward = null;
		if (data.ContainsKey("reward"))
		{
			reward = Reward.FromObject(data["reward"]);
		}
		FeedUnitAction feedUnitAction = new FeedUnitAction(id, num, num2, num3, reward);
		feedUnitAction.DropTargetDataFromDict(data);
		return feedUnitAction;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["hungerResourceId"] = hungerResourceId;
		dictionary["hungerPeriod"] = hungerPeriod;
		if (reward != null)
		{
			dictionary["reward"] = reward.ToDict();
		}
		if (prevHungerResourceId.HasValue)
		{
			dictionary["prevHungerResourceId"] = prevHungerResourceId;
		}
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
		entity.HungerResourceId = null;
		entity.WishExpiresAt = null;
		ulong hungryAt = GetTime() + hungerPeriod;
		entity.HungryAt = hungryAt;
		entity.FullnessLength = hungerPeriod;
		entity.PreviousResourceId = prevHungerResourceId;
		simulated.EnterInitialState(EntityManager.ResidentActions["try_spin"], game.simulation);
		game.resourceManager.Add(hungerResourceId, -1, game);
		if (reward != null)
		{
			game.ApplyReward(reward, GetTime());
			AddPickup(game.simulation);
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResidentEntity.UpdateHungerTimeInGameState(gameState, target, GetTime() + hungerPeriod);
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, target);
		unitGameState.Remove("wish_product_id");
		unitGameState["fullness_length"] = hungerPeriod;
		if (prevHungerResourceId.HasValue)
		{
			unitGameState["prev_wish_product_id"] = prevHungerResourceId;
		}
		ResourceManager.ApplyCostToGameState(hungerResourceId, 1, gameState);
		if (reward != null)
		{
			RewardManager.ApplyToGameState(reward, GetTime(), gameState);
			AddPickupToGameState(gameState);
		}
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		Resource.AddToTriggerData(ref data, hungerResourceId);
	}
}
