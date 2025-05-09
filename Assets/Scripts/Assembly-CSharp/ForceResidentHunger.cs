#define ASSERTS_ON
using System.Collections.Generic;

public class ForceResidentHunger : SessionActionDefinition
{
	public const string TYPE = "force_wish";

	private const string DEFINITION_ID = "definition_id";

	private const string IDENTITY = "identity";

	private const string RESOURCE_ID = "resource_id";

	private int? targetDid;

	private Identity targetIdentity;

	private int resourceId;

	public static ForceResidentHunger Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceResidentHunger forceResidentHunger = new ForceResidentHunger();
		forceResidentHunger.Parse(data, id, startConditions, originatedFromQuest);
		return forceResidentHunger;
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
		resourceId = TFUtils.LoadInt(data, "resource_id");
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
		TFUtils.Assert(simulated != null, "Failed to find a simulated for Force Hunger Session Action: " + ToString());
		if (simulated != null)
		{
			simulated.forcedWish = resourceId;
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			if (entity != null)
			{
				entity.PreviousResourceId = entity.HungerResourceId;
				entity.HungerResourceId = resourceId;
				entity.WishExpiresAt = 18446744073709551614uL;
				simulated.ClearPendingCommands();
				if (entity.m_pTask == null)
				{
					simulated.EnterInitialState(EntityManager.ResidentActions["wishing"], session.TheGame.simulation);
				}
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}
}
