using System.Collections.Generic;

public class MockClickSimulated : SessionActionDefinition
{
	public const string TYPE = "mock_click_simulated";

	public const string ACTION = "mock_click_sessionaction";

	private const string INSTANCE_ID = "instance_id";

	private const string DEFINITION_ID = "definition_id";

	private Identity targetId;

	private int? targetDid;

	public Identity TargetId
	{
		get
		{
			return null;
		}
	}

	public int? TargetDid
	{
		get
		{
			return null;
		}
	}

	public static MockClickSimulated Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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

	public void HandleClick(Session session, SessionActionTracker action, Simulated simulated)
	{
	}
}
