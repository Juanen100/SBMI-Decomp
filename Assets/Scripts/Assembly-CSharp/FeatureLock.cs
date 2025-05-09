#define ASSERTS_ON
using System.Collections.Generic;

public class FeatureLock
{
	private string feature;

	private Dictionary<string, object> unlockActionData;

	public string Feature
	{
		get
		{
			return feature;
		}
	}

	public FeatureLock(Dictionary<string, object> data)
	{
		feature = TFUtils.LoadString(data, "feature");
		TFUtils.Assert(Features.FeatureSet.Contains(feature), "Unknown Feature found " + feature + " in FeatureLocks.");
		if (data.ContainsKey("unlock_action"))
		{
			unlockActionData = TFUtils.LoadDict(data, "unlock_action");
		}
	}

	public void Activate(Game game)
	{
		SessionActionDefinition definition = SessionActionFactory.Create(unlockActionData, new ConstantCondition(0u, true));
		SessionActionTracker sessionAction = new SessionActionTracker(definition);
		game.sessionActionManager.Request(sessionAction, game);
	}
}
