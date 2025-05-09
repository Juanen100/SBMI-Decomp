using System;
using System.Collections.Generic;

public class CraftCompleteAction : PersistedSimulatedAction
{
	public const string CRAFT_FINISHED = "cf";

	private Reward reward;

	private int slotId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public CraftCompleteAction(Identity id, int slotId, Reward reward)
		: base("cf", id, "cf")
	{
		this.reward = reward;
		this.slotId = slotId;
	}

	public new static CraftCompleteAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		Reward reward = ((!data.ContainsKey("reward")) ? null : Reward.FromObject(data["reward"]));
		return new CraftCompleteAction(id, num, reward);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.craftManager.RemoveCraftingInstance(target, slotId);
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		game.simulation.Router.CancelMatching(Command.TYPE.CRAFTED, target, target, new Dictionary<string, object> { { "slot_id", slotId } });
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		entity.CraftingComplete(reward);
		simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["crafts"];
		string targetString = target.Describe();
		Predicate<object> match = (object c) => ((string)((Dictionary<string, object>)c)["building_label"]).Equals(targetString) && TFUtils.LoadInt((Dictionary<string, object>)c, "slot_id") == slotId;
		list.Remove((Dictionary<string, object>)list.Find(match));
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list2.Find(match);
		Reward reward = new Reward(null, null, null, null, null, null, null, null, false, this.reward.ThoughtIcon);
		reward += this.reward;
		if (dictionary.ContainsKey("craft_rewards"))
		{
			reward += Reward.FromObject(dictionary["craft_rewards"]);
		}
		else if (dictionary.ContainsKey("craft.rewards"))
		{
			reward += Reward.FromObject(dictionary["craft.rewards"]);
		}
		dictionary["craft_rewards"] = reward.ToDict();
		base.Confirm(gameState);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = reward.ToDict();
		dictionary["slot_id"] = slotId;
		return dictionary;
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		reward.AddDataToTrigger(ref data);
	}
}
