using System.Collections.Generic;

public class PurchaseCraftingSlotAction : PersistedSimulatedAction
{
	public const string PURCHASE_CRAFTING_SLOT = "pcs";

	private Cost cost;

	private int slots;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public PurchaseCraftingSlotAction(Identity id, Cost cost, int slots)
		: base(null, null, null)
	{
	}

	public new static PurchaseCraftingSlotAction FromDict(Dictionary<string, object> data)
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
}
