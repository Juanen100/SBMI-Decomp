#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class NewBuildingAction : PersistedSimulatedAction
{
	public const string NEW_BUILDING = "nb";

	public Vector2 position;

	public bool flip;

	public string blueprint;

	public bool built;

	public ulong buildCompleteTime;

	public int dId;

	public EntityType extensions;

	public Cost cost;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public NewBuildingAction(Identity id, string blueprint, int did, EntityType types, bool built, ulong buildCompleteTime, Vector2 position, bool flip, Cost cost)
		: base("nb", id, typeof(NewBuildingAction).ToString())
	{
		Initialize(blueprint, did, types, built, buildCompleteTime, position, flip, cost);
	}

	public NewBuildingAction(Simulated simulated, Cost cost)
		: base("nb", simulated.Id, typeof(NewBuildingAction).ToString())
	{
		Entity entity = simulated.entity;
		ErectableDecorator decorator = entity.GetDecorator<ErectableDecorator>();
		Initialize(entity.BlueprintName, entity.DefinitionId, entity.AllTypes, false, decorator.ErectionCompleteTime.Value, simulated.Position, simulated.Flip, cost);
	}

	public new static NewBuildingAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		bool flag = (bool)data["built"];
		string text = (string)data["blueprint"];
		int did = TFUtils.LoadInt(data, "did");
		EntityType types = (EntityType)TFUtils.LoadUint(data, "extensions");
		ulong num = TFUtils.LoadUlong(data, "buildCompleteTime");
		Vector2 vector = new Vector2(TFUtils.LoadInt(data, "x"), TFUtils.LoadInt(data, "y"));
		bool flag2 = (bool)data["flip"];
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new NewBuildingAction(id, text, did, types, flag, num, vector, flag2, cost);
	}

	private void Initialize(string blueprint, int did, EntityType types, bool built, ulong buildCompleteTime, Vector2 position, bool flip, Cost cost)
	{
		this.blueprint = blueprint;
		dId = did;
		extensions = types;
		this.built = built;
		this.buildCompleteTime = buildCompleteTime;
		this.position = position;
		this.flip = flip;
		this.cost = cost;
		TFUtils.Assert(cost != null, "Cannot create a NewBuildingAction with a null cost");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["x"] = position.x;
		dictionary["y"] = position.y;
		dictionary["flip"] = flip;
		dictionary["blueprint"] = blueprint;
		dictionary["did"] = dId;
		dictionary["extensions"] = (uint)extensions;
		dictionary["buildCompleteTime"] = buildCompleteTime;
		dictionary["activatedTime"] = null;
		dictionary["built"] = built;
		dictionary["cost"] = cost.ToDict();
		dictionary["product.ready"] = null;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Entity entity = game.entities.Create(extensions, dId, target, true);
		BuildingEntity decorator = entity.GetDecorator<BuildingEntity>();
		decorator.Slots = game.craftManager.GetInitialSlots(decorator.DefinitionId);
		decorator.GetDecorator<ErectableDecorator>().ErectionCompleteTime = buildCompleteTime;
		game.resourceManager.Apply(cost, game);
		Simulated simulated = Simulated.Building.Load(decorator, game.simulation, position, flip, utcNow);
		if ((extensions & EntityType.ANNEX) != EntityType.INVALID)
		{
			Simulated.Annex.Extend(simulated, game.simulation);
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		Dictionary<string, object> data = new Dictionary<string, object>();
		data["did"] = dId;
		data["extensions"] = (uint)extensions;
		ActivatableDecorator.Serialize(ref data, 0uL);
		data["label"] = target.Describe();
		data["x"] = position.x;
		data["y"] = position.y;
		data["flip"] = flip;
		data["build_finish_time"] = buildCompleteTime;
		data["rent_ready_time"] = null;
		list.Add(data);
		ResourceManager.ApplyCostToGameState(cost, gameState);
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		data["notification_time"] = TFUtils.EpochToDateTime(buildCompleteTime);
		data["notification_label"] = "build:" + target.Describe();
	}
}
