#define ASSERTS_ON
using System.Collections.Generic;

public class ForceRentReady : SessionActionDefinition
{
	public const string TYPE = "force_rent_ready";

	private const string BUILDING_DID = "building_did";

	private const string BUILDING_IDENTITY = "building_identity";

	private int? targetDid;

	private Identity targetIdentity;

	public static ForceRentReady Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceRentReady forceRentReady = new ForceRentReady();
		forceRentReady.Parse(data, id, startConditions, originatedFromQuest);
		return forceRentReady;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
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
		TFUtils.Assert(simulated != null, "Failed to find a simulated for Force Rent Ready Session Action: " + ToString());
		if (simulated != null)
		{
			simulated.ClearPendingCommands();
			session.TheGame.simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id);
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			if (entity.HasDecorator<PeriodicProductionDecorator>())
			{
				entity.GetDecorator<PeriodicProductionDecorator>().ProductReadyTime = TFUtils.EpochTime();
				simulated.EnterInitialState(EntityManager.BuildingActions["produced"], session.TheGame.simulation);
			}
			else
			{
				simulated.EnterInitialState(EntityManager.BuildingActions["active"], session.TheGame.simulation);
			}
		}
		action.MarkSucceeded();
	}
}
