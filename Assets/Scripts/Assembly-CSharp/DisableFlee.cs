using System.Collections.Generic;

public class DisableFlee : SessionActionDefinition
{
	public const string TYPE = "disable_flee";

	public const string WANDERER_ID = "id";

	private int? wandererID;

	public static DisableFlee Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
	}
}
