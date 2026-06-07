using System.Collections.Generic;

public class FeatureManager
{
	private static readonly string FEATURE_DATA_PATH;

	private HashSet<string> unlockedFeatures;

	private Dictionary<string, FeatureLock> featureLocks;

	public HashSet<string> ActiveFeatures
	{
		get
		{
			return null;
		}
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	private void LoadFeatures()
	{
	}

	public bool CheckFeature(string feature)
	{
		return false;
	}

	public void UnlockFeature(string feature)
	{
	}

	public void UnlockAllFeatures()
	{
	}

	public void UnlockAllFeaturesToGamestate(Dictionary<string, object> gameState)
	{
	}

	public void ActivateFeatureActions(Game game, string feature)
	{
	}
}
