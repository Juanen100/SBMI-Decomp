using System.Collections.Generic;

public class LevelingManager : IResourceProgressCalculator
{
	private List<MilestoneMarker> milestones;

	private List<string> headlines;

	private List<string> headlineImages;

	private List<string> voiceOvers;

	private List<int> autoQuestLengths;

	private int maxLevel;

	public int MaxLevel
	{
		get
		{
			return 0;
		}
	}

	public int GetResourceType()
	{
		return 0;
	}

	public string Headline(int level)
	{
		return null;
	}

	public string HeadlineImage(int level)
	{
		return null;
	}

	public int AutoQuestLength(int level)
	{
		return 0;
	}

	public string VoiceOver(int level)
	{
		return null;
	}

	public List<Reward> GetLevelUpRewards(Simulation simulation, int oldLevel, int newXp)
	{
		return null;
	}

	public int GetXpRequiredForLevel(int level)
	{
		return 0;
	}

	public void GetRewardsForIncreasingResource(Simulation simulation, Dictionary<int, Resource> currentResources, int amountToIncrease, out List<Reward> rewards)
	{
		rewards = null;
	}

	public float ComputeProgressPercentage(Dictionary<int, Resource> currentResources)
	{
		return 0f;
	}

	public string ComputeProgressFraction(Dictionary<int, Resource> currentResources)
	{
		return null;
	}

	private void LoadLevelingMilestones()
	{
	}

	private void LoadLevelingHeadlines()
	{
	}

	private Dictionary<string, object> LoadLevelingMilestonesFromSpread()
	{
		return null;
	}

	private Dictionary<string, object> LoadLevelingHeadlinesFromSpread()
	{
		return null;
	}
}
