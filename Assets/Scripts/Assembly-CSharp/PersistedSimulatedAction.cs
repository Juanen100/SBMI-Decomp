using System.Collections.Generic;

public abstract class PersistedSimulatedAction : PersistedTriggerableAction
{
	public const string SIMULATED = "simulated";

	protected Identity entityId;

	protected int definitionId;

	protected string simType;

	private string triggerType;

	public Identity dropID;

	protected PersistedSimulatedAction(string type, Identity target, string triggerType)
		: base(null, null)
	{
	}

	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}

	public override ITrigger CreateTrigger(string type)
	{
		return null;
	}

	protected void DropTargetDataFromDict(Dictionary<string, object> data)
	{
	}

	protected void DropTargetDataToDict(Dictionary<string, object> data)
	{
	}

	public void AddDropData(Simulated simulated, Identity dropID)
	{
	}

	public void AddPickup(Simulation simulation)
	{
	}

	public void AddPickupToGameState(Dictionary<string, object> gameState)
	{
	}
}
