using System.Collections.Generic;

public class ForceCraftingInstanceSlot : SessionActionDefinition
{
	public const string TYPE = "force_crafting_instance_slot";

	public const string ACTION = "force_crafting_instance_slot_sessionaction";

	private int slotId;

	private const string SLOT_ID = "slot_id";

	public int SlotID
	{
		get
		{
			return 0;
		}
	}

	public static ForceCraftingInstanceSlot Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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
