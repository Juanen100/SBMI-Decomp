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

	public float beatLength;

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
	}

	public override string ToString()
	{
		return null;
	}
}
