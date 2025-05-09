using System.Collections.Generic;

public class MicroEventStartAction : PersistedTriggerableAction
{
	public const string MICRO_EVENT_START = "msa";

	private MicroEvent m_pMicroEvent;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public MicroEventStartAction(MicroEvent pMicroEvent)
		: base("msa", Identity.Null())
	{
		m_pMicroEvent = pMicroEvent;
	}

	public new static MicroEventStartAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "micro_event_invariable");
		MicroEvent pMicroEvent = new MicroEvent(null, pInvariableData, true);
		return new MicroEventStartAction(pMicroEvent);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("micro_event_invariable", m_pMicroEvent.m_pMicroEventData.GetInvariableData());
		return dictionary;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		pGame.microEventManager.AddMicroEvent(pGame, new MicroEvent(pGame, m_pMicroEvent.GetInvariableData()));
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["micro_events"];
		list.Add(m_pMicroEvent.GetInvariableData());
		base.Confirm(pGameState);
	}

	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		pData.Add("micro_event_id", m_pMicroEvent.m_pMicroEventData.m_nDID);
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
