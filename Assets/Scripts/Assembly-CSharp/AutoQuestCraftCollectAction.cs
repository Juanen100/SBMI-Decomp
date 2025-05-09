using System.Collections.Generic;

public class AutoQuestCraftCollectAction : PersistedSimulatedAction
{
	public const string AUTO_QUEST_CRAFT_COLLECT = "aqcc";

	private Reward reward;

	private int recipeId;

	private int count;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public AutoQuestCraftCollectAction(int nDID, int nCount)
		: base("aqcc", Identity.Null(), typeof(AutoQuestCraftCollectAction).ToString())
	{
		recipeId = nDID;
		count = nCount;
		reward = new Reward(new Dictionary<int, int> { 
		{
			nDID,
			-nCount
		} }, null, null, null, null, null, null, null, false, null);
	}

	public new static AutoQuestCraftCollectAction FromDict(Dictionary<string, object> data)
	{
		int nDID = TFUtils.LoadInt(data, "recipe_id");
		int nCount = TFUtils.LoadInt(data, "count");
		return new AutoQuestCraftCollectAction(nDID, nCount);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["count"] = count;
		dictionary["recipe_id"] = reward.ToDict();
		return dictionary;
	}

	public override void Process(Game game)
	{
		game.ApplyReward(reward, GetTime());
		base.Process(game);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.ApplyReward(reward, GetTime());
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		RewardManager.ApplyToGameState(reward, GetTime(), gameState);
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		reward.AddDataToTrigger(ref data);
		data["recipe_id"] = recipeId;
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		Simulated simulated = (Simulated)data["simulated"];
		entityId = simulated.entity.Id;
		definitionId = simulated.entity.DefinitionId;
		simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
