using System.Collections.Generic;
using UnityEngine;

public class AmazonGameCircleExampleWSNumberList
{
	public enum AvailableListType
	{
		HighNumber = 0,
		LowNumber = 1,
		LatestNumber = 2
	}

	private const string notInitializedLabel = "Syncable number list not yet initialized";

	private const string refreshSyncableNumberElementsButtonLabel = "Refresh List";

	private const string emptyListLabel = "List is empty";

	private const string addValuesButtonLabel = "Add values";

	private const string metadataKey = "key";

	private const string metadataValue = "value";

	private const string maxSizeLabel = "Max Size {0}";

	private const string updateMaxSizeButtonLabel = "Update Max Size";

	private const string isListSetLabel = "Has list been set yet? {0}";

	private const int intIncrement = 1;

	private const long longIncrement = -5L;

	private const double doubleIncrement = 0.1;

	private const int stringMultiplier = 2;

	private const int minMaxSize = 3;

	private const int maxMaxSize = 8;

	private readonly AvailableListType listType;

	private AGSSyncableNumberList syncableNumberList;

	private AGSSyncableNumberElement[] syncableNumberElements;

	private AmazonGameCircleExampleWSNumberListElementCache[] syncableNumberElementsCache;

	private bool isSet;

	private int intVal;

	private long longVal;

	private double doubleVal;

	private int? maxSize;

	private bool foldout;

	private readonly Dictionary<string, string> defaultMetadataDictionary = new Dictionary<string, string> { { "key", "value" } };

	public AmazonGameCircleExampleWSNumberList(AvailableListType availableListType)
	{
		listType = availableListType;
	}

	public void DrawGUI(AGSGameDataMap dataMap)
	{
		if (dataMap == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Syncable number list not yet initialized");
			return;
		}
		if (syncableNumberList == null)
		{
			InitSyncableNumberList(dataMap);
		}
		if (syncableNumberList == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Syncable number list not yet initialized");
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box);
		foldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(foldout, ListName());
		if (foldout)
		{
			if (GUILayout.Button("Refresh List"))
			{
				RefreshList();
			}
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("Has list been set yet? {0}", isSet));
			if (maxSize.HasValue)
			{
				maxSize = (int)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(maxSize.Value, 3f, 8f, "Max Size {0}");
				if (GUILayout.Button("Update Max Size"))
				{
					syncableNumberList.SetMaxSize(maxSize.Value);
				}
			}
			if (syncableNumberElementsCache == null || syncableNumberElementsCache.Length == 0)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel("List is empty");
			}
			else
			{
				AmazonGameCircleExampleWSNumberListElementCache[] array = syncableNumberElementsCache;
				foreach (AmazonGameCircleExampleWSNumberListElementCache amazonGameCircleExampleWSNumberListElementCache in array)
				{
					amazonGameCircleExampleWSNumberListElementCache.DrawElement();
				}
			}
			if (GUILayout.Button("Add values"))
			{
				AddValuesToList();
				IncrementValues();
				AddValuesToListWithMetadata();
				IncrementValues();
				RefreshList();
			}
		}
		GUILayout.EndVertical();
	}

	private void InitSyncableNumberList(AGSGameDataMap dataMap)
	{
		switch (listType)
		{
		case AvailableListType.HighNumber:
			syncableNumberList = dataMap.GetHighNumberList(ListName());
			break;
		case AvailableListType.LatestNumber:
			syncableNumberList = dataMap.GetLatestNumberList(ListName());
			break;
		case AvailableListType.LowNumber:
			syncableNumberList = dataMap.GetLowNumberList(ListName());
			break;
		}
		maxSize = syncableNumberList.GetMaxSize();
		isSet = syncableNumberList.IsSet();
	}

	private void RefreshList()
	{
		if (syncableNumberList != null)
		{
			maxSize = syncableNumberList.GetMaxSize();
			isSet = syncableNumberList.IsSet();
			syncableNumberElements = syncableNumberList.GetValues();
			syncableNumberElementsCache = new AmazonGameCircleExampleWSNumberListElementCache[syncableNumberElements.Length];
			for (int i = 0; i < syncableNumberElements.Length; i++)
			{
				syncableNumberElementsCache[i] = new AmazonGameCircleExampleWSNumberListElementCache(syncableNumberElements[i].AsInt(), syncableNumberElements[i].AsLong(), syncableNumberElements[i].AsDouble(), syncableNumberElements[i].AsString(), syncableNumberElements[i].GetMetadata());
			}
		}
	}

	private void AddValuesToList()
	{
		syncableNumberList.Add(intVal);
		syncableNumberList.Add(longVal);
		syncableNumberList.Add(doubleVal);
		syncableNumberList.Add((intVal * 2).ToString());
	}

	private void AddValuesToListWithMetadata()
	{
		syncableNumberList.Add(intVal, defaultMetadataDictionary);
		syncableNumberList.Add(longVal, defaultMetadataDictionary);
		syncableNumberList.Add(doubleVal, defaultMetadataDictionary);
		syncableNumberList.Add((intVal * 2).ToString(), defaultMetadataDictionary);
	}

	private void IncrementValues()
	{
		intVal++;
		longVal += -5L;
		doubleVal += 0.1;
	}

	private string ListName()
	{
		return listType.ToString();
	}
}
