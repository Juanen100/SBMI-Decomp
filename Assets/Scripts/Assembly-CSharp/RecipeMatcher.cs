using System.Collections.Generic;

public class RecipeMatcher : Matcher
{
	public const string RECIPE_ID = "recipe_id";

	public static RecipeMatcher FromDict(Dictionary<string, object> dict)
	{
		RecipeMatcher recipeMatcher = new RecipeMatcher();
		recipeMatcher.RegisterProperty("recipe_id", dict, recipeMatcher.RecipeIdMatchFn);
		return recipeMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		if (game != null && game.craftManager != null)
		{
			return game.craftManager.Recipes[int.Parse(GetTarget("recipe_id"))].recipeName;
		}
		return string.Empty;
	}

	public uint RecipeIdMatchFn(MatchableProperty idProperty, Dictionary<string, object> triggerData, Game game)
	{
		int num = int.Parse(idProperty.Target.ToString());
		if (triggerData.ContainsKey("recipes"))
		{
			List<object> list = (List<object>)triggerData["recipes"];
			if (list.Contains(num))
			{
				return 1u;
			}
		}
		return 0u;
	}
}
