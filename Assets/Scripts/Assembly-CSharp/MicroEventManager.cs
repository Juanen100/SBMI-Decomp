using System;
using System.Collections.Generic;
using UnityEngine;

public class MicroEventManager
{
	private float m_fUpdateTimer;

	private float m_fPreviousClosedEventUpdateTime;

	private float m_nClosedEventMinWaitTime = 300f;

	private Dictionary<int, MicroEventData> m_pMicroEventDatas;

	private Dictionary<int, MicroEvent> m_pMicroEvents;

	public MicroEventManager()
	{
		m_pMicroEvents = new Dictionary<int, MicroEvent>();
		LoadFromSpreadsheet();
	}

	public void AddMicroEvent(Game pGame, MicroEvent pMicroEvent, bool bLoading = false)
	{
		if (pMicroEvent != null)
		{
			m_pMicroEvents.Add(pMicroEvent.m_pMicroEventData.m_nDID, pMicroEvent);
		}
	}

	public MicroEventData GetMicroEventData(int nDID, bool bDefaultActiveMicroEventData = false)
	{
		if (bDefaultActiveMicroEventData && m_pMicroEvents.ContainsKey(nDID))
		{
			return m_pMicroEvents[nDID].m_pMicroEventData;
		}
		if (m_pMicroEventDatas.ContainsKey(nDID))
		{
			return m_pMicroEventDatas[nDID];
		}
		return null;
	}

	public MicroEvent GetMicroEvent(int nDID)
	{
		if (m_pMicroEvents.ContainsKey(nDID))
		{
			return m_pMicroEvents[nDID];
		}
		return null;
	}

	public bool IsMicroEventActive(int nDID)
	{
		if (m_pMicroEvents.ContainsKey(nDID))
		{
			return IsMicroEventActive(m_pMicroEvents[nDID]);
		}
		if (m_pMicroEventDatas.ContainsKey(nDID))
		{
			return IsMicroEventActive(m_pMicroEventDatas[nDID]);
		}
		return false;
	}

	public bool IsMicroEventActive(MicroEvent pMicroEvent)
	{
		if (pMicroEvent != null)
		{
			return pMicroEvent.IsActive();
		}
		return false;
	}

	public bool IsMicroEventActive(MicroEventData pMicroEventData)
	{
		if (pMicroEventData != null)
		{
			return pMicroEventData.IsActive();
		}
		return false;
	}

	public void OnUpdate(Session pSession)
	{
		m_fUpdateTimer += Time.deltaTime;
		if (m_fPreviousClosedEventUpdateTime == 0f || !(Time.time - m_fPreviousClosedEventUpdateTime <= m_nClosedEventMinWaitTime))
		{
			UpdateClosedTypeEvents(pSession.TheGame);
			m_fPreviousClosedEventUpdateTime = Time.time;
		}
	}

	private void UpdateClosedTypeEvents(Game pGame)
	{
		foreach (KeyValuePair<int, MicroEvent> pMicroEvent in m_pMicroEvents)
		{
			MicroEvent value = pMicroEvent.Value;
			if (value.m_pMicroEventData.m_bClosedEvent)
			{
				if (value.m_bIsClosed && value.m_pMicroEventData.IsActive())
				{
					value.m_bIsClosed = false;
					pGame.simulation.ModifyGameState(new MicroEventOpenAction(value));
					pGame.questManager.HandleMicroEventClosedStatusChange(pGame, value);
				}
				else if (!value.m_bIsClosed && !value.m_pMicroEventData.IsActive())
				{
					value.m_bIsClosed = true;
					pGame.simulation.ModifyGameState(new MicroEventCloseAction(value));
					pGame.questManager.HandleMicroEventClosedStatusChange(pGame, value);
				}
			}
		}
	}

	private void LoadFromSpreadsheet()
	{
		m_pMicroEventDatas = new Dictionary<int, MicroEventData>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "MicroEvents";
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
			if (m_pMicroEventDatas.ContainsKey(intCell))
			{
				TFUtils.ErrorLog("Micro Event Collision! DID: " + intCell);
				continue;
			}
			dictionary.Add(MicroEventData._sDID, intCell);
			dictionary.Add(MicroEventData._sCLOSE_DIALOG_SEQUENCE_DID, instance.GetIntCell(sheetIndex, rowIndex, "closing dialog sequence"));
			dictionary.Add(MicroEventData._sCLOSED_EVENT, instance.GetIntCell(sheetIndex, rowIndex, "closed event") == 1);
			dictionary.Add(MicroEventData._sNAME, instance.GetStringCell(sheetIndex, rowIndex, "name"));
			DateTime result;
			if (DateTime.TryParse(instance.GetStringCell(sheetIndex, rowIndex, "start date"), out result))
			{
				dictionary.Add(MicroEventData._sSTART_DATE, (DateTime.SpecifyKind(result, DateTimeKind.Utc) - SoaringTime.Epoch).TotalSeconds);
			}
			else
			{
				dictionary.Add(MicroEventData._sSTART_DATE, 0);
				TFUtils.ErrorLog("MicroEventManager | cannot parse start date for micro event DID: " + intCell);
			}
			if (DateTime.TryParse(instance.GetStringCell(sheetIndex, rowIndex, "end date"), out result))
			{
				dictionary.Add(MicroEventData._sEND_DATE, (DateTime.SpecifyKind(result, DateTimeKind.Utc) - SoaringTime.Epoch).TotalSeconds);
			}
			else
			{
				dictionary.Add(MicroEventData._sEND_DATE, 0);
				TFUtils.ErrorLog("MicroEventManager | cannot parse end date for micro event DID: " + intCell);
			}
			m_pMicroEventDatas.Add(intCell, new MicroEventData(dictionary, null));
		}
	}
}
