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

	public static string _sDID = "did";

	public static string _sSOURCE_DID = "source_did";

	public static string _sTARGET_DID = "target_did";

	public static string _sPARTNER_DID = "partner_did";

	public static string _sMICRO_EVENT_DID = "micro_event_did";

	public static string _sACTIVE_QUEST_DID = "active_quest_did";

	public static string _sQUEST_UNLOCK_DID = "quest_unlock_did";

	public static string _sMIN_LEVEL = "min_level";

	public static string _sTARGET_TYPE = "target_type";

	public static string _sSOURCE_COSTUME_DID = "source_unit_required_costume";

	public static string _sPARTNER_COSTUME_DID = "partner_unit_required_costume";

	public static string _sTASK_TYPE = "task_type";

	public static string _sQUEST_RELOCK_DID = "quest_relock_did";

	public static string _sQUEST_REUNLOCK_DID = "quest_re_unlock_did";

	public static string _sDURATION = "duration";

	public static string _sNAME = "name";

	public static string _sREWARD = "reward";

	public static string _sPOS_OFFSET_TARG_X = "position_offset_from_target_x";

	public static string _sPOS_OFFSET_TARG_Y = "position_offset_from_target_y";

	public static string _sPARTNER_POS_OFFSET_TARG_X = "partner_position_offset_from_target_x";

	public static string _sPARTNER_POS_OFFSET_TARG_Y = "partner_position_offset_from_target_y";

	public static string _sMOVEMENT_SPEED = "movement_speed";

	public static string _sHIDDEN_UNTIL_UNLOCKED = "hidden_until_unlocked";

	public static string _sWANDER_TIME = "wander_time";

	public static string _sIDLE_TIME = "idle_time";

	public static string _sSOURCE_DISPLAY_STATE_WALK = "source_display_state_walk";

	public static string _sPARTNER_DISPLAY_STATE_WALK = "partner_display_state_walk";

	public static string _sSOURCE_DISPLAY_STATE_IDLE = "source_display_state_idle";

	public static string _sPARTNER_DISPLAY_STATE_IDLE = "partner_display_state_idle";

	public static string _sTARGET_DISPLAY_STATE = "target_display_state";

	public static string _sSTART_VO = "start_vo";

	public static string _sFINISH_VO = "finish_vo";

	public static string _sSTART_SOUND = "start_sound";

	public static string _sFINISH_SOUND = "finish_sound";

	public static string _sSOURCE_FLIPPED = "source_flipped";

	public static string _sPARTNER_FLIPPED = "partner_flipped";

	public static string _sEVENT_ONLY = "event_only";

	public static string _sSORT_ORDER = "sort_order";

	public static string _sREPEATABLE = "repeatable";

	public static string _sPAYTABLE_REWARD_ICON = "paytable_reward_icon";

	private static string[] _sInvariableKeys = new string[14]
	{
		_sDID, _sSOURCE_DID, _sTARGET_DID, _sPARTNER_DID, _sMICRO_EVENT_DID, _sACTIVE_QUEST_DID, _sQUEST_UNLOCK_DID, _sMIN_LEVEL, _sTARGET_TYPE, _sSOURCE_COSTUME_DID,
		_sPARTNER_COSTUME_DID, _sTASK_TYPE, _sQUEST_RELOCK_DID, _sQUEST_REUNLOCK_DID
	};

	private static string[] _sVariableKeys = new string[26]
	{
		_sDURATION, _sNAME, _sREWARD, _sPOS_OFFSET_TARG_X, _sPOS_OFFSET_TARG_Y, _sPARTNER_POS_OFFSET_TARG_X, _sPARTNER_POS_OFFSET_TARG_Y, _sMOVEMENT_SPEED, _sHIDDEN_UNTIL_UNLOCKED, _sWANDER_TIME,
		_sIDLE_TIME, _sSOURCE_DISPLAY_STATE_WALK, _sPARTNER_DISPLAY_STATE_WALK, _sSOURCE_DISPLAY_STATE_IDLE, _sPARTNER_DISPLAY_STATE_IDLE, _sTARGET_DISPLAY_STATE, _sSTART_VO, _sFINISH_VO, _sSTART_SOUND, _sFINISH_SOUND,
		_sSOURCE_FLIPPED, _sPARTNER_FLIPPED, _sEVENT_ONLY, _sSORT_ORDER, _sREPEATABLE, _sPAYTABLE_REWARD_ICON
	};

	private Reward m_pRewardData;

	public List<int> tasksHasBonus = new List<int>();

	public int m_nDID
	{
		get
		{
			return (int)m_pData[_sDID];
		}
	}

	public int m_nSourceDID
	{
		get
		{
			return (int)m_pData[_sSOURCE_DID];
		}
	}

	public int m_nPartnerDID
	{
		get
		{
			return (int)m_pData[_sPARTNER_DID];
		}
	}

	public int m_nTargetDID
	{
		get
		{
			return (int)m_pData[_sTARGET_DID];
		}
	}

	public int m_nSourceCostumeDID
	{
		get
		{
			return (int)m_pData[_sSOURCE_COSTUME_DID];
		}
	}

	public int m_nPartnerCostumeDID
	{
		get
		{
			return (int)m_pData[_sPARTNER_COSTUME_DID];
		}
	}

	public int m_nMicroEventDID
	{
		get
		{
			return (int)m_pData[_sMICRO_EVENT_DID];
		}
	}

	public int m_nActiveQuestDID
	{
		get
		{
			return (int)m_pData[_sACTIVE_QUEST_DID];
		}
	}

	public int m_nQuestUnlockDID
	{
		get
		{
			return (int)m_pData[_sQUEST_UNLOCK_DID];
		}
	}

	public int m_nDuration
	{
		get
		{
			return (int)m_pData[_sDURATION];
		}
	}

	public int m_nMinLevel
	{
		get
		{
			return (int)m_pData[_sMIN_LEVEL];
		}
	}

	public int m_nSortOrder
	{
		get
		{
			return (int)m_pData[_sSORT_ORDER];
		}
	}

	public int m_nQuestRelockDid
	{
		get
		{
			return (int)m_pData[_sQUEST_RELOCK_DID];
		}
	}

	public int m_nQuestReunlockDid
	{
		get
		{
			return (int)m_pData[_sQUEST_REUNLOCK_DID];
		}
	}

	public float m_fMovementSpeed
	{
		get
		{
			return (float)m_pData[_sMOVEMENT_SPEED];
		}
	}

	public float m_fWanderTime
	{
		get
		{
			return (float)m_pData[_sWANDER_TIME];
		}
	}

	public float m_fIdleTime
	{
		get
		{
			return (float)m_pData[_sIDLE_TIME];
		}
	}

	public bool m_bHiddenUntilUnlocked
	{
		get
		{
			return (bool)m_pData[_sHIDDEN_UNTIL_UNLOCKED];
		}
	}

	public bool m_bSourceFlipped
	{
		get
		{
			return (bool)m_pData[_sSOURCE_FLIPPED];
		}
	}

	public bool m_bPartnerFlipped
	{
		get
		{
			return (bool)m_pData[_sPARTNER_FLIPPED];
		}
	}

	public bool m_bEventOnly
	{
		get
		{
			return (bool)m_pData[_sEVENT_ONLY];
		}
	}

	public bool m_bRepeatable
	{
		get
		{
			return (bool)m_pData[_sREPEATABLE];
		}
	}

	public string m_sName
	{
		get
		{
			return (string)m_pData[_sNAME];
		}
	}

	public string m_sTargetType
	{
		get
		{
			return (string)m_pData[_sTARGET_TYPE];
		}
	}

	public string m_sSourceDisplayStateWalk
	{
		get
		{
			return (string)m_pData[_sSOURCE_DISPLAY_STATE_WALK];
		}
	}

	public string m_sPartnerDisplayStateWalk
	{
		get
		{
			return (string)m_pData[_sPARTNER_DISPLAY_STATE_WALK];
		}
	}

	public string m_sSourceDisplayStateIdle
	{
		get
		{
			return (string)m_pData[_sSOURCE_DISPLAY_STATE_IDLE];
		}
	}

	public string m_sPartnerDisplayStateIdle
	{
		get
		{
			return (string)m_pData[_sPARTNER_DISPLAY_STATE_IDLE];
		}
	}

	public string m_sTargetDisplayState
	{
		get
		{
			return (string)m_pData[_sTARGET_DISPLAY_STATE];
		}
	}

	public string m_sStartVO
	{
		get
		{
			return (string)m_pData[_sSTART_VO];
		}
	}

	public string m_sFinishVO
	{
		get
		{
			return (string)m_pData[_sFINISH_VO];
		}
	}

	public string m_sStartSound
	{
		get
		{
			return (string)m_pData[_sSTART_SOUND];
		}
	}

	public string m_sFinishSound
	{
		get
		{
			return (string)m_pData[_sFINISH_SOUND];
		}
	}

	public string m_sPaytableRewardIcon
	{
		get
		{
			return (string)m_pData[_sPAYTABLE_REWARD_ICON];
		}
	}

	public _eTaskType m_eTaskType
	{
		get
		{
			return (_eTaskType)(int)m_pData[_sTASK_TYPE];
		}
	}

	public Vector2 m_pPosOffsetFromTarget
	{
		get
		{
			return new Vector2((int)m_pData[_sPOS_OFFSET_TARG_X], (int)m_pData[_sPOS_OFFSET_TARG_Y]);
		}
	}

	public Vector2 m_pPartnerPosOffsetFromTarget
	{
		get
		{
			return new Vector2((int)m_pData[_sPARTNER_POS_OFFSET_TARG_X], (int)m_pData[_sPARTNER_POS_OFFSET_TARG_Y]);
		}
	}

	public Reward m_pReward
	{
		get
		{
			return m_pRewardData;
		}
	}

	public ReadOnlyIndexer m_pData { get; private set; }

	public TaskData(Dictionary<string, object> pDatabaseData, Dictionary<string, object> pInvariableData)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(_sInvariableKeys.Length + _sVariableKeys.Length)
		{
			{
				_sDID,
				GetDictPriorityInt(_sDID, pInvariableData, pDatabaseData)
			},
			{
				_sSOURCE_DID,
				GetDictPriorityInt(_sSOURCE_DID, pInvariableData, pDatabaseData)
			},
			{
				_sPARTNER_DID,
				GetDictPriorityInt(_sPARTNER_DID, pInvariableData, pDatabaseData)
			},
			{
				_sMICRO_EVENT_DID,
				GetDictPriorityInt(_sMICRO_EVENT_DID, pInvariableData, pDatabaseData)
			},
			{
				_sACTIVE_QUEST_DID,
				GetDictPriorityInt(_sACTIVE_QUEST_DID, pInvariableData, pDatabaseData)
			},
			{
				_sQUEST_UNLOCK_DID,
				GetDictPriorityInt(_sQUEST_UNLOCK_DID, pInvariableData, pDatabaseData)
			},
			{
				_sTARGET_DID,
				GetDictPriorityInt(_sTARGET_DID, pInvariableData, pDatabaseData)
			},
			{
				_sSOURCE_COSTUME_DID,
				GetDictPriorityInt(_sSOURCE_COSTUME_DID, pInvariableData, pDatabaseData)
			},
			{
				_sPARTNER_COSTUME_DID,
				GetDictPriorityInt(_sPARTNER_COSTUME_DID, pInvariableData, pDatabaseData)
			},
			{
				_sDURATION,
				GetDictPriorityInt(_sDURATION, pInvariableData, pDatabaseData)
			},
			{
				_sMIN_LEVEL,
				GetDictPriorityInt(_sMIN_LEVEL, pInvariableData, pDatabaseData)
			},
			{
				_sPOS_OFFSET_TARG_X,
				GetDictPriorityInt(_sPOS_OFFSET_TARG_X, pInvariableData, pDatabaseData)
			},
			{
				_sPOS_OFFSET_TARG_Y,
				GetDictPriorityInt(_sPOS_OFFSET_TARG_Y, pInvariableData, pDatabaseData)
			},
			{
				_sPARTNER_POS_OFFSET_TARG_X,
				GetDictPriorityInt(_sPARTNER_POS_OFFSET_TARG_X, pInvariableData, pDatabaseData)
			},
			{
				_sPARTNER_POS_OFFSET_TARG_Y,
				GetDictPriorityInt(_sPARTNER_POS_OFFSET_TARG_Y, pInvariableData, pDatabaseData)
			},
			{
				_sTASK_TYPE,
				GetDictPriorityInt(_sTASK_TYPE, pInvariableData, pDatabaseData)
			},
			{
				_sSORT_ORDER,
				GetDictPriorityInt(_sSORT_ORDER, pInvariableData, pDatabaseData)
			},
			{
				_sQUEST_RELOCK_DID,
				GetDictPriorityInt(_sQUEST_RELOCK_DID, pInvariableData, pDatabaseData)
			},
			{
				_sQUEST_REUNLOCK_DID,
				GetDictPriorityInt(_sQUEST_REUNLOCK_DID, pInvariableData, pDatabaseData)
			},
			{
				_sMOVEMENT_SPEED,
				GetDictPriorityFloat(_sMOVEMENT_SPEED, pInvariableData, pDatabaseData)
			},
			{
				_sWANDER_TIME,
				GetDictPriorityFloat(_sWANDER_TIME, pInvariableData, pDatabaseData)
			},
			{
				_sIDLE_TIME,
				GetDictPriorityFloat(_sIDLE_TIME, pInvariableData, pDatabaseData)
			},
			{
				_sHIDDEN_UNTIL_UNLOCKED,
				GetDictPriorityBool(_sHIDDEN_UNTIL_UNLOCKED, pInvariableData, pDatabaseData)
			},
			{
				_sSOURCE_FLIPPED,
				GetDictPriorityBool(_sSOURCE_FLIPPED, pInvariableData, pDatabaseData)
			},
			{
				_sPARTNER_FLIPPED,
				GetDictPriorityBool(_sPARTNER_FLIPPED, pInvariableData, pDatabaseData)
			},
			{
				_sEVENT_ONLY,
				GetDictPriorityBool(_sEVENT_ONLY, pInvariableData, pDatabaseData)
			},
			{
				_sREPEATABLE,
				GetDictPriorityBool(_sREPEATABLE, pInvariableData, pDatabaseData)
			},
			{
				_sNAME,
				GetDictPriorityString(_sNAME, pInvariableData, pDatabaseData)
			},
			{
				_sTARGET_TYPE,
				GetDictPriorityString(_sTARGET_TYPE, pInvariableData, pDatabaseData)
			},
			{
				_sSOURCE_DISPLAY_STATE_WALK,
				GetDictPriorityString(_sSOURCE_DISPLAY_STATE_WALK, pInvariableData, pDatabaseData)
			},
			{
				_sPARTNER_DISPLAY_STATE_WALK,
				GetDictPriorityString(_sPARTNER_DISPLAY_STATE_WALK, pInvariableData, pDatabaseData)
			},
			{
				_sSOURCE_DISPLAY_STATE_IDLE,
				GetDictPriorityString(_sSOURCE_DISPLAY_STATE_IDLE, pInvariableData, pDatabaseData)
			},
			{
				_sPARTNER_DISPLAY_STATE_IDLE,
				GetDictPriorityString(_sPARTNER_DISPLAY_STATE_IDLE, pInvariableData, pDatabaseData)
			},
			{
				_sTARGET_DISPLAY_STATE,
				GetDictPriorityString(_sTARGET_DISPLAY_STATE, pInvariableData, pDatabaseData)
			},
			{
				_sSTART_VO,
				GetDictPriorityString(_sSTART_VO, pInvariableData, pDatabaseData)
			},
			{
				_sFINISH_VO,
				GetDictPriorityString(_sFINISH_VO, pInvariableData, pDatabaseData)
			},
			{
				_sSTART_SOUND,
				GetDictPriorityString(_sSTART_SOUND, pInvariableData, pDatabaseData)
			},
			{
				_sFINISH_SOUND,
				GetDictPriorityString(_sFINISH_SOUND, pInvariableData, pDatabaseData)
			},
			{
				_sREWARD,
				GetDictPriorityDict(_sREWARD, pInvariableData, pDatabaseData)
			},
			{
				_sPAYTABLE_REWARD_ICON,
				GetDictPriorityString(_sPAYTABLE_REWARD_ICON, pInvariableData, pDatabaseData)
			}
		};
		m_pRewardData = Reward.FromDict((Dictionary<string, object>)dictionary[_sREWARD]);
		m_pData = new ReadOnlyIndexer(dictionary);
		if (m_sPaytableRewardIcon != "n/a")
		{
			tasksHasBonus.Add(m_nDID);
		}
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

	public int CompareTo(TaskData pTaskData)
	{
		if (pTaskData == null)
		{
			return 1;
		}
		return m_nSortOrder.CompareTo(pTaskData.m_nSortOrder);
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
