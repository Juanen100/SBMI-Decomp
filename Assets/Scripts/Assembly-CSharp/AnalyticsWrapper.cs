using System.Collections.Generic;
using DeltaDNA;
using UnityEngine;

public class AnalyticsWrapper : Object
{
	public static void LogQuestStarted(Game pGame, QuestDefinition pQuestDef)
	{
		Quest quest = pGame.questManager.GetQuest(pQuestDef.Did);
		if (quest == null)
		{
		}
		SBMIDeltaDNA.LogMissionStart(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did));
	}

	public static void LogQuestCompleted(Game pGame, QuestDefinition pQuestDef, Reward pReward)
	{
		SBMIAnalytics.LogQuestCompleted(pGame, new SBMIAnalytics.QuestObject("quest", pQuestDef.Name, pQuestDef.Tag, (int)pQuestDef.Did, pQuestDef.Branch), new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		Quest quest = pGame.questManager.GetQuest(pQuestDef.Did);
		SBMIDeltaDNA.LogMissionComplete(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did, (quest.CompletionTime.Value >= quest.StartTime.Value) ? (quest.CompletionTime.Value - quest.StartTime.Value) : 0), new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame));
	}

	public static void LogAutoQuestStarted(Game pGame, QuestDefinition pQuestDef, SoaringDictionary pFoodDict)
	{
		SBMIAnalytics.LogAutoQuestStarted(pGame, new SBMIAnalytics.AutoQuestObject("autoquest", pQuestDef.Name, pQuestDef.AutoQuestID, pQuestDef.AutoQuestCharacterID, pFoodDict));
		Quest quest = pGame.questManager.GetQuest(pQuestDef.Did);
		if (quest == null)
		{
		}
		SBMIDeltaDNA.LogMissionStart(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did));
	}

	public static void LogAutoQuestCompleted(Game pGame, QuestDefinition pQuestDef, SoaringDictionary pFoodDict, Reward pReward)
	{
		SBMIAnalytics.LogAutoQuestCompleted(pGame, new SBMIAnalytics.AutoQuestObject("autoquest", pQuestDef.Name, pQuestDef.AutoQuestID, pQuestDef.AutoQuestCharacterID, pFoodDict), new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		Quest quest = pGame.questManager.GetQuest(pQuestDef.Did);
		SBMIDeltaDNA.LogMissionComplete(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did, (quest.CompletionTime.Value >= quest.StartTime.Value) ? (quest.CompletionTime.Value - quest.StartTime.Value) : 0), new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame));
	}

	public static void LogTaskStarted(Game pGame, Task pTask)
	{
		Simulated simulated = pGame.simulation.FindSimulated(pTask.m_pTaskData.m_nSourceDID);
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		int? hungerResourceId = entity.HungerResourceId;
		int? nFullnessTime = null;
		if (!hungerResourceId.HasValue)
		{
			nFullnessTime = (int)Mathf.Clamp(entity.HungryAt - TFUtils.EpochTime(), 0f, float.MaxValue);
		}
		CostumeManager.Costume costume = null;
		if (entity.CostumeDID.HasValue || entity.DefaultCostumeDID.HasValue)
		{
			costume = pGame.costumeManager.GetCostume((!entity.CostumeDID.HasValue) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
		}
		SBMIAnalytics.LogTaskStarted(pGame, new SBMIAnalytics.TaskObject("task", pTask.m_pTaskData.m_sName, pTask.m_pTaskData.m_nDID, pTask.m_pTaskData.m_nSourceDID, pTask.m_pTaskData.m_nDuration), new SBMIAnalytics.CharacterObject("character", simulated.entity.Name, entity.DefinitionId, hungerResourceId, nFullnessTime), (costume != null) ? new SBMIAnalytics.CostumeObject("costume", costume.m_sName, costume.m_nDID, entity.DefinitionId) : null);
		SBMIDeltaDNA.LogMissionStart(pGame, new SBMIDeltaDNA.MissionObject("mission", pTask.m_pTaskData.m_sName, "task", pTask.m_pTaskData.m_nDID));
	}

	public static void LogTaskCompleted(Game pGame, Task pTask)
	{
		Simulated simulated = pGame.simulation.FindSimulated(pTask.m_pTaskData.m_nSourceDID);
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		int? hungerResourceId = entity.HungerResourceId;
		int? nFullnessTime = null;
		if (!hungerResourceId.HasValue)
		{
			nFullnessTime = (int)Mathf.Clamp(entity.HungryAt - TFUtils.EpochTime(), 0f, float.MaxValue);
		}
		CostumeManager.Costume costume = null;
		if (entity.CostumeDID.HasValue || entity.DefaultCostumeDID.HasValue)
		{
			costume = pGame.costumeManager.GetCostume((!entity.CostumeDID.HasValue) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
		}
		SBMIAnalytics.LogTaskCompleted(pGame, new SBMIAnalytics.TaskObject("task", pTask.m_pTaskData.m_sName, pTask.m_pTaskData.m_nDID, pTask.m_pTaskData.m_nSourceDID, pTask.m_pTaskData.m_nDuration), new SBMIAnalytics.CharacterObject("character", simulated.entity.Name, entity.DefinitionId, hungerResourceId, nFullnessTime), (costume != null) ? new SBMIAnalytics.CostumeObject("costume", costume.m_sName, costume.m_nDID, entity.DefinitionId) : null, new SBMIAnalytics.RewardObject("reward", pTask.m_pTaskData.m_pReward.ResourceAmounts));
		SBMIDeltaDNA.LogMissionComplete(pGame, new SBMIDeltaDNA.MissionObject("mission", pTask.m_pTaskData.m_sName, "task", pTask.m_pTaskData.m_nDID, (pTask.m_ulCompleteTime >= pTask.m_ulStartTime) ? (pTask.m_ulCompleteTime - pTask.m_ulStartTime) : 0), new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pTask.m_pTaskData.m_pReward, pGame));
	}

	public static void LogCostumeUnlocked(Game pGame, CostumeManager.Costume pCostume)
	{
		SBMIAnalytics.LogCostumeUnlocked(pGame, new SBMIAnalytics.CostumeObject("costume", pCostume.m_sName, pCostume.m_nDID, pCostume.m_nUnitDID));
	}

	public static void LogCostumeChanged(Game pGame, ResidentEntity pResidentEntity, CostumeManager.Costume pOldCostume, CostumeManager.Costume pNewCostume)
	{
		int? hungerResourceId = pResidentEntity.HungerResourceId;
		int? nFullnessTime = null;
		if (!hungerResourceId.HasValue)
		{
			nFullnessTime = (int)Mathf.Clamp(pResidentEntity.HungryAt - TFUtils.EpochTime(), 0f, float.MaxValue);
		}
		SBMIAnalytics.LogCostumeChanged(pGame, new SBMIAnalytics.CharacterObject("character", pResidentEntity.Name, pResidentEntity.DefinitionId, hungerResourceId, nFullnessTime), new SBMIAnalytics.CostumeObject("costume_old", (pOldCostume != null) ? pOldCostume.m_sName : null, (pOldCostume != null) ? pOldCostume.m_nDID : (-1), pResidentEntity.DefinitionId), new SBMIAnalytics.CostumeObject("costume_new", pNewCostume.m_sName, pNewCostume.m_nDID, pResidentEntity.DefinitionId));
	}

	public static void LogDailyReward(Game pGame, int nDay, Reward pReward)
	{
		SBMIAnalytics.LogDailyReward(pGame, nDay, new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "daily_bonus"));
	}

	public static void LogChestPickup(Game pGame, Simulated pChestSimulated, Reward pReward)
	{
		SBMIAnalytics.LogChestPickup(pGame, new SBMIAnalytics.ChestObject("chest", "home", pChestSimulated.entity.DefinitionId), new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "treasure_chest"));
	}

	public static void LogPatchyChestPickup(Game pGame, Simulated pChestSimulated, Reward pReward)
	{
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "patchy_treasure_chest"));
	}

	public static void LogRentCollected(Game pGame, Simulated pSimulated, Reward pReward)
	{
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "rent_collected_" + pSimulated.entity.Name));
	}

	public static void LogCharacterFeed(Game pGame, ResidentEntity pResidentEntity, int nHungerResourceDID, Reward pReward)
	{
		CostumeManager.Costume costume = null;
		if (pResidentEntity.CostumeDID.HasValue || pResidentEntity.DefaultCostumeDID.HasValue)
		{
			costume = pGame.costumeManager.GetCostume((!pResidentEntity.CostumeDID.HasValue) ? pResidentEntity.DefaultCostumeDID.Value : pResidentEntity.CostumeDID.Value);
		}
		SBMIAnalytics.LogCharacterFeed(pGame, new SBMIAnalytics.CharacterObject("character", pResidentEntity.Name, pResidentEntity.DefinitionId, nHungerResourceDID, null), (costume != null) ? new SBMIAnalytics.CostumeObject("costume", costume.m_sName, costume.m_nDID, pResidentEntity.DefinitionId) : null, new SBMIAnalytics.ItemObject("item", pGame.simulation.resourceManager.Resources[nHungerResourceDID].Name, "recipes", nHungerResourceDID, -1, -1), (pReward != null) ? new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts) : null);
		string empty = string.Empty;
		if (pGame.resourceManager.Resources.ContainsKey(nHungerResourceDID))
		{
			empty = pGame.resourceManager.Resources[nHungerResourceDID].Name;
		}
		SBMIDeltaDNA.LogWishGranted(pGame, empty, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame));
	}

	public static void LogBonusChest(Game pGame, Simulated pSimulated, Reward pReward)
	{
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "bonus_chest_" + pSimulated.entity.Name));
	}

	public static void LogVisitPark(Game pGame)
	{
		SBMIAnalytics.LogVisitPark(pGame);
		SBMIDeltaDNA.LogUIInteraction(pGame, pGame.LastAction(), pGame.GetType().ToString(), pGame.LastAction());
	}

	public static void LogSessionBegin(Game pGame, ulong ulPauseTime)
	{
		SBMIAnalytics.LogSessionBegin(pGame);
		if (ulPauseTime >= 120)
		{
			Singleton<SDK>.Instance.NewSession();
		}
	}

	public static void LogPurchaseComplete(Game pGame, SoaringPurchasable pSoaringPurchasable, string sReceipt, string sTransactionID, RmtProduct rmtProduct = null)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		string sCurrencyType = "jelly";
		if (pSoaringPurchasable.ResourceType == 3)
		{
			sCurrencyType = "coin";
			dictionary.Add(ResourceManager.SOFT_CURRENCY, pSoaringPurchasable.Amount);
		}
		else
		{
			dictionary.Add(ResourceManager.HARD_CURRENCY, pSoaringPurchasable.Amount);
		}
		SBMIAnalytics.LogPurchaseComplete(pGame, new SBMIAnalytics.IAPObject("iap", pSoaringPurchasable.ProductID, pSoaringPurchasable.Alias, sCurrencyType, pSoaringPurchasable.Amount, pSoaringPurchasable.USDPrice));
		string sTransactionServer = "APPLE";
		if (SoaringPlatform.Platform == SoaringPlatformType.Amazon)
		{
			sTransactionServer = "AMAZON";
		}
		else if (SoaringPlatform.Platform == SoaringPlatformType.Android)
		{
			sTransactionServer = "GOOGLE";
		}
		string sRealCurrencyType = "USD";
		int nRealCurrencyAmount = pSoaringPurchasable.USDPrice * 100;
		if (rmtProduct != null)
		{
			sRealCurrencyType = rmtProduct.currencyCode;
			nRealCurrencyAmount = Mathf.RoundToInt(rmtProduct.price * 100f);
		}
		SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", null, pGame, nRealCurrencyAmount, sRealCurrencyType), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(dictionary, null, null, null, null, null, null, null, false, null), pGame), new SBMIDeltaDNA.TransactionObject("transaction", "server", sTransactionServer, sReceipt, pSoaringPurchasable.ProductID, sTransactionID, true), "PURCHASE", pSoaringPurchasable.DisplayName);
	}

	public static void LogLevelUp(Game pGame, List<Reward> pRewards, int nLevel)
	{
		SBMIAnalytics.LogLevelUp(pGame);
		int count = pRewards.Count;
		Reward pReward = ((count <= 0) ? null : pRewards[0]);
		for (int i = 1; i < count; i++)
		{
			pReward += pRewards[i];
		}
		SBMIDeltaDNA.LogLevelUp(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame), nLevel);
	}

	public static void LogItemPlacement(Game pGame, Entity pEntity, bool bFromInventory, bool bAccepted)
	{
		int num = -1;
		int num2 = -1;
		if (bAccepted)
		{
			Cost cost = pGame.catalog.GetCost(pEntity.DefinitionId);
			if (cost != null)
			{
				if (cost.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
				{
					num = cost.ResourceAmounts[ResourceManager.SOFT_CURRENCY];
				}
				if (cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
				{
					num2 = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
				}
			}
		}
		SBMIAnalytics.LogItemPlacement(pGame, new SBMIAnalytics.ItemObject("item", pEntity.Name, "buildings", pEntity.DefinitionId, num, num2), bFromInventory, (!bAccepted) ? "cancel" : "confirm");
		if (!bFromInventory)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (num > 0)
			{
				dictionary.Add(ResourceManager.SOFT_CURRENCY, num);
			}
			if (num2 > 0)
			{
				dictionary.Add(ResourceManager.HARD_CURRENCY, num2);
			}
			SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", new Reward(dictionary, null, null, null, null, null, null, null, false, null), pGame), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(null, new Dictionary<int, int> { { pEntity.DefinitionId, 1 } }, null, null, null, null, null, null, false, null), pGame), null, "PURCHASE", pEntity.Name);
		}
	}

	public static void LogExpansion(Game pGame, int nExpansionID, Cost pCost)
	{
		SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", new Reward((pCost != null) ? pCost.ResourceAmounts : null, null, null, null, null, null, null, null, false, null), pGame), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(null, null, null, null, null, null, new List<int> { nExpansionID }, null, false, null), pGame), null, "PURCHASE", "expansion_" + nExpansionID);
	}

	public static void LogCostumePurchased(Game pGame, CostumeManager.Costume pCostume, int nCurrencyDID, int nNumCurrency)
	{
		SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", new Reward(new Dictionary<int, int> { { nCurrencyDID, nNumCurrency } }, null, null, null, null, null, null, null, false, null), pGame), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(null, null, null, null, null, new List<int> { pCostume.m_nDID }, null, null, false, null), pGame), null, "PURCHASE", pCostume.m_sName);
	}

	public static void LogJellyConfirmation(Game pGame, int nItemDID, int nJellyCost, string sItemName, string sItemType, string sTriggerEventType, string sSpeedupType, string sAction)
	{
		SBMIAnalytics.LogJellyConfirmation(pGame, new SBMIAnalytics.ItemObject("item", sItemName, sItemType, nItemDID, -1, nJellyCost), sTriggerEventType, sSpeedupType, sAction);
		Reward reward = null;
		Reward reward2 = null;
		if (sItemType == "building" || sItemType == "buildings")
		{
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, sItemName, "building", "PURCHASE", sItemName);
		}
		if (sItemType == "costumes")
		{
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, sItemName, "costume", "PURCHASE", sItemName);
		}
		if (sItemType == "debris")
		{
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, sItemName, "debris", "PURCHASE", sItemName);
		}
		if (sItemType == "craft")
		{
			int num = 0;
			num = nItemDID;
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, num, sItemName, "resource", "PURCHASE", sItemName);
			num = 0;
		}
		switch (sItemType)
		{
		case "!!BLDG_NAME_TREAT_SHOP":
		case "!!BLDG_NAME_JUICEBAR":
		case "!!BLDG_NAME_KK":
		case "!!BLDG_NAME_BAKERY":
		case "!!BLDG_NAME_CHUMINATOR":
		case "!!BLDG_NAME_CHUMBUCKET":
		case "!!BLDG_NAME_SEEDSTORE":
		case "!!BLDG_NAME_RUSTYS_RIB_EYE":
		case "!!BLDG_NAME_ICECREAMSHOP":
		case "!!BLDG_NAME_FLOWERSHOP":
		case "!!BLDG_NAME_PERFUMESHOP":
		case "!!BLDG_NAME_WHOLE_BROW_FOODS":
		{
			string text = "Production_Slot_" + sItemName;
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, text, "production slot", "PURCHASE", text);
			break;
		}
		}
		if (sItemType == "task")
		{
			string text = "Accelerate_" + sItemName;
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, 5, text, "task", "PURCHASE", text);
		}
	}

	public static void LogRecievedEventItem(Game pGame, int nItemDID, string sItemName)
	{
		SBMIAnalytics.LogRecievedEventItem(pGame, new SBMIAnalytics.ItemObject("item", sItemName, string.Empty, nItemDID, -1, -1));
	}

	public static void LogCraftCollected(Game pGame, Entity pBuildingEntity, int nCraftDID, int nNumCrafted, string sCraftName)
	{
		SBMIAnalytics.LogCraftCollected(pGame, new SBMIAnalytics.ItemObject("item", pBuildingEntity.Name, "buildings", pBuildingEntity.DefinitionId, -1, -1), new SBMIAnalytics.ItemObject("item", sCraftName, "recipes", nCraftDID, -1, -1), nNumCrafted);
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", new Reward(new Dictionary<int, int> { { nCraftDID, nNumCrafted } }, null, null, null, null, null, null, null, false, null), pGame, -1, null, "craft"));
	}

	public static void LogStoreImpressions(Game pGame, List<SBGUIMarketplaceScreen.StoreImpression> pStoreImpressions)
	{
		SBMIAnalytics.LogStoreImpressions(pGame, pStoreImpressions);
	}

	public static void LogMarketplaceUI(Game pGame, string sAction, string sOpenType, string sLeaveType)
	{
		SBMIAnalytics.LogMarketplaceUI(pGame, sAction, sOpenType, sLeaveType);
	}

	public static void LogEventButtonClick(Game pGame)
	{
		SBMIAnalytics.LogEventButtonClick(pGame);
	}

	public static void LogUIInteraction(Game pGame, string sUIName, string sType, string sAction)
	{
		SBMIDeltaDNA.LogUIInteraction(pGame, sUIName, sType, sAction);
	}

	public static void LogFeatureUnlocked(Game pGame, string sFeatureName, string sFeatureType)
	{
		SBMIDeltaDNA.LogFeatureUnlocked(pGame, sFeatureName, sFeatureType);
	}

	public static void LogShopTabOpened(Game pGame, string sTabName)
	{
		SBMIDeltaDNA.LogShopEntered(pGame, sTabName);
	}
}
