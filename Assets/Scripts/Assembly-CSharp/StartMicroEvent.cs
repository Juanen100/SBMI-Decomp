using System.Collections.Generic;

public class StartMicroEvent : SessionActionDefinition
{
	public const string TYPE = "start_micro_event";

	public const string MICRO_EVENT_DID = "id";

	private int? microEventDID;

	public static StartMicroEvent Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		StartMicroEvent startMicroEvent = new StartMicroEvent();
		startMicroEvent.Parse(data, id, startConditions, originatedFromQuest);
		return startMicroEvent;
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
		if (microEvent != null)
		{
			action.MarkFailed();
			return;
		}
		MicroEventData microEventData = session.TheGame.microEventManager.GetMicroEventData(microEventDID.Value);
		if (microEventData == null)
		{
			action.MarkFailed();
			return;
		}
		session.TheGame.microEventManager.AddMicroEvent(session.TheGame, new MicroEvent(session.TheGame, microEventDID.Value, TFUtils.EpochTime()));
		microEvent = session.TheGame.microEventManager.GetMicroEvent(microEventDID.Value);
		if (microEvent == null)
		{
			action.MarkFailed();
			return;
		}
		session.TheGame.simulation.ModifyGameState(new MicroEventStartAction(microEvent));
		action.MarkSucceeded();
	}
}
