using System.Collections.Generic;

public class CompleteMicroEvent : SessionActionDefinition
{
	public const string TYPE = "complete_micro_event";

	public const string MICRO_EVENT_DID = "id";

	private int? microEventDID;

	public static CompleteMicroEvent Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		CompleteMicroEvent completeMicroEvent = new CompleteMicroEvent();
		completeMicroEvent.Parse(data, id, startConditions, originatedFromQuest);
		return completeMicroEvent;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		microEventDID = TFUtils.TryLoadInt(data, "id");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		int? num = microEventDID;
		dictionary["id"] = (num.HasValue ? microEventDID : new int?(-1));
		return dictionary;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = microEventDID;
		if (!num.HasValue || microEventDID.Value < 0)
		{
			action.MarkFailed();
			return;
		}
		MicroEvent microEvent = session.TheGame.microEventManager.GetMicroEvent(microEventDID.Value);
		if (microEvent == null || microEvent.IsCompleted())
		{
			action.MarkFailed();
			return;
		}
		microEvent.m_ulCompleteTime = TFUtils.EpochTime();
		session.TheGame.simulation.ModifyGameState(new MicroEventCompleteAction(microEvent));
		action.MarkSucceeded();
	}
}
