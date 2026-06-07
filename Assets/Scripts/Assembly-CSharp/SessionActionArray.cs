using System.Collections.Generic;

public class SessionActionArray : SessionActionCollection
{
	public const string TYPE = "array";

	public static SessionActionArray Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	public override void PreActivate(Game game, SessionActionTracker action)
	{
	}

	public override bool ActiveProcess(Game game, SessionActionTracker action)
	{
		return false;
	}

	public override void PostComplete(Game game, SessionActionTracker action)
	{
	}
}
