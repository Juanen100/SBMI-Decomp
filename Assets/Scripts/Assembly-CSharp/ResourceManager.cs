using System.Collections.Generic;

public class ResourceManager
{
	private static readonly string QUESTS_PATH;

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

	public const string TYPE_ANNIVERSARY_CURRENCY = "aniversary_currency";

	public const string TYPE_VALENTINES_2015_CURRENCY = "valentines_2015_currency";

	public static int DEFAULT_WISH;

	public static int HARD_CURRENCY;

	public static int SOFT_CURRENCY;

	public static int HALLOWEEN_CURRENCY;

	public static int CHRISTMAS_CURRENCY;

	public static int CHRISTMAS_CURRENCY_V2;

	public static int VALENTINES_CURRENCY;

	public static int ANNIVERSARY_CURRENCY;

	public static int SPONGY_GAMES_CURRENCY;

	public static int BONES_CURRENCY;

	public static int SPECIAL_CURRENCY;

	public static int LEVEL;

	public static int XP;

	public static int DEFAULT_JJ;

	public const string EMPTY_WISH_TEXTURE = "empty.png";

	public static int VALENTINES_2015_CURRENCY;

	public const string UPDATE_RESOURCE = "UpdateResource";

	public const int RESOURCE_TYPE_HOLIDAY_CHEER = 9100;

	private Dictionary<int, Resource> resources;

	private static Dictionary<int, Resource> internal_resources;

	private HashSet<int> consumableResources;

	public List<string> resourceCategoryOrder;

	private Session session;

	public Dictionary<string, ResourceCategory> resourceCategories;

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
			return null;
		}
	}

	public int PlayerLevelAmount
	{
		get
		{
			return 0;
		}
	}

	public ResourceManager(Session session)
	{
	}

	public static string TypeDescription(int typeID)
	{
		return null;
	}

	public static void ApplyCostToGameState(Cost cost, Dictionary<string, object> gameState)
	{
	}

	public static void ApplyCostToGameState(int resourceId, int amount, Dictionary<string, object> gameState)
	{
	}

	public static void AddAmountToGameState(int resourceId, int amount, Dictionary<string, object> gameState)
	{
	}

	public static void ApplyPurchasesToGameState(Cost cost, Dictionary<string, object> gameState)
	{
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private Dictionary<int, Resource> LoadResourceDefinitions()
	{
		return null;
	}

	public void LoadResources(List<object> resources)
	{
	}

	public void UpdateLevelExpToMilestone(LevelingManager manager)
	{
	}

	public bool CanPay(Cost cost)
	{
		return false;
	}

	public void Apply(Cost cost, Game game)
	{
	}

	public void SellFor(Cost cost, Game game)
	{
	}

	public bool HasEnough(int resourceId, int minimumAmount)
	{
		return false;
	}

	public int Query(int resourceId)
	{
		return 0;
	}

	public float QueryProgressPercentage(IResourceProgressCalculator calculator)
	{
		return 0f;
	}

	public string QueryProgressFraction(IResourceProgressCalculator calculator)
	{
		return null;
	}

	public void Spend(Cost cost, Game game)
	{
	}

	public void Spend(int resourceId, int amount, Game game)
	{
	}

	public void Add(int resourceId, int amount, Game game)
	{
	}

	public Dictionary<string, object>[] ToDict()
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}

	public List<int> ConsumableProducts(CraftingManager craftManager)
	{
		return null;
	}

	public void PurchaseResourcesWithHardCurrency(int hcCost, Cost resources, Game game)
	{
	}

	public void SetPurchasedResources(Cost resources)
	{
	}

	public int GetResourcesPackageCostInHardCurrencyValue(Cost resourcesNeeded)
	{
		return 0;
	}

	public static Cost CalculateCraftRushCost(ulong recipeTime)
	{
		return null;
	}

	public static Cost CalculateRentRushCost(ulong rentTime)
	{
		return null;
	}

	public static Cost CalculateFullnessRushCost(ulong fullnessTime)
	{
		return null;
	}

	public static Cost CalculateDebrisRushCost(ulong timeLeft)
	{
		return null;
	}

	public static Cost CalculateConstructionRushCost(ulong timeLeft)
	{
		return null;
	}

	public static Cost CalculateTaskRushCost(ulong timeLeft)
	{
		return null;
	}

	private static Cost CalculateTimeToJjCost(ulong time, double timeToJjFactor, double timeCompressionBase)
	{
		return null;
	}

	private static int CompressTimeCost(double originalCost, double compressionBase)
	{
		return 0;
	}

	public int GetNumDisplayableResources()
	{
		return 0;
	}

	private ITrigger CreateModifyResourceTrigger(int resourceId, int amount)
	{
		return null;
	}

	public List<int> SortRecipesByProductGroup(CraftingManager craftManager, List<int> unsortedList)
	{
		return null;
	}

	public void UpdateProductGroups(CraftingManager craftManager)
	{
	}

	private Dictionary<string, object> GetConversionDataFromSpread()
	{
		return null;
	}

	private Dictionary<string, object> GetCategoryOrderDataFromSpread()
	{
		return null;
	}

	private List<object> GetResourceDictsFromSpread()
	{
		return null;
	}
}
