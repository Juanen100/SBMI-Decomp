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
		protected SoaringDictionary m_pData;

		public string m_sKey { get; protected set; }

		public void AddToDict(SoaringDictionary pDict, string sOverrideKey = null, bool bNested = true)
		{
		}
	}

	public class MetaObject : Object
	{
		public MetaObject(string sObjectKey, string sEventName, string sDeviceName, string sBinaryVersion, string sOSVersion, string sManifest, string sPlatform, string sGUID, string sDeviceGUID, int nTrackingVersion, ulong ulSequence, ulong ulEventTime)
		{
		}
	}

	public class PlayerObject : Object
	{
		public PlayerObject(string sObjectKey, string sPlayerID, string sLiveEventName, ulong ulFirstPlayTime, int nLevel, int nNumCharacters, int nNumHouses, int nNumLandExpansions, int nNumSoftCurrency, int nNumHardCurrency, int nSpecialCurrencyDID, int nSpecialCurrencyAmount, SoaringDictionary sABTest)
		{
		}
	}

	public class QuestObject : Object
	{
		public QuestObject(string sObjectKey, string sName, string sTag, int nID, string sBranch)
		{
		}
	}

	public class RewardObject : Object
	{
		public RewardObject(string sObjectKey, Dictionary<int, int> pRewards)
		{
		}
	}

	public class AutoQuestObject : Object
	{
		public AutoQuestObject(string sObjectKey, string sName, int nID, int nCharacterID, SoaringDictionary pFoodIDs)
		{
		}
	}

	public class TaskObject : Object
	{
		public TaskObject(string sObjectKey, string sName, int nID, int nCharacterID, int nDuration)
		{
		}
	}

	public class CharacterObject : Object
	{
		public CharacterObject(string sObjectKey, string sName, int nID, int? nWishID, int? nFullnessTime)
		{
		}
	}

	public class CostumeObject : Object
	{
		public CostumeObject(string sObjectKey, string sName, int nID, int nCharacterID)
		{
		}
	}

	public class ItemObject : Object
	{
		public ItemObject(string sObjectKey, string sName, string sCategory, int nID, int nSoftCost, int nHardCost)
		{
		}
	}

	public class ChestObject : Object
	{
		public ChestObject(string sObjectKey, string sLocation, int nID)
		{
		}
	}

	public class IAPObject : Object
	{
		public IAPObject(string sObjectKey, string sProductCode, string sProductLinkCode, string sCurrencyType, int nAmount, int nCost)
		{
		}
	}

	public static int _nTRACKING_VERSION;

	public static int _nTRACKING_VERSION_LOG_ITEM_PLACEMENT;

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

	private static SoaringDictionary GetDictFromCommonData(CommonData pCommonData)
	{
		return null;
	}

	public static void LogQuestCompleted(Game pGame, QuestObject pQuest, RewardObject pReward)
	{
	}

	public static void LogAutoQuestStarted(Game pGame, AutoQuestObject pAutoQuest)
	{
	}

	public static void LogAutoQuestCompleted(Game pGame, AutoQuestObject pAutoQuest, RewardObject pReward)
	{
	}

	public static void LogTaskStarted(Game pGame, TaskObject pTask, CharacterObject pCharacter, CostumeObject pCostume)
	{
	}

	public static void LogTaskCompleted(Game pGame, TaskObject pTask, CharacterObject pCharacter, CostumeObject pCostume, RewardObject pReward)
	{
	}

	public static void LogCostumeUnlocked(Game pGame, CostumeObject pCostume)
	{
	}

	public static void LogCostumeChanged(Game pGame, CharacterObject pCharacter, CostumeObject pCostumeOld, CostumeObject pCostumeNew)
	{
	}

	public static void LogDailyReward(Game pGame, int nDay, RewardObject pReward)
	{
	}

	public static void LogChestPickup(Game pGame, ChestObject pChest, RewardObject pReward)
	{
	}

	public static void LogCharacterFeed(Game pGame, CharacterObject pCharacter, CostumeObject pCostume, ItemObject pItem, RewardObject pReward)
	{
	}

	public static void LogVisitPark(Game pGame)
	{
	}

	public static void LogSessionBegin(Game pGame)
	{
	}

	public static void LogPurchaseComplete(Game pGame, IAPObject pIAP)
	{
	}

	public static void LogLevelUp(Game pGame)
	{
	}

	public static void LogItemPlacement(Game pGame, ItemObject pItem, bool bFromInventory, string sAction)
	{
	}

	public static void LogJellyConfirmation(Game pGame, ItemObject pItem, string sTriggerEventType, string sSpeedupType, string sAction)
	{
	}

	public static void LogRecievedEventItem(Game pGame, ItemObject pItem)
	{
	}

	public static void LogCraftCollected(Game pGame, ItemObject pItemOld, ItemObject pItemNew, int nItemCount)
	{
	}

	public static void LogStoreImpressions(Game pGame, List<SBGUIMarketplaceScreen.StoreImpression> pStoreImpressions)
	{
	}

	public static void LogMarketplaceUI(Game pGame, string sAction, string sOpenType, string sLeaveType)
	{
	}

	public static void LogEventButtonClick(Game pGame)
	{
	}
}
