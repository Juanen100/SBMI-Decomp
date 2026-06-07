using System.Collections.Generic;

public class FeatureUnlocksAction : PersistedTriggerableAction
{
	public const string UNLOCK_FEATURE = "uf";

	public List<string> features;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public FeatureUnlocksAction(List<string> features)
		: base(null, null)
	{
	}

	public new static FeatureUnlocksAction FromDict(Dictionary<string, object> data)
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
