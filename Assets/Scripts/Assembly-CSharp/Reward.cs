using System.Collections.Generic;
using UnityEngine;

public class Reward
{
	public const string THOUGHT_ICON = "thought_icon";

	public const string RECIPES = "recipes";

	public const string BUILDINGS = "buildings";

	public const string COSTUMES = "costumes";

	public const string RANDOM_LAND = "random_land";

	public const string BUILDING_UNLOCKS = "building_unlocks";

	public const string MOVIES = "movies";

	private Dictionary<int, int> resourceAmounts;

	private Dictionary<int, int> buildingAmounts;

	private Dictionary<int, Vector2> buildingPositions;

	private List<int> recipesAwarded;

	private List<int> moviesAwarded;

	private List<int> costumesAwarded;

	private List<int> clearedLandsAwarded;

	private List<int> buildingUnlocksAwarded;

	private Dictionary<string, object> buildingLabels;

	private string rewardThoughtIcon;

	private bool rewardRandomLand;

	public Dictionary<int, int> ResourceAmounts
	{
		get
		{
			return null;
		}
	}

	public Dictionary<int, int> BuildingAmounts
	{
		get
		{
			return null;
		}
	}

	public Dictionary<int, Vector2> BuildingPositions
	{
		get
		{
			return null;
		}
	}

	public List<int> RecipeUnlocks
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public List<int> MovieUnlocks
	{
		get
		{
			return null;
		}
	}

	public List<int> CostumeUnlocks
	{
		get
		{
			return null;
		}
	}

	public List<int> ClearedLands
	{
		get
		{
			return null;
		}
	}

	public List<int> BuildingUnlocks
	{
		get
		{
			return null;
		}
	}

	public string ThoughtIcon
	{
		get
		{
			return null;
		}
	}

	public bool RandomLand
	{
		get
		{
			return false;
		}
	}

	public Dictionary<string, object> BuildingLabels
	{
		get
		{
			return null;
		}
	}

	public Reward(Dictionary<int, int> resourceAmounts, Dictionary<int, int> buildingAmounts, Dictionary<int, Vector2> buildingPositions, List<int> recipesAwarded, List<int> moviesAwarded, List<int> costumesAwarded, List<int> clearedLandsAwarded, List<int> buildingUnlocksAwarded, bool randomLand, string rewardThoughtIcon)
	{
	}

	public Reward(Dictionary<int, int> resourceAmounts, Dictionary<int, int> buildingAmounts, Dictionary<int, Vector2> buildingPositions, List<int> recipesAwarded, List<int> moviesAwarded, List<int> costumesAwarded, List<int> clearedLandsAwarded, List<int> buildingUnlocksAwarded, bool randomLand, string rewardThoughtIcon, Dictionary<string, object> buildingLabels)
	{
	}

	public static Reward FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public static Reward FromObject(object o)
	{
		return null;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void AddDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public static Dictionary<string, object> RewardToDict(Reward reward)
	{
		return null;
	}

	private static List<int> ParseIntListOrEmpty(Dictionary<string, object> dict, string key)
	{
		return null;
	}

	private static Dictionary<int, int> ParseAmountDictOrEmpty(Dictionary<string, object> dict, string key)
	{
		return null;
	}

	private static Dictionary<int, Vector2> ParseAmountDictOrEmptyVector2(Dictionary<string, object> dict, string key)
	{
		return null;
	}

	public static Reward operator +(Reward r1, Reward r2)
	{
		return null;
	}
}
