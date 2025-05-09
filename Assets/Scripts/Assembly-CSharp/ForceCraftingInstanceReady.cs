#define ASSERTS_ON
using System.Collections.Generic;

public class ForceCraftingInstanceReady : SessionActionDefinition
{
	public const string TYPE = "force_crafting_instance_ready";

	private const string SLOT_ID = "slot_id";

	private const string BUILDING_DID = "building_did";

	private const string BUILDING_IDENTITY = "building_identity";

	private int slotId;

	private int? targetDid;

	private Identity targetIdentity;

	public static ForceCraftingInstanceReady Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceCraftingInstanceReady forceCraftingInstanceReady = new ForceCraftingInstanceReady();
		forceCraftingInstanceReady.Parse(data, id, startConditions, originatedFromQuest);
		return forceCraftingInstanceReady;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		slotId = TFUtils.LoadInt(data, "slot_id");
		targetDid = TFUtils.TryLoadNullableInt(data, "building_did");
		string text = TFUtils.TryLoadString(data, "building_identity");
		if (text != null)
		{
			targetIdentity = new Identity(text);
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		int? num = targetDid;
		if (num.HasValue)
		{
			dictionary["building_did"] = targetDid;
		}
		return dictionary;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		Simulated simulated = null;
		if (targetIdentity != null)
		{
			simulated = session.TheGame.simulation.FindSimulated(targetIdentity);
		}
		else if (targetDid.HasValue)
		{
			simulated = session.TheGame.simulation.FindSimulated(targetDid.Value);
		}
		TFUtils.Assert(simulated != null, "Failed to find a simulated for Force Hunger Session Action: " + ToString());
		if (simulated != null)
		{
			CraftingInstance craftingInstance = session.TheGame.craftManager.GetCraftingInstance(simulated.Id, slotId);
			if (craftingInstance != null)
			{
				craftingInstance.ReadyTimeFromNow = 0uL;
				session.TheGame.simulation.Router.CancelMatching(Command.TYPE.CRAFTED, simulated.Id, simulated.Id, new Dictionary<string, object> { { "slot_id", slotId } });
				session.TheGame.simulation.Router.Send(CraftedCommand.Create(simulated.Id, simulated.Id, slotId), 0uL);
			}
		}
		action.MarkSucceeded();
	}
}
