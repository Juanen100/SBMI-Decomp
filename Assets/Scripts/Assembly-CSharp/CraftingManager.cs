using System.Collections.Generic;

public class CraftingManager
{
	public const bool DEBUG_LOG_CRAFTING = false;

	private const string _sRECIPES = "Recipes";

	private const string _sCOOKBOOKS = "CraftBuildings";

	private const string _sPRODUCTION_SLOTS = "ProductionSlots";

	public const string CRAFTING_SLOT = "slot_id";

	public const int INVALID_SLOT = -1;

	public EventDispatcher UnlockedEvent;

	private static readonly string CRAFTING_PATH;

	private Dictionary<int, CraftingCookbook> cookbooks;

	private Dictionary<int, CraftingRecipe> recipes;

	private Dictionary<int, ProductionSlotTable> prodSlotTables;

	private HashSet<int> unlockedRecipes;

	private HashSet<int> unlockedProductsShallow;

	private HashSet<int> unlockedProductsDeep;

	private HashSet<int> reservedRecipes;

	private HashSet<int> jellyBasedRecipes;

	private HashSet<int> ignoreRandomQuestRecipes;

	private Dictionary<Identity, Dictionary<int, CraftingInstance>> activeCrafts;

	public Dictionary<int, CraftingRecipe> Recipes
	{
		get
		{
			return null;
		}
	}

	public HashSet<int> UnlockedRecipesCopy
	{
		get
		{
			return null;
		}
	}

	public HashSet<int> UnlockedProductsShallowCopy
	{
		get
		{
			return null;
		}
	}

	public HashSet<int> UnlockedProductsDeepCopy
	{
		get
		{
			return null;
		}
	}

	public HashSet<int> ReservedRecipesCopy
	{
		get
		{
			return null;
		}
	}

	public HashSet<int> JellyBasedRecipesCopy
	{
		get
		{
			return null;
		}
	}

	public HashSet<int> IgnoreRandomQuestRecipesCopy
	{
		get
		{
			return null;
		}
	}

	public CraftingCookbook GetCookbookById(int id)
	{
		return null;
	}

	public bool ContainsRecipe(int id)
	{
		return false;
	}

	public CraftingRecipe GetRecipeById(int id)
	{
		return null;
	}

	public CraftingRecipe GetRecipeByProductId(int productId)
	{
		return null;
	}

	public void UnlockRecipe(int id, Game game)
	{
	}

	public bool LockRecipe(int id)
	{
		return false;
	}

	public void UnlockAllRecipes(Game game)
	{
	}

	public void UnlockAllRecipesToGamestate(Dictionary<string, object> gameState)
	{
	}

	public bool CanMakeRecipe(int id)
	{
		return false;
	}

	public void ReserveRecipe(int recipeId)
	{
	}

	public int GetNumUnlockedComplexRecipes()
	{
		return 0;
	}

	public bool IsComplexRecipe(CraftingRecipe recipe)
	{
		return false;
	}

	public bool IsRecipeUnlocked(int id)
	{
		return false;
	}

	public int GetNextSlot(Identity id, int maxSlot)
	{
		return 0;
	}

	public bool AddCraftingInstance(CraftingInstance instance)
	{
		return false;
	}

	public CraftingInstance GetCraftingInstance(Identity id, int slot)
	{
		return null;
	}

	public void RemoveCraftingInstance(Identity id, int slot)
	{
	}

	public bool Crafting(Identity id)
	{
		return false;
	}

	public bool HasCapacity(Identity id, int maxSlots)
	{
		return false;
	}

	public bool HasInitialSlots(int did)
	{
		return false;
	}

	public int GetInitialSlots(int did)
	{
		return 0;
	}

	public int GetMaxSlots(int did)
	{
		return 0;
	}

	public Cost GetSlotExpandCost(int did, int slotId)
	{
		return null;
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	private void LoadCrafting()
	{
	}

	private void LoadRecipesFromSpreadseet(string sSheetName)
	{
	}

	private void LoadCookbooksFromSpreadseet(string sSheetName)
	{
	}

	private void LoadProductionSlotsFromSpreadseet(string sSheetName)
	{
	}
}
