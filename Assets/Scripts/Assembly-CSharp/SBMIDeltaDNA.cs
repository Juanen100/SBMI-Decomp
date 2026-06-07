using System.Collections.Generic;
using DeltaDNA;

public class SBMIDeltaDNA
{
	public abstract class Object
	{
		protected GameEvent gameEvent;

		public string m_sKey { get; protected set; }

		public void AddToDict(GameEvent pEventBuilder, string sOverrideKey = null, bool bNested = true)
		{
		}
	}

	public class DeviceObject : Object
	{
		public DeviceObject(string sObjectKey, string sDeviceName, string sDeviceType, string sHardwareVersion, string sOS, string sOSVersion, string sManufacturer, string sTimezoneOffset, string sUserLanguage)
		{
		}
	}

	public class PlayerObject : Object
	{
		public PlayerObject(string sObjectKey, int nLevel, int nXP, int nHardCurrency, int nSoftCurrency)
		{
		}
	}

	public class MissionObject : Object
	{
		public MissionObject(string sObjectKey, string sMissionName, string sMissionType, int nMissionID)
		{
		}

		public MissionObject(string sObjectKey, string sMissionName, string sMissionType, int nMissionID, ulong ulMissionDuration)
		{
		}

		private void SetBaseData(string sObjectKey, string sMissionName, string sMissionType, int nMissionID)
		{
		}
	}

	public class RewardObject : Object
	{
		public RewardObject(string sObjectKey, string sRewardName, Reward pReward, Game pGame, int nRealCurrencyAmount = -1, string sRealCurrencyType = null, string sTypeOverride = null)
		{
		}
	}

	public class TransactionObject : Object
	{
		public TransactionObject(string sObjectKey, string sTransactorID, string sTransactionServer, string sTransactionReceipt, string sProductID, string sTransactionID, bool bIsInitiator)
		{
		}
	}

	private static bool _bDEBUG_LOG;

	public static void LogMissionStart(Game pGame, MissionObject pMission)
	{
	}

	public static void LogMissionComplete(Game pGame, MissionObject pMission, RewardObject pReward)
	{
	}

	public static void LogLevelUp(Game pGame, RewardObject pReward, int nLevel)
	{
	}

	public static void LogWishGranted(Game pGame, string sWishName, RewardObject pReward)
	{
	}

	public static void LogUIInteraction(Game pGame, string sUIName, string sType, string sAction)
	{
	}

	public static void LogFeatureUnlocked(Game pGame, string sFeatureName, string sFeatureType)
	{
	}

	public static void LogShopEntered(Game pGame, string sShopName)
	{
	}

	public static void LogTransaction(Game pGame, RewardObject pSpent, RewardObject pRecieved, TransactionObject pTransaction, string sTransactionType, string sTransactionName)
	{
	}

	public static void LogTransaction(Game pGame, int jellyfishJellyCost, int itemDID, string itemName, string itemType, string sTransactionType, string sTransactionName)
	{
	}

	public static void LogItemCollected(Game pGame, RewardObject pReward)
	{
	}

	public static void LogAdAvailable(Game pGame, string scope, string calledFrom)
	{
	}

	public static void LogAdStarted(Game pGame, string scope, Dictionary<string, object> callData)
	{
	}

	public static void LogAdCompleted(Game pGame, string scope, Dictionary<string, object> callData)
	{
	}

	public static void LogChangeCostume(Game pGame, string characterName, string costumeName)
	{
	}

	public static void LogBuildingUpgrade(Game pGame, string buildingName, string initiator, string upgradeName)
	{
	}

	public static void LogAdjustAttributionUpgrade(Game pGame, string channel, string activity, string adgroup, string campaign, string creative, string network, string tracker, string token)
	{
	}
}
