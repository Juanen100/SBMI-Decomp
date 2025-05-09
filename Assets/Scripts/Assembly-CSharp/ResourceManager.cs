#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarg;

public class ResourceManager
{
	private const int MAX_AMOUNT = 32767;

	public const string TYPE_HARD_CURRENCY = "hard_currency";

	public const string TYPE_SOFT_CURRENCY = "soft_currency";

	public const string TYPE_HALLOWEEN_CURRENCY = "halloween_currency";

	public const string TYPE_CHRISTMAS_CURRENCY = "christmas_currency";

	public const string TYPE_VALENTINES_CURRENCY = "valentines_currency";

	public const string TYPE_SPONGY_GAMES_CURRENCY = "squilliams_currency";

	public const string TYPE_BONES_CURRENCY = "halloween_bones_currency";

	public const string TYPE_CHRISTMAS_CURRENCY_V2 = "christmas_bottles_currency";

	public const string TYPE_LEVEL = "level";

	public const string TYPE_XP = "xp";

	public const string TYPE_DEFAULT_WISH = "default_wish";

	public const string TYPE_DEFAULT_JJ = "default_jj";

	public const string TYPE_VALENTINES_2015_CURRENCY = "valentines_2015_currency";

	public const string EMPTY_WISH_TEXTURE = "empty.png";

	public const string UPDATE_RESOURCE = "UpdateResource";

	public const int RESOURCE_TYPE_HOLIDAY_CHEER = 9100;

	private static readonly string QUESTS_PATH = "Resources";

	public static int DEFAULT_WISH = -1;

	public static int HARD_CURRENCY = 2;

	public static int SOFT_CURRENCY = 3;

	public static int HALLOWEEN_CURRENCY = -1;

	public static int CHRISTMAS_CURRENCY = -1;

	public static int CHRISTMAS_CURRENCY_V2 = -1;

	public static int VALENTINES_CURRENCY = -1;

	public static int SPONGY_GAMES_CURRENCY = -1;

	public static int BONES_CURRENCY = -1;

	public static int SPECIAL_CURRENCY = -1;

	public static int LEVEL = 4;

	public static int XP = 5;

	public static int DEFAULT_JJ = 33;

	public static int VALENTINES_2015_CURRENCY = -1;

	private Dictionary<int, Resource> resources;

	private static Dictionary<int, Resource> internal_resources;

	private HashSet<int> consumableResources;

	public List<string> resourceCategoryOrder;

	private Session session;

	public Dictionary<string, ResourceCategory> resourceCategories = new Dictionary<string, ResourceCategory>();

	private static double RESOURCE_TIME_FACTOR;

	private static double RESOURCE_COMPRESSION_BASE;

	private static double RENT_TIME_FACTOR;

	private static double RENT_COMPRESSION_BASE;

	private static double FULLNESS_TIME_FACTOR;

	private static double FULLNESS_COMPRESSION_BASE;

	private static double DEBRIS_TIME_FACTOR;

	private static double DEBRIS_COMPRESSION_BASE;

	private static double CONSTRUCTION_TIME_FACTOR;

	private static double CONSTRUCTION_COMPRESSION_BASE;

	private static double TASK_TIME_FACTOR;

	private static double TASK_COMPRESSION_BASE;

	public Dictionary<int, Resource> Resources
	{
		get
		{
			return resources;
		}
	}

	public int PlayerLevelAmount
	{
		get
		{
			return resources[LEVEL].Amount;
		}
	}

	public ResourceManager(Session session)
	{
		consumableResources = new HashSet<int>();
		resources = LoadResourceDefinitions();
		internal_resources = resources;
		this.session = session;
	}

	public static string TypeDescription(int typeID)
	{
		if (typeID == DEFAULT_WISH)
		{
			return "default_wish";
		}
		if (typeID == HARD_CURRENCY)
		{
			return "hard_currency";
		}
		if (typeID == SOFT_CURRENCY)
		{
			return "soft_currency";
		}
		if (typeID == LEVEL)
		{
			return "level";
		}
		if (typeID == XP)
		{
			return "xp";
		}
		if (typeID == HALLOWEEN_CURRENCY)
		{
			return "halloween_currency";
		}
		if (typeID == CHRISTMAS_CURRENCY)
		{
			return "christmas_currency";
		}
		if (typeID == CHRISTMAS_CURRENCY_V2)
		{
			return "christmas_bottles_currency";
		}
		if (typeID == VALENTINES_CURRENCY)
		{
			return "valentines_currency";
		}
		if (typeID == SPONGY_GAMES_CURRENCY)
		{
			return "squilliams_currency";
		}
		if (typeID == BONES_CURRENCY)
		{
			return "halloween_bones_currency";
		}
		if (typeID == VALENTINES_2015_CURRENCY)
		{
			return "valentines_2015_currency";
		}
		TFUtils.Assert(false, "Invalid Resource for Type Description, please add to ResourceManager if needed");
		return "Invalid";
	}

	public static void ApplyCostToGameState(Cost cost, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (Dictionary<string, object> item in list)
		{
			int key = TFUtils.LoadInt(item, "did");
			int value = 0;
			if (cost.ResourceAmounts.TryGetValue(key, out value))
			{
				item["amount_spent"] = TFUtils.LoadInt(item, "amount_spent") + value;
			}
		}
	}

	public static void ApplyCostToGameState(int resourceId, int amount, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (Dictionary<string, object> item in list)
		{
			int num = TFUtils.LoadInt(item, "did");
			if (num == resourceId)
			{
				item["amount_spent"] = TFUtils.LoadInt(item, "amount_spent") + amount;
				return;
			}
		}
		TFUtils.Assert(false, string.Format("Just tried to apply a spend on Resource Did {0} and do not have that defined in the GameState!", resourceId));
	}

	public static void AddAmountToGameState(int resourceId, int amount, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (Dictionary<string, object> item in list)
		{
			int num = TFUtils.LoadInt(item, "did");
			if (num == resourceId)
			{
				item["amount_earned"] = TFUtils.LoadInt(item, "amount_earned") + amount;
				return;
			}
		}
		if (internal_resources != null && !internal_resources.ContainsKey(resourceId))
		{
			string message = "Invalid Resource ID Added: " + resourceId;
			TFUtils.ErrorLog(message);
			TFUtils.LogDump(null, "invalid_resource", new Exception(message));
			return;
		}
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2["did"] = resourceId;
		dictionary2["amount_earned"] = amount;
		dictionary2["amount_spent"] = 0;
		dictionary2["amount_purchased"] = 0;
		list.Add(dictionary2);
	}

	public static void ApplyPurchasesToGameState(Cost cost, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (Dictionary<string, object> item in list)
		{
			int key = TFUtils.LoadInt(item, "did");
			if (cost.ResourceAmounts.ContainsKey(key))
			{
				item["amount_purchased"] = cost.ResourceAmounts[key];
			}
		}
	}

	private string[] GetFilesToLoad()
	{
		return TFUtils.GetFilesInPath(QUESTS_PATH, "resources.json");
	}

	private Dictionary<int, Resource> LoadResourceDefinitions()
	{
		Dictionary<string, object> conversionDataFromSpread = GetConversionDataFromSpread();
		Dictionary<string, object> categoryOrderDataFromSpread = GetCategoryOrderDataFromSpread();
		Dictionary<string, object> d = TFUtils.LoadDict(conversionDataFromSpread, "time_factor");
		Dictionary<string, object> d2 = TFUtils.LoadDict(conversionDataFromSpread, "compression_base");
		RESOURCE_TIME_FACTOR = TFUtils.LoadDouble(d, "resource");
		RESOURCE_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "resource");
		RENT_TIME_FACTOR = TFUtils.LoadDouble(d, "rent");
		RENT_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "rent");
		FULLNESS_TIME_FACTOR = TFUtils.LoadDouble(d, "fullness");
		FULLNESS_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "fullness");
		DEBRIS_TIME_FACTOR = TFUtils.LoadDouble(d, "debris");
		DEBRIS_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "debris");
		CONSTRUCTION_TIME_FACTOR = TFUtils.LoadDouble(d, "construction");
		CONSTRUCTION_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "construction");
		TASK_TIME_FACTOR = TFUtils.LoadDouble(d, "task");
		TASK_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "task");
		resourceCategoryOrder = TFUtils.LoadList<string>(categoryOrderDataFromSpread, "category_order");
		List<Dictionary<string, object>> list = TFUtils.LoadList<Dictionary<string, object>>(categoryOrderDataFromSpread, "category_to_productgroups");
		foreach (Dictionary<string, object> item in list)
		{
			ResourceCategory resourceCategory = ResourceCategory.FromDict(item);
			resourceCategories.Add(resourceCategory.name, resourceCategory);
		}
		List<object> resourceDictsFromSpread = GetResourceDictsFromSpread();
		Dictionary<int, Resource> dictionary = new Dictionary<int, Resource>();
		foreach (Dictionary<string, object> item2 in resourceDictsFromSpread)
		{
			int num = TFUtils.LoadInt(item2, "did");
			string text = Language.Get(TFUtils.LoadString(item2, "name"));
			string text2 = TFUtils.TryLoadString(item2, "name_plural");
			text2 = ((text2 != null) ? Language.Get(text2) : text);
			string tag = TFUtils.TryLoadString(item2, "tag");
			int maxAmount = 32767;
			if (item2.ContainsKey("max_amount"))
			{
				maxAmount = TFUtils.LoadInt(item2, "max_amount");
			}
			float width = -1f;
			float height = -1f;
			string text3 = null;
			if (item2.ContainsKey("texture"))
			{
				text3 = TFUtils.LoadString(item2, "texture");
				TFUtils.Assert(YGTextureLibrary.HasAtlasCoords(text3), "The texture atlas does not have an entry for " + text3);
				AtlasCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(text3).atlasCoords;
				width = (float)TFAnimatedSprite.CalcWorldSize(atlasCoords.frame.width, 0.8);
				height = (float)TFAnimatedSprite.CalcWorldSize(atlasCoords.frame.height, 0.8);
			}
			string collectedSound = null;
			if (item2.ContainsKey("collected_sound"))
			{
				collectedSound = TFUtils.LoadString(item2, "collected_sound");
			}
			string tapSound = null;
			if (item2.ContainsKey("tap_sound"))
			{
				tapSound = TFUtils.LoadString(item2, "tap_sound");
			}
			string eatenSound = null;
			if (item2.ContainsKey("eaten_sound"))
			{
				eatenSound = TFUtils.LoadString(item2, "eaten_sound");
			}
			RewardDefinition reward = null;
			if (item2.ContainsKey("reward"))
			{
				reward = RewardDefinition.FromObject(item2["reward"]);
			}
			float jellyConversion = ((!item2.ContainsKey("jelly_conversion")) ? 1f : TFUtils.LoadFloat(item2, "jelly_conversion"));
			int fullnessTime = (item2.ContainsKey("fullness_time") ? TFUtils.LoadInt(item2, "fullness_time") : 0);
			bool forceTapToCollect = false;
			object value;
			if (item2.TryGetValue("force_tap_to_collect", out value))
			{
				forceTapToCollect = (bool)value;
			}
			bool forceWishMatch = false;
			object value2;
			if (item2.TryGetValue("force_wish_match", out value2))
			{
				forceWishMatch = (bool)value2;
			}
			bool forceNoWishPayout = false;
			object value3;
			if (item2.TryGetValue("force_no_wish_payout", out value3))
			{
				forceNoWishPayout = (bool)value3;
			}
			bool ignoreWishDurationTimer = false;
			object value4;
			if (item2.TryGetValue("ignore_wish_duration_timer", out value4))
			{
				ignoreWishDurationTimer = (bool)value4;
			}
			bool flag = false;
			object value5;
			if (item2.TryGetValue("consumable", out value5))
			{
				flag = (bool)value5;
			}
			if (flag)
			{
				consumableResources.Add(num);
			}
			int currencyDisplayQuestTrigger = TFUtils.LoadInt(item2, "currency_display_quest_trigger");
			if (item2.ContainsKey("type"))
			{
				string text4 = TFUtils.LoadString(item2, "type");
				switch (text4)
				{
				case "default_wish":
					DEFAULT_WISH = num;
					break;
				case "soft_currency":
					SOFT_CURRENCY = num;
					break;
				case "hard_currency":
					HARD_CURRENCY = num;
					break;
				case "halloween_currency":
					HALLOWEEN_CURRENCY = num;
					break;
				case "christmas_currency":
					CHRISTMAS_CURRENCY = num;
					break;
				case "christmas_bottles_currency":
					CHRISTMAS_CURRENCY_V2 = num;
					break;
				case "valentines_currency":
					VALENTINES_CURRENCY = num;
					break;
				case "squilliams_currency":
					SPONGY_GAMES_CURRENCY = num;
					break;
				case "halloween_bones_currency":
					BONES_CURRENCY = num;
					break;
				case "level":
					LEVEL = num;
					break;
				case "xp":
					XP = num;
					break;
				case "default_jj":
					DEFAULT_JJ = num;
					break;
				case "valentines_2015_currency":
					VALENTINES_2015_CURRENCY = num;
					break;
				default:
					TFUtils.Assert(false, "Unknown Resource 'type' found in Resource Definition JSON: " + text4);
					break;
				}
			}
			dictionary[num] = new Resource(text, text2, tag, width, height, maxAmount, text3, collectedSound, tapSound, eatenSound, reward, jellyConversion, fullnessTime, forceTapToCollect, forceWishMatch, ignoreWishDurationTimer, forceNoWishPayout, num, currencyDisplayQuestTrigger, flag);
		}
		return dictionary;
	}

	public void LoadResources(List<object> resources)
	{
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		try
		{
			foreach (Dictionary<string, object> resource in resources)
			{
				num = TFUtils.LoadInt(resource, "did");
				num2 = TFUtils.LoadInt(resource, "amount_earned");
				num3 = TFUtils.LoadInt(resource, "amount_spent");
				this.resources[num].SetAmounts(num2, num3);
				int amountPurchased = TFUtils.LoadInt(resource, "amount_purchased");
				this.resources[num].SetAmountPurchased(amountPurchased);
			}
		}
		catch (Exception ex)
		{
			ex.Data.Add("did", num);
			ex.Data.Add("amount_earned", num2);
			ex.Data.Add("amount_spent", num3);
			ex.Data.Add("error_code", 303);
			throw ex;
		}
	}

	public void UpdateLevelExpToMilestone(LevelingManager manager)
	{
		int amount = resources[XP].Amount;
		int amount2 = resources[LEVEL].Amount;
		if (amount2 < manager.MaxLevel)
		{
			int xpRequiredForLevel = manager.GetXpRequiredForLevel(amount2);
			int xpRequiredForLevel2 = manager.GetXpRequiredForLevel(amount2 + 1);
			if (amount < xpRequiredForLevel)
			{
				resources[XP].SetAmountEarned(xpRequiredForLevel);
			}
			else if (amount >= xpRequiredForLevel2)
			{
				resources[XP].SetAmountEarned(xpRequiredForLevel2 - 1);
			}
		}
	}

	public bool CanPay(Cost cost)
	{
		foreach (int key in cost.ResourceAmounts.Keys)
		{
			if (!HasEnough(key, cost.ResourceAmounts[key]))
			{
				TFUtils.DebugLog("Not enough " + resources[key].Name);
				return false;
			}
		}
		return true;
	}

	public void Apply(Cost cost, Game game)
	{
		foreach (int key in cost.ResourceAmounts.Keys)
		{
			Spend(key, cost.ResourceAmounts[key], game);
		}
	}

	public void SellFor(Cost cost, Game game)
	{
		foreach (int key in cost.ResourceAmounts.Keys)
		{
			Add(key, cost.ResourceAmounts[key], game);
		}
	}

	public bool HasEnough(int resourceId, int minimumAmount)
	{
		return resources.ContainsKey(resourceId) && resources[resourceId].Amount >= minimumAmount;
	}

	public int Query(int resourceId)
	{
		return resources[resourceId].Amount;
	}

	public float QueryProgressPercentage(IResourceProgressCalculator calculator)
	{
		return calculator.ComputeProgressPercentage(resources);
	}

	public string QueryProgressFraction(IResourceProgressCalculator calculator)
	{
		return calculator.ComputeProgressFraction(resources);
	}

	public void Spend(Cost cost, Game game)
	{
		foreach (int key in cost.ResourceAmounts.Keys)
		{
			Spend(key, cost.ResourceAmounts[key], game);
		}
	}

	public void Spend(int resourceId, int amount, Game game)
	{
		if (!HasEnough(resourceId, amount))
		{
			TFUtils.ErrorLog("Not enough of this resource(" + resourceId + ")");
			return;
		}
		bool flag = false;
		if (session.gameInitialized)
		{
			if (resourceId == HARD_CURRENCY && resources[resourceId].Amount >= 20)
			{
				flag = true;
			}
			if (resourceId == SOFT_CURRENCY && resources[resourceId].Amount >= 100)
			{
				flag = true;
			}
		}
		resources[resourceId].SubtractAmount(amount);
		if (flag)
		{
			if (resourceId == HARD_CURRENCY && resources[resourceId].Amount < 20)
			{
				session.PlayHavenController.RequestContent("low_balance_jellyfish_jelly");
			}
			if (resourceId == SOFT_CURRENCY && resources[resourceId].Amount < 100)
			{
				session.PlayHavenController.RequestContent("low_balance_coins");
			}
		}
		if (resources[resourceId].Tag != null)
		{
			game.analytics.LogResourceEconomySink(resources[resourceId].Tag, amount, resources[LEVEL].Amount);
		}
		game.triggerRouter.RouteTrigger(CreateModifyResourceTrigger(resourceId, -amount), game);
	}

	public void Add(int resourceId, int amount, Game game)
	{
		int amount2 = resources[LEVEL].Amount;
		resources[resourceId].AddAmount(amount);
		if (resources[resourceId].Tag != null)
		{
			game.analytics.LogResourceEconomySource(resources[resourceId].Tag, amount, amount2, resources[LEVEL].Amount, this);
		}
		if (amount2 != resources[LEVEL].Amount)
		{
			List<Reward> levelUpRewards = game.levelingManager.GetLevelUpRewards(game.simulation, amount2, game.levelingManager.GetXpRequiredForLevel(resources[LEVEL].Amount));
			AnalyticsWrapper.LogLevelUp(game, levelUpRewards, resources[LEVEL].Amount);
			Catalog catalog = session.TheGame.catalog;
			Dictionary<int, SBMarketOffer> dictionary = new Dictionary<int, SBMarketOffer>();
			SBMarketOffer value;
			foreach (object item in (List<object>)catalog.CatalogDict["offers"])
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item;
				if (!dictionary2.ContainsKey("show_in_store") || (bool)dictionary2["show_in_store"])
				{
					value = new SBMarketOffer((Dictionary<string, object>)item);
					dictionary[value.identity] = value;
				}
			}
			foreach (object item2 in (List<object>)catalog.CatalogDict["market"])
			{
				SBMarketCategory sBMarketCategory = new SBMarketCategory((Dictionary<string, object>)item2);
				int[] dids = sBMarketCategory.Dids;
				foreach (int key in dids)
				{
					if (!dictionary.TryGetValue(key, out value))
					{
						continue;
					}
					if (value.type == null)
					{
						value.type = sBMarketCategory.Type;
					}
					Blueprint blueprint = EntityManager.GetBlueprint(value.type, value.identity, true);
					if (blueprint != null)
					{
						int num = (int)blueprint.Invariable["level.minimum"];
						if (num > amount2 && num <= resources[LEVEL].Amount)
						{
							AnalyticsWrapper.LogFeatureUnlocked(session.TheGame, (string)blueprint.Invariable["name"], sBMarketCategory.DeltaDNAName);
						}
					}
				}
			}
		}
		game.triggerRouter.RouteTrigger(CreateModifyResourceTrigger(resourceId, amount), game);
	}

	public Dictionary<string, object>[] ToDict()
	{
		Dictionary<string, object>[] array = new Dictionary<string, object>[resources.Count];
		int num = 0;
		foreach (int key in resources.Keys)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = key;
			dictionary["amount"] = resources[key];
			array[num++] = dictionary;
		}
		return array;
	}

	public override string ToString()
	{
		string text = "Resources:\n";
		string[] array = new string[resources.Count];
		int num = 0;
		foreach (int key in resources.Keys)
		{
			array[num++] = "\t" + resources[key].Name + ": " + resources[key];
		}
		return text + string.Join("\n", array);
	}

	public List<int> ConsumableProducts(CraftingManager craftManager)
	{
		return craftManager.UnlockedProductsShallowCopy.Intersect(consumableResources).ToList();
	}

	public void PurchaseResourcesWithHardCurrency(int hcCost, Cost resources, Game game)
	{
		Spend(HARD_CURRENCY, hcCost, game);
		foreach (KeyValuePair<int, int> resourceAmount in resources.ResourceAmounts)
		{
			Add(resourceAmount.Key, resourceAmount.Value, game);
		}
	}

	public void SetPurchasedResources(Cost resources)
	{
		foreach (KeyValuePair<int, int> resourceAmount in resources.ResourceAmounts)
		{
			this.resources[resourceAmount.Key].SetAmountPurchased(resourceAmount.Value);
			TFUtils.DebugLog("SetPurchasedResources: " + resourceAmount.Key + " : " + resourceAmount.Value);
		}
	}

	public int GetResourcesPackageCostInHardCurrencyValue(Cost resourcesNeeded)
	{
		float num = 0f;
		foreach (KeyValuePair<int, int> resourceAmount in resourcesNeeded.ResourceAmounts)
		{
			TFUtils.Assert(resources[resourceAmount.Key].HardCurrencyConversion != 0f, "Should not have a zero value conversion!");
			TFUtils.Assert((float)resourceAmount.Value / resources[resourceAmount.Key].HardCurrencyConversion > 0f, "Hard Currency Conversion should NEVER result in giving the User HardCurrency!");
			num += (float)resourceAmount.Value / resources[resourceAmount.Key].HardCurrencyConversion;
		}
		return Mathf.CeilToInt(num);
	}

	public static Cost CalculateCraftRushCost(ulong recipeTime)
	{
		return CalculateTimeToJjCost(recipeTime, RESOURCE_TIME_FACTOR, RESOURCE_COMPRESSION_BASE);
	}

	public static Cost CalculateRentRushCost(ulong rentTime)
	{
		return CalculateTimeToJjCost(rentTime, RENT_TIME_FACTOR, RENT_COMPRESSION_BASE);
	}

	public static Cost CalculateFullnessRushCost(ulong fullnessTime)
	{
		return CalculateTimeToJjCost(fullnessTime, FULLNESS_TIME_FACTOR, FULLNESS_COMPRESSION_BASE);
	}

	public static Cost CalculateDebrisRushCost(ulong timeLeft)
	{
		return CalculateTimeToJjCost(timeLeft, DEBRIS_TIME_FACTOR, DEBRIS_COMPRESSION_BASE);
	}

	public static Cost CalculateConstructionRushCost(ulong timeLeft)
	{
		return CalculateTimeToJjCost(timeLeft, CONSTRUCTION_TIME_FACTOR, CONSTRUCTION_COMPRESSION_BASE);
	}

	public static Cost CalculateTaskRushCost(ulong timeLeft)
	{
		return CalculateTimeToJjCost(timeLeft, TASK_TIME_FACTOR, TASK_COMPRESSION_BASE);
	}

	private static Cost CalculateTimeToJjCost(ulong time, double timeToJjFactor, double timeCompressionBase)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		double originalCost = (double)time * timeToJjFactor;
		int value = CompressTimeCost(originalCost, timeCompressionBase);
		dictionary.Add(HARD_CURRENCY, value);
		return new Cost(dictionary);
	}

	private static int CompressTimeCost(double originalCost, double compressionBase)
	{
		return (int)Math.Ceiling(Math.Log(originalCost + 1.0, compressionBase));
	}

	public int GetNumDisplayableResources()
	{
		int num = 0;
		foreach (Resource value in resources.Values)
		{
			if (value.GetResourceTexture() != null)
			{
				num++;
			}
		}
		return num;
	}

	private ITrigger CreateModifyResourceTrigger(int resourceId, int amount)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[resourceId.ToString()] = amount;
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2["resource_amounts"] = dictionary;
		return new Trigger("UpdateResource", dictionary2);
	}

	public List<int> SortRecipesByProductGroup(CraftingManager craftManager, List<int> unsortedList)
	{
		List<int> list = new List<int>();
		foreach (string item in resourceCategoryOrder)
		{
			ResourceCategory resourceCategory = resourceCategories[item];
			foreach (ResourceProductGroup productGroup in resourceCategory.productGroups)
			{
				foreach (int recipeDid in productGroup.recipeDids)
				{
					CraftingRecipe recipeById = craftManager.GetRecipeById(recipeDid);
					if (unsortedList.Contains(recipeById.identity))
					{
						list.Add(recipeById.identity);
					}
				}
			}
		}
		return list;
	}

	public void UpdateProductGroups(CraftingManager craftManager)
	{
		foreach (CraftingRecipe value in craftManager.Recipes.Values)
		{
			foreach (ResourceCategory value2 in resourceCategories.Values)
			{
				ResourceProductGroup productGroupByName = value2.GetProductGroupByName(value.productGroup);
				if (productGroupByName != null)
				{
					productGroupByName.AddRecipe(craftManager, value);
					break;
				}
			}
		}
	}

	private Dictionary<string, object> GetConversionDataFromSpread()
	{
		string text = "Conversions";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("time_factor", new Dictionary<string, object>());
		dictionary.Add("compression_base", new Dictionary<string, object>());
		Dictionary<string, object> dictionary2 = dictionary;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(text, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			string stringCell = instance.GetStringCell(text, rowName, "conversion type");
			((Dictionary<string, object>)dictionary2["time_factor"]).Add(stringCell, instance.GetFloatCell(sheetIndex, rowIndex, "time factor"));
			((Dictionary<string, object>)dictionary2["compression_base"]).Add(stringCell, instance.GetFloatCell(sheetIndex, rowIndex, "compression base"));
		}
		return dictionary2;
	}

	private Dictionary<string, object> GetCategoryOrderDataFromSpread()
	{
		string text = "StoreOrder";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		List<string> list = new List<string>();
		List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
		string text2 = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(text, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "num items");
			}
			string stringCell = instance.GetStringCell(text, rowName, "category");
			if (list.Contains(stringCell))
			{
				continue;
			}
			list.Add(stringCell);
			List<string> list3 = new List<string>();
			for (int j = 1; j <= num2; j++)
			{
				string stringCell2 = instance.GetStringCell(text, rowName, "item " + j);
				if (stringCell2 == text2)
				{
					break;
				}
				if (!list3.Contains(stringCell2))
				{
					list3.Add(stringCell2);
				}
			}
			list2.Add(new Dictionary<string, object> { { stringCell, list3 } });
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("category_order", list);
		dictionary.Add("category_to_productgroups", list2);
		return dictionary;
	}

	private List<object> GetResourceDictsFromSpread()
	{
		string text = "Resources";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		List<object> list = new List<object>();
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "tag");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "ignore wish duration");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "force wish match");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "force no wish payout");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "consumable");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "expected order");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "fullness time");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "max amount");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "jelly conversion");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "texture");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "collected sound");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "eaten sound");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet17 = instance.GetColumnIndexInSheet(sheetIndex, "tap sound");
		int columnIndexInSheet18 = instance.GetColumnIndexInSheet(sheetIndex, "reward gold");
		int columnIndexInSheet19 = instance.GetColumnIndexInSheet(sheetIndex, "reward xp");
		int columnIndexInSheet20 = instance.GetColumnIndexInSheet(sheetIndex, "special currency hud display");
		int columnIndexInSheet21 = instance.GetColumnIndexInSheet(sheetIndex, "currency display quest trigger");
		string text2 = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2));
			dictionary.Add("currency_display_quest_trigger", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet21));
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3));
			dictionary.Add("tag", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("ignore_wish_duration_timer", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5) == 1);
			dictionary.Add("force_wish_match", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6) == 1);
			dictionary.Add("force_no_wish_payout", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7) == 1);
			dictionary.Add("consumable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8) == 1);
			if (SPECIAL_CURRENCY < 0 && instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet20) == 1)
			{
				SPECIAL_CURRENCY = (int)dictionary["did"];
			}
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9);
			if (intCell >= 0)
			{
				dictionary.Add("_expected_order", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10);
			if (intCell >= 0)
			{
				dictionary.Add("fullness_time", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11);
			if (intCell >= 0)
			{
				dictionary.Add("max_amount", intCell);
			}
			float floatCell = instance.GetFloatCell(sheetIndex, rowIndex, columnIndexInSheet12);
			if (floatCell >= 0f)
			{
				dictionary.Add("jelly_conversion", floatCell);
			}
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet13);
			if (stringCell != text2)
			{
				dictionary.Add("texture", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet14);
			if (stringCell != text2)
			{
				dictionary.Add("collected_sound", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet15);
			if (stringCell != text2)
			{
				dictionary.Add("eaten_sound", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet16);
			if (stringCell != text2)
			{
				dictionary.Add("type", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet17);
			if (stringCell != text2)
			{
				dictionary.Add("tap_sound", stringCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet18);
			if (intCell > 0)
			{
				if (!dictionary.ContainsKey("reward"))
				{
					dictionary.Add("reward", new Dictionary<string, object> { 
					{
						"resources",
						new Dictionary<string, object>()
					} });
				}
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["reward"])["resources"]).Add("3", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet19);
			if (intCell > 0)
			{
				if (!dictionary.ContainsKey("reward"))
				{
					dictionary.Add("reward", new Dictionary<string, object> { 
					{
						"resources",
						new Dictionary<string, object>()
					} });
				}
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["reward"])["resources"]).Add("5", intCell);
			}
			list.Add(dictionary);
		}
		return list;
	}
}
