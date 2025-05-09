#define ASSERTS_ON
using System.Collections.Generic;

public class ForceResidentBonusReward : SessionActionDefinition
{
	public const string TYPE = "force_bonus_reward";

	private const string DEFINITION_ID = "definition_id";

	private const string IDENTITY = "identity";

	private const string REWARD = "reward";

	private int? targetDid;

	private Identity targetIdentity;

	private RewardDefinition reward;

	public static ForceResidentBonusReward Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceResidentBonusReward forceResidentBonusReward = new ForceResidentBonusReward();
		forceResidentBonusReward.Parse(data, id, startConditions, originatedFromQuest);
		return forceResidentBonusReward;
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
		if (data.ContainsKey("reward"))
		{
			reward = RewardDefinition.FromObject(data["reward"]);
		}
		TFUtils.Assert(reward != null, "Failed to define a RewardDefinition for ForceResidentBonusReward Session Action");
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
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			if (entity != null)
			{
				entity.ForcedBonusReward = reward;
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}
}
