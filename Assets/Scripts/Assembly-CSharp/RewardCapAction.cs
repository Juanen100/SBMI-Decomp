using System.Collections.Generic;

public class RewardCapAction : PersistedTriggerableAction
{
	private ulong expiration;

	private int recipes;

	private int jelly;

	public const string REWARD_CAP = "cap";

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RewardCapAction(int jelly, int recipes, ulong expiration)
		: base(null, null)
	{
	}

	public new static RewardCapAction FromDict(Dictionary<string, object> data)
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

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
