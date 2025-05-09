using System.Collections.Generic;

public class ForceCraftingInstanceSlot : SessionActionDefinition
{
	public const string TYPE = "force_crafting_instance_slot";

	public const string ACTION = "force_crafting_instance_slot_sessionaction";

	private const string SLOT_ID = "slot_id";

	private int slotId;

	public int SlotID
	{
		get
		{
			return slotId;
		}
	}

	public static ForceCraftingInstanceSlot Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceCraftingInstanceSlot forceCraftingInstanceSlot = new ForceCraftingInstanceSlot();
		forceCraftingInstanceSlot.Parse(data, id, startConditions, originatedFromQuest);
		return forceCraftingInstanceSlot;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		slotId = TFUtils.LoadInt(data, "slot_id");
	}

	public override Dictionary<string, object> ToDict()
	{
		return base.ToDict();
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		session.AddAsyncResponse("force_crafting_instance_slot_sessionaction", action);
		action.MarkStarted();
		action.MarkSucceeded();
	}
}
