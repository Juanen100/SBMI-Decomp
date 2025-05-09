using System.Collections.Generic;

public static class SBMIAnalytics
{
	public struct CommonData
	{
		public ulong ulDateTime;

		public ulong ulFirstPlayTime;

		public int nPlayerLevel;

		public int nSoftCurrency;

		public int nHardCurreny;

		public int nCharacters;

		public int nHouses;

		public int nLandExpansions;

		public int nSpongyCurrency;

		public bool bIsEligibleForSpongyGames;

		public string sPlayerID;

		public string sPlatform;

		public string sDeviceName;

		public string sBinaryVersion;

		public string sOSVersion;

		public string sManifest;

		public string sGUID;

		public string sDeviceGUID;

		public ulong ulSequence;

		public SoaringDictionary sCampaignData;
	}

	public abstract class Object
	{
		protected SoaringDictionary m_pData = new SoaringDictionary();

		public string m_sKey { get; protected set; }

		public void AddToDict(SoaringDictionary pDict, string sOverrideKey = null, bool bNested = true)
		{
			if (bNested)
			{
				string text = (string.IsNullOrEmpty(sOverrideKey) ? m_sKey : sOverrideKey);
				if (m_pData != null && pDict != null && !string.IsNullOrEmpty(text))
				{
					pDict.addValue(m_pData, text);
				}
				return;
			}
			string[] array = m_pData.allKeys();
			SoaringObjectBase[] array2 = m_pData.allValues();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				pDict.addValue(array2[i], array[i]);
			}
		}
	}

	public class MetaObject : Object
	{
		public MetaObject(string sObjectKey, string sEventName, string sDeviceName, string sBinaryVersion, string sOSVersion, string sManifest, string sPlatform, string sGUID, string sDeviceGUID, int nTrackingVersion, ulong ulSequence, ulong ulEventTime)
		{
			base.m_sKey = sObjectKey;
			m_pData.addValue(sEventName, "event_name");
			m_pData.addValue(sDeviceName, "device_name");
			m_pData.addValue(sBinaryVersion, "binary_version");
			m_pData.addValue(sOSVersion, "os_version");
			m_pData.addValue(sManifest, "manifest");
			m_pData.addValue(sPlatform, "platform");
			m_pData.addValue(sDeviceGUID, "device_seq_id");
			m_pData.addValue(sGUID, "guid");
			m_pData.addValue(ulSequence, "device_seq_num");
			m_pData.addValue(nTrackingVersion, "tracking_version");
			m_pData.addValue(ulEventTime, "event_time");
		}
	}

	public class PlayerObject : Object
	{
		public PlayerObject(string sObjectKey, string sPlayerID, string sLiveEventName, ulong ulFirstPlayTime, int nLevel, int nNumCharacters, int nNumHouses, int nNumLandExpansions, int nNumSoftCurrency, int nNumHardCurrency, int nSpecialCurrencyDID, int nSpecialCurrencyAmount, SoaringDictionary sABTest)
		{
			base.m_sKey = sObjectKey;
			m_pData.addValue(sPlayerID, "player_id");
			m_pData.addValue(ulFirstPlayTime, "first_play_time");
			m_pData.addValue(nLevel, "player_level");
			m_pData.addValue(nNumSoftCurrency, "gold_balance");
			m_pData.addValue(nNumHardCurrency, "jelly_balance");
			m_pData.addValue(nNumCharacters, "count_of_characters");
			m_pData.addValue(nNumHouses, "count_of_houses");
			m_pData.addValue(nNumLandExpansions, "count_of_land_expansions");
			if (nSpecialCurrencyDID >= 0)
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(nSpecialCurrencyAmount, nSpecialCurrencyDID.ToString());
				m_pData.addValue(soaringDictionary, "special_currency");
			}
			if (!string.IsNullOrEmpty(sLiveEventName))
			{
				m_pData.addValue(sLiveEventName, "live_event_name");
			}
			if (sABTest != null)
			{
				m_pData.addValue(sABTest, "ab_test");
			}
		}
	}

	public class QuestObject : Object
	{
		public QuestObject(string sObjectKey, string sName, string sTag, int nID, string sBranch)
		{
			base.m_sKey = sObjectKey;
			m_pData.addValue(sName, "name");
			m_pData.addValue(sTag, "tag");
			m_pData.addValue(nID, "id");
			m_pData.addValue(sBranch, "branch");
		}
	}

	public class RewardObject : Object
	{
		public RewardObject(string sObjectKey, Dictionary<int, int> pRewards)
		{
			base.m_sKey = sObjectKey;
			foreach (KeyValuePair<int, int> pReward in pRewards)
			{
				if (pReward.Key == ResourceManager.SOFT_CURRENCY)
				{
					m_pData.addValue(pReward.Value, "gold_amount");
					continue;
				}
				if (pReward.Key == ResourceManager.HARD_CURRENCY)
				{
					m_pData.addValue(pReward.Value, "jelly_amount");
					continue;
				}
				if (pReward.Key == ResourceManager.XP)
				{
					m_pData.addValue(pReward.Value, "xp_amount");
					continue;
				}
				if (!m_pData.containsKey("other_rewards"))
				{
					m_pData.addValue(new SoaringDictionary(), "other_rewards");
				}
				((SoaringDictionary)m_pData["other_rewards"]).addValue(pReward.Value, pReward.Key.ToString());
			}
		}
	}

	public class AutoQuestObject : Object
	{
		public AutoQuestObject(string sObjectKey, string sName, int nID, int nCharacterID, SoaringDictionary pFoodIDs)
		{
			base.m_sKey = sObjectKey;
			m_pData.addValue(sName, "name");
			m_pData.addValue(nID, "id");
			m_pData.addValue(nCharacterID, "character_id");
			if (pFoodIDs != null && pFoodIDs.count() > 0)
			{
				m_pData.addValue(pFoodIDs, "food_tasks");
			}
		}
	}

	public class TaskObject : Object
	{
		public TaskObject(string sObjectKey, string sName, int nID, int nCharacterID, int nDuration)
		{
			base.m_sKey = sObjectKey;
			m_pData.addValue(sName, "name");
			m_pData.addValue(nID, "id");
			m_pData.addValue(nCharacterID, "character_id");
			m_pData.addValue(nDuration, "duration_seconds");
		}
	}

	public class CharacterObject : Object
	{
		public CharacterObject(string sObjectKey, string sName, int nID, int? nWishID, int? nFullnessTime)
		{
			base.m_sKey = sObjectKey;
			m_pData.addValue(sName, "name");
			m_pData.addValue(nID, "id");
			if (nWishID.HasValue)
			{
				m_pData.addValue(nWishID.Value, "wish_id");
			}
			if (nFullnessTime.HasValue)
			{
				m_pData.addValue(nFullnessTime.Value, "fullness_timer_seconds");
			}
		}
	}

	public class CostumeObject : Object
	{
		public CostumeObject(string sObjectKey, string sName, int nID, int nCharacterID)
		{
			base.m_sKey = sObjectKey;
			if (!string.IsNullOrEmpty(sName))
			{
				m_pData.addValue(sName, "name");
			}
			if (nID >= 0)
			{
				m_pData.addValue(nID, "id");
			}
			if (nCharacterID >= 0)
			{
				m_pData.addValue(nCharacterID, "character_id");
			}
		}
	}

	public class ItemObject : Object
	{
		public ItemObject(string sObjectKey, string sName, string sCategory, int nID, int nSoftCost, int nHardCost)
		{
			base.m_sKey = sObjectKey;
			if (!string.IsNullOrEmpty(sName))
			{
				m_pData.addValue(sName, "name");
			}
			if (nID > 0)
			{
				m_pData.addValue(nID, "id");
			}
			if (!string.IsNullOrEmpty(sCategory))
			{
				m_pData.addValue(sCategory, "category");
			}
			if (nSoftCost > 0)
			{
				m_pData.addValue(nSoftCost, "gold_cost");
			}
			if (nHardCost > 0)
			{
				m_pData.addValue(nHardCost, "jelly_cost");
			}
		}
	}

	public class ChestObject : Object
	{
		public ChestObject(string sObjectKey, string sLocation, int nID)
		{
			base.m_sKey = sObjectKey;
			m_pData.addValue(sLocation, "location");
			m_pData.addValue(nID, "id");
		}
	}

	public class IAPObject : Object
	{
		public IAPObject(string sObjectKey, string sProductCode, string sProductLinkCode, string sCurrencyType, int nAmount, int nCost)
		{
			base.m_sKey = sObjectKey;
			if (!string.IsNullOrEmpty(sProductCode))
			{
				m_pData.addValue(sProductCode, "product_code");
			}
			if (!string.IsNullOrEmpty(sProductLinkCode))
			{
				m_pData.addValue(sProductLinkCode, "product_link_code");
			}
			if (!string.IsNullOrEmpty(sCurrencyType))
			{
				m_pData.addValue(sCurrencyType, "product_currency_type");
			}
			if (nAmount > 0)
			{
				m_pData.addValue(nAmount, "product_amount");
			}
			if (nCost > 0)
			{
				m_pData.addValue(nCost, "product_usd_cost");
			}
		}
	}

	public const string _sSPEEDUP = "speedup";

	public const string _sUNLOCK = "unlock";

	public const string _sINSTANT_PURCHASE = "instant_purchase";

	public const string _sSTORE_RESTOCKING = "store_restocking";

	public const string _sCONSTRUCTION = "construction";

	public const string _sFULLNESS = "fullness";

	public const string _sCOMMUNITY_EVENT_PURCHASE = "community_event_purchase";

	public const string _sRENT = "rent";

	public const string _sCRAFT = "craft";

	public const string _sOPEN = "open";

	public const string _sCLOSE = "close";

	public const string _sSTORE_BUTTON = "store_open_button";

	public const string _sOPEN_IAP_TAB_SOFT = "store_open_plus_buy_gold";

	public const string _sOPEN_IAP_TAB_HARD = "store_open_plus_buy_jelly";

	public const string _sOPEN_IAP_TAB_REDIRECT = "store_open_need_currency_redirect";

	public const string _sIAP_ERROR_DIALOG_OPEN = "store_open_iap_error_return";

	public const string _sCANCEL_PURCHASE = "store_open_too_poor_return";

	public const string _sNOT_ENOUGH_CURRENCY_OPEN = "store_open_too_poor_return";

	public const string _sBACK_BUTTON = "store_close_back_button";

	public const string _sIAP_ERROR = "store_close_unknown_error_iap";

	public const string _sIAP_ERROR_DIALOG_CLOSE = "store_close_known_error_iap";

	public const string _sNOT_ENOUGH_CURRENCY_CLOSE = "store_close_im_broke";

	public const string _sPURCHASE_IAP_CLOSE = "store_close_purchase_iap";

	public const string _sPAVING = "store_close_road_purchase_start";

	public const string _sITEM_PURCHASE_START = "store_close_item_purchase_start";

	public const string _sEXPANDING = "expanding";

	public const string _sPLACING = "placing";

	public const string _sSHOPS = "shops";

	public const string _sRECIPES = "recipes";

	public const string _sDEBRIS = "debris";

	public const string _sBUILDINGS = "buildings";

	public const string _sCHARACTERS = "characters";

	public const string _sCOSTUMES = "costumes";

	public const string _sCONFIRM = "confirm";

	public const string _sCANCEL = "cancel";

	public const string _sTASK = "task";

	public const string _sDATA = "data";

	public static int _nTRACKING_VERSION = 3;

	public static int _nTRACKING_VERSION_LOG_ITEM_PLACEMENT = 4;

	private static SoaringDictionary GetDictFromCommonData(CommonData pCommonData)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(pCommonData.ulDateTime, "event_time");
		soaringDictionary.addValue(pCommonData.ulFirstPlayTime, "first_play_time");
		soaringDictionary.addValue(pCommonData.nPlayerLevel, "player_level");
		soaringDictionary.addValue(pCommonData.nSoftCurrency, "player_coin_balance");
		soaringDictionary.addValue(pCommonData.nHardCurreny, "player_jelly_balance");
		soaringDictionary.addValue(pCommonData.nCharacters, "count_of_characters");
		soaringDictionary.addValue(pCommonData.nHouses, "count_of_houses");
		soaringDictionary.addValue(pCommonData.nLandExpansions, "count_of_land_expansions");
		soaringDictionary.addValue(pCommonData.bIsEligibleForSpongyGames ? 1 : 0, "is_eligible_for_spongy_games");
		soaringDictionary.addValue(pCommonData.nSpongyCurrency, "doorknob_balance");
		soaringDictionary.addValue(pCommonData.sPlayerID, "player_id");
		soaringDictionary.addValue(pCommonData.sPlatform, "platform");
		soaringDictionary.addValue(pCommonData.sDeviceName, "device_name");
		soaringDictionary.addValue(pCommonData.sBinaryVersion, "binary_version");
		soaringDictionary.addValue(pCommonData.sOSVersion, "os_version");
		soaringDictionary.addValue(pCommonData.sManifest, "manifest");
		soaringDictionary.addValue(pCommonData.sGUID, "guid");
		soaringDictionary.addValue(pCommonData.sDeviceGUID, "device_seq_id");
		soaringDictionary.addValue(pCommonData.ulSequence, "device_seq_num");
		if (pCommonData.sCampaignData != null)
		{
			soaringDictionary.addValue(pCommonData.sCampaignData, "player_ab_test");
		}
		return soaringDictionary;
	}

	public static void LogQuestCompleted(Game pGame, QuestObject pQuest, RewardObject pReward)
	{
		string text = "quest_completed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pQuest.AddToDict(soaringDictionary2, "quest");
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogAutoQuestStarted(Game pGame, AutoQuestObject pAutoQuest)
	{
		string text = "autoquest_started";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pAutoQuest.AddToDict(soaringDictionary2, "autoquest");
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogAutoQuestCompleted(Game pGame, AutoQuestObject pAutoQuest, RewardObject pReward)
	{
		string text = "autoquest_completed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pAutoQuest.AddToDict(soaringDictionary2, "autoquest");
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogTaskStarted(Game pGame, TaskObject pTask, CharacterObject pCharacter, CostumeObject pCostume)
	{
		string text = "character_task_started";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pTask.AddToDict(soaringDictionary2, "task");
		pCharacter.AddToDict(soaringDictionary2, "character");
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogTaskCompleted(Game pGame, TaskObject pTask, CharacterObject pCharacter, CostumeObject pCostume, RewardObject pReward)
	{
		string text = "character_task_completed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pTask.AddToDict(soaringDictionary2, "task");
		pCharacter.AddToDict(soaringDictionary2, "character");
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward");
		}
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogCostumeUnlocked(Game pGame, CostumeObject pCostume)
	{
		string text = "costume_unlock";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogCostumeChanged(Game pGame, CharacterObject pCharacter, CostumeObject pCostumeOld, CostumeObject pCostumeNew)
	{
		string text = "change_costume";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pCharacter.AddToDict(soaringDictionary2, "character");
		if (pCostumeOld != null)
		{
			pCostumeOld.AddToDict(soaringDictionary2, "costume_old");
		}
		if (pCostumeNew != null)
		{
			pCostumeNew.AddToDict(soaringDictionary2, "costume_new");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogDailyReward(Game pGame, int nDay, RewardObject pReward)
	{
		string text = "daily_reward";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		soaringDictionary2.addValue(nDay, "day_number");
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogChestPickup(Game pGame, ChestObject pChest, RewardObject pReward)
	{
		string text = "chest_pickup";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pChest.AddToDict(soaringDictionary2, "chest");
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogCharacterFeed(Game pGame, CharacterObject pCharacter, CostumeObject pCostume, ItemObject pItem, RewardObject pReward)
	{
		string text = "character_feed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		pCharacter.AddToDict(soaringDictionary2, "character");
		pItem.AddToDict(soaringDictionary2, "item");
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume");
		}
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogVisitPark(Game pGame)
	{
		string text = "visit_park";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogSessionBegin(Game pGame)
	{
		string text = "session_begin";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogPurchaseComplete(Game pGame, IAPObject pIAP)
	{
		string text = "purchase_complete";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		if (pIAP != null)
		{
			pIAP.AddToDict(soaringDictionary2, "iap");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogLevelUp(Game pGame)
	{
		string text = "level_up";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogItemPlacement(Game pGame, ItemObject pItem, bool bFromInventory, string sAction)
	{
		string text = "item_placement";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, _nTRACKING_VERSION_LOG_ITEM_PLACEMENT).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		if (pItem != null)
		{
			pItem.AddToDict(soaringDictionary2, "item");
		}
		soaringDictionary2.addValue(bFromInventory ? 1 : 0, "from_inventory");
		soaringDictionary2.addValue((!bFromInventory) ? 1 : 0, "from_marketplace");
		if (!string.IsNullOrEmpty(sAction))
		{
			soaringDictionary2.addValue(sAction, "action");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogJellyConfirmation(Game pGame, ItemObject pItem, string sTriggerEventType, string sSpeedupType, string sAction)
	{
		string text = "jelly_confirmation";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		if (pItem != null)
		{
			pItem.AddToDict(soaringDictionary2, "item");
		}
		if (!string.IsNullOrEmpty(sTriggerEventType))
		{
			soaringDictionary2.addValue(sTriggerEventType, "trigger_event_type");
		}
		if (!string.IsNullOrEmpty(sSpeedupType))
		{
			soaringDictionary2.addValue(sSpeedupType, "speedup_type");
		}
		if (!string.IsNullOrEmpty(sAction))
		{
			soaringDictionary2.addValue(sAction, "action");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogRecievedEventItem(Game pGame, ItemObject pItem)
	{
		string text = "received_event_item";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		if (pItem != null)
		{
			pItem.AddToDict(soaringDictionary2, "item");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogCraftCollected(Game pGame, ItemObject pItemOld, ItemObject pItemNew, int nItemCount)
	{
		string text = "item_craf_collected";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		if (pItemOld != null)
		{
			pItemOld.AddToDict(soaringDictionary2, "item_parent");
		}
		if (pItemNew != null)
		{
			pItemNew.AddToDict(soaringDictionary2, "item_child");
		}
		soaringDictionary2.addValue(nItemCount, "item_count");
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogStoreImpressions(Game pGame, List<SBGUIMarketplaceScreen.StoreImpression> pStoreImpressions)
	{
		int count = pStoreImpressions.Count;
		if (count <= 0)
		{
			return;
		}
		SoaringDictionary dictFromCommonData = GetDictFromCommonData(pGame.GetAnalyticsCommonData());
		SoaringArray soaringArray = new SoaringArray();
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			SBGUIMarketplaceScreen.StoreImpression storeImpression = pStoreImpressions[i];
			int count2 = storeImpression.m_pOffers.Count;
			for (int j = 0; j < count2; j++)
			{
				SBMarketOffer sBMarketOffer = storeImpression.m_pOffers[j];
				int num2 = (sBMarketOffer.cost.ContainsKey(ResourceManager.HARD_CURRENCY) ? sBMarketOffer.cost[ResourceManager.HARD_CURRENCY] : 0);
				int num3 = (sBMarketOffer.cost.ContainsKey(ResourceManager.SOFT_CURRENCY) ? sBMarketOffer.cost[ResourceManager.SOFT_CURRENCY] : 0);
				string text = null;
				if (sBMarketOffer.type == "rmt" || sBMarketOffer.type == "path")
				{
					text = sBMarketOffer.innerOffer;
				}
				else
				{
					Blueprint blueprint = EntityManager.GetBlueprint(sBMarketOffer.type, sBMarketOffer.identity);
					if (blueprint != null)
					{
						text = (string)blueprint.Invariable["name"];
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(sBMarketOffer.identity, "item_did");
				soaringDictionary.addValue(text, "item_name");
				soaringDictionary.addValue(storeImpression.m_fTimeDelta, "time_seconds");
				soaringDictionary.addValue(num, "group_id");
				soaringDictionary.addValue(sBMarketOffer.itemLocked ? 1 : 0, "item_locked");
				if (sBMarketOffer.itemLocked)
				{
					TFUtils.ErrorLog("\nitem is locked: " + sBMarketOffer.identity);
				}
				if (sBMarketOffer.type == "rmt")
				{
					if (pGame.store.soaringProducts.ContainsKey(text))
					{
						soaringDictionary.addValue(pGame.store.soaringProducts[text].USDPrice, "product_usd_cost");
					}
				}
				else
				{
					soaringDictionary.addValue(num2, "jelly_cost");
					soaringDictionary.addValue(num3, "gold_cost");
				}
				soaringArray.addObject(soaringDictionary);
			}
			if (count2 > 0)
			{
				num++;
			}
		}
		if (soaringArray.count() > 0)
		{
			dictFromCommonData.addValue(soaringArray, "impressions");
			Soaring.SaveStat("store_item_impressions", dictFromCommonData);
		}
	}

	public static void LogMarketplaceUI(Game pGame, string sAction, string sOpenType, string sLeaveType)
	{
		string text = "store_event";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2);
		if (!string.IsNullOrEmpty(sAction))
		{
			soaringDictionary.addValue(sAction, "action");
		}
		if (!string.IsNullOrEmpty(sOpenType))
		{
			soaringDictionary.addValue(sOpenType, "open_type");
		}
		if (!string.IsNullOrEmpty(sLeaveType))
		{
			soaringDictionary.addValue(sLeaveType, "leave_type");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	public static void LogEventButtonClick(Game pGame)
	{
		SoaringDictionary dictFromCommonData = GetDictFromCommonData(pGame.GetAnalyticsCommonData());
		Soaring.SaveStat("event_button_click", dictFromCommonData);
	}
}
