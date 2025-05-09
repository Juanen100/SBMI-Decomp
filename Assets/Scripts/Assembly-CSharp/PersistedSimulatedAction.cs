#define ASSERTS_ON
using System.Collections.Generic;

public abstract class PersistedSimulatedAction : PersistedTriggerableAction
{
	public const string SIMULATED = "simulated";

	protected Identity entityId;

	protected int definitionId;

	protected string simType;

	private string triggerType = "undefined";

	public Identity dropID;

	protected PersistedSimulatedAction(string type, Identity target, string triggerType)
		: base(type, target)
	{
		this.triggerType = triggerType;
	}

	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["simulated_guid"] = entityId;
		data["simulated_id"] = definitionId;
		data["simulated_type"] = simType;
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		if (!data.ContainsKey("simulated"))
		{
			TFUtils.Assert(data.ContainsKey("simulated"), string.Format("Did not find key({0}) in dictionary:{1}", "simulated", TFUtils.DebugDictToString(data)));
		}
		Simulated simulated = (Simulated)data["simulated"];
		entityId = simulated.entity.Id;
		definitionId = simulated.entity.DefinitionId;
		simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}

	public override ITrigger CreateTrigger(string type)
	{
		return triggerable.BuildTrigger(type, AddMoreDataToTrigger, target, dropID);
	}

	protected void DropTargetDataFromDict(Dictionary<string, object> data)
	{
		entityId = new Identity((string)data["entityId"]);
		definitionId = TFUtils.LoadInt(data, "definitionId");
		simType = (string)data["simType"];
		if (data.ContainsKey("dropID"))
		{
			dropID = new Identity((string)data["dropID"]);
		}
	}

	protected void DropTargetDataToDict(Dictionary<string, object> data)
	{
		data["entityId"] = entityId.Describe();
		data["definitionId"] = definitionId;
		data["simType"] = simType;
		if (dropID != null)
		{
			data["dropID"] = dropID.Describe();
		}
	}

	public void AddDropData(Simulated simulated, Identity dropID)
	{
		entityId = simulated.entity.Id;
		definitionId = simulated.entity.DefinitionId;
		simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		this.dropID = dropID;
	}

	public void AddPickup(Simulation simulation)
	{
		Trigger trigger = (Trigger)CreateTrigger(triggerType);
		simulation.DropManager.AddPickupTrigger(trigger.ToDict());
	}

	public void AddPickupToGameState(Dictionary<string, object> gameState)
	{
		Trigger trigger = (Trigger)CreateTrigger(triggerType);
		ItemDropManager.AddPickupTriggerToGameState(gameState, trigger.ToDict());
	}
}
