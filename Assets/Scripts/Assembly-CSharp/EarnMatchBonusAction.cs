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
		: base("emb", id, typeof(EarnMatchBonusAction).ToString())
	{
		this.reward = reward;
	}

	public new static EarnMatchBonusAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromDict(TFUtils.LoadDict(data, "match_bonus"));
		return new EarnMatchBonusAction(id, reward);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["match_bonus"] = ((reward != null) ? reward.ToDict() : null);
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.MatchBonus = reward;
		simulated.ClearPendingCommands();
		if (reward != null)
		{
			simulated.EnterInitialState(EntityManager.ResidentActions["wait_bonus"], game.simulation);
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, target);
		unitGameState["match_bonus"] = ((reward != null) ? reward.ToDict() : null);
		base.Confirm(gameState);
	}
}
