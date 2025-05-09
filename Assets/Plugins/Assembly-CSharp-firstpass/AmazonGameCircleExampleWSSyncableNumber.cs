using System.Collections.Generic;
using UnityEngine;

public class AmazonGameCircleExampleWSSyncableNumber
{
	public enum SyncableNumberBehavior
	{
		Highest = 0,
		Lowest = 1,
		Latest = 2
	}

	public enum AvailableSyncableNumberType
	{
		Int = 0,
		Long = 1,
		Double = 2,
		String = 3
	}

	private const string behaviorAndTypeLabel = "{0}:{1}";

	private const string getValueLabel = "Get {0}";

	private const string setValueLabel = "Set {0}";

	private const string setWithMetadataValueLabel = "Set {0} with metadata";

	private const string numberSliderLabel = "{0}";

	private const string unhandledSyncableNumberTypeError = "Whispersync unhandled syncable number type";

	private const string metadataKey = "key";

	private const string metadataValue = "value";

	private const string getMetadataButtonLabel = "Get metadata";

	private const string noMetaDataAvailableLabel = "No metadata set.";

	private const float lowestSliderValue = -10000f;

	private const float highestSlidervalue = 10000f;

	private readonly SyncableNumberBehavior behavior;

	private readonly AvailableSyncableNumberType type;

	private bool foldoutOpen;

	private int intNumber;

	private long longNumber;

	private double doubleNumber;

	private string stringNumber = 0.ToString();

	private Dictionary<string, string> metadataDictionary;

	private readonly Dictionary<string, string> defaultMetadataDictionary = new Dictionary<string, string> { { "key", "value" } };

	public AmazonGameCircleExampleWSSyncableNumber(SyncableNumberBehavior newBehavior, AvailableSyncableNumberType newType)
	{
		behavior = newBehavior;
		type = newType;
	}

	public void DrawGUI(AGSGameDataMap dataMap)
	{
		GUILayout.BeginVertical(GUI.skin.box);
		foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(foldoutOpen, BehaviorAndTypeAsString());
		if (foldoutOpen)
		{
			DrawSlider();
			if (GUILayout.Button(string.Format("Get {0}", BehaviorAndTypeAsString())))
			{
				using (AGSSyncableNumber aGSSyncableNumber = GetSyncableNumber(dataMap))
				{
					if (aGSSyncableNumber != null)
					{
						GetSyncableValue(aGSSyncableNumber);
					}
				}
			}
			GUILayout.Label(GUIContent.none);
			if (GUILayout.Button(string.Format("Set {0}", BehaviorAndTypeAsString())))
			{
				using (AGSSyncableNumber aGSSyncableNumber2 = GetSyncableNumber(dataMap))
				{
					if (aGSSyncableNumber2 != null)
					{
						SetSyncableValue(aGSSyncableNumber2);
					}
				}
			}
			GUILayout.Label(GUIContent.none);
			if (GUILayout.Button(string.Format("Set {0} with metadata", BehaviorAndTypeAsString())))
			{
				using (AGSSyncableNumber aGSSyncableNumber3 = GetSyncableNumber(dataMap))
				{
					if (aGSSyncableNumber3 != null)
					{
						SetSyncableValueWithMetadata(aGSSyncableNumber3);
					}
				}
			}
			GUILayout.Label(GUIContent.none);
			if (GUILayout.Button(string.Format("Get metadata", BehaviorAndTypeAsString())))
			{
				using (AGSSyncableNumber aGSSyncableNumber4 = GetSyncableNumber(dataMap))
				{
					if (aGSSyncableNumber4 != null)
					{
						metadataDictionary = GetMetadata(aGSSyncableNumber4);
					}
				}
			}
			DisplayMetadata();
		}
		GUILayout.EndVertical();
	}

	private void DisplayMetadata()
	{
		if (metadataDictionary != null && metadataDictionary.Count > 0)
		{
			foreach (KeyValuePair<string, string> item in metadataDictionary)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(item.ToString());
			}
			return;
		}
		AmazonGameCircleExampleGUIHelpers.CenteredLabel("No metadata set.");
	}

	private string BehaviorAndTypeAsString()
	{
		return string.Format("{0}:{1}", behavior.ToString(), type.ToString());
	}

	private void DrawSlider()
	{
		switch (type)
		{
		case AvailableSyncableNumberType.Int:
			intNumber = (int)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(intNumber, -10000f, 10000f, "{0}");
			break;
		case AvailableSyncableNumberType.Double:
			doubleNumber = AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider((float)doubleNumber, -10000f, 10000f, "{0}");
			break;
		case AvailableSyncableNumberType.Long:
			longNumber = (long)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(longNumber, -10000f, 10000f, "{0}");
			break;
		case AvailableSyncableNumberType.String:
			if (int.TryParse(stringNumber, out intNumber))
			{
				intNumber = (int)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(intNumber, -10000f, 10000f, "{0}");
				stringNumber = intNumber.ToString();
			}
			else
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(stringNumber);
			}
			break;
		default:
			AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
			break;
		}
	}

	private AGSSyncableNumber GetSyncableNumber(AGSGameDataMap dataMap)
	{
		if (dataMap == null)
		{
			return null;
		}
		string name = BehaviorAndTypeAsString();
		switch (behavior)
		{
		case SyncableNumberBehavior.Highest:
			return dataMap.GetHighestNumber(name);
		case SyncableNumberBehavior.Lowest:
			return dataMap.GetLowestNumber(name);
		case SyncableNumberBehavior.Latest:
			return dataMap.GetLatestNumber(name);
		default:
			AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
			return null;
		}
	}

	private void GetSyncableValue(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber != null)
		{
			switch (type)
			{
			case AvailableSyncableNumberType.Int:
				intNumber = syncableNumber.AsInt();
				break;
			case AvailableSyncableNumberType.Double:
				doubleNumber = syncableNumber.AsDouble();
				break;
			case AvailableSyncableNumberType.Long:
				longNumber = syncableNumber.AsLong();
				break;
			case AvailableSyncableNumberType.String:
				stringNumber = syncableNumber.AsString();
				break;
			default:
				AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
				break;
			}
		}
	}

	private void SetSyncableValue(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber != null)
		{
			switch (type)
			{
			case AvailableSyncableNumberType.Int:
				syncableNumber.Set(intNumber);
				break;
			case AvailableSyncableNumberType.Double:
				syncableNumber.Set(doubleNumber);
				break;
			case AvailableSyncableNumberType.Long:
				syncableNumber.Set(longNumber);
				break;
			case AvailableSyncableNumberType.String:
				syncableNumber.Set(stringNumber);
				break;
			default:
				AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
				break;
			}
		}
	}

	private void SetSyncableValueWithMetadata(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber != null)
		{
			switch (type)
			{
			case AvailableSyncableNumberType.Int:
				syncableNumber.Set(intNumber, defaultMetadataDictionary);
				break;
			case AvailableSyncableNumberType.Double:
				syncableNumber.Set(doubleNumber, defaultMetadataDictionary);
				break;
			case AvailableSyncableNumberType.Long:
				syncableNumber.Set(longNumber, defaultMetadataDictionary);
				break;
			case AvailableSyncableNumberType.String:
				syncableNumber.Set(stringNumber, defaultMetadataDictionary);
				break;
			default:
				AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
				break;
			}
		}
	}

	private Dictionary<string, string> GetMetadata(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber == null)
		{
			return null;
		}
		return syncableNumber.GetMetadata();
	}
}
