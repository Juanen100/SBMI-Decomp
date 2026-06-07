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
			return false;
		}
	}

	public AutoQuestCraftCollectAction(int nDID, int nCount)
		: base(null, null, null)
	{
	}

	public new static AutoQuestCraftCollectAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Process(Game game)
	{
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
