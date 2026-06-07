using System.Collections.Generic;

public class CraftStartAction : PersistedSimulatedAction
{
	public const string CRAFT_START = "cs";

	private Reward reward;

	private int recipeId;

	private Cost craftingCost;

	private ulong readyTime;

	private int slotId;

	public int RecipeId
	{
		get
		{
			return 0;
		}
	}

	public ulong ReadyTime
	{
		get
		{
			return 0uL;
		}
	}

	protected Cost CraftingCost
	{
		get
		{
			return null;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public CraftStartAction(Identity id, int slotId, int recipeId, ulong readyTime, Reward reward, Cost cost)
		: base(null, null, null)
	{
	}

	public new static CraftStartAction FromDict(Dictionary<string, object> data)
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

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
