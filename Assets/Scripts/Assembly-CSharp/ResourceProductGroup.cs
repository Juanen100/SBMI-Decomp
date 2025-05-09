using System.Collections.Generic;

public class ResourceProductGroup
{
	public string name;

	public List<int> recipeDids = new List<int>();

	public void AddRecipe(CraftingManager craftingManager, CraftingRecipe recipe)
	{
		int num = recipeDids.Count;
		for (int i = 0; i < recipeDids.Count; i++)
		{
			CraftingRecipe recipeById = craftingManager.GetRecipeById(recipeDids[i]);
			if (recipe.groupOrder > recipeById.groupOrder)
			{
				num = i;
				break;
			}
		}
		if (num == recipeDids.Count)
		{
			recipeDids.Add(recipe.identity);
		}
		else
		{
			recipeDids.Insert(num, recipe.identity);
		}
	}
}
