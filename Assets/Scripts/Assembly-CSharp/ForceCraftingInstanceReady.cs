using System.Collections.Generic;

public class ForceCraftingInstanceReady : SessionActionDefinition
{
	public const string TYPE = "force_crafting_instance_ready";

	private int slotId;

	private int? targetDid;

	private Identity targetIdentity;

	private const string SLOT_ID = "slot_id";

	private const string BUILDING_DID = "building_did";

	private const string BUILDING_IDENTITY = "building_identity";

	public static ForceCraftingInstanceReady Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
	}
}
