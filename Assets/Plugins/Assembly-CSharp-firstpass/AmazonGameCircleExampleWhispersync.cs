using System;
using System.Collections.Generic;
using UnityEngine;

public class AmazonGameCircleExampleWhispersync : AmazonGameCircleExampleBase
{
	private const string whispersyncMenuTitle = "Whispersync";

	private const string syncableNumbersLabel = "Syncable Numbers";

	private const string accumulatingNumbersLabel = "Accumulating Numbers";

	private const string syncDataButtonLabel = "Synchronize";

	private const string flushButtonLabel = "Flush";

	private const string noCloudDataReceivedLabel = "No cloud data received.";

	private const string cloudDataLastReceivedLabel = "Received cloud data {0,5:N1} second ago.";

	private const string hashSetsLabel = "Hash Sets";

	private const string numberListsLabel = "Syncable Number Lists";

	private const string whispersyncUnavailableLabel = "No Whispersync data available.";

	private DateTime? lastCloudDataAvailable;

	private bool syncableNumbersFoldout;

	private bool accumulatingNumbersFoldout;

	private bool syncableNumberListsFoldout;

	private bool hashSetsFoldout;

	private List<AmazonGameCircleExampleWSSyncableNumber> syncableNumbers;

	private List<AmazonGameCircleExampleWSAccumulatingNumber> accumulatingNumbers;

	private List<AmazonGameCircleExampleWSNumberList> syncableNumberLists;

	private AmazonGameCircleExampleWSHashSets hashSets;

	private AGSGameDataMap dataMap;

	public AmazonGameCircleExampleWhispersync()
	{
		InitSyncableNumbers();
		InitSyncableNumberLists();
		InitAccumulatingNumbers();
		InitHashSets();
	}

	public override string MenuTitle()
	{
		return "Whispersync";
	}

	public override void DrawMenu()
	{
		if (lastCloudDataAvailable.HasValue)
		{
			double totalSeconds = (DateTime.Now - lastCloudDataAvailable.Value).TotalSeconds;
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("Received cloud data {0,5:N1} second ago.", totalSeconds));
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No cloud data received.");
		}
		if (GUILayout.Button("Synchronize"))
		{
			AGSWhispersyncClient.Synchronize();
		}
		GUILayout.Label(GUIContent.none);
		if (GUILayout.Button("Flush"))
		{
			AGSWhispersyncClient.Flush();
		}
		GUILayout.Label(GUIContent.none);
		InitializeDataMapIfAvailable();
		if (dataMap == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No Whispersync data available.");
			return;
		}
		DrawSyncableNumbers();
		DrawAccumulatingNumbers();
		DrawSyncableNumberLists();
		DrawHashSets();
	}

	private void DrawSyncableNumbers()
	{
		if (syncableNumbers == null)
		{
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box);
		syncableNumbersFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(syncableNumbersFoldout, "Syncable Numbers");
		if (syncableNumbersFoldout)
		{
			foreach (AmazonGameCircleExampleWSSyncableNumber syncableNumber in syncableNumbers)
			{
				syncableNumber.DrawGUI(dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	private void DrawAccumulatingNumbers()
	{
		if (accumulatingNumbers == null)
		{
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box);
		accumulatingNumbersFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(accumulatingNumbersFoldout, "Accumulating Numbers");
		if (accumulatingNumbersFoldout)
		{
			foreach (AmazonGameCircleExampleWSAccumulatingNumber accumulatingNumber in accumulatingNumbers)
			{
				accumulatingNumber.DrawGUI(dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	private void DrawSyncableNumberLists()
	{
		if (syncableNumberLists == null)
		{
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box);
		syncableNumberListsFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(syncableNumberListsFoldout, "Syncable Number Lists");
		if (syncableNumberListsFoldout)
		{
			foreach (AmazonGameCircleExampleWSNumberList syncableNumberList in syncableNumberLists)
			{
				syncableNumberList.DrawGUI(dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	private void DrawHashSets()
	{
		if (hashSets != null)
		{
			GUILayout.BeginVertical(GUI.skin.box);
			hashSetsFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(hashSetsFoldout, "Hash Sets");
			if (hashSetsFoldout)
			{
				hashSets.DrawGUI(dataMap);
			}
			GUILayout.EndVertical();
		}
	}

	private void InitializeDataMapIfAvailable()
	{
		if (dataMap == null)
		{
			dataMap = AGSWhispersyncClient.GetGameData();
			if (dataMap != null)
			{
				AGSWhispersyncClient.OnNewCloudDataEvent += OnNewCloudData;
			}
		}
	}

	private void InitSyncableNumbers()
	{
		if (syncableNumbers != null)
		{
			return;
		}
		syncableNumbers = new List<AmazonGameCircleExampleWSSyncableNumber>();
		Array values = Enum.GetValues(typeof(AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior));
		Array values2 = Enum.GetValues(typeof(AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType));
		foreach (int item in values)
		{
			foreach (int item2 in values2)
			{
				syncableNumbers.Add(new AmazonGameCircleExampleWSSyncableNumber((AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior)item, (AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType)item2));
			}
		}
	}

	private void InitSyncableNumberLists()
	{
		if (syncableNumberLists != null)
		{
			return;
		}
		syncableNumberLists = new List<AmazonGameCircleExampleWSNumberList>();
		Array values = Enum.GetValues(typeof(AmazonGameCircleExampleWSNumberList.AvailableListType));
		foreach (int item in values)
		{
			syncableNumberLists.Add(new AmazonGameCircleExampleWSNumberList((AmazonGameCircleExampleWSNumberList.AvailableListType)item));
		}
	}

	private void InitAccumulatingNumbers()
	{
		if (accumulatingNumbers != null)
		{
			return;
		}
		accumulatingNumbers = new List<AmazonGameCircleExampleWSAccumulatingNumber>();
		Array values = Enum.GetValues(typeof(AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType));
		foreach (int item in values)
		{
			accumulatingNumbers.Add(new AmazonGameCircleExampleWSAccumulatingNumber((AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType)item));
		}
	}

	private void InitHashSets()
	{
		if (hashSets == null)
		{
			hashSets = new AmazonGameCircleExampleWSHashSets();
		}
	}

	private void OnNewCloudData()
	{
		lastCloudDataAvailable = DateTime.Now;
	}
}
