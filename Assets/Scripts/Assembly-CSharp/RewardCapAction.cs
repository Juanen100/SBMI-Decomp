using System.Collections.Generic;

public class RewardCapAction : PersistedTriggerableAction
{
	public const string REWARD_CAP = "cap";

	private ulong expiration;

	private int recipes;

	private int jelly;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RewardCapAction(int jelly, int recipes, ulong expiration)
		: base("cap", Identity.Null())
	{
		this.jelly = jelly;
		this.recipes = recipes;
		this.expiration = expiration;
	}

	public new static RewardCapAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "jelly_count");
		int num2 = TFUtils.LoadInt(data, "recipe_count");
		ulong num3 = TFUtils.LoadUlong(data, "expiration");
		return new RewardCapAction(num, num2, num3);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["recipe_count"] = recipes;
		dictionary["jelly_count"] = jelly;
		dictionary["expiration"] = expiration;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.simulation.rewardCap.Reset(jelly, recipes, expiration);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["recipe_count"] = recipes;
		dictionary["jelly_count"] = jelly;
		dictionary["expiration"] = expiration;
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)gameState["farm"];
		dictionary2["caps"] = dictionary;
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
