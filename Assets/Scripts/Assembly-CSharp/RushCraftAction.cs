using System.Collections.Generic;

public class RushCraftAction : PersistedSimulatedAction
{
	public const string RUSH_CRAFT = "rc";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	private Cost rushCost;

	private ulong craftReadyTime;

	private Reward craftReward;

	private int slotId;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public RushCraftAction(Identity id, int slotId, Cost rushCost, ulong newReadyTime, Reward craftReward)
		: base("rc", id, typeof(RushCraftAction).ToString())
	{
		this.rushCost = rushCost;
		craftReadyTime = newReadyTime;
		this.craftReward = craftReward;
		this.slotId = slotId;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = rushCost.ToDict();
		dictionary["ready_time"] = craftReadyTime;
		dictionary["reward"] = craftReward.ToDict();
		dictionary["slot_id"] = slotId;
		return dictionary;
	}

	public new static RushCraftAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		ulong newReadyTime = TFUtils.LoadUlong(data, "ready_time");
		Reward reward = Reward.FromDict((Dictionary<string, object>)data["reward"]);
		return new RushCraftAction(id, num, cost, newReadyTime, reward);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.craftManager.GetCraftingInstance(target, slotId).ReadyTimeUtc = utcNow;
		game.simulation.Router.CancelMatching(Command.TYPE.CRAFTED, target, target, new Dictionary<string, object> { { "slot_id", slotId } });
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		if (utcNow > craftReadyTime)
		{
			game.simulation.Router.Send(CraftedCommand.Create(target, target, slotId), utcNow - craftReadyTime);
			if (simulated.GetEntity<BuildingEntity>().CraftRewards == null)
			{
				simulated.LoadInitialState(EntityManager.BuildingActions["crafting"]);
			}
			else
			{
				simulated.LoadInitialState(EntityManager.BuildingActions["craftcycling"]);
			}
		}
		else
		{
			simulated.LoadInitialState(EntityManager.BuildingActions["craftcycling"]);
		}
		game.resourceManager.Apply(rushCost, game);
		base.Apply(game, utcNow);
	}

	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (craftReadyTime == ulong.MaxValue)
		{
			craftReadyTime = time;
		}
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(rushCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["crafts"];
		string targetString = target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object c) => ((string)((Dictionary<string, object>)c)["building_label"]).Equals(targetString));
		dictionary["ready_time"] = craftReadyTime;
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}
}
