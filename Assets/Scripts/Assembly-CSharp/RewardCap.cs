using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardCap
{
	public const string REWARD_CAP_FIELD = "caps";

	public const string RECIPE_COUNT = "recipe_count";

	public const string JELLY_COUNT = "jelly_count";

	public const string EXPIRATION = "expiration";

	public const int CONSOLATION_SOFT_CURRENCY_AMOUNT = 25;

	private const int JELLY_CAP = 1000;

	private const int RECIPE_CAP = 5;

	private const int PERIOD = 86400;

	private ulong expiration;

	private int recipes;

	private int jelly;

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["recipe_count"] = recipes;
		dictionary["jelly_count"] = jelly;
		dictionary["expiration"] = expiration;
		return dictionary;
	}

	public bool Filter(Simulation simulation, ref Reward reward)
	{
		bool result = false;
		bool flag = false;
		if (TFUtils.EpochTime() >= expiration)
		{
			Clear();
		}
		if (reward.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
		{
			int num = reward.ResourceAmounts[ResourceManager.HARD_CURRENCY];
			if (jelly + num > 1000)
			{
				num = 1000 - jelly;
				if (num > 0)
				{
					reward.ResourceAmounts[ResourceManager.HARD_CURRENCY] = 1000 - num;
				}
				else
				{
					reward.ResourceAmounts.Remove(ResourceManager.HARD_CURRENCY);
				}
				Debug.LogWarning("Hit the JJ Cap - think about increasing the cap!");
				result = true;
			}
			flag = true;
			jelly += num;
		}
		if (reward.RecipeUnlocks != null)
		{
			int complexRecipes = 0;
			if (FilterRecipes(simulation, reward, out complexRecipes))
			{
				Debug.LogWarning("Hit the Recipe Cap - think about increasing the cap!");
				recipes = 5;
				result = true;
			}
			else
			{
				recipes += complexRecipes;
			}
			if (complexRecipes > 0)
			{
				flag = true;
			}
		}
		if (flag)
		{
			simulation.game.Record(new RewardCapAction(jelly, recipes, expiration));
		}
		return result;
	}

	public void Reset(int jelly, int recipes, ulong expiration)
	{
		this.jelly = jelly;
		this.recipes = recipes;
		this.expiration = expiration;
	}

	private void Clear()
	{
		jelly = 0;
		recipes = 0;
		expiration = TFUtils.EpochTime() + 86400;
	}

	private bool FilterRecipes(Simulation simulation, Reward reward, out int complexRecipes)
	{
		int count = reward.RecipeUnlocks.Count;
		int i = 0;
		Predicate<int> match = (int recipeId) => !simulation.craftManager.IsComplexRecipe(simulation.craftManager.GetRecipeById(recipeId)) || i++ < 5 - recipes;
		reward.RecipeUnlocks = reward.RecipeUnlocks.FindAll(match);
		complexRecipes = i;
		return count != reward.RecipeUnlocks.Count;
	}
}
