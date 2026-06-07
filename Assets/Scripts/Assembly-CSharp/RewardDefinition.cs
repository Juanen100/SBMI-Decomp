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
		}

		public override string ToString()
		{
			return null;
		}
	}

	private const string SUMMARY = "summary";

	private CdfDictionary<GeneratorBucket> generatorBuckets;

	private Reward summary;

	public Reward Summary
	{
		get
		{
			return null;
		}
	}

	private RewardDefinition(CdfDictionary<GeneratorBucket> buckets, Reward summary)
	{
	}

	public static RewardDefinition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	private static GeneratorBucket FromDictInnerHelper(object obj)
	{
		return null;
	}

	public static RewardDefinition FromObject(object o)
	{
		return null;
	}

	public int LowestResourceValue(int nKey)
	{
		return 0;
	}

	public Reward GenerateReward(Simulation simulation, bool forceReward)
	{
		return null;
	}

	public Reward GenerateReward(Simulation simulation, bool inferThoughtIconIfNull, bool forceReward)
	{
		return null;
	}

	public Reward GenerateReward(Simulation simulation, Reward consolationReward, bool inferThoughtIconIfNull, bool forceReward)
	{
		return null;
	}

	public RewardDefinition Join(RewardDefinition that)
	{
		return null;
	}

	public void Normalize()
	{
	}

	public void Validate(bool ensureFullRange)
	{
	}

	private static Dictionary<int, ResultGenerator> ParseOrNull(Dictionary<string, object> dict, string key)
	{
		return null;
	}

	private string InferThoughtIcon(Dictionary<int, int> resourceAmounts, ResourceManager resourceMgr)
	{
		return null;
	}

	private bool IdToStringHelper(int productId, ref string rv, Dictionary<int, int> resourceAmounts, ResourceManager resourceMgr)
	{
		return false;
	}

	public override string ToString()
	{
		return null;
	}
}
