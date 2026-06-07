using System;
using System.Collections.Generic;

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

	private LoadingFunnel loadingFunnel;

	private string deviceId;

	private string deviceInfo;

	private string playerId;

	private bool isOffline;

	private int jjamount;

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

	public string PlayerId
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool IsOffline
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public int StartingJJAmount
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public string androidDeviceType
	{
		get
		{
			return null;
		}
	}

	public void AddCommon(Dictionary<string, object> eventData)
	{
	}

	public void AddSubtypes(Dictionary<string, object> eventData, string subtype1, string subtype2 = null, string subtype3 = null)
	{
	}

	public static void AddCost(Dictionary<string, object> eventData, Cost cost)
	{
	}

	public void LogLoadingFunnelStep(string stepName)
	{
	}

	public void LogStartedPlaying(int playerLevel)
	{
	}

	public void LogSessionBegin()
	{
	}

	public void LogRequestInAppPurchase(string iapBundleName, int playerLevel)
	{
	}

	public void LogCompleteInAppPurchase(string iapBundleName, int playerLevel)
	{
	}

	public void LogCancelInAppPurchase(string iapBundleName, int playerLevel)
	{
	}

	public void LogFailInAppPurchase(string iapBundleName, int playerLevel)
	{
	}

	public void LogSoaringIAPPurchaseComplete(string iapBundleName)
	{
	}

	public void LogPlayerConfirmHardSpend(int amountOfJelly, bool canAfford, int playerLevel)
	{
	}

	public void LogPlayerRejectHardSpend(int amountOfJelly, int playerLevel)
	{
	}

	private void LogRush(string logName, string eventID, string subeventID, int rushCost)
	{
	}

	public void LogRushBuild(string buildingName, int rushCost, bool able)
	{
	}

	public void LogRushRent(string generatorName, int rushCost, bool able)
	{
	}

	public void LogRushTask(string taskName, int rushCost, bool able)
	{
	}

	public void LogRushFullness(string characterName, int rushCost, bool able)
	{
	}

	public void LogRushCraft(string recipeName, int rushCost, bool able)
	{
	}

	public void LogRushClear(string debrisName, int rushCost, bool able)
	{
	}

	public void LogRushRestock(string buildingName, int rushCost, bool able)
	{
	}

	public void LogResourceEconomySource(string nameOfResource, int amountOfResource, int playerLevelBeforeEvent, int playerLevelPostEvent, ResourceManager resourceMgr)
	{
	}

	public void LogSinkResources(string nameOfResource, int amountOfResource, int playerLevel)
	{
	}

	public void LogResourceEconomySink(string nameOfResource, int amountOfResource, int playerLevel)
	{
	}

	public void LogQuestStart(string questTag, string questName, uint questUID, int playerLevel)
	{
	}

	public void LogQuestCompleteSoaring(string questTag)
	{
	}

	public void LogQuestComplete(string questTag, string questName, uint questUID, int playerLevel)
	{
	}

	public void LogQuestCompleteJJAMT(string questTag, string questName, uint questUID, int playerLevel, int amtjj)
	{
	}

	public void LogQuestCompleteGoldAMT(string questTag, string questName, uint questUID, int playerLevel, int amtgold)
	{
	}

	public void LogAutoQuestStarted(string questTag)
	{
	}

	public void LogAutoQuestCompleted(string questTag)
	{
	}

	public void LogTaskStarted(int taskDID)
	{
	}

	public void LogTaskCompleted(int taskDID, int playerLevel)
	{
	}

	public void LogCostumeUnlocked(int costumeDID)
	{
	}

	public void LogCostumeChanged(int costumeDID)
	{
	}

	public void LogDailyReward(int day)
	{
	}

	public void LogChestPickup(int did)
	{
	}

	public void LogCharacterFeed(int characterDID, int foodDID)
	{
	}

	public void LogVisitPark()
	{
	}

	public void LogEligiblePromoEvent(int playerLevel, string promoEventName)
	{
	}

	public void LogPlacement(string itemName, bool decoration, bool premium, Cost cost, int playerLevel, float fps)
	{
	}

	public void LogPlacementFromInventory(string itemName, int playerLevel)
	{
	}

	public void LogPurchaseEventReward(string itemName, Cost cost, int playerLevel)
	{
	}

	public void LogBuildingAddToInventory(string itemName, int playerLevel)
	{
	}

	public void LogAchievement(string achievementName, int playerLevel)
	{
	}

	public void LogLevelGold(int amountOfSoftCurrency, int newLevel)
	{
	}

	public void LogLevelJJ(int amountOfHardCurrency, int newLevel)
	{
	}

	public void LogLevelPositions(int newLevel)
	{
	}

	public void LogLevelPlaytime(int levelJustFinished, ulong walltimeMinutes, ulong playtimeMinutes)
	{
	}

	public void LogTask(string taskName, string srcUnit, string targetName, Type targetType, int coinsEarned, int playerLevel)
	{
	}

	public void LogClearDebris(string debrisName, Cost cost, int playerLevel)
	{
	}

	public void LogMoveObject(string objectName, int playerLevel, float distance, float fps)
	{
	}

	public void LogOpenSettings(int playerLevel)
	{
	}

	public void LogSell(string objectName, bool decoration, Cost cost, int playerLevel)
	{
	}

	public void LogCrafting(string productName, int playerLevel)
	{
	}

	public void LogPremiumCrafting(string productName, int playerLevel, Cost cost, bool canAfford)
	{
	}

	public void LogCollectCraftedGood(int buildingDid, int playerLevel)
	{
	}

	public void LogCollectRentReward(int buildingDid, int playerLevel)
	{
	}

	public void LogCollectVendedReward(int buildingDid, int playerLevel)
	{
	}

	public void LogPremiumVending(string productName, int playerLevel, Cost cost, bool canAfford)
	{
	}

	public void LogResourceDrop(string ResourceName, int amount, int playerLevel)
	{
	}

	public void LogBuildingDrop(string BuildingName, int amount, int playerLevel)
	{
	}

	public void LogRecipeDrop(string recipeName, int playerLevel)
	{
	}

	public void LogMovieDrop(string movieName, int playerLevel)
	{
	}

	public void LogPlayMovie(string movieName, ulong timePlayed, int playerLevel)
	{
	}

	public void LogExpansion(int expansionId, Cost cost, int playerLevel)
	{
	}

	public void LogPurchaseProductionSlot(string buildingName, int slotId, Cost cost, bool able, int playerLevel)
	{
	}

	public void LogInsufficientDialog(string purchaseName, int cost, int playerLevel)
	{
	}

	public void LogDialog(string dialogName, string buttonName, double elapsedTimeInMilliseconds, int playerLevel)
	{
	}

	public void LogPlayerInfo(int startingJJ, bool IsOffline, bool firstSession, int level)
	{
	}

	public void InitGameValues(Game game)
	{
	}

	public void UpdateGameValues(Game game)
	{
	}

	public void LogFrameRenderRates(string bucketType, int frameRenderTime)
	{
	}
}
