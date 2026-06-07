using System.Collections.Generic;

public class CompleteMicroEvent : SessionActionDefinition
{
	public const string TYPE = "complete_micro_event";

	public const string MICRO_EVENT_DID = "id";

	private int? microEventDID;

	public static CompleteMicroEvent Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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
