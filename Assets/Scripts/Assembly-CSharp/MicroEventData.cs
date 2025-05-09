using System.Collections.Generic;

public class MicroEventData
{
	public static string _sDID = "did";

	public static string _sNAME = "name";

	public static string _sCLOSED_EVENT = "closed_event";

	public static string _sSTART_DATE = "start_date";

	public static string _sEND_DATE = "end_date";

	public static string _sCLOSE_DIALOG_SEQUENCE_DID = "close_dialog_sequence_did";

	private static string[] _sInvariableKeys = new string[1] { _sDID };

	private static string[] _sVariableKeys = new string[5] { _sNAME, _sCLOSED_EVENT, _sSTART_DATE, _sEND_DATE, _sCLOSE_DIALOG_SEQUENCE_DID };

	public int m_nDID
	{
		get
		{
			return (int)m_pData[_sDID];
		}
	}

	public int m_nCloseDialogSequenceDID
	{
		get
		{
			return (int)m_pData[_sCLOSE_DIALOG_SEQUENCE_DID];
		}
	}

	public long m_lStartDate
	{
		get
		{
			return (long)m_pData[_sSTART_DATE];
		}
	}

	public long m_lEndDate
	{
		get
		{
			return (long)m_pData[_sEND_DATE];
		}
	}

	public bool m_bClosedEvent
	{
		get
		{
			return (bool)m_pData[_sCLOSED_EVENT];
		}
	}

	public string m_sName
	{
		get
		{
			return (string)m_pData[_sNAME];
		}
	}

	public ReadOnlyIndexer m_pData { get; private set; }

	public MicroEventData(Dictionary<string, object> pDatabaseData, Dictionary<string, object> pInvariableData)
	{
		m_pData = new ReadOnlyIndexer(new Dictionary<string, object>(_sInvariableKeys.Length + _sVariableKeys.Length)
		{
			{
				_sDID,
				GetDictPriorityInt(_sDID, pInvariableData, pDatabaseData)
			},
			{
				_sCLOSE_DIALOG_SEQUENCE_DID,
				GetDictPriorityInt(_sCLOSE_DIALOG_SEQUENCE_DID, pInvariableData, pDatabaseData)
			},
			{
				_sSTART_DATE,
				GetDictPriorityLong(_sSTART_DATE, pInvariableData, pDatabaseData)
			},
			{
				_sEND_DATE,
				GetDictPriorityLong(_sEND_DATE, pInvariableData, pDatabaseData)
			},
			{
				_sCLOSED_EVENT,
				GetDictPriorityBool(_sCLOSED_EVENT, pInvariableData, pDatabaseData)
			},
			{
				_sNAME,
				GetDictPriorityString(_sNAME, pInvariableData, pDatabaseData)
			}
		});
	}

	public bool IsActive()
	{
		long adjustedServerTime = SoaringTime.AdjustedServerTime;
		if (adjustedServerTime >= m_lStartDate && adjustedServerTime <= m_lEndDate)
		{
			return true;
		}
		return false;
	}

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int num = _sInvariableKeys.Length;
		for (int i = 0; i < num; i++)
		{
			dictionary.Add(_sInvariableKeys[i], m_pData[_sInvariableKeys[i]]);
		}
		num = _sVariableKeys.Length;
		for (int j = 0; j < num; j++)
		{
			dictionary.Add(_sVariableKeys[j], m_pData[_sVariableKeys[j]]);
		}
		return dictionary;
	}

	public Dictionary<string, object> GetInvariableData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int num = _sInvariableKeys.Length;
		for (int i = 0; i < num; i++)
		{
			dictionary.Add(_sInvariableKeys[i], m_pData[_sInvariableKeys[i]]);
		}
		return dictionary;
	}

	private int GetDictPriorityInt(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadInt(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadInt(pDictTwo, sKey);
		}
		return 0;
	}

	private long GetDictPriorityLong(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadLong(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadLong(pDictTwo, sKey);
		}
		return 0L;
	}

	private float GetDictPriorityFloat(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadFloat(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadFloat(pDictTwo, sKey);
		}
		return 0f;
	}

	private bool GetDictPriorityBool(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadBool(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadBool(pDictTwo, sKey);
		}
		return false;
	}

	private string GetDictPriorityString(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadString(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadString(pDictTwo, sKey);
		}
		return string.Empty;
	}

	private List<T> GetDictPriorityList<T>(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadList<T>(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadList<T>(pDictTwo, sKey);
		}
		return new List<T>();
	}

	private Dictionary<string, object> GetDictPriorityDict(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadDict(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadDict(pDictTwo, sKey);
		}
		return new Dictionary<string, object>();
	}
}
