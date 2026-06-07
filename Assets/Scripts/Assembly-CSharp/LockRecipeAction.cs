using System.Collections.Generic;

public class LockRecipeAction : PersistedTriggerableAction
{
	public const string LOCK_RECIPE = "lr";

	public int did;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public LockRecipeAction(int did)
		: base(null, null)
	{
	}

	public new static LockRecipeAction FromDict(Dictionary<string, object> data)
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
