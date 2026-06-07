using System.Collections.Generic;

public class EarnMatchBonusAction : PersistedSimulatedAction
{
	public const string EARN_MATCH_BONUS = "emb";

	public Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public EarnMatchBonusAction(Identity id, Reward reward)
		: base(null, null, null)
	{
	}

	public new static EarnMatchBonusAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
