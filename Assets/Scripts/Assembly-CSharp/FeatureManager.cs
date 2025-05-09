using System.Collections.Generic;
using MiniJSON;

public class FeatureManager
{
	private static readonly string FEATURE_DATA_PATH = "Features";

	private HashSet<string> unlockedFeatures;

	private Dictionary<string, FeatureLock> featureLocks;

	public HashSet<string> ActiveFeatures
	{
		get
		{
			return unlockedFeatures;
		}
	}

	public FeatureManager()
	{
		unlockedFeatures = new HashSet<string>();
		featureLocks = new Dictionary<string, FeatureLock>();
		LoadFeatures();
	}

	private string[] GetFilesToLoad()
	{
		return Config.FEATURE_DATA_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private void LoadFeatures()
	{
		string[] filesToLoad = GetFilesToLoad();
		string[] array = filesToLoad;
		foreach (string filePath in array)
		{
			string filePathFromString = GetFilePathFromString(filePath);
			TFUtils.DebugLog("Loading features: " + filePathFromString, TFUtils.LogFilter.Features);
			string json = TFUtils.ReadAllText(filePathFromString);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
			switch ((string)dictionary["type"])
			{
			case "unlock":
			{
				FeatureLock featureLock = new FeatureLock(dictionary);
				featureLocks.Add(featureLock.Feature, featureLock);
				break;
			}
			}
		}
	}

	public bool CheckFeature(string feature)
	{
		if (featureLocks.ContainsKey(feature))
		{
			return unlockedFeatures.Contains(feature);
		}
		return true;
	}

	public void UnlockFeature(string feature)
	{
		if (featureLocks.ContainsKey(feature) && !unlockedFeatures.Contains(feature))
		{
			unlockedFeatures.Add(feature);
		}
	}

	public void UnlockAllFeatures()
	{
		foreach (KeyValuePair<string, FeatureLock> featureLock in featureLocks)
		{
			string key = featureLock.Key;
			if (!unlockedFeatures.Contains(key))
			{
				unlockedFeatures.Add(key);
			}
		}
	}

	public void UnlockAllFeaturesToGamestate(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("features"))
		{
			dictionary["features"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["features"];
		foreach (KeyValuePair<string, FeatureLock> featureLock in featureLocks)
		{
			string key = featureLock.Key;
			if (!list.Contains(key))
			{
				list.Add(key);
			}
		}
	}

	public void ActivateFeatureActions(Game game, string feature)
	{
		featureLocks[feature].Activate(game);
	}
}
