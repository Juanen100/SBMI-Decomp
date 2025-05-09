#define ASSERTS_ON
using System.Collections.Generic;

public class UnlockableMatcher : Matcher
{
	public const string UNLOCKABLE_TYPE = "unlockable_type";

	public const string UNLOCKABLE_ID = "unlockable_id";

	public static UnlockableMatcher FromDict(Dictionary<string, object> dict)
	{
		UnlockableMatcher unlockableMatcher = new UnlockableMatcher();
		unlockableMatcher.RegisterProperty("unlockable_type", dict);
		unlockableMatcher.RegisterProperty("unlockable_id", dict);
		TFUtils.Assert(unlockableMatcher.IsRequired("unlockable_type"), "You must specify an unlockable type");
		return unlockableMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		switch (GetTarget("unlockable_type"))
		{
		case "recipe":
			return "!!COND_SECRET_RECIPE";
		default:
			TFUtils.ErrorLog("Trying to describe an unkown unlockable type! type=" + GetTarget("unlockable_type"));
			return string.Empty;
		}
	}

	public override uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		if (GetTarget("unlockable_type") == "recipe")
		{
			int result = -1;
			if (int.TryParse(GetProperty("unlockable_id").Target.ToString(), out result) && game.craftManager.IsRecipeUnlocked(result))
			{
				return 1u;
			}
		}
		return base.MatchAmount(game, data);
	}
}
