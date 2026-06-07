using System.Collections.Generic;

public class MicroEventData
{
	public static string _sDID;

	public static string _sNAME;

	public static string _sCLOSED_EVENT;

	public static string _sSTART_DATE;

	public static string _sEND_DATE;

	public static string _sCLOSE_DIALOG_SEQUENCE_DID;

	private static string[] _sInvariableKeys;

	private static string[] _sVariableKeys;

	public int m_nDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nCloseDialogSequenceDID
	{
		get
		{
			return 0;
		}
	}

	public long m_lStartDate
	{
		get
		{
			return 0L;
		}
	}

	public long m_lEndDate
	{
		get
		{
			return 0L;
		}
	}

	public bool m_bClosedEvent
	{
		get
		{
			return false;
		}
	}

	public string m_sName
	{
		get
		{
			return null;
		}
	}

	public ReadOnlyIndexer m_pData { get; private set; }

	public MicroEventData(Dictionary<string, object> pDatabaseData, Dictionary<string, object> pInvariableData)
	{
	}

	public bool IsActive()
	{
		return false;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public Dictionary<string, object> GetInvariableData()
	{
		return null;
	}

	private int GetDictPriorityInt(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return 0;
	}

	private long GetDictPriorityLong(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return 0L;
	}

	private float GetDictPriorityFloat(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return 0f;
	}

	private bool GetDictPriorityBool(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return false;
	}

	private string GetDictPriorityString(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return null;
	}

	private List<T> GetDictPriorityList<T>(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return null;
	}

	private Dictionary<string, object> GetDictPriorityDict(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return null;
	}
}
