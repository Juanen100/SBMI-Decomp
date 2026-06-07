using System.Collections.Generic;

public class ForceProduce : SessionActionDefinition
{
	public const string TYPE = "force_produce";

	private int? targetDid;

	private Identity targetIdentity;

	private const string DEFINITION_ID = "definition_id";

	private const string IDENTITY = "identity";

	public static ForceProduce Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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
