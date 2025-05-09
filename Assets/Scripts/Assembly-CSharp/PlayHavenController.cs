using System.Collections.Generic;
using UnityEngine;

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

	public const string PIRATE_BOOTY_GAME_INITIALIZED_NO_SHIP = "loading_screen_end_existingplayer_no_ship";

	public const string PIRATE_BOOTY_GAME_INITIALIZED_HAS_SHIP = "loading_screen_end_existingplayer_with_ship";

	public const int LOW_BALANCE_COINS_THRESHOLD = 100;

	public const int LOW_BALANCE_JJ_THRESHOLD = 20;

	public const string DASHBOARD_RESOURCE_JELLY = "_jelly";

	public const string DASHBOARD_RESOURCE_GOLD = "_gold";

	public const string DASHBOARD_RESOURCE_XP = "_xp";

	public const string DASHBOARD_BUILDING_PREFIX = "_building_";

	public const string DASHBOARD_RECIPE_PREFIX = "_recipe_";

	public const string DASHBOARD_MOVIE_PREFIX = "_movie_";

	public static int? PAYMIUM_ITEM_DID = 9010;

	public static int? PIRATE_BOOTY_SHIP_DID = 9011;

	public Dictionary<string, string> namesToResource = new Dictionary<string, string>();

	private Session session;

	public PlayHavenController()
	{
		namesToResource = new Dictionary<string, string>();
		namesToResource["_gold"] = ResourceManager.SOFT_CURRENCY.ToString();
		namesToResource["_jelly"] = ResourceManager.HARD_CURRENCY.ToString();
		namesToResource["_xp"] = ResourceManager.XP.ToString();
	}

	public void Initialize(Session session)
	{
		this.session = session;
		session.TheGame.store.PurchaseError += OnPurchaseError;
		session.TheGame.store.PurchaseReceiptReceived += OnPurchaseReceiptReceived;
		UpsightManager.unlockedRewardEvent += OnRewardGiven;
		UpsightManager.makePurchaseEvent += OnVirtualGoodsPromotionClicked;
	}

	public void RequestContent(string placement)
	{
		TFUtils.DebugLog("Requesting placement to Playhaven " + placement);
		Upsight.sendContentRequest(placement, false);
	}

	public void OnRewardGiven(UpsightReward reward)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		if (namesToResource.ContainsKey(reward.name))
		{
			string key = namesToResource[reward.name];
			dictionary[key] = reward.quantity;
		}
		PopulateRewardDict("_building_", dictionary2, reward.name, reward.quantity);
		PopulateRewardDict("_movie_", dictionary4, reward.name, reward.quantity);
		PopulateRewardDict("_recipe_", dictionary3, reward.name, reward.quantity);
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		dictionary5["resources"] = dictionary;
		dictionary5["recipes"] = dictionary3;
		dictionary5["movies"] = dictionary4;
		dictionary5["buildings"] = dictionary2;
		if (session == null || session.TheGame == null)
		{
			return;
		}
		RewardDefinition rewardDefinition = RewardDefinition.FromDict(dictionary5);
		Reward reward2 = rewardDefinition.GenerateReward(session.TheGame.simulation, false);
		foreach (KeyValuePair<int, int> buildingAmount in reward2.BuildingAmounts)
		{
			int key2 = buildingAmount.Key;
			Blueprint blueprint = EntityManager.GetBlueprint("building", key2, true);
			int? num = null;
			if (blueprint != null)
			{
				num = blueprint.GetInstanceLimitByLevel(session.TheGame.resourceManager.PlayerLevelAmount);
			}
			if (!num.HasValue)
			{
				continue;
			}
			int num2 = 0;
			List<Simulated> list = session.TheGame.simulation.FindAllSimulateds(key2);
			foreach (Simulated item in list)
			{
				if ((item.Entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
				{
					num2++;
				}
			}
			foreach (SBInventoryItem item2 in session.TheGame.inventory.GetItems())
			{
				if (item2.entity.DefinitionId == key2)
				{
					num2++;
				}
			}
			if (num2 > num.Value)
			{
				Debug.LogError("Cannot add another instance of this building since instance limit of " + num.Value + " has been reached!");
				reward2.BuildingAmounts[buildingAmount.Key] = 0;
			}
		}
		session.TheGame.ApplyReward(reward2, TFUtils.EpochTime(), false);
		session.TheGame.ModifyGameState(new ReceiveRewardAction(reward2, reward.name));
	}

	private void PopulateRewardDict(string prefix, Dictionary<string, object> dict, string rewardName, int quantity)
	{
		if (rewardName.StartsWith(prefix))
		{
			string key = rewardName.Substring(prefix.Length);
			dict[key] = quantity;
		}
	}

	public void OnPurchaseError(object sender, RmtStore.StoreEventArgs args)
	{
		string text = "OnPurchaseError | ";
		Dictionary<string, object> results = args.results;
		foreach (KeyValuePair<string, object> item in results)
		{
			string text2 = text;
			text = text2 + " key: " + item.Key + " value: " + item.Value.ToString();
		}
		TFUtils.DebugLog(text);
		string productId = (string)args.results["productId"];
		if ((string)args.results["reason"] == "userCancelled")
		{
			PurchaseItem(productId, 1, null, PurchaseResolution.CANCEL, null);
		}
		else
		{
			PurchaseItem(productId, 1, null, PurchaseResolution.ERROR, null);
		}
	}

	public void OnPurchaseReceiptReceived(object sender, RmtStore.StoreEventArgs args)
	{
		string text = (string)args.results["productId"];
		string receipt = (string)args.results["receipt"];
		string text2 = null;
		text2 = ((!args.results.ContainsKey("transactionId")) ? text : ((string)args.results["transactionId"]));
		PurchaseItem(text, 1, receipt, PurchaseResolution.BUY, text2);
	}

	public void PurchaseItem(string productId, int quantity, string receipt, PurchaseResolution resolution, string transactionID)
	{
		TFUtils.DebugLog("Tracking a purchase item for " + productId + " with resolution " + resolution);
		UpsightAndroidPurchaseResolution resolutionType = UpsightAndroidPurchaseResolution.Error;
		switch (resolution)
		{
		case PurchaseResolution.BUY:
			resolutionType = UpsightAndroidPurchaseResolution.Bought;
			break;
		case PurchaseResolution.CANCEL:
			resolutionType = UpsightAndroidPurchaseResolution.Cancelled;
			break;
		case PurchaseResolution.ERROR:
			resolutionType = UpsightAndroidPurchaseResolution.Error;
			break;
		}
		Upsight.trackInAppPurchase(productId, quantity, resolutionType, 0.0, transactionID, null);
	}

	public void OnVirtualGoodsPromotionClicked(UpsightPurchase purchase)
	{
		TFUtils.DebugLog("Virtual goods promotion clicked:" + purchase.quantity + " " + purchase.productIdentifier);
		session.PurchasePremiumProduct(purchase.productIdentifier);
	}
}
