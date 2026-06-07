using System.Collections.Generic;

public class ForceResidentBonusReward : SessionActionDefinition
{
	public const string TYPE = "force_bonus_reward";

	private int? targetDid;

	private Identity targetIdentity;

	private RewardDefinition reward;

	private const string DEFINITION_ID = "definition_id";

	private const string IDENTITY = "identity";

	private const string REWARD = "reward";

	public static ForceResidentBonusReward Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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
