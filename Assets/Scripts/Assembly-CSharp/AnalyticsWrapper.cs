using System.Collections.Generic;
using UnityEngine;

public class AnalyticsWrapper : Object
{
	public static void LogQuestStarted(Game pGame, QuestDefinition pQuestDef)
	{
	}

	public static void LogQuestCompleted(Game pGame, QuestDefinition pQuestDef, Reward pReward)
	{
	}

	public static void LogAutoQuestStarted(Game pGame, QuestDefinition pQuestDef, SoaringDictionary pFoodDict)
	{
	}

	public static void LogAutoQuestCompleted(Game pGame, QuestDefinition pQuestDef, SoaringDictionary pFoodDict, Reward pReward)
	{
	}

	public static void LogTaskStarted(Game pGame, Task pTask)
	{
	}

	public static void LogTaskCompleted(Game pGame, Task pTask)
	{
	}

	public static void LogCostumeUnlocked(Game pGame, CostumeManager.Costume pCostume)
	{
	}

	public static void LogCostumeChanged(Game pGame, ResidentEntity pResidentEntity, CostumeManager.Costume pOldCostume, CostumeManager.Costume pNewCostume)
	{
	}

	public static void LogDailyReward(Game pGame, int nDay, Reward pReward)
	{
	}

	public static void LogChestPickup(Game pGame, Simulated pChestSimulated, Reward pReward)
	{
	}

	public static void LogPatchyChestPickup(Game pGame, Simulated pChestSimulated, Reward pReward)
	{
	}

	public static void LogRentCollected(Game pGame, Simulated pSimulated, Reward pReward)
	{
	}

	public static void LogCharacterFeed(Game pGame, ResidentEntity pResidentEntity, int nHungerResourceDID, Reward pReward)
	{
	}

	public static void LogBonusChest(Game pGame, Simulated pSimulated, Reward pReward)
	{
	}

	public static void LogVisitPark(Game pGame)
	{
	}

	public static void LogSessionBegin(Game pGame, ulong ulPauseTime)
	{
	}

	public static void LogPurchaseComplete(Game pGame, SoaringPurchasable pSoaringPurchasable, string sReceipt, string sTransactionID, RmtProduct rmtProduct = null)
	{
	}

	public static void LogLevelUp(Game pGame, List<Reward> pRewards, int nLevel)
	{
	}

	public static void LogItemPlacement(Game pGame, Entity pEntity, bool bFromInventory, bool bAccepted)
	{
	}

	public static void LogExpansion(Game pGame, int nExpansionID, Cost pCost)
	{
	}

	public static void LogCostumePurchased(Game pGame, CostumeManager.Costume pCostume, int nCurrencyDID, int nNumCurrency)
	{
	}

	public static void LogJellyConfirmation(Game pGame, int nItemDID, int nJellyCost, string sItemName, string sItemType, string sTriggerEventType, string sSpeedupType, string sAction)
	{
	}

	public static void LogRecievedEventItem(Game pGame, int nItemDID, string sItemName)
	{
	}

	public static void LogCraftCollected(Game pGame, Entity pBuildingEntity, int nCraftDID, int nNumCrafted, string sCraftName)
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

	public static void LogUIInteraction(Game pGame, string sUIName, string sType, string sAction)
	{
	}

	public static void LogFeatureUnlocked(Game pGame, string sFeatureName, string sFeatureType)
	{
	}

	public static void LogShopTabOpened(Game pGame, string sTabName)
	{
	}
}
