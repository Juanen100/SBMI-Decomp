using System.Collections.Generic;

public class MicroEventCompleteAction : PersistedTriggerableAction
{
	public const string MICRO_EVENT_COMPLETE = "mca";

	private MicroEvent m_pMicroEvent;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public MicroEventCompleteAction(MicroEvent pMicroEvent)
		: base("mca", Identity.Null())
	{
		m_pMicroEvent = pMicroEvent;
	}

	public new static MicroEventCompleteAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "micro_event_invariable");
		MicroEvent pMicroEvent = new MicroEvent(null, pInvariableData, true);
		return new MicroEventCompleteAction(pMicroEvent);
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
		MicroEvent microEvent = pGame.microEventManager.GetMicroEvent(m_pMicroEvent.m_pMicroEventData.m_nDID);
		if (microEvent != null)
		{
			microEvent.m_ulCompleteTime = m_pMicroEvent.m_ulCompleteTime;
		}
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["micro_events"];
		int nDID = m_pMicroEvent.m_pMicroEventData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(dictionary, MicroEventData._sDID) == nDID)
			{
				if (dictionary.ContainsKey(MicroEvent._sCOMPLETE_TIME))
				{
					dictionary[MicroEvent._sCOMPLETE_TIME] = m_pMicroEvent.m_ulCompleteTime;
				}
				else
				{
					dictionary.Add(MicroEvent._sCOMPLETE_TIME, m_pMicroEvent.m_ulCompleteTime);
				}
				break;
			}
		}
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
