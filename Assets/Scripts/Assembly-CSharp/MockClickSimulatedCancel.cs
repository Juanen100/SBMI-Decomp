using System.Collections.Generic;

public class MockClickSimulatedCancel : SessionActionDefinition
{
	public const string TYPE = "mock_click_simulated_cancel";

	public static MockClickSimulatedCancel Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		MockClickSimulatedCancel mockClickSimulatedCancel = new MockClickSimulatedCancel();
		mockClickSimulatedCancel.Parse(data, id, startConditions, originatedFromQuest);
		return mockClickSimulatedCancel;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
	}

	public override Dictionary<string, object> ToDict()
	{
		return base.ToDict();
	}

	public void HandleCancel(Session session, SessionActionTracker action)
	{
		session.CheckAsyncRequest("mock_click_sessionaction");
		action.MarkStarted();
		action.MarkSucceeded();
	}
}
