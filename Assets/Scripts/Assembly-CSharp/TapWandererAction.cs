using System.Collections.Generic;

public class TapWandererAction : PersistedSimulatedAction
{
	public const string TAP_WANDERER = "tw";

	public int dId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TapWandererAction(Identity id, int did)
		: base(null, null, null)
	{
	}

	public TapWandererAction(Simulated simulated)
		: base(null, null, null)
	{
	}

	public new static TapWandererAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}
}
