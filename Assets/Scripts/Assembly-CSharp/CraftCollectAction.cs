using System;
using System.Collections.Generic;

public class CraftCollectAction : PersistedSimulatedAction
{
	public const string CRAFT_COLLECT = "cc";

	public const string PICKUP_TRIGGERTYPE = "CraftPickup";

	private Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public CraftCollectAction(Identity id, Reward reward)
		: base("cc", id, "CraftPickup")
	{
		this.reward = reward;
	}

	public new static CraftCollectAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = ((!data.ContainsKey("reward")) ? null : Reward.FromObject(data["reward"]));
		CraftCollectAction craftCollectAction = new CraftCollectAction(id, reward);
		craftCollectAction.DropTargetDataFromDict(data);
		return craftCollectAction;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.ApplyReward(reward, GetTime());
		AddPickup(game.simulation);
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		entity.ClearCraftingRewards();
		simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		RewardManager.ApplyToGameState(reward, GetTime(), gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		if (dictionary.ContainsKey("craft.rewards"))
		{
			dictionary.Remove("craft.rewards");
		}
		else
		{
			dictionary.Remove("craft_rewards");
		}
		AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = reward.ToDict();
		DropTargetDataToDict(dictionary);
		return dictionary;
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		reward.AddDataToTrigger(ref data);
	}
}
