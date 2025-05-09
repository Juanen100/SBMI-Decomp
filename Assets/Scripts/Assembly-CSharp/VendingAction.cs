using System;
using System.Collections.Generic;

public class VendingAction : PersistedSimulatedAction
{
	public const string VENDING_ACTION = "va";

	private int slotId;

	private Reward reward;

	private Cost cost;

	private bool special;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public VendingAction(Identity id, int slotId, bool special, Reward reward, Cost cost)
		: base("va", id, typeof(VendingAction).ToString())
	{
		this.slotId = slotId;
		this.cost = cost;
		this.reward = reward;
		this.special = special;
	}

	public new static VendingAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		Cost cost = Cost.FromObject(data["cost"]);
		Reward reward = Reward.FromObject(data["reward"]);
		bool flag = TFUtils.LoadBool(data, "special");
		return new VendingAction(id, num, flag, reward, cost);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["slot_id"] = slotId;
		dictionary["reward"] = reward.ToDict();
		dictionary["cost"] = cost.ToDict();
		dictionary["special"] = special;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		VendingInstance vendingInstance = ((!special) ? game.vendingManager.GetVendingInstance(target, slotId) : game.vendingManager.GetSpecialInstance(target));
		if (vendingInstance.remaining > 0)
		{
			vendingInstance.remaining--;
			game.ApplyReward(reward, TFUtils.EpochTime());
			game.resourceManager.Apply(cost, game);
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["vending"];
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> data = (Dictionary<string, object>)list.Find(match);
		Dictionary<string, object> data2 = ((!special) ? TFUtils.LoadDict(data, "general_instances") : TFUtils.LoadDict(data, "special_instances"));
		Dictionary<string, object> dictionary = TFUtils.LoadDict(data2, slotId.ToString());
		int num = TFUtils.LoadInt(dictionary, "remaining");
		if (num > 0)
		{
			RewardManager.ApplyToGameState(reward, TFUtils.EpochTime(), gameState);
			ResourceManager.ApplyCostToGameState(cost, gameState);
			dictionary["remaining"] = num - 1;
		}
		else
		{
			TFUtils.ErrorLog("Trying to confirm selling an item which is out of stock: " + TFUtils.DebugDictToString(dictionary));
		}
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		reward.AddDataToTrigger(ref data);
	}
}
