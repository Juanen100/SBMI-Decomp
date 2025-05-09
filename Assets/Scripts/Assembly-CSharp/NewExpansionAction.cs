using System.Collections.Generic;

public class NewExpansionAction : PersistedTriggerableAction
{
	public const string NEW_EXPANSION = "ne";

	public int did;

	public Cost cost;

	public List<TerrainSlotObject> debris;

	public List<TerrainSlotObject> landmarks;

	public TriggerableMixin Triggerable
	{
		get
		{
			return triggerable;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public NewExpansionAction(int id, Cost cost, List<TerrainSlotObject> debris, List<TerrainSlotObject> landmarks)
		: base("ne", Identity.Null())
	{
		did = id;
		this.cost = cost;
		this.debris = debris;
		this.landmarks = landmarks;
	}

	public new static NewExpansionAction FromDict(Dictionary<string, object> data)
	{
		int id = TFUtils.LoadInt(data, "did");
		List<TerrainSlotObject> list = TerrainSlot.LoadExpansionObjectData((List<object>)data["debris"]);
		List<TerrainSlotObject> list2 = TerrainSlot.LoadExpansionObjectData((List<object>)data["landmarks"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new NewExpansionAction(id, cost, list, list2);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = did;
		dictionary["cost"] = cost.ToDict();
		dictionary["debris"] = TerrainSlot.SerializeExpansionObjectData(debris);
		dictionary["landmarks"] = TerrainSlot.SerializeExpansionObjectData(landmarks);
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Apply(cost, game);
		foreach (TerrainSlotObject landmark in landmarks)
		{
			Simulated simulated = game.simulation.FindSimulated(landmark.id);
			if (simulated != null)
			{
				game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), simulated.Id));
				continue;
			}
			simulated = game.simulation.CreateSimulated(EntityType.LANDMARK, landmark.did, landmark.position.ToVector2());
			simulated.Warp(simulated.Position, game.simulation);
			simulated.Visible = true;
		}
		foreach (TerrainSlotObject debri in debris)
		{
			Simulated simulated2 = game.simulation.FindSimulated(debri.id);
			if (simulated2 != null)
			{
				game.simulation.Router.Send(PurchaseCommand.Create(simulated2.Id, simulated2.Id));
				continue;
			}
			simulated2 = game.simulation.CreateSimulated(EntityType.DEBRIS, debri.did, debri.position.ToVector2());
			simulated2.Warp(simulated2.Position, game.simulation);
			simulated2.Visible = true;
		}
		game.terrain.AddExpansionSlot(did);
		if (game.featureManager.CheckFeature("purchase_expansions"))
		{
			game.terrain.UpdateRealtySigns(game.entities.DisplayControllerManager, SBCamera.BillboardDefinition, game);
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["expansions"];
		list.Add(did);
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["landmarks"];
		foreach (TerrainSlotObject landmark in landmarks)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = landmark.did;
			dictionary["label"] = landmark.id.Describe();
			dictionary["x"] = landmark.position.X;
			dictionary["y"] = landmark.position.Y;
			list2.Add(dictionary);
		}
		List<object> list3 = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
		foreach (TerrainSlotObject debri in debris)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["did"] = debri.did;
			dictionary2["label"] = debri.id.Describe();
			dictionary2["x"] = debri.position.X;
			dictionary2["y"] = debri.position.Y;
			list3.Add(dictionary2);
		}
		ResourceManager.ApplyCostToGameState(cost, gameState);
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data.Add("expansion_id", did);
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
