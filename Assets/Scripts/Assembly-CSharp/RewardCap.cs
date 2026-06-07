using System.Collections.Generic;

public class RewardCap
{
	public const string REWARD_CAP_FIELD = "caps";

	public const string RECIPE_COUNT = "recipe_count";

	public const string JELLY_COUNT = "jelly_count";

	public const string EXPIRATION = "expiration";

	public const int CONSOLATION_SOFT_CURRENCY_AMOUNT = 25;

	private ulong expiration;

	private int recipes;

	private int jelly;

	private const int JELLY_CAP = 1000;

	private const int RECIPE_CAP = 5;

	private const int PERIOD = 86400;

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public bool Filter(Simulation simulation, ref Reward reward)
	{
		return false;
	}

	public void Reset(int jelly, int recipes, ulong expiration)
	{
	}

	private void Clear()
	{
	}

	private bool FilterRecipes(Simulation simulation, Reward reward, out int complexRecipes)
	{
		complexRecipes = default(int);
		return false;
	}
}
