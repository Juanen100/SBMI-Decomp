using System;
using System.Collections.Generic;
using UnityEngine;

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
			return maxLevel;
		}
	}

	public LevelingManager()
	{
		LoadLevelingMilestones();
		LoadLevelingHeadlines();
	}

	public int GetResourceType()
	{
		return ResourceManager.XP;
	}

	public string Headline(int level)
	{
		return headlines[level - 2];
	}

	public string HeadlineImage(int level)
	{
		return headlineImages[level - 2];
	}

	public int AutoQuestLength(int level)
	{
		return autoQuestLengths[Mathf.Max(0, level - 2)];
	}

	public string VoiceOver(int level)
	{
		return voiceOvers[level - 2];
	}

	public List<Reward> GetLevelUpRewards(Simulation simulation, int oldLevel, int newXp)
	{
		List<Reward> list = new List<Reward>();
		for (int i = 0; i < milestones.Count; i++)
		{
			int num = i + 2;
			if (num > oldLevel && newXp >= milestones[i].xp)
			{
				list.Add(milestones[i].rewardDef.GenerateReward(simulation, false));
			}
		}
		return list;
	}

	public int GetXpRequiredForLevel(int level)
	{
		int num = level - 2;
		if (level == 1 || num >= milestones.Count)
		{
			return 0;
		}
		return milestones[num].xp;
	}

	public void GetRewardsForIncreasingResource(Simulation simulation, Dictionary<int, Resource> currentResources, int amountToIncrease, out List<Reward> rewards)
	{
		int amount = currentResources[ResourceManager.LEVEL].Amount;
		int amount2 = currentResources[ResourceManager.XP].Amount;
		int newXp = amount2 + amountToIncrease;
		rewards = GetLevelUpRewards(simulation, amount, newXp);
	}

	public float ComputeProgressPercentage(Dictionary<int, Resource> currentResources)
	{
		int amount = currentResources[ResourceManager.XP].Amount;
		int amount2 = currentResources[ResourceManager.LEVEL].Amount;
		int xpRequiredForLevel = GetXpRequiredForLevel(amount2);
		int xpRequiredForLevel2 = GetXpRequiredForLevel(amount2 + 1);
		if (amount2 >= maxLevel)
		{
			return 100f;
		}
		return (float)(amount - xpRequiredForLevel) / (float)(xpRequiredForLevel2 - xpRequiredForLevel) * 100f;
	}

	public string ComputeProgressFraction(Dictionary<int, Resource> currentResources)
	{
		int amount = currentResources[ResourceManager.XP].Amount;
		int amount2 = currentResources[ResourceManager.LEVEL].Amount;
		int xpRequiredForLevel = GetXpRequiredForLevel(amount2);
		int xpRequiredForLevel2 = GetXpRequiredForLevel(amount2 + 1);
		if (amount2 >= maxLevel)
		{
			return amount + " / " + amount;
		}
		return amount - xpRequiredForLevel + " / " + (xpRequiredForLevel2 - xpRequiredForLevel);
	}

	private void LoadLevelingMilestones()
	{
		milestones = new List<MilestoneMarker>();
		Dictionary<string, object> dictionary = LoadLevelingMilestonesFromSpread();
		List<object> list = (List<object>)dictionary["milestones"];
		foreach (object item in list)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item;
			int xp = Convert.ToInt32(dictionary2["xp"]);
			object o = dictionary2["reward"];
			RewardDefinition rewardDef = RewardDefinition.FromObject(o);
			milestones.Add(new MilestoneMarker(xp, rewardDef));
		}
		maxLevel = milestones.Count + 1;
	}

	private void LoadLevelingHeadlines()
	{
		headlines = new List<string>();
		headlineImages = new List<string>();
		voiceOvers = new List<string>();
		autoQuestLengths = new List<int>();
		Dictionary<string, object> dictionary = LoadLevelingHeadlinesFromSpread();
		List<object> list = (List<object>)dictionary["levels"];
		string text = "This is the level headline.";
		string text2 = "Newspaper_HeadlineImage_Spongebob01.png";
		string text3 = "SB_GreatJob";
		foreach (object item2 in list)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item2;
			text = Convert.ToString(dictionary2["headline"]);
			headlines.Add(text);
			text2 = Convert.ToString(dictionary2["texture"]);
			headlineImages.Add(text2);
			text3 = Convert.ToString(dictionary2["voice_over"]);
			voiceOvers.Add(text3);
			int item = Convert.ToInt32(dictionary2["auto_quest_length"]);
			autoQuestLengths.Add(item);
		}
	}

	private Dictionary<string, object> LoadLevelingMilestonesFromSpread()
	{
		string text = "Leveling";
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return dictionary;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return dictionary;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return dictionary;
		}
		dictionary.Add("type", "leveling");
		dictionary.Add("milestones", new List<object>());
		string text2 = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "level");
			if (intCell <= 1)
			{
				continue;
			}
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("_level", intCell);
			dictionary2.Add("xp", instance.GetIntCell(sheetIndex, rowIndex, "xp"));
			dictionary2.Add("reward", new Dictionary<string, object> { { "thought_icon", null } });
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "reward jelly");
			if (intCell > 0)
			{
				((Dictionary<string, object>)dictionary2["reward"]).Add("resources", new Dictionary<string, object> { { "2", intCell } });
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "reward gold");
			if (intCell > 0)
			{
				if (!((Dictionary<string, object>)dictionary2["reward"]).ContainsKey("resources"))
				{
					((Dictionary<string, object>)dictionary2["reward"]).Add("resources", new Dictionary<string, object>());
				}
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary2["reward"])["resources"]).Add("3", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "reward special amount");
			if (intCell > 0)
			{
				string stringCell = instance.GetStringCell(text, rowName, "reward special type");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
				{
					if (!((Dictionary<string, object>)dictionary2["reward"]).ContainsKey(stringCell))
					{
						((Dictionary<string, object>)dictionary2["reward"]).Add(stringCell, new Dictionary<string, object>());
					}
					((Dictionary<string, object>)((Dictionary<string, object>)dictionary2["reward"])[stringCell]).Add(instance.GetIntCell(sheetIndex, rowIndex, "reward special did").ToString(), intCell);
				}
			}
			((List<object>)dictionary["milestones"]).Add(dictionary2);
		}
		return dictionary;
	}

	private Dictionary<string, object> LoadLevelingHeadlinesFromSpread()
	{
		string text = "Leveling";
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return dictionary;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return dictionary;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return dictionary;
		}
		dictionary.Add("type", "leveling");
		dictionary.Add("levels", new List<object>());
		string text2 = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "level");
			if (intCell > 1)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("_level", intCell);
				dictionary2.Add("auto_quest_length", instance.GetIntCell(sheetIndex, rowIndex, "max autoquest length"));
				string stringCell = instance.GetStringCell(text, rowName, "headline");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
				{
					dictionary2.Add("headline", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "texture");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
				{
					dictionary2.Add("texture", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "voice over");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
				{
					dictionary2.Add("voice_over", stringCell);
				}
				((List<object>)dictionary["levels"]).Add(dictionary2);
			}
		}
		return dictionary;
	}
}
