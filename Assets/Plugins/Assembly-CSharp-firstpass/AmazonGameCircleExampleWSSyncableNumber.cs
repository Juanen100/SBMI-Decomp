using System.Collections.Generic;

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

	private readonly SyncableNumberBehavior behavior;

	private readonly AvailableSyncableNumberType type;

	private bool foldoutOpen;

	private int intNumber;

	private long longNumber;

	private double doubleNumber;

	private string stringNumber;

	private Dictionary<string, string> metadataDictionary;

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

	private readonly Dictionary<string, string> defaultMetadataDictionary;

	public AmazonGameCircleExampleWSSyncableNumber(SyncableNumberBehavior newBehavior, AvailableSyncableNumberType newType)
	{
	}

	public void DrawGUI(AGSGameDataMap dataMap)
	{
	}

	private void DisplayMetadata()
	{
	}

	private string BehaviorAndTypeAsString()
	{
		return null;
	}

	private void DrawSlider()
	{
	}

	private AGSSyncableNumber GetSyncableNumber(AGSGameDataMap dataMap)
	{
		return null;
	}

	private void GetSyncableValue(AGSSyncableNumber syncableNumber)
	{
	}

	private void SetSyncableValue(AGSSyncableNumber syncableNumber)
	{
	}

	private void SetSyncableValueWithMetadata(AGSSyncableNumber syncableNumber)
	{
	}

	private Dictionary<string, string> GetMetadata(AGSSyncableNumber syncableNumber)
	{
		return null;
	}
}
