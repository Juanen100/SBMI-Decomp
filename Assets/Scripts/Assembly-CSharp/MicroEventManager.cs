using System.Collections.Generic;

public class MicroEventManager
{
	private float m_fUpdateTimer;

	private float m_fPreviousClosedEventUpdateTime;

	private float m_nClosedEventMinWaitTime;

	private Dictionary<int, MicroEventData> m_pMicroEventDatas;

	private Dictionary<int, MicroEvent> m_pMicroEvents;

	public void AddMicroEvent(Game pGame, MicroEvent pMicroEvent, bool bLoading = false)
	{
	}

	public MicroEventData GetMicroEventData(int nDID, bool bDefaultActiveMicroEventData = false)
	{
		return null;
	}

	public MicroEvent GetMicroEvent(int nDID)
	{
		return null;
	}

	public bool IsMicroEventActive(int nDID)
	{
		return false;
	}

	public bool IsMicroEventActive(MicroEvent pMicroEvent)
	{
		return false;
	}

	public bool IsMicroEventActive(MicroEventData pMicroEventData)
	{
		return false;
	}

	public void OnUpdate(Session pSession)
	{
	}

	private void UpdateClosedTypeEvents(Game pGame)
	{
	}

	private void LoadFromSpreadsheet()
	{
	}
}
