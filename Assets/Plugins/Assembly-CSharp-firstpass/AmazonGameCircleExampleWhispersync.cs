using System;
using System.Collections.Generic;

public class AmazonGameCircleExampleWhispersync : AmazonGameCircleExampleBase
{
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

	public override string MenuTitle()
	{
		return null;
	}

	public override void DrawMenu()
	{
	}

	private void DrawSyncableNumbers()
	{
	}

	private void DrawAccumulatingNumbers()
	{
	}

	private void DrawSyncableNumberLists()
	{
	}

	private void DrawHashSets()
	{
	}

	private void InitializeDataMapIfAvailable()
	{
	}

	private void InitSyncableNumbers()
	{
	}

	private void InitSyncableNumberLists()
	{
	}

	private void InitAccumulatingNumbers()
	{
	}

	private void InitHashSets()
	{
	}

	private void OnNewCloudData()
	{
	}
}
