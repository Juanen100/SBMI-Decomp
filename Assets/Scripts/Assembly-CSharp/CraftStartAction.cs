using System.Collections.Generic;

public class CraftStartAction : PersistedSimulatedAction
{
	public const string CRAFT_START = "cs";

	private Reward reward;

	private int recipeId;

	private Cost craftingCost;

	private ulong readyTime;

	private int slotId;

	public int RecipeId
	{
		get
		{
			return recipeId;
		}
	}

	public ulong ReadyTime
	{
		get
		{
			return readyTime;
		}
	}

	protected Cost CraftingCost
	{
		get
		{
			return craftingCost;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public CraftStartAction(Identity id, int slotId, int recipeId, ulong readyTime, Reward reward, Cost cost)
		: base("cs", id, typeof(CraftStartAction).ToString())
	{
		this.recipeId = recipeId;
		this.readyTime = readyTime;
		craftingCost = cost;
		this.slotId = slotId;
		this.reward = reward;
	}

	public new static CraftStartAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		int num2 = TFUtils.LoadInt(data, "recipe_id");
		ulong num3 = TFUtils.LoadUlong(data, "ready_time");
		Reward reward = Reward.FromObject(data["reward"]);
		Cost cost = Cost.FromObject(data["cost"]);
		return new CraftStartAction(id, num, num2, num3, reward, cost);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["ready_time"] = readyTime;
		dictionary["cost"] = craftingCost.ToDict();
		dictionary["recipe_id"] = recipeId;
		dictionary["slot_id"] = slotId;
		dictionary["reward"] = reward.ToDict();
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		if (!game.craftManager.AddCraftingInstance(new CraftingInstance(target, recipeId, readyTime, reward, slotId)))
		{
			TFUtils.DebugLog("invalid action: " + ToString());
			TFUtils.DebugLog("crafting state: " + game.craftManager.GetCraftingInstance(target, slotId).ToString());
			TFUtils.ErrorLog("we are about to apply a crafting state that does not create a valid state");
		}
		game.resourceManager.Apply(craftingCost, game);
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		if (utcNow < readyTime)
		{
			game.simulation.Router.Send(CraftedCommand.Create(target, target, slotId), readyTime - utcNow);
		}
		else
		{
			game.simulation.Router.Send(CraftedCommand.Create(target, target, slotId));
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(craftingCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["crafts"];
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("building_label", target.Describe());
		dictionary.Add("ready_time", readyTime);
		dictionary.Add("recipe_id", RecipeId);
		dictionary.Add("slot_id", slotId);
		dictionary.Add("reward", reward.ToDict());
		list.Add(dictionary);
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		reward.AddDataToTrigger(ref data);
		data["recipe_id"] = recipeId;
		data["notification_time"] = TFUtils.EpochToDateTime(readyTime);
		data["notification_label"] = "craft:" + target.Describe();
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		Simulated simulated = (Simulated)data["simulated"];
		entityId = simulated.entity.Id;
		definitionId = simulated.entity.DefinitionId;
		simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
