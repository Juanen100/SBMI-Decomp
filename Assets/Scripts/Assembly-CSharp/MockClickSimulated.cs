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
			return targetId;
		}
	}

	public int? TargetDid
	{
		get
		{
			return targetDid;
		}
	}

	public static MockClickSimulated Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		MockClickSimulated mockClickSimulated = new MockClickSimulated();
		mockClickSimulated.Parse(data, id, startConditions, originatedFromQuest);
		return mockClickSimulated;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		string text = TFUtils.TryLoadString(data, "instance_id");
		if (text != null)
		{
			targetId = new Identity(text);
		}
		targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		if (targetId != null)
		{
			dictionary["instance_id"] = targetId;
		}
		int? num = targetDid;
		if (num.HasValue)
		{
			dictionary["definition_id"] = targetDid;
		}
		return dictionary;
	}

	public void HandleClick(Session session, SessionActionTracker action, Simulated simulated)
	{
		session.AddAsyncResponse("mock_click_sessionaction", simulated);
		action.MarkStarted();
		action.MarkSucceeded();
	}
}
