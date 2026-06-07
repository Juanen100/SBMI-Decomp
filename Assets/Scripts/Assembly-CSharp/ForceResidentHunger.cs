using System.Collections.Generic;

public class ForceResidentHunger : SessionActionDefinition
{
	public const string TYPE = "force_wish";

	private int? targetDid;

	private Identity targetIdentity;

	private int resourceId;

	private const string DEFINITION_ID = "definition_id";

	private const string IDENTITY = "identity";

	private const string RESOURCE_ID = "resource_id";

	public static ForceResidentHunger Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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
