using System;
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
			return resourceAmounts;
		}
	}

	public Dictionary<int, int> BuildingAmounts
	{
		get
		{
			return buildingAmounts;
		}
	}

	public Dictionary<int, Vector2> BuildingPositions
	{
		get
		{
			return buildingPositions;
		}
	}

	public List<int> RecipeUnlocks
	{
		get
		{
			return recipesAwarded;
		}
		set
		{
			recipesAwarded = value;
		}
	}

	public List<int> MovieUnlocks
	{
		get
		{
			return moviesAwarded;
		}
	}

	public List<int> CostumeUnlocks
	{
		get
		{
			return costumesAwarded;
		}
	}

	public List<int> ClearedLands
	{
		get
		{
			return clearedLandsAwarded;
		}
	}

	public List<int> BuildingUnlocks
	{
		get
		{
			return buildingUnlocksAwarded;
		}
	}

	public string ThoughtIcon
	{
		get
		{
			return rewardThoughtIcon;
		}
	}

	public bool RandomLand
	{
		get
		{
			return rewardRandomLand;
		}
	}

	public Dictionary<string, object> BuildingLabels
	{
		get
		{
			if (buildingLabels == null && buildingAmounts != null)
			{
				buildingLabels = new Dictionary<string, object>();
				foreach (KeyValuePair<int, int> buildingAmount in BuildingAmounts)
				{
					int key = buildingAmount.Key;
					List<object> list = new List<object>();
					for (int i = 0; i < buildingAmount.Value; i++)
					{
						Identity identity = new Identity();
						list.Add(identity.Describe());
					}
					buildingLabels[key.ToString()] = list;
				}
			}
			return buildingLabels;
		}
	}

	public Reward(Dictionary<int, int> resourceAmounts, Dictionary<int, int> buildingAmounts, Dictionary<int, Vector2> buildingPositions, List<int> recipesAwarded, List<int> moviesAwarded, List<int> costumesAwarded, List<int> clearedLandsAwarded, List<int> buildingUnlocksAwarded, bool randomLand, string rewardThoughtIcon)
	{
		this.resourceAmounts = ((resourceAmounts != null) ? resourceAmounts : new Dictionary<int, int>());
		this.buildingAmounts = ((buildingAmounts != null) ? buildingAmounts : new Dictionary<int, int>());
		this.buildingPositions = ((buildingPositions != null) ? buildingPositions : new Dictionary<int, Vector2>());
		this.recipesAwarded = ((recipesAwarded != null) ? recipesAwarded : new List<int>());
		this.moviesAwarded = ((moviesAwarded != null) ? moviesAwarded : new List<int>());
		this.costumesAwarded = ((costumesAwarded != null) ? costumesAwarded : new List<int>());
		this.clearedLandsAwarded = ((clearedLandsAwarded != null) ? clearedLandsAwarded : new List<int>());
		this.buildingUnlocksAwarded = ((buildingUnlocksAwarded != null) ? buildingUnlocksAwarded : new List<int>());
		this.rewardThoughtIcon = rewardThoughtIcon;
		rewardRandomLand = randomLand;
	}

	public Reward(Dictionary<int, int> resourceAmounts, Dictionary<int, int> buildingAmounts, Dictionary<int, Vector2> buildingPositions, List<int> recipesAwarded, List<int> moviesAwarded, List<int> costumesAwarded, List<int> clearedLandsAwarded, List<int> buildingUnlocksAwarded, bool randomLand, string rewardThoughtIcon, Dictionary<string, object> buildingLabels)
	{
		this.resourceAmounts = ((resourceAmounts != null) ? resourceAmounts : new Dictionary<int, int>());
		this.buildingAmounts = ((buildingAmounts != null) ? buildingAmounts : new Dictionary<int, int>());
		this.buildingPositions = ((buildingPositions != null) ? buildingPositions : new Dictionary<int, Vector2>());
		this.recipesAwarded = ((recipesAwarded != null) ? recipesAwarded : new List<int>());
		this.moviesAwarded = ((moviesAwarded != null) ? moviesAwarded : new List<int>());
		this.costumesAwarded = ((costumesAwarded != null) ? costumesAwarded : new List<int>());
		this.clearedLandsAwarded = ((clearedLandsAwarded != null) ? clearedLandsAwarded : new List<int>());
		this.buildingUnlocksAwarded = ((buildingUnlocksAwarded != null) ? buildingUnlocksAwarded : new List<int>());
		this.rewardThoughtIcon = rewardThoughtIcon;
		this.buildingLabels = buildingLabels;
		rewardRandomLand = randomLand;
	}

	public static Reward FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = ParseAmountDictOrEmpty(dict, "resources");
		Dictionary<int, int> dictionary2 = ParseAmountDictOrEmpty(dict, "buildings");
		Dictionary<int, Vector2> dictionary3 = ParseAmountDictOrEmptyVector2(dict, "building_positions");
		List<int> list = ParseIntListOrEmpty(dict, "recipes");
		List<int> list2 = ParseIntListOrEmpty(dict, "movies");
		List<int> list3 = ParseIntListOrEmpty(dict, "costumes");
		List<int> list4 = ParseIntListOrEmpty(dict, "cleared_land");
		List<int> list5 = ParseIntListOrEmpty(dict, "building_unlocks");
		Dictionary<string, object> dictionary4 = (dict.ContainsKey("building_labels") ? ((Dictionary<string, object>)dict["building_labels"]) : null);
		string text = TFUtils.TryLoadNullableString(dict, "thought_icon");
		bool? flag = TFUtils.TryLoadBool(dict, "random_land");
		return new Reward(dictionary, dictionary2, dictionary3, list, list2, list3, list4, list5, flag.HasValue && flag.Value, text, dictionary4);
	}

	public static Reward FromObject(object o)
	{
		return (o != null) ? FromDict((Dictionary<string, object>)o) : null;
	}

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (resourceAmounts != null)
		{
			dictionary["resources"] = AmountDictionary.ToJSONDict(resourceAmounts);
		}
		if (buildingAmounts != null)
		{
			dictionary["buildings"] = AmountDictionary.ToJSONDict(buildingAmounts);
		}
		if (buildingPositions != null)
		{
			dictionary["building_positions"] = AmountDictionary.ToJSONDict(buildingPositions);
		}
		if (recipesAwarded != null)
		{
			dictionary["recipes"] = recipesAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		}
		if (moviesAwarded != null)
		{
			dictionary["movies"] = moviesAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		}
		if (costumesAwarded != null)
		{
			dictionary["costumes"] = costumesAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		}
		if (clearedLandsAwarded != null)
		{
			dictionary["cleared_land"] = clearedLandsAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		}
		if (buildingUnlocksAwarded != null)
		{
			dictionary["building_unlocks"] = buildingUnlocksAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		}
		if (BuildingLabels != null)
		{
			dictionary["building_labels"] = buildingLabels;
		}
		dictionary["thought_icon"] = rewardThoughtIcon;
		dictionary["random_land"] = rewardRandomLand;
		return dictionary;
	}

	public void AddDataToTrigger(ref Dictionary<string, object> data)
	{
		data["resource_amounts"] = AmountDictionary.ToJSONDict(resourceAmounts);
		data["recipes"] = recipesAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		data["buildings"] = moviesAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		data["costumes"] = costumesAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
		data["building_unlocks"] = buildingUnlocksAwarded.ConvertAll((Converter<int, object>)((int x) => x.ToString()));
	}

	public static Dictionary<string, object> RewardToDict(Reward reward)
	{
		return (reward != null) ? reward.ToDict() : null;
	}

	private static List<int> ParseIntListOrEmpty(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return new List<int>();
		}
		return ((List<object>)dict[key]).ConvertAll((object x) => Convert.ToInt32(x));
	}

	private static Dictionary<int, int> ParseAmountDictOrEmpty(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return new Dictionary<int, int>();
		}
		return AmountDictionary.FromJSONDict((Dictionary<string, object>)dict[key]);
	}

	private static Dictionary<int, Vector2> ParseAmountDictOrEmptyVector2(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return new Dictionary<int, Vector2>();
		}
		return AmountDictionary.FromJSONDictVector2((Dictionary<string, object>)dict[key]);
	}

	public static Reward operator +(Reward r1, Reward r2)
	{
		if (r1 == null)
		{
			return r2;
		}
		if (r2 == null)
		{
			return r1;
		}
		Reward reward = new Reward(null, null, null, null, null, null, null, null, false, null);
		reward.resourceAmounts = ((r1.resourceAmounts != null) ? TFUtils.CloneDictionary(r1.resourceAmounts) : new Dictionary<int, int>());
		reward.buildingAmounts = ((r1.buildingAmounts != null) ? TFUtils.CloneDictionary(r1.buildingAmounts) : new Dictionary<int, int>());
		reward.buildingPositions = ((r1.buildingPositions != null) ? TFUtils.CloneDictionary(r1.buildingPositions) : new Dictionary<int, Vector2>());
		reward.recipesAwarded = ((r1.recipesAwarded != null) ? r1.recipesAwarded : new List<int>());
		reward.moviesAwarded = ((r1.moviesAwarded != null) ? r1.moviesAwarded : new List<int>());
		reward.costumesAwarded = ((r1.costumesAwarded != null) ? r1.costumesAwarded : new List<int>());
		reward.clearedLandsAwarded = ((r1.clearedLandsAwarded != null) ? r1.clearedLandsAwarded : new List<int>());
		reward.buildingUnlocksAwarded = ((r1.buildingUnlocksAwarded != null) ? r1.buildingUnlocksAwarded : new List<int>());
		if (r2.resourceAmounts != null)
		{
			foreach (int key4 in r2.resourceAmounts.Keys)
			{
				if (reward.resourceAmounts.ContainsKey(key4))
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary = (dictionary2 = reward.resourceAmounts);
					int key2;
					int key = (key2 = key4);
					key2 = dictionary2[key2];
					dictionary[key] = key2 + r2.resourceAmounts[key4];
				}
				else
				{
					reward.resourceAmounts[key4] = r2.resourceAmounts[key4];
				}
			}
		}
		if (r2.buildingAmounts != null)
		{
			foreach (int key5 in r2.buildingAmounts.Keys)
			{
				if (reward.buildingAmounts.ContainsKey(key5))
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary3 = (dictionary4 = reward.buildingAmounts);
					int key2;
					int key3 = (key2 = key5);
					key2 = dictionary4[key2];
					dictionary3[key3] = key2 + r2.buildingAmounts[key5];
				}
				else
				{
					reward.buildingAmounts[key5] = r2.buildingAmounts[key5];
				}
			}
		}
		if (r2.buildingPositions != null)
		{
			foreach (int key6 in r2.buildingPositions.Keys)
			{
				if (!reward.buildingPositions.ContainsKey(key6))
				{
					reward.buildingPositions[key6] = r2.buildingPositions[key6];
				}
			}
		}
		if (r2.recipesAwarded != null)
		{
			foreach (int item in r2.recipesAwarded)
			{
				if (!r1.recipesAwarded.Contains(item))
				{
					reward.recipesAwarded.Add(item);
				}
			}
		}
		if (r2.moviesAwarded != null)
		{
			foreach (int item2 in r2.moviesAwarded)
			{
				if (!r1.moviesAwarded.Contains(item2))
				{
					reward.moviesAwarded.Add(item2);
				}
			}
		}
		if (r2.costumesAwarded != null)
		{
			foreach (int item3 in r2.costumesAwarded)
			{
				if (!r1.costumesAwarded.Contains(item3))
				{
					reward.costumesAwarded.Add(item3);
				}
			}
		}
		if (r2.clearedLandsAwarded != null)
		{
			foreach (int item4 in r2.clearedLandsAwarded)
			{
				if (!r1.clearedLandsAwarded.Contains(item4))
				{
					reward.clearedLandsAwarded.Add(item4);
				}
			}
		}
		if (r2.buildingUnlocksAwarded != null)
		{
			foreach (int item5 in r2.buildingUnlocksAwarded)
			{
				if (!r1.buildingUnlocksAwarded.Contains(item5))
				{
					reward.buildingUnlocksAwarded.Add(item5);
				}
			}
		}
		if (r1.rewardThoughtIcon == r2.rewardThoughtIcon)
		{
			reward.rewardThoughtIcon = r1.rewardThoughtIcon;
		}
		return reward;
	}
}
