using System.Collections.Generic;

public class PurchaseResourcesAction : PersistedTriggerableAction
{
	public const string PURCHASE_RESOURCES = "pr";

	public Cost purchasedResources;

	public Cost rmtCost;

	public TriggerableMixin Triggerable
	{
		get
		{
			return null;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	private PurchaseResourcesAction(Identity id, Cost resources, Cost cost)
		: base(null, null)
	{
	}

	public PurchaseResourcesAction(Identity id, int rmtCost, Cost resources)
		: base(null, null)
	{
	}

	public new static PurchaseResourcesAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
