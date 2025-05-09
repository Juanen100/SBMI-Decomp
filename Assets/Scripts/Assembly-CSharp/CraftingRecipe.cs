using System;
using System.Collections.Generic;

public class CraftingRecipe
{
	public const string TYPE = "recipe";

	public int identity;

	public int productId;

	public string craftDescription;

	public string recipeName;

	public string recipeTag;

	public string recipeSubType;

	public RewardDefinition rewardDefinition;

	public Cost cost;

	public Cost rushCost;

	public ulong craftTime;

	public int minimumLevel;

	public string displayedCraftTime;

	public string startSoundImmediate;

	public string startSoundBeat;

	public string readySoundImmediate;

	public string readySoundBeat;

	public float beatLength = 1f;

	public int buildingId;

	public int height;

	public int width;

	public string productGroup;

	public int groupOrder;

	public bool ignoreRandomRecipeQuest;

	public bool ignoreRecipeCap;

	public Dictionary<string, string> developmentDisplayStates;

	public CraftingRecipe(Dictionary<string, object> data)
	{
		recipeName = TFUtils.LoadString(data, "name");
		craftDescription = TFUtils.LoadString(data, "craft_description");
		identity = TFUtils.LoadInt(data, "did");
		productId = TFUtils.LoadInt(data, "product_id");
		buildingId = TFUtils.LoadInt(data, "building_id");
		recipeTag = TFUtils.TryLoadString(data, "tag");
		if (recipeTag == null)
		{
			TFUtils.WarningLog("No recipe tag! Using default.");
			recipeTag = string.Format("recipe_{0}", identity);
		}
		Dictionary<string, object> dict = (Dictionary<string, object>)data["reward"];
		rewardDefinition = RewardDefinition.FromDict(dict);
		Dictionary<string, object> dict2 = (Dictionary<string, object>)data["ingredients"];
		cost = Cost.FromDict(dict2);
		minimumLevel = TFUtils.LoadInt(data, "minimum_level");
		craftTime = TFUtils.LoadUlong(data, "craft.time");
		displayedCraftTime = TFUtils.DurationToString(craftTime);
		productGroup = TFUtils.LoadString(data, "product_group");
		groupOrder = TFUtils.LoadInt(data, "group_order");
		rushCost = ResourceManager.CalculateCraftRushCost(craftTime);
		startSoundImmediate = TFUtils.LoadString(data, "start_sound");
		startSoundBeat = TFUtils.TryLoadString(data, "start_sound_beat");
		readySoundImmediate = TFUtils.LoadString(data, "ready_sound");
		readySoundBeat = TFUtils.TryLoadString(data, "ready_sound_beat");
		recipeSubType = "!!RECIPE";
		if (data.ContainsKey("subtype"))
		{
			recipeSubType = TFUtils.LoadString(data, "subtype");
		}
		if (data.ContainsKey("display"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["display"];
			width = Convert.ToInt32(dictionary["width"]);
			height = Convert.ToInt32(dictionary["height"]);
		}
		developmentDisplayStates = new Dictionary<string, string>();
		if (data.ContainsKey("development_displaystates"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)data["development_displaystates"];
			foreach (string key in dictionary2.Keys)
			{
				developmentDisplayStates.Add(key, (string)dictionary2[key]);
			}
		}
		bool? flag = TFUtils.TryLoadBool(data, "ignore_recipe_cap");
		if (flag.HasValue)
		{
			ignoreRecipeCap = flag.Value;
		}
		else
		{
			ignoreRecipeCap = false;
		}
		flag = TFUtils.TryLoadBool(data, "ignore_random_recipe_quest");
		if (flag.HasValue)
		{
			ignoreRandomRecipeQuest = flag.Value;
		}
		else
		{
			ignoreRandomRecipeQuest = false;
		}
	}

	public override string ToString()
	{
		return "[CraftingRecipe (productId=" + productId + ", receipName=" + recipeName + ")]";
	}
}
