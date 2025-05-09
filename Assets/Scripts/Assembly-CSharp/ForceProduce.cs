#define ASSERTS_ON
using System.Collections.Generic;

public class ForceProduce : SessionActionDefinition
{
	public const string TYPE = "force_produce";

	private const string DEFINITION_ID = "definition_id";

	private const string IDENTITY = "identity";

	private int? targetDid;

	private Identity targetIdentity;

	public static ForceProduce Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceProduce forceProduce = new ForceProduce();
		forceProduce.Parse(data, id, startConditions, originatedFromQuest);
		return forceProduce;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
		string text = TFUtils.TryLoadString(data, "identity");
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
			dictionary["definition_id"] = targetDid;
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
		TFUtils.Assert(simulated != null, "Failed to find a simulated for Force Produce Session Action: " + ToString());
		if (simulated != null && simulated.GetEntity<BuildingEntity>() != null)
		{
			simulated.ClearPendingCommands();
			simulated.EnterInitialState(EntityManager.BuildingActions["produced"], session.TheGame.simulation);
			action.MarkSucceeded();
		}
		else
		{
			action.MarkFailed();
		}
	}
}
