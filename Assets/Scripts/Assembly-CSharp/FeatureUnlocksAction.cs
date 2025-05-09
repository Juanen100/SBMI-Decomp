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
		: base("uf", Identity.Null())
	{
		this.features = features;
	}

	public new static FeatureUnlocksAction FromDict(Dictionary<string, object> data)
	{
		List<string> list = TFUtils.LoadList<string>(data, "features");
		return new FeatureUnlocksAction(list);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["features"] = TFUtils.CloneAndCastList<string, object>(features);
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		foreach (string feature in features)
		{
			game.featureManager.UnlockFeature(feature);
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("features"))
		{
			dictionary["features"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["features"];
		foreach (string feature in features)
		{
			if (!list.Contains(feature))
			{
				list.Add(feature);
			}
		}
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
