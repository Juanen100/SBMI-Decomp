using System.Collections.Generic;

public class LockInput : SessionActionDefinition
{
	public const string TYPE = "lock_input";

	private bool activated;

	public static LockInput Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public void Handle(Session session, SessionActionTracker action)
	{
	}

	public override void OnDestroy(Game game)
	{
	}
}
