using System.Collections.Generic;

public class TapWandererAction : PersistedSimulatedAction
{
	public const string TAP_WANDERER = "tw";

	public int dId;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public TapWandererAction(Identity id, int did)
		: base("tw", id, typeof(TapWandererAction).ToString())
	{
		dId = did;
	}

	public TapWandererAction(Simulated simulated)
		: base("tw", simulated.Id, typeof(TapWandererAction).ToString())
	{
		Entity entity = simulated.entity;
		dId = entity.DefinitionId;
	}

	public new static TapWandererAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int did = TFUtils.LoadInt(data, "did");
		return new TapWandererAction(id, did);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = dId;
		return dictionary;
	}
}
