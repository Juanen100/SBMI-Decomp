using System.Collections.Generic;

public class RestockVendorAction : PersistedSimulatedAction
{
	public const string VENDOR_RESTOCK = "vr";

	private Dictionary<string, object> generalInstances;

	private Dictionary<string, object> specialInstances;

	private ulong nextRestock;

	private ulong nextSpecialRestock;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RestockVendorAction(Identity id, ulong restockTime, ulong specialRestockTime, Dictionary<string, object> generalInstances, Dictionary<string, object> specialInstances)
		: base(null, null, null)
	{
	}

	public static RestockVendorAction Create(Identity id, ulong restockTime, ulong specialRestockTime, Dictionary<int, VendingInstance> generalInstances, Dictionary<int, VendingInstance> specialInstances)
	{
		return null;
	}

	public new static RestockVendorAction FromDict(Dictionary<string, object> data)
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
