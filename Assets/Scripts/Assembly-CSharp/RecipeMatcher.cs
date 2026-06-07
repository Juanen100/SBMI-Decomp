using System.Collections.Generic;

public class RecipeMatcher : Matcher
{
	public const string RECIPE_ID = "recipe_id";

	public static RecipeMatcher FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string DescribeSubject(Game game)
	{
		return null;
	}

	public uint RecipeIdMatchFn(MatchableProperty idProperty, Dictionary<string, object> triggerData, Game game)
	{
		return 0u;
	}
}
