#define ASSERTS_ON
using System.Collections.Generic;

public class CraftingManager
{
	public const bool DEBUG_LOG_CRAFTING = false;

	private const string _sRECIPES = "Recipes";

	private const string _sCOOKBOOKS = "CraftBuildings";

	private const string _sPRODUCTION_SLOTS = "ProductionSlots";

	public const string CRAFTING_SLOT = "slot_id";

	public const int INVALID_SLOT = -1;

	public EventDispatcher UnlockedEvent = new EventDispatcher();

	private static readonly string CRAFTING_PATH = "Crafting";

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
			return recipes;
		}
	}

	public HashSet<int> UnlockedRecipesCopy
	{
		get
		{
			return new HashSet<int>(unlockedRecipes);
		}
	}

	public HashSet<int> UnlockedProductsShallowCopy
	{
		get
		{
			return new HashSet<int>(unlockedProductsShallow);
		}
	}

	public HashSet<int> UnlockedProductsDeepCopy
	{
		get
		{
			return new HashSet<int>(unlockedProductsDeep);
		}
	}

	public HashSet<int> ReservedRecipesCopy
	{
		get
		{
			return new HashSet<int>(reservedRecipes);
		}
	}

	public HashSet<int> JellyBasedRecipesCopy
	{
		get
		{
			return new HashSet<int>(jellyBasedRecipes);
		}
	}

	public HashSet<int> IgnoreRandomQuestRecipesCopy
	{
		get
		{
			return new HashSet<int>(ignoreRandomQuestRecipes);
		}
	}

	public CraftingManager()
	{
		cookbooks = new Dictionary<int, CraftingCookbook>();
		recipes = new Dictionary<int, CraftingRecipe>();
		activeCrafts = new Dictionary<Identity, Dictionary<int, CraftingInstance>>();
		prodSlotTables = new Dictionary<int, ProductionSlotTable>();
		unlockedRecipes = new HashSet<int>();
		unlockedProductsShallow = new HashSet<int>();
		unlockedProductsDeep = new HashSet<int>();
		reservedRecipes = new HashSet<int>();
		jellyBasedRecipes = new HashSet<int>();
		ignoreRandomQuestRecipes = new HashSet<int>();
		LoadCrafting();
	}

	public CraftingCookbook GetCookbookById(int id)
	{
		TFUtils.Assert(cookbooks.ContainsKey(id), "Crafting Manager does not have a cookbook with id = " + id);
		return cookbooks[id];
	}

	public bool ContainsRecipe(int id)
	{
		return recipes.ContainsKey(id);
	}

	public CraftingRecipe GetRecipeById(int id)
	{
		TFUtils.Assert(recipes.ContainsKey(id), "Crafting Manager does not have a recipe with id = " + id);
		return recipes[id];
	}

	public CraftingRecipe GetRecipeByProductId(int productId)
	{
		foreach (KeyValuePair<int, CraftingRecipe> recipe in recipes)
		{
			if (recipe.Value.productId == productId)
			{
				return recipe.Value;
			}
		}
		TFUtils.ErrorLog("Crafting Manager does not have a recipe with product_id = " + productId);
		return null;
	}

	public void UnlockRecipe(int id, Game game)
	{
		if (!recipes.ContainsKey(id))
		{
			TFUtils.ErrorLog("Cannot unlock recipe with id=" + id + " since it was not loaded [properly]");
			return;
		}
		int productId = recipes[id].productId;
		unlockedRecipes.Add(id);
		foreach (KeyValuePair<int, int> resourceAmount in recipes[id].cost.ResourceAmounts)
		{
			if (resourceAmount.Key == ResourceManager.HARD_CURRENCY)
			{
				jellyBasedRecipes.Add(productId);
			}
		}
		if (recipes[id].ignoreRandomRecipeQuest)
		{
			ignoreRandomQuestRecipes.Add(productId);
		}
		if (!unlockedProductsShallow.Contains(productId))
		{
			unlockedProductsShallow.Add(productId);
		}
		foreach (KeyValuePair<int, CraftingRecipe> recipe in recipes)
		{
			if (!unlockedProductsShallow.Contains(recipe.Key) || unlockedProductsDeep.Contains(recipe.Key))
			{
				continue;
			}
			bool flag = true;
			foreach (KeyValuePair<int, int> resourceAmount2 in recipes[recipe.Key].cost.ResourceAmounts)
			{
				if (!unlockedProductsShallow.Contains(resourceAmount2.Key) && resourceAmount2.Key != ResourceManager.HARD_CURRENCY && resourceAmount2.Key != ResourceManager.SOFT_CURRENCY)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				unlockedProductsDeep.Add(recipe.Key);
			}
		}
		game.triggerRouter.RouteTrigger(UnlockableTrigger.CreateTrigger("recipe", id), game);
		UnlockedEvent.FireEvent();
	}

	public bool LockRecipe(int id)
	{
		if (!recipes.ContainsKey(id))
		{
			TFUtils.ErrorLog("Cannot unlock recipe with id=" + id + " since it was not loaded [properly]");
			return false;
		}
		int productId = recipes[id].productId;
		unlockedRecipes.Remove(id);
		jellyBasedRecipes.Remove(productId);
		ignoreRandomQuestRecipes.Remove(productId);
		unlockedProductsShallow.Remove(productId);
		unlockedProductsDeep.Remove(id);
		unlockedProductsDeep.Remove(productId);
		return true;
	}

	public void UnlockAllRecipes(Game game)
	{
		foreach (KeyValuePair<int, CraftingRecipe> recipe in recipes)
		{
			int key = recipe.Key;
			if (!unlockedRecipes.Contains(key))
			{
				UnlockRecipe(key, game);
			}
		}
	}

	public void UnlockAllRecipesToGamestate(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("recipes"))
		{
			dictionary["recipes"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["recipes"];
		foreach (KeyValuePair<int, CraftingRecipe> recipe in recipes)
		{
			int key = recipe.Key;
			if (!list.Contains(key))
			{
				list.Add(key);
			}
		}
	}

	public bool CanMakeRecipe(int id)
	{
		foreach (KeyValuePair<int, int> resourceAmount in recipes[id].cost.ResourceAmounts)
		{
			if (!unlockedProductsShallow.Contains(resourceAmount.Key) && resourceAmount.Key != ResourceManager.HARD_CURRENCY && resourceAmount.Key != ResourceManager.SOFT_CURRENCY)
			{
				return false;
			}
		}
		return true;
	}

	public void ReserveRecipe(int recipeId)
	{
		reservedRecipes.Add(recipeId);
	}

	public int GetNumUnlockedComplexRecipes()
	{
		int num = 0;
		foreach (int unlockedRecipe in unlockedRecipes)
		{
			CraftingRecipe recipeById = GetRecipeById(unlockedRecipe);
			if (IsComplexRecipe(recipeById))
			{
				num++;
			}
		}
		return num;
	}

	public bool IsComplexRecipe(CraftingRecipe recipe)
	{
		if (recipe.cost.ResourceAmounts.Count == 0 || recipe.ignoreRecipeCap || (recipe.cost.ResourceAmounts.Count == 1 && recipe.cost.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY)))
		{
			return false;
		}
		return true;
	}

	public bool IsRecipeUnlocked(int id)
	{
		return unlockedRecipes.Contains(id) || reservedRecipes.Contains(id);
	}

	public int GetNextSlot(Identity id, int maxSlot)
	{
		if (maxSlot == -1)
		{
			return -1;
		}
		if (activeCrafts.ContainsKey(id))
		{
			for (int i = 0; i < maxSlot; i++)
			{
				if (!activeCrafts[id].ContainsKey(i))
				{
					return i;
				}
			}
			return -1;
		}
		return 0;
	}

	public bool AddCraftingInstance(CraftingInstance instance)
	{
		if (!activeCrafts.ContainsKey(instance.buildingLabel))
		{
			activeCrafts[instance.buildingLabel] = new Dictionary<int, CraftingInstance>();
		}
		if (activeCrafts[instance.buildingLabel].ContainsKey(instance.slotId))
		{
			TFUtils.ErrorLog(string.Format("already have a crafting instance {0} for building {1}", activeCrafts[instance.buildingLabel][instance.slotId], instance.buildingLabel));
			return false;
		}
		activeCrafts[instance.buildingLabel].Add(instance.slotId, instance);
		return true;
	}

	public CraftingInstance GetCraftingInstance(Identity id, int slot)
	{
		if (activeCrafts.ContainsKey(id) && activeCrafts[id].ContainsKey(slot))
		{
			return activeCrafts[id][slot];
		}
		return null;
	}

	public void RemoveCraftingInstance(Identity id, int slot)
	{
		TFUtils.Assert(activeCrafts.ContainsKey(id) && activeCrafts[id][slot] != null, "Trying to remove an instance which does not exist for id: " + id.Describe());
		activeCrafts[id].Remove(slot);
	}

	public bool Crafting(Identity id)
	{
		if (activeCrafts.ContainsKey(id))
		{
			return activeCrafts[id].Count > 0;
		}
		return false;
	}

	public bool HasCapacity(Identity id, int maxSlots)
	{
		if (activeCrafts.ContainsKey(id))
		{
			return activeCrafts[id].Count < maxSlots;
		}
		return true;
	}

	public bool HasInitialSlots(int did)
	{
		return prodSlotTables.ContainsKey(did);
	}

	public int GetInitialSlots(int did)
	{
		if (prodSlotTables.ContainsKey(did))
		{
			return prodSlotTables[did].MinSlots;
		}
		return -1;
	}

	public int GetMaxSlots(int did)
	{
		if (prodSlotTables.ContainsKey(did))
		{
			return prodSlotTables[did].MaxSlots;
		}
		return -1;
	}

	public Cost GetSlotExpandCost(int did, int slotId)
	{
		if (prodSlotTables.ContainsKey(did))
		{
			return prodSlotTables[did].GetCostForSlot(slotId);
		}
		return null;
	}

	private string[] GetFilesToLoad()
	{
		return Config.CRAFTING_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private void LoadCrafting()
	{
		LoadRecipesFromSpreadseet("Recipes");
		LoadCookbooksFromSpreadseet("CraftBuildings");
		LoadProductionSlotsFromSpreadseet("ProductionSlots");
	}

	private void LoadRecipesFromSpreadseet(string sSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(sSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sSheetName);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + sSheetName);
			return;
		}
		int num = instance.GetNumRows(sSheetName);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + sSheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary6 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary8 = new Dictionary<string, object>();
		List<object> list = new List<object>();
		string text = "n/a";
		int num2 = 6;
		int num3 = 3;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			dictionary.Clear();
			dictionary2.Clear();
			dictionary3.Clear();
			dictionary4.Clear();
			dictionary5.Clear();
			dictionary6.Clear();
			list.Clear();
			dictionary8.Clear();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "product id");
			dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
			dictionary.Add("product_id", intCell);
			dictionary.Add("building_id", instance.GetIntCell(sheetIndex, rowIndex, "building id"));
			dictionary.Add("craft.time", instance.GetIntCell(sheetIndex, rowIndex, "craft time"));
			dictionary.Add("minimum_level", instance.GetIntCell(sheetIndex, rowIndex, "minimum level"));
			dictionary.Add("group_order", instance.GetIntCell(sheetIndex, rowIndex, "group order"));
			dictionary.Add("ignore_recipe_cap", instance.GetIntCell(sheetIndex, rowIndex, "ignore recipe cap") == 1);
			dictionary.Add("ignore_random_recipe_quest", instance.GetIntCell(sheetIndex, rowIndex, "ignore random recipe quest") == 1);
			dictionary.Add("type", "recipe");
			string stringCell = instance.GetStringCell(sSheetName, rowName, "name");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("name", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "tag");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("tag", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "craft  description");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("craft_description", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "start sound");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("start_sound", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "ready sound");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("ready_sound", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "product group");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("product_group", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "garden display started");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary2.Add("started", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "garden display half");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary2.Add("halfdone", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "garden display done");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary2.Add("done", stringCell);
			}
			if (dictionary2.Count > 0)
			{
				dictionary.Add("development_displaystates", dictionary2);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "touch sound");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("sound_on_touch", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "start sound beat");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("start_sound_beat", stringCell);
			}
			stringCell = instance.GetStringCell(sSheetName, rowName, "ready sound beat");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary.Add("ready_sound_beat", stringCell);
			}
			bool flag = false;
			for (int j = 0; j < num3; j++)
			{
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "coral bits amount " + (j + 1));
				if (intCell2 != -1)
				{
					dictionary4.Add(intCell2.ToString(), instance.GetFloatCell(sheetIndex, rowIndex, "coral bits odds " + (j + 1)));
					flag = true;
				}
			}
			if (dictionary4.Count > 0)
			{
				dictionary6.Add("2001", dictionary4);
			}
			dictionary6.Add("5", instance.GetIntCell(sheetIndex, rowIndex, "xp reward"));
			dictionary6.Add(intCell.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "amount crafted"));
			if (!flag)
			{
				dictionary5.Add("resources", dictionary6);
			}
			else
			{
				list.Clear();
				dictionary8.Clear();
				dictionary7.Clear();
				dictionary7.Add("resources", dictionary6);
				dictionary8.Add("p", 1f);
				dictionary8.Add("value", dictionary7);
				list.Add(dictionary8);
				dictionary5.Add("cdf", list);
			}
			dictionary.Add("reward", dictionary5);
			for (int k = 0; k < num2; k++)
			{
				int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "ingredient id " + (k + 1));
				if (intCell3 != -1)
				{
					dictionary3.Add(intCell3.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "ingredient amount " + (k + 1)));
				}
			}
			dictionary.Add("ingredients", dictionary3);
			CraftingRecipe craftingRecipe = new CraftingRecipe(dictionary);
			int identity = craftingRecipe.identity;
			if (recipes.ContainsKey(identity))
			{
				TFUtils.ErrorLog(string.Concat("Recipe Collision!\nOld=", recipes[identity], "\nNew=", craftingRecipe));
			}
			else
			{
				recipes.Add(identity, craftingRecipe);
			}
		}
	}

	private void LoadCookbooksFromSpreadseet(string sSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(sSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sSheetName);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + sSheetName);
			return;
		}
		int num = instance.GetNumRows(sSheetName);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + sSheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		List<object> list2 = new List<object>();
		int num2 = -1;
		string text = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			dictionary.Clear();
			list.Clear();
			list2.Clear();
			int intCell = instance.GetIntCell(sSheetName, rowName, "id");
			dictionary.Add("id", intCell);
			intCell = instance.GetRowIndex(sheetIndex, intCell.ToString());
			dictionary.Add("type", "cookbook");
			list.Add(instance.GetIntCell(sheetIndex, intCell, "background color r"));
			list.Add(instance.GetIntCell(sheetIndex, intCell, "background color g"));
			list.Add(instance.GetIntCell(sheetIndex, intCell, "background color b"));
			dictionary.Add("background.color", list);
			dictionary.Add("session_action_id", instance.GetStringCell(sSheetName, rowName, "session action id"));
			dictionary.Add("texture.cancelbutton", instance.GetStringCell(sSheetName, rowName, "cancel button texture"));
			dictionary.Add("texture.title", instance.GetStringCell(sSheetName, rowName, "title texture"));
			dictionary.Add("texture.slot", instance.GetStringCell(sSheetName, rowName, "slot texture"));
			dictionary.Add("texture.titleicon", instance.GetStringCell(sSheetName, rowName, "title icon texture"));
			dictionary.Add("button.label", instance.GetStringCell(sSheetName, rowName, "button label"));
			dictionary.Add("open_sound", instance.GetStringCell(sSheetName, rowName, "open sound"));
			dictionary.Add("close_sound", instance.GetStringCell(sSheetName, rowName, "close sound"));
			string text2 = instance.GetStringCell(sSheetName, rowName, "button icon");
			if (string.IsNullOrEmpty(text2) || text2 == text)
			{
				text2 = null;
			}
			dictionary.Add("button.icon", text2);
			text2 = instance.GetStringCell(sSheetName, rowName, "music");
			if (string.IsNullOrEmpty(text2) || text2 == text)
			{
				text2 = null;
			}
			dictionary.Add("music", text2);
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, intCell, "recipe columns");
			}
			for (int j = 0; j < num2; j++)
			{
				int intCell2 = instance.GetIntCell(sheetIndex, intCell, "Recipe " + (j + 1));
				if (intCell2 >= 0)
				{
					list2.Add(intCell2);
				}
			}
			dictionary.Add("recipes", list2);
			CraftingCookbook craftingCookbook = new CraftingCookbook(dictionary);
			int identity = craftingCookbook.identity;
			if (cookbooks.ContainsKey(identity))
			{
				TFUtils.ErrorLog(string.Concat("Cookbook Collision!\nOld=", cookbooks[identity], "\nNew=", craftingCookbook));
			}
			else
			{
				cookbooks.Add(craftingCookbook.identity, craftingCookbook);
			}
		}
	}

	private void LoadProductionSlotsFromSpreadseet(string sSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(sSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sSheetName);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + sSheetName);
			return;
		}
		int num = instance.GetNumRows(sSheetName);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + sSheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			dictionary.Clear();
			list.Clear();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
			dictionary.Add("type", "slot_costs");
			dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "building did"));
			dictionary.Add("init_slots", instance.GetIntCell(sheetIndex, rowIndex, "starting slots"));
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "total slots");
			}
			for (int j = 0; j < num2; j++)
			{
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "currency type " + (j + 1));
				if (intCell >= 0)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>(1);
					dictionary2.Add(intCell.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "slot cost " + (j + 1)));
					list.Add(dictionary2);
				}
			}
			dictionary.Add("costs", list);
			ProductionSlotTable productionSlotTable = new ProductionSlotTable(dictionary);
			if (prodSlotTables.ContainsKey(productionSlotTable.Did))
			{
				TFUtils.ErrorLog(string.Concat("Crafting Production Slot Table Collision!\nOld=", prodSlotTables[productionSlotTable.Did], "\nNew=", productionSlotTable));
			}
			else
			{
				prodSlotTables.Add(productionSlotTable.Did, productionSlotTable);
			}
		}
	}
}
