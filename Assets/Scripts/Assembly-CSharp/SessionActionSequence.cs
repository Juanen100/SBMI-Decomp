using System.Collections.Generic;

public class SessionActionSequence : SessionActionCollection
{
	public const string TYPE = "sequence";

	public const string STEP = "step";

	public static SessionActionSequence Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	public override void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
	}

	public override bool ActiveProcess(Game game, SessionActionTracker action)
	{
		return false;
	}

	public override void OnObliterate(Game game, SessionActionTracker tracker)
	{
	}

	private void ObliterateAllSteps(ref List<SessionActionTracker> steps, Game game)
	{
	}
}
