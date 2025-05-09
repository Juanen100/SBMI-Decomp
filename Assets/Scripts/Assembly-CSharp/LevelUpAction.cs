using System.Collections.Generic;

public class LevelUpAction : PersistedTriggerableAction
{
	public const string LEVEL_UP = "lu";

	private const string WALLTIME_START_PREVIOUS_LEVEL = "wts_begin";

	private const string PLAYTIME_TO_LEVEL = "time_to";

	private Reward reward;

	private Dictionary<string, object> buildingLabels;

	private ulong buildCompleteTime;

	private int level;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public LevelUpAction(int level, Reward reward, ulong buildCompleteTime)
		: base("lu", Identity.Null())
	{
		this.level = level;
		this.reward = reward;
		this.buildCompleteTime = buildCompleteTime;
	}

	public new static LevelUpAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "level");
		Reward reward = Reward.FromObject(data["reward"]);
		ulong num2 = TFUtils.LoadUlong(data, "build_complete_time");
		return new LevelUpAction(num, reward, num2);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = Reward.RewardToDict(reward);
		dictionary["build_complete_time"] = buildCompleteTime;
		dictionary["level"] = level;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Add(ResourceManager.LEVEL, 1, game);
		if (reward != null)
		{
			game.ApplyReward(reward, GetTime());
		}
		game.playtimeRegistrar.UpdateLevel(game.resourceManager.Query(ResourceManager.LEVEL), GetTime());
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.AddAmountToGameState(ResourceManager.LEVEL, 1, gameState);
		if (reward != null)
		{
			RewardManager.ApplyToGameState(reward, buildCompleteTime, gameState);
		}
		PlaytimeRegistrar.ApplyToGameState(ref gameState, level, GetTime(), GetTime(), 0uL);
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(level, "level");
		soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
		Soaring.FireEvent("LevelUp", soaringDictionary);
		base.Confirm(gameState);
	}
}
