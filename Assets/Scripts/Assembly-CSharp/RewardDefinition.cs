using System;
using System.Collections.Generic;

public class RewardDefinition
{
	private class GeneratorBucket
	{
		public Dictionary<int, ResultGenerator> resourcesGenerator;

		public Dictionary<int, ResultGenerator> buildingsGenerator;

		public Dictionary<int, ResultGenerator> recipesGenerator;

		public Dictionary<int, ResultGenerator> moviesGenerator;

		public Dictionary<int, ResultGenerator> costumesGenerator;

		public Reward summary;

		public GeneratorBucket(Dictionary<int, ResultGenerator> resourcesGenerator, Dictionary<int, ResultGenerator> buildingsGenerator, Dictionary<int, ResultGenerator> recipesGenerator, Dictionary<int, ResultGenerator> moviesGenerator, Dictionary<int, ResultGenerator> costumesGenerator, Reward summary)
		{
			this.resourcesGenerator = resourcesGenerator;
			this.buildingsGenerator = buildingsGenerator;
			this.recipesGenerator = recipesGenerator;
			this.moviesGenerator = moviesGenerator;
			this.costumesGenerator = costumesGenerator;
			this.summary = summary;
		}

		public override string ToString()
		{
			string rv = "[GeneratorBucket (";
			Action<Dictionary<int, ResultGenerator>> action = delegate(Dictionary<int, ResultGenerator> generators)
			{
				if (generators != null)
				{
					foreach (KeyValuePair<int, ResultGenerator> generator in generators)
					{
						string text = rv;
						rv = text + generator.Key + ": " + generator.Value;
					}
				}
			};
			action(resourcesGenerator);
			action(buildingsGenerator);
			action(recipesGenerator);
			action(moviesGenerator);
			action(costumesGenerator);
			rv += ")]";
			return rv;
		}
	}

	private const string SUMMARY = "summary";

	private CdfDictionary<GeneratorBucket> generatorBuckets;

	private Reward summary;

	public Reward Summary
	{
		get
		{
			return summary;
		}
	}

	private RewardDefinition(CdfDictionary<GeneratorBucket> buckets, Reward summary)
	{
		generatorBuckets = buckets;
		this.summary = summary;
	}

	public static RewardDefinition FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		Reward reward = null;
		if (dict.ContainsKey("summary"))
		{
			reward = Reward.FromObject(TFUtils.LoadDict(dict, "summary"));
		}
		CdfDictionary<GeneratorBucket> cdfDictionary;
		if (!dict.ContainsKey("cdf"))
		{
			cdfDictionary = new CdfDictionary<GeneratorBucket>();
			GeneratorBucket generatorBucket = FromDictInnerHelper(dict);
			cdfDictionary.Add(generatorBucket, 1.0);
			if (reward == null)
			{
				reward = generatorBucket.summary;
			}
		}
		else
		{
			cdfDictionary = CdfDictionary<GeneratorBucket>.FromList(TFUtils.LoadList<object>(dict, "cdf"), FromDictInnerHelper);
		}
		return new RewardDefinition(cdfDictionary, reward);
	}

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		foreach (KeyValuePair<int, int> resourceAmount in summary.ResourceAmounts)
		{
			dictionary2[resourceAmount.Key.ToString()] = resourceAmount.Value;
		}
		dictionary["resources"] = dictionary2;
		return dictionary;
	}

	private static GeneratorBucket FromDictInnerHelper(object obj)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
		Dictionary<int, ResultGenerator> dictionary2 = ParseOrNull(dictionary, "resources");
		Dictionary<int, ResultGenerator> buildingsGenerator = ParseOrNull(dictionary, "buildings");
		Dictionary<int, ResultGenerator> recipesGenerator = ParseOrNull(dictionary, "recipes");
		Dictionary<int, ResultGenerator> moviesGenerator = ParseOrNull(dictionary, "movies");
		Dictionary<int, ResultGenerator> costumesGenerator = ParseOrNull(dictionary, "costumes");
		Reward reward = null;
		if (dictionary.ContainsKey("summary"))
		{
			reward = Reward.FromDict((Dictionary<string, object>)dictionary["summary"]);
		}
		else if (dictionary2 != null)
		{
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			foreach (int key in dictionary2.Keys)
			{
				dictionary3[key] = (int)float.Parse(dictionary2[key].GetExpectedValue());
			}
			string rewardThoughtIcon = TFUtils.TryLoadNullableString(dictionary, "thought_icon");
			bool? flag = TFUtils.TryLoadBool(dictionary, "random_land");
			reward = new Reward(dictionary3, null, null, null, null, null, null, null, flag.HasValue && flag.Value, rewardThoughtIcon);
		}
		return new GeneratorBucket(dictionary2, buildingsGenerator, recipesGenerator, moviesGenerator, costumesGenerator, reward);
	}

	public static RewardDefinition FromObject(object o)
	{
		return (o != null) ? FromDict((Dictionary<string, object>)o) : null;
	}

	public int LowestResourceValue(int nKey)
	{
		List<GeneratorBucket> valuesClone = generatorBuckets.ValuesClone;
		int count = valuesClone.Count;
		int num = -1;
		for (int i = 0; i < count; i++)
		{
			GeneratorBucket generatorBucket = valuesClone[i];
			if (generatorBucket != null && generatorBucket.resourcesGenerator != null && generatorBucket.resourcesGenerator.ContainsKey(nKey))
			{
				int num2 = (int)float.Parse(generatorBucket.resourcesGenerator[nKey].GetExpectedValue());
				if (num == -1 || num2 < num)
				{
					num = num2;
				}
			}
		}
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public Reward GenerateReward(Simulation simulation, bool forceReward)
	{
		return GenerateReward(simulation, false, forceReward);
	}

	public Reward GenerateReward(Simulation simulation, bool inferThoughtIconIfNull, bool forceReward)
	{
		return GenerateReward(simulation, new Reward(new Dictionary<int, int>(), null, null, null, null, null, null, null, summary != null && summary.RandomLand, null), inferThoughtIconIfNull, forceReward);
	}

	public Reward GenerateReward(Simulation simulation, Reward consolationReward, bool inferThoughtIconIfNull, bool forceReward)
	{
		GeneratorBucket generatorBucket = generatorBuckets.Spin();
		Dictionary<int, int> dictionary = null;
		Dictionary<int, int> dictionary2 = null;
		Dictionary<int, int> dictionary3 = null;
		Dictionary<int, int> dictionary4 = null;
		Dictionary<int, int> dictionary5 = null;
		if (generatorBucket == null)
		{
			return null;
		}
		dictionary = ProbabilityDictionary.GenerateAmounts(generatorBucket.resourcesGenerator);
		dictionary2 = ProbabilityDictionary.GenerateAmounts(generatorBucket.buildingsGenerator);
		dictionary3 = ((generatorBucket.recipesGenerator == null) ? null : ProbabilityDictionary.GenerateAmounts(generatorBucket.recipesGenerator));
		dictionary4 = ProbabilityDictionary.GenerateAmounts(generatorBucket.moviesGenerator);
		dictionary5 = ProbabilityDictionary.GenerateAmounts(generatorBucket.costumesGenerator);
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		if (dictionary3 != null)
		{
			foreach (int key2 in dictionary3.Keys)
			{
				if (!simulation.craftManager.Recipes.ContainsKey(key2))
				{
					TFUtils.WarningLog("missing recipe: " + key2 + "! make sure this is a recipe and NOT a resource id");
				}
				else if (dictionary3[key2] > 0 && !simulation.craftManager.UnlockedRecipesCopy.Contains(key2) && !simulation.craftManager.ReservedRecipesCopy.Contains(key2) && simulation.craftManager.GetRecipeById(key2).minimumLevel <= simulation.resourceManager.Query(ResourceManager.LEVEL))
				{
					if (forceReward)
					{
						list.Add(key2);
					}
					else if (simulation.craftManager.CanMakeRecipe(key2))
					{
						list.Add(key2);
					}
				}
			}
		}
		if (dictionary4 != null)
		{
			foreach (int key3 in dictionary4.Keys)
			{
				if (dictionary4[key3] > 0 && !simulation.movieManager.UnlockedMovies.Contains(key3))
				{
					list2.Add(key3);
				}
			}
		}
		if (dictionary5 != null)
		{
			foreach (int key4 in dictionary5.Keys)
			{
				if (dictionary5[key4] > 0 && !simulation.game.costumeManager.IsCostumeUnlocked(key4))
				{
					list3.Add(key4);
				}
			}
		}
		string text = ((summary != null) ? summary.ThoughtIcon : null);
		if (inferThoughtIconIfNull && text == null)
		{
			text = InferThoughtIcon(dictionary, simulation.resourceManager);
		}
		Reward reward = (((dictionary != null && dictionary.Count != 0) || (dictionary2 != null && dictionary2.Count != 0) || list.Count != 0 || list2.Count != 0 || list3.Count != 0) ? new Reward(dictionary, dictionary2, null, list, list2, list3, null, null, summary != null && summary.RandomLand, text) : consolationReward);
		if (!forceReward && simulation.rewardCap.Filter(simulation, ref reward))
		{
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				Dictionary<int, int> resourceAmounts;
				Dictionary<int, int> dictionary6 = (resourceAmounts = reward.ResourceAmounts);
				int sOFT_CURRENCY;
				int key = (sOFT_CURRENCY = ResourceManager.SOFT_CURRENCY);
				sOFT_CURRENCY = resourceAmounts[sOFT_CURRENCY];
				dictionary6[key] = sOFT_CURRENCY + 25;
			}
			else
			{
				reward.ResourceAmounts[ResourceManager.SOFT_CURRENCY] = 25;
			}
		}
		return reward;
	}

	public RewardDefinition Join(RewardDefinition that)
	{
		return new RewardDefinition(generatorBuckets.Join(that.generatorBuckets), null);
	}

	public void Normalize()
	{
		generatorBuckets.Normalize();
	}

	public void Validate(bool ensureFullRange)
	{
		generatorBuckets.Validate(ensureFullRange, string.Empty);
	}

	private static Dictionary<int, ResultGenerator> ParseOrNull(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return null;
		}
		return ProbabilityDictionary.FromJSONDict((Dictionary<string, object>)dict[key]);
	}

	private string InferThoughtIcon(Dictionary<int, int> resourceAmounts, ResourceManager resourceMgr)
	{
		foreach (int key in resourceAmounts.Keys)
		{
			if (key != ResourceManager.SOFT_CURRENCY && key != ResourceManager.HARD_CURRENCY && key != ResourceManager.XP && key != ResourceManager.LEVEL)
			{
				return resourceMgr.Resources[key].GetResourceTexture();
			}
		}
		string rv = null;
		if (IdToStringHelper(ResourceManager.HARD_CURRENCY, ref rv, resourceAmounts, resourceMgr) || IdToStringHelper(ResourceManager.SOFT_CURRENCY, ref rv, resourceAmounts, resourceMgr) || !IdToStringHelper(ResourceManager.XP, ref rv, resourceAmounts, resourceMgr))
		{
		}
		return rv;
	}

	private bool IdToStringHelper(int productId, ref string rv, Dictionary<int, int> resourceAmounts, ResourceManager resourceMgr)
	{
		if (resourceAmounts.ContainsKey(productId))
		{
			rv = resourceMgr.Resources[productId].GetResourceTexture();
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		string text = "[RewardDefinition (";
		text = text + "summary=" + summary;
		text = text + "\nbuckets=" + generatorBuckets;
		return text + ")]";
	}
}
