#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SBAnalytics
{
	private const string PLAYER_ID = "PlayerId";

	private const string DEVICE_ID = "DeviceId";

	private const string DEVICE_INFO = "DeviceInfo";

	private const string OFFLINE = "Offline";

	private const string JJAMT = "JJAmount";

	private const string SUBTYPE_1 = "subtype1";

	private const string SUBTYPE_2 = "subtype2";

	private const string SUBTYPE_3 = "subtype3";

	private const string LEVEL = "level";

	private const string VALUE = "value";

	private const string COST_TYPE = "CostType";

	private const string CATEGORY_MONETIZATION = "MonetizationByLevel";

	private const string CATEGORY_ACQUISITION = "Acquisition";

	private const string CATEGORY_PROGRESSION = "Progression";

	private const string CATEGORY_RETENTION = "Retention";

	private const string CATEGORY_PLAYER = "PlayerInfo";

	private const string CATEGORY_JJ_ECONOMY = "JJEconomy";

	private const string CATEGORY_COIN_ECONOMY = "CoinEconomy";

	private const string CATEGORY_REZ_ECONOMY = "ResourceEconomy";

	private const string CATEGORY_PERFORMANCE = "Performance";

	private const string CATEGORY_DROPS = "Drops";

	private const string CATEGORY_INTERACTIONS = "Interactions";

	private const string EVENT_SOARING_IN_APP_PURCHASE = "Soaring_IAP";

	private const string EVENT_IN_APP_PURCHASE = "IAP";

	private const string EVENT_HARDSPEND_CONFIRMATION = "JJMicroConfirm";

	private const string EVENT_SPEND_JELLY = "SpendJelly";

	private const string EVENT_NOT_ENOUGH_JELLY = "NotEnoughJelly";

	private const string EVENT_TUTORIAL = "Tutorial";

	private const string EVENT_QUEST_START = "QuestStart";

	private const string EVENT_QUEST_COMPLETE = "QuestComplete";

	private const string EVENT_QUEST_COMPLETE_JJAMT = "QuestCompleteJJAmt";

	private const string EVENT_QUEST_COMPLETE_GOLDAMT = "QuestCompleteGoldAmt";

	private const string EVENT_QUEST_COMPLETE_SOARING = "quest_completed";

	private const string EVENT_AUTO_QUEST_START = "autoquest_started";

	private const string EVENT_AUTO_QUEST_COMPLETE = "autoquest_completed";

	private const string EVENT_TASK_START = "character_task_started";

	private const string EVENT_TASK_COMPLETE = "character_task_completed";

	private const string EVENT_COSTUME_UNLOCK = "change_costume";

	private const string EVENT_COSTUME_CHANGED = "costume_unlock";

	private const string EVENT_DAILY_REWARD = "daily_reward";

	private const string EVENT_CHEST_PICKUP = "chest_pickup";

	private const string EVENT_CHARACTER_FEED = "character_feed";

	private const string EVENT_VISIT_PARK = "visit_park";

	private const string EVENT_PROMOTION_EVENT = "PromoEvent";

	private const string EVENT_BUILD = "Build";

	private const string EVENT_DIALOG = "Dialog";

	private const string EVENT_DECORATION = "Decoration";

	private const string EVENT_PREMIUM_BUILD = "BuyPremBuild";

	private const string EVENT_PREMIUM_DECORATION = "BuyPremDeco";

	private const string EVENT_ACHIEVEMENT = "Achievement";

	private const string EVENT_LEVEL = "Level";

	private const string EVENT_LEVEL_GOLD = "LevelGold";

	private const string EVENT_LEVEL_JJ = "LevelJJ";

	private const string EVENT_CHARACTER_TASK = "CharacterTask";

	private const string EVENT_PLAY_TASK = "PlayTask";

	private const string EVENT_BUILDING_TASK = "BuildingTask";

	private const string EVENT_ANY_TASK = "AnyTask";

	private const string EVENT_CLEAR_DEBRIS = "ClearDebris";

	private const string EVENT_OPEN_SETTINGS = "OpenSettings";

	private const string EVENT_SELL_BUILDING = "SellBuilding";

	private const string EVENT_SELL_DECO = "SellDeco";

	private const string EVENT_CRAFTING = "Crafting";

	private const string EVENT_CRAFTING_PREMIUM = "PremiumCrafting";

	private const string EVENT_GET_RECIPE = "GetRecipe";

	private const string EVENT_GET_MOVIE = "GetMovie";

	private const string EVENT_EXPANSION = "BuyExpansion";

	private const string EVENT_PRODUCTION_SLOT = "BuyProductionSlot";

	private const string EVENT_LOGIN = "Login";

	private const string EVENT_SESSION_BEGIN = "kontagent_session_begin";

	private const string EVENT_SOURCE_GOLD = "SourceGold";

	private const string EVENT_SOURCE_JJ = "SourceJJ";

	private const string EVENT_SOURCE_REZ = "SourceRez";

	private const string EVENT_SINK_GOLD = "SinkGold";

	private const string EVENT_SINK_JJ = "SinkJJ";

	private const string EVENT_SINK_REZ = "SinkRez";

	private const string EVENT_SINK = "Sink";

	private const string EVENT_DEVICE_TYPE = "iOSDeviceType";

	private const string EVENT_INTERACTION = "Interaction";

	private const string EVENT_FRAMERATE = "FramePerf";

	private const string EVENT_RESOURCE_DROP = "ResourceDrop";

	private const string EVENT_BUILDING_DROP = "BuildingDrop";

	private const string EVENT_COLLECT_RENT = "CollectRent";

	private const string EVENT_COLLECT_CRAFT = "CollectCraft";

	private const string EVENT_COLLECT_VEND = "CollectVend";

	private const string EVENT_VENDING_PREMIUM = "PremiumVending";

	private const string EVENT_BUY_EVENT_REWARD = "BuyEventReward";

	private const string EVENT_ADD_BUILDING_TO_INVENTORY = "BuildingAddedToInventory";

	private const string EVENT_PLACE_FROM_INVENTORY = "PlaceFromInventory";

	private const string SUBEVENT_REQUEST_IAP = "RequestIap";

	private const string SUBEVENT_SUCCEED_IAP = "SucceedIap";

	private const string SUBEVENT_FAIL_IAP = "FailIap";

	private const string SUBEVENT_CANCEL_IAP = "CancelIap";

	private const string SUBEVENT_SPEED_BUILD = "SpeedBuild";

	private const string SUBEVENT_SPEED_PAY = "SpeedPay";

	private const string SUBEVENT_SPEED_TASK = "SpeedTask";

	private const string SUBEVENT_SPEED_FULLNESS = "SpeedFullness";

	private const string SUBEVENT_SPEED_CRAFT = "SpeedCraft";

	private const string SUBEVENT_SPEED_CLEAR = "SpeedClear";

	private const string SUBEVENT_SPEED_RESTOCK = "SpeedRestock";

	private const string SUBEVENT_BUY_INGREDIENTS = "BuyIngredients";

	private const string SUBEVENT_BUY_PRODUCTION_SLOT = "BuyProductionSlot";

	private const string SUBEVENT_CANT_AFFORD_SPEED_BUILD = "CantAffordSpeedBuild";

	private const string SUBEVENT_CANT_AFFORD_SPEED_PAY = "CantAffordSpeedPay";

	private const string SUBEVENT_CANT_AFFORD_SPEED_TASK = "CantAffordSpeedTask";

	private const string SUBEVENT_CANT_AFFORD_INGREDIENTS = "CantAffordIngredients";

	private const string SUBEVENT_FIRST_LOGIN_OF_DAY = "FirstLoginOfDay";

	private const string SUBEVENT_SECOND_LOGIN_OF_DAY = "SecondLoginOfDay";

	private const string SUBEVENT_MORE_LOGIN_OF_DAY = "MoreLoginOfDay";

	private const string SUBEVENT_SOURCE_GOLD = "SourceGold";

	private const string SUBEVENT_SOURCE_JJ = "SourceJJ";

	private const string SUBEVENT_SINK_GOLD = "SinkGold";

	private const string SUBEVENT_SINK_JJ = "SinkJJ";

	private const string SUBEVENT_SINK_REZ = "Sink";

	private const string SUBEVENT_SOURCE_REZ = "Source";

	private const string SUBEVENT_MOVE_OBJECT = "MoveObject";

	private const string SUBEVENT_PLAY_MOVIE = "SkipMovie";

	private const string QUEST_UID = "QuestUID";

	private const string QUEST_NAME = "QuestName";

	private const string TASK_SRC_UNIT = "SrcUnit";

	private const string TASK_NAME = "TaskName";

	private const string DIALOG_DURATION = "Duration";

	private LoadingFunnel loadingFunnel;

	private string deviceId;

	private string deviceInfo;

	private string playerId;

	private bool isOffline;

	private int jjamount;

	public string PlayerId
	{
		get
		{
			return playerId;
		}
		set
		{
			playerId = value;
		}
	}

	public bool IsOffline
	{
		get
		{
			return isOffline;
		}
		set
		{
			isOffline = value;
		}
	}

	public int StartingJJAmount
	{
		get
		{
			return jjamount;
		}
		set
		{
			jjamount = value;
		}
	}

	public string androidDeviceType
	{
		get
		{
			return TFUtils.GetAndroidDeviceTypeString();
		}
	}

	public SBAnalytics()
	{
		deviceId = TFUtils.DeviceId;
		deviceInfo = string.Format("{0} {1} {2}", SystemInfo.deviceModel, SystemInfo.processorType, SystemInfo.operatingSystem);
		Dictionary<string, object> commonData = new Dictionary<string, object>();
		AddCommon(commonData);
		GameObject gameObject = new GameObject("LoadingFunnel");
		loadingFunnel = gameObject.AddComponent<LoadingFunnel>();
		loadingFunnel.Initialize(ref commonData);
	}

	public void AddCommon(Dictionary<string, object> eventData)
	{
		eventData["DeviceId"] = deviceId;
		eventData["DeviceInfo"] = deviceInfo;
		eventData["PlayerId"] = playerId;
		eventData["Offline"] = isOffline;
		eventData["JJAmount"] = jjamount;
	}

	public void AddSubtypes(Dictionary<string, object> eventData, string subtype1, string subtype2 = null, string subtype3 = null)
	{
		if (subtype1 != null)
		{
			eventData["subtype1"] = subtype1;
		}
		if (subtype2 != null)
		{
			eventData["subtype2"] = subtype2;
		}
		if (subtype3 != null)
		{
			eventData["subtype3"] = subtype3;
		}
	}

	public static void AddCost(Dictionary<string, object> eventData, Cost cost)
	{
		int count = cost.ResourceAmounts.Count;
		if (count == 1)
		{
			int onlyCostKey = cost.GetOnlyCostKey();
			eventData["CostType"] = ResourceManager.TypeDescription(onlyCostKey);
			eventData["value"] = cost.ResourceAmounts[onlyCostKey];
		}
		foreach (int key in cost.ResourceAmounts.Keys)
		{
			eventData[ResourceManager.TypeDescription(key)] = cost.ResourceAmounts[key];
		}
	}

	public void LogLoadingFunnelStep(string stepName)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		loadingFunnel.LogStep(stepName, ref eventData);
	}

	public void LogStartedPlaying(int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent("Login", dictionary);
	}

	public void LogSessionBegin()
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression");
		TFAnalytics.LogEvent("kontagent_session_begin", eventData);
	}

	public void LogRequestInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "RequestIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
	}

	public void LogCompleteInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "SucceedIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
		TFAnalytics.LogRevenueTracking(1);
	}

	public void LogCancelInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "CancelIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
	}

	public void LogFailInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "FailIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
	}

	public void LogSoaringIAPPurchaseComplete(string iapBundleName)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "MonetizationByLevel", "Soaring_IAP", "FailIap");
		TFAnalytics.LogEvent(iapBundleName, eventData);
	}

	public void LogPlayerConfirmHardSpend(int amountOfJelly, bool canAfford, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", "JJMicroConfirm");
		dictionary["value"] = amountOfJelly;
		dictionary["level"] = playerLevel;
		string eventName = ((!canAfford) ? "InsufficientJJ" : "Success");
		TFAnalytics.LogEvent(eventName, dictionary);
	}

	public void LogPlayerRejectHardSpend(int amountOfJelly, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", "JJMicroConfirm");
		dictionary["value"] = amountOfJelly;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent("Reject", dictionary);
	}

	private void LogRush(string logName, string eventID, string subeventID, int rushCost)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", eventID, subeventID);
		dictionary["value"] = rushCost;
		TFAnalytics.LogEvent(logName, dictionary);
	}

	public void LogRushBuild(string buildingName, int rushCost, bool able)
	{
		LogRush(buildingName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedBuild", rushCost);
	}

	public void LogRushRent(string generatorName, int rushCost, bool able)
	{
		LogRush(generatorName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedPay", rushCost);
	}

	public void LogRushTask(string taskName, int rushCost, bool able)
	{
		LogRush(taskName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedTask", rushCost);
	}

	public void LogRushFullness(string characterName, int rushCost, bool able)
	{
		LogRush(characterName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedFullness", rushCost);
	}

	public void LogRushCraft(string recipeName, int rushCost, bool able)
	{
		LogRush(recipeName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedCraft", rushCost);
	}

	public void LogRushClear(string debrisName, int rushCost, bool able)
	{
		LogRush(debrisName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedClear", rushCost);
	}

	public void LogRushRestock(string buildingName, int rushCost, bool able)
	{
		LogRush(buildingName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedRestock", rushCost);
	}

	public void LogResourceEconomySource(string nameOfResource, int amountOfResource, int playerLevelBeforeEvent, int playerLevelPostEvent, ResourceManager resourceMgr)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "ResourceEconomy", nameOfResource, "Source");
		dictionary["value"] = amountOfResource;
		dictionary["level"] = playerLevelBeforeEvent;
		TFAnalytics.LogEvent(nameOfResource, dictionary);
		for (int i = 1 + playerLevelBeforeEvent; i <= playerLevelPostEvent; i++)
		{
			LogLevelGold(resourceMgr.Resources[ResourceManager.SOFT_CURRENCY].Amount, i);
			LogLevelJJ(resourceMgr.Resources[ResourceManager.HARD_CURRENCY].Amount, i);
			LogLevelPositions(i);
		}
	}

	public void LogSinkResources(string nameOfResource, int amountOfResource, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", "SinkRez");
		dictionary["value"] = amountOfResource * -1;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(nameOfResource, dictionary);
	}

	public void LogResourceEconomySink(string nameOfResource, int amountOfResource, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "ResourceEconomy", nameOfResource, "Sink");
		dictionary["value"] = amountOfResource * -1;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(nameOfResource, dictionary);
	}

	public void LogQuestStart(string questTag, string questName, uint questUID, int playerLevel)
	{
		TFUtils.Assert(questTag != null, "All quest logging must have a valid questTag!");
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "QuestStart");
		dictionary["level"] = playerLevel;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	public void LogQuestCompleteSoaring(string questTag)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "quest_completed");
		TFAnalytics.LogEvent(questTag, eventData);
	}

	public void LogQuestComplete(string questTag, string questName, uint questUID, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "QuestComplete");
		dictionary["level"] = playerLevel;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	public void LogQuestCompleteJJAMT(string questTag, string questName, uint questUID, int playerLevel, int amtjj)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "QuestCompleteJJAmt");
		dictionary["level"] = playerLevel;
		dictionary["value"] = amtjj;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	public void LogQuestCompleteGoldAMT(string questTag, string questName, uint questUID, int playerLevel, int amtgold)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "QuestCompleteGoldAmt");
		dictionary["level"] = playerLevel;
		dictionary["value"] = amtgold;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	public void LogAutoQuestStarted(string questTag)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "autoquest_started");
		TFAnalytics.LogEvent(questTag, eventData);
	}

	public void LogAutoQuestCompleted(string questTag)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "autoquest_completed");
		TFAnalytics.LogEvent(questTag, eventData);
	}

	public void LogTaskStarted(int taskDID)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "character_task_started");
		TFAnalytics.LogEvent(taskDID.ToString(), eventData);
	}

	public void LogTaskCompleted(int taskDID, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "character_task_completed");
		dictionary["value"] = playerLevel;
		TFAnalytics.LogEvent(taskDID.ToString(), dictionary);
	}

	public void LogCostumeUnlocked(int costumeDID)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "change_costume");
		TFAnalytics.LogEvent(costumeDID.ToString(), eventData);
	}

	public void LogCostumeChanged(int costumeDID)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "costume_unlock");
		TFAnalytics.LogEvent(costumeDID.ToString(), eventData);
	}

	public void LogDailyReward(int day)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "daily_reward");
		TFAnalytics.LogEvent("day_" + day, eventData);
	}

	public void LogChestPickup(int did)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "chest_pickup");
		TFAnalytics.LogEvent(did.ToString(), eventData);
	}

	public void LogCharacterFeed(int characterDID, int foodDID)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "character_feed");
		dictionary["value"] = foodDID;
		TFAnalytics.LogEvent(characterDID.ToString(), dictionary);
	}

	public void LogVisitPark()
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		AddCommon(eventData);
		AddSubtypes(eventData, "Progression", "visit_park");
		TFAnalytics.LogEvent("visit_park", eventData);
	}

	public void LogEligiblePromoEvent(int playerLevel, string promoEventName)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Acquisition", "PromoEvent");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(promoEventName, dictionary);
	}

	public void LogPlacement(string itemName, bool decoration, bool premium, Cost cost, int playerLevel, float fps)
	{
		string[] array = new string[4] { "Build", "BuyPremBuild", "Decoration", "BuyPremDeco" };
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", array[2 * (decoration ? 1 : 0) + (premium ? 1 : 0)]);
		dictionary["level"] = playerLevel;
		dictionary["value"] = (int)fps;
		AddCost(dictionary, cost);
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	public void LogPlacementFromInventory(string itemName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "PlaceFromInventory");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	public void LogPurchaseEventReward(string itemName, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "BuyEventReward");
		dictionary["level"] = playerLevel;
		AddCost(dictionary, cost);
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	public void LogBuildingAddToInventory(string itemName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "BuildingAddedToInventory");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	public void LogAchievement(string achievementName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "Achievement");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(achievementName, dictionary);
	}

	public void LogLevelGold(int amountOfSoftCurrency, int newLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "LevelGold");
		dictionary["level"] = TFUtils.KontagentCurrencyLevelIndex(newLevel);
		dictionary["value"] = amountOfSoftCurrency;
		TFAnalytics.LogEvent(string.Format("Level{0:00}", newLevel), dictionary);
	}

	public void LogLevelJJ(int amountOfHardCurrency, int newLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "LevelJJ");
		dictionary["level"] = TFUtils.KontagentCurrencyLevelIndex(newLevel);
		dictionary["value"] = amountOfHardCurrency;
		TFAnalytics.LogEvent(string.Format("Level{0:00}", newLevel), dictionary);
	}

	public void LogLevelPositions(int newLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "Level");
		dictionary["level"] = newLevel;
		dictionary["value"] = 1;
		TFAnalytics.LogEvent("user_level_positions", dictionary);
		dictionary["level"] = newLevel - 1;
		dictionary["value"] = -1;
		TFAnalytics.LogEvent("user_level_positions", dictionary);
	}

	public void LogLevelPlaytime(int levelJustFinished, ulong walltimeMinutes, ulong playtimeMinutes)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "LevelWalltime");
		dictionary["level"] = 0;
		dictionary["value"] = walltimeMinutes;
		if (walltimeMinutes != 0)
		{
			TFAnalytics.LogEvent(string.Format("Level{0:00}", levelJustFinished), dictionary);
		}
		AddSubtypes(dictionary, "Progression", "LevelPlaytime");
		dictionary["value"] = playtimeMinutes;
		if (playtimeMinutes != 0)
		{
			TFAnalytics.LogEvent(string.Format("Level{0:00}", levelJustFinished), dictionary);
		}
	}

	public void LogTask(string taskName, string srcUnit, string targetName, Type targetType, int coinsEarned, int playerLevel)
	{
		string eventName = targetName;
		string text = "AnyTask";
		if (targetType != null && targetType == typeof(ResidentEntity))
		{
			text = "CharacterTask";
		}
		else if (targetType != null && targetType == typeof(BuildingEntity))
		{
			text = "BuildingTask";
		}
		else if (targetType != null && targetType == typeof(DebrisEntity))
		{
			text = "PlayTask";
		}
		else if (targetType != null && targetType == typeof(LandmarkEntity))
		{
			text = "PlayTask";
		}
		else
		{
			text = "AnyTask";
			eventName = taskName;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", text);
		dictionary["value"] = coinsEarned;
		dictionary["level"] = playerLevel;
		dictionary["SrcUnit"] = srcUnit;
		dictionary["TaskName"] = taskName;
		TFAnalytics.LogEvent(eventName, dictionary);
	}

	public void LogClearDebris(string debrisName, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "ClearDebris");
		dictionary["level"] = playerLevel;
		AddCost(dictionary, cost);
		TFAnalytics.LogEvent(debrisName, dictionary);
	}

	public void LogMoveObject(string objectName, int playerLevel, float distance, float fps)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Acquisition", "Interaction", "MoveObject");
		dictionary["value"] = (int)fps;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("Move_FPS_{0}", objectName), dictionary);
		dictionary["value"] = (int)distance;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("Move_Dist_{0}", objectName), dictionary);
	}

	public void LogOpenSettings(int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "OpenSettings");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent("OpenSettings", dictionary);
	}

	public void LogSell(string objectName, bool decoration, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", (!decoration) ? "SellBuilding" : "SellDeco");
		dictionary["level"] = playerLevel;
		AddCost(dictionary, cost);
		TFAnalytics.LogEvent(string.Format("Sell{0}", objectName), dictionary);
	}

	public void LogCrafting(string productName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "Crafting");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(productName, dictionary);
	}

	public void LogPremiumCrafting(string productName, int playerLevel, Cost cost, bool canAfford)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", (!canAfford) ? "NotEnoughJelly" : "SpendJelly", "PremiumCrafting");
		dictionary["level"] = playerLevel;
		AddCost(dictionary, cost);
		TFAnalytics.LogEvent(productName, dictionary);
	}

	public void LogCollectCraftedGood(int buildingDid, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Interactions", "CollectCraft");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("CollectCraft_{0}", buildingDid), dictionary);
	}

	public void LogCollectRentReward(int buildingDid, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Interactions", "CollectRent");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("CollectRent_{0}", buildingDid), dictionary);
	}

	public void LogCollectVendedReward(int buildingDid, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Interactions", "CollectVend");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("CollectVend_{0}", buildingDid), dictionary);
	}

	public void LogPremiumVending(string productName, int playerLevel, Cost cost, bool canAfford)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", (!canAfford) ? "NotEnoughJelly" : "SpendJelly", "PremiumVending");
		dictionary["level"] = playerLevel;
		AddCost(dictionary, cost);
		TFAnalytics.LogEvent(productName, dictionary);
	}

	public void LogResourceDrop(string ResourceName, int amount, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Drops", "ResourceDrop");
		dictionary["level"] = playerLevel;
		dictionary["value"] = amount;
		TFAnalytics.LogEvent(string.Format("ResourceDrop_{0}", ResourceName), dictionary);
	}

	public void LogBuildingDrop(string BuildingName, int amount, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Drops", "BuildingDrop");
		dictionary["level"] = playerLevel;
		dictionary["value"] = amount;
		TFAnalytics.LogEvent(string.Format("BuildingDrop_{0}", BuildingName), dictionary);
	}

	public void LogRecipeDrop(string recipeName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "GetRecipe");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(recipeName, dictionary);
	}

	public void LogMovieDrop(string movieName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "GetMovie");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(movieName, dictionary);
	}

	public void LogPlayMovie(string movieName, ulong timePlayed, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		dictionary["level"] = playerLevel;
		dictionary["value"] = timePlayed;
		AddSubtypes(dictionary, "Acquisition", "Interaction", "SkipMovie");
		TFAnalytics.LogEvent(Path.GetFileName(movieName), dictionary);
	}

	public void LogExpansion(int expansionId, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Progression", "BuyExpansion");
		dictionary["level"] = playerLevel;
		AddCost(dictionary, cost);
		TFAnalytics.LogEvent(string.Format("Expansion{0:0000}", expansionId), dictionary);
	}

	public void LogPurchaseProductionSlot(string buildingName, int slotId, Cost cost, bool able, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "MonetizationByLevel", (!able) ? "NotEnoughJelly" : "SpendJelly", "BuyProductionSlot");
		AddCost(dictionary, cost);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("{0}_Slot{1}", buildingName, slotId), dictionary);
	}

	public void LogInsufficientDialog(string purchaseName, int cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Acquisition", "Dialog");
		dictionary["level"] = playerLevel;
		dictionary["value"] = cost;
		TFAnalytics.LogEvent(string.Format("NotEnough_{0}", purchaseName), dictionary);
	}

	public void LogDialog(string dialogName, string buttonName, double elapsedTimeInMilliseconds, int playerLevel)
	{
		int num = (int)(elapsedTimeInMilliseconds / 100.0 + 0.5);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Acquisition", "Dialog");
		dictionary["level"] = playerLevel;
		dictionary["value"] = num;
		string text = string.Format("Dialog_{0}", dialogName);
		if (text.Length > 32)
		{
			text = text.Substring(0, 32);
		}
		TFAnalytics.LogEvent(text, dictionary);
	}

	public void LogPlayerInfo(int startingJJ, bool IsOffline, bool firstSession, int level)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "PlayerInfo");
		dictionary["value"] = startingJJ;
		dictionary["level"] = level;
		TFAnalytics.LogEvent("startingJJ", dictionary);
		dictionary["value"] = TFUtils.BoolToInt(IsOffline);
		if (firstSession)
		{
			TFAnalytics.LogEvent("offline_game_start", dictionary);
		}
		else
		{
			AddSubtypes(dictionary, "Acquisition");
			TFAnalytics.LogEvent("return_game_start", dictionary);
		}
		AddSubtypes(dictionary, "PlayerInfo", androidDeviceType);
		TFAnalytics.LogEvent("iOSDeviceType", dictionary);
	}

	public void InitGameValues(Game game)
	{
		bool firstSession = true;
		LogLevelGold(game.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount, 1);
		LogLevelJJ(game.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount, 1);
		LogPlayerInfo(game.resourceManager.Resources[ResourceManager.DEFAULT_JJ].Amount, isOffline, firstSession, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
	}

	public void UpdateGameValues(Game game)
	{
		bool firstSession = false;
		LogPlayerInfo(game.resourceManager.Resources[ResourceManager.DEFAULT_JJ].Amount, isOffline, firstSession, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
	}

	public void LogFrameRenderRates(string bucketType, int frameRenderTime)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		AddCommon(dictionary);
		AddSubtypes(dictionary, "Performance", "FramePerf");
		dictionary["value"] = frameRenderTime;
		TFAnalytics.LogEvent(bucketType, dictionary);
	}
}
