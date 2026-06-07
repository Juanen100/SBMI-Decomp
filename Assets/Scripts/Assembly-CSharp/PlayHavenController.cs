using System.Collections.Generic;

public class PlayHavenController
{
	public enum PurchaseResolution
	{
		BUY = 0,
		CANCEL = 1,
		ERROR = 2
	}

	public const string MORE_NICK_PLACEMENT = "more_nick_click";

	public const string FIRST_TIME_APP_START_PLACEMENT = "first_time_app_start";

	public const string APP_START_PLACEMENT = "app_start";

	public const string APP_RESUME_PLACEMENT = "app_resume";

	public const string LOADING_SCREEN_END_PLACEMENT = "loading_screen_end";

	public const string SHOP_OPEN_PLACEMENT = "shop_open";

	public const string LEVEL_PLACEMENT = "level_";

	public const string LOW_BALANCE_COINS_PLACEMENT = "low_balance_coins";

	public const string LOW_BALANCE_JJ_PLACEMENT = "low_balance_jellyfish_jelly";

	public const string PAYMIUM_END_TUTORIAL_PAYMIUM_ITEM_IN_INVENTORY = "end_tutorial_paymium_item_in_inventory";

	public static int? PAYMIUM_ITEM_DID;

	public const string PIRATE_BOOTY_GAME_INITIALIZED_NO_SHIP = "loading_screen_end_existingplayer_no_ship";

	public const string PIRATE_BOOTY_GAME_INITIALIZED_HAS_SHIP = "loading_screen_end_existingplayer_with_ship";

	public static int? PIRATE_BOOTY_SHIP_DID;

	public const int LOW_BALANCE_COINS_THRESHOLD = 100;

	public const int LOW_BALANCE_JJ_THRESHOLD = 20;

	public Dictionary<string, string> namesToResource;

	public const string DASHBOARD_RESOURCE_JELLY = "_jelly";

	public const string DASHBOARD_RESOURCE_GOLD = "_gold";

	public const string DASHBOARD_RESOURCE_XP = "_xp";

	public const string DASHBOARD_BUILDING_PREFIX = "_building_";

	public const string DASHBOARD_RECIPE_PREFIX = "_recipe_";

	public const string DASHBOARD_MOVIE_PREFIX = "_movie_";

	private Session session;

	public void Initialize(Session session)
	{
	}

	public void RequestContent(string placement)
	{
	}

	public void OnRewardGiven(UpsightReward reward)
	{
	}

	private void PopulateRewardDict(string prefix, Dictionary<string, object> dict, string rewardName, int quantity)
	{
	}

	public void OnPurchaseError(object sender, RmtStore.StoreEventArgs args)
	{
	}

	public void OnPurchaseReceiptReceived(object sender, RmtStore.StoreEventArgs args)
	{
	}

	public void PurchaseItem(string productId, int quantity, string currency, double price, string receipt, PurchaseResolution resolution, string transactionID)
	{
	}

	public void OnVirtualGoodsPromotionClicked(UpsightPurchase purchase)
	{
	}
}
