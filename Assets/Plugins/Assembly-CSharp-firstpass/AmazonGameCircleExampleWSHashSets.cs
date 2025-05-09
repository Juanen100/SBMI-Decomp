using System.Collections.Generic;
using UnityEngine;

public class AmazonGameCircleExampleWSHashSets
{
	private const string nullDataMapLabel = "Missing Whispersync DataMap";

	private const string nullHashSets = "HashSets not yet initialized";

	private const string refreshAllHashSetsButtonLabel = "Refresh All";

	private List<AmazonGameCircleExampleWSHashSet> hashSets;

	public void DrawGUI(AGSGameDataMap dataMap)
	{
		if (dataMap == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Missing Whispersync DataMap");
			return;
		}
		if (hashSets == null)
		{
			InitializeHashSets(dataMap);
		}
		if (hashSets == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("HashSets not yet initialized");
			return;
		}
		if (GUILayout.Button("Refresh All"))
		{
			RefreshAllHashSets();
		}
		foreach (AmazonGameCircleExampleWSHashSet hashSet in hashSets)
		{
			hashSet.DrawGUI();
		}
	}

	private void InitializeHashSets(AGSGameDataMap dataMap)
	{
		if (dataMap != null)
		{
			hashSets = new List<AmazonGameCircleExampleWSHashSet>();
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetHighestNumberKeys", dataMap.GetHighestNumberKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLowestNumberKeys", dataMap.GetLowestNumberKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestNumberKeys", dataMap.GetLatestNumberKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetHighNumberListKeys", dataMap.GetHighNumberListKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLowNumberListKeys", dataMap.GetLowNumberListKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestNumberListKeys", dataMap.GetLatestNumberListKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestStringKeys", dataMap.GetLatestStringKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestStringListKeys", dataMap.GetLatestStringListKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetStringSetKeys", dataMap.GetStringSetKeys));
			hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetMapKeys", dataMap.GetMapKeys));
		}
	}

	private void RefreshAllHashSets()
	{
		if (hashSets == null)
		{
			return;
		}
		foreach (AmazonGameCircleExampleWSHashSet hashSet in hashSets)
		{
			hashSet.Refresh();
		}
	}
}
