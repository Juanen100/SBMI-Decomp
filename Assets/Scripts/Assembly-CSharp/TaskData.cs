using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskData : IComparable<TaskData>
{
	public enum _eTaskType
	{
		eWander = 0,
		eEnter = 1,
		eStand = 2,
		eActivate = 3,
		eNumTypes = 4
	}

	public static string _sDID;

	public static string _sSOURCE_DID;

	public static string _sTARGET_DID;

	public static string _sPARTNER_DID;

	public static string _sMICRO_EVENT_DID;

	public static string _sACTIVE_QUEST_DID;

	public static string _sQUEST_UNLOCK_DID;

	public static string _sMIN_LEVEL;

	public static string _sTARGET_TYPE;

	public static string _sSOURCE_COSTUME_DID;

	public static string _sPARTNER_COSTUME_DID;

	public static string _sTASK_TYPE;

	public static string _sQUEST_RELOCK_DID;

	public static string _sQUEST_REUNLOCK_DID;

	public static string _sUPGRADE_BUILDING_DID;

	public static string _sDURATION;

	public static string _sNAME;

	public static string _sREWARD;

	public static string _sPOS_OFFSET_TARG_X;

	public static string _sPOS_OFFSET_TARG_Y;

	public static string _sPARTNER_POS_OFFSET_TARG_X;

	public static string _sPARTNER_POS_OFFSET_TARG_Y;

	public static string _sMOVEMENT_SPEED;

	public static string _sHIDDEN_UNTIL_UNLOCKED;

	public static string _sWANDER_TIME;

	public static string _sIDLE_TIME;

	public static string _sSOURCE_DISPLAY_STATE_WALK;

	public static string _sPARTNER_DISPLAY_STATE_WALK;

	public static string _sSOURCE_DISPLAY_STATE_IDLE;

	public static string _sPARTNER_DISPLAY_STATE_IDLE;

	public static string _sTARGET_DISPLAY_STATE;

	public static string _sSTART_VO;

	public static string _sFINISH_VO;

	public static string _sSTART_SOUND;

	public static string _sFINISH_SOUND;

	public static string _sSOURCE_FLIPPED;

	public static string _sPARTNER_FLIPPED;

	public static string _sEVENT_ONLY;

	public static string _sSORT_ORDER;

	public static string _sREPEATABLE;

	public static string _sPAYTABLE_REWARD_ICON;

	private static string[] _sInvariableKeys;

	private static string[] _sVariableKeys;

	private Reward m_pRewardData;

	public List<int> tasksHasBonus;

	public int m_nDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nSourceDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nPartnerDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nTargetDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nSourceCostumeDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nPartnerCostumeDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nMicroEventDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nActiveQuestDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nQuestUnlockDID
	{
		get
		{
			return 0;
		}
	}

	public int m_nDuration
	{
		get
		{
			return 0;
		}
	}

	public int m_nMinLevel
	{
		get
		{
			return 0;
		}
	}

	public int m_nSortOrder
	{
		get
		{
			return 0;
		}
	}

	public int m_nQuestRelockDid
	{
		get
		{
			return 0;
		}
	}

	public int m_nQuestReunlockDid
	{
		get
		{
			return 0;
		}
	}

	public float m_fMovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	public float m_fWanderTime
	{
		get
		{
			return 0f;
		}
	}

	public float m_fIdleTime
	{
		get
		{
			return 0f;
		}
	}

	public bool m_bHiddenUntilUnlocked
	{
		get
		{
			return false;
		}
	}

	public bool m_bSourceFlipped
	{
		get
		{
			return false;
		}
	}

	public bool m_bPartnerFlipped
	{
		get
		{
			return false;
		}
	}

	public bool m_bEventOnly
	{
		get
		{
			return false;
		}
	}

	public bool m_bRepeatable
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

	public string m_sTargetType
	{
		get
		{
			return null;
		}
	}

	public string m_sSourceDisplayStateWalk
	{
		get
		{
			return null;
		}
	}

	public string m_sPartnerDisplayStateWalk
	{
		get
		{
			return null;
		}
	}

	public string m_sSourceDisplayStateIdle
	{
		get
		{
			return null;
		}
	}

	public string m_sPartnerDisplayStateIdle
	{
		get
		{
			return null;
		}
	}

	public string m_sTargetDisplayState
	{
		get
		{
			return null;
		}
	}

	public string m_sStartVO
	{
		get
		{
			return null;
		}
	}

	public string m_sFinishVO
	{
		get
		{
			return null;
		}
	}

	public string m_sStartSound
	{
		get
		{
			return null;
		}
	}

	public string m_sFinishSound
	{
		get
		{
			return null;
		}
	}

	public int m_nBuildingUpgradeDID
	{
		get
		{
			return 0;
		}
	}

	public string m_sPaytableRewardIcon
	{
		get
		{
			return null;
		}
	}

	public _eTaskType m_eTaskType
	{
		get
		{
			return default(_eTaskType);
		}
	}

	public Vector2 m_pPosOffsetFromTarget
	{
		get
		{
			return default(Vector2);
		}
	}

	public Vector2 m_pPartnerPosOffsetFromTarget
	{
		get
		{
			return default(Vector2);
		}
	}

	public Reward m_pReward
	{
		get
		{
			return null;
		}
	}

	public ReadOnlyIndexer m_pData { get; private set; }

	public TaskData(Dictionary<string, object> pDatabaseData, Dictionary<string, object> pInvariableData)
	{
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public Dictionary<string, object> GetInvariableData()
	{
		return null;
	}

	public int CompareTo(TaskData pTaskData)
	{
		return 0;
	}

	private int GetDictPriorityInt(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		return 0;
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
