using System.Collections.Generic;

public class AmazonGameCircleExampleWSNumberList
{
	public enum AvailableListType
	{
		HighNumber = 0,
		LowNumber = 1,
		LatestNumber = 2
	}

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

	private readonly Dictionary<string, string> defaultMetadataDictionary;

	public AmazonGameCircleExampleWSNumberList(AvailableListType availableListType)
	{
	}

	public void DrawGUI(AGSGameDataMap dataMap)
	{
	}

	private void InitSyncableNumberList(AGSGameDataMap dataMap)
	{
	}

	private void RefreshList()
	{
	}

	private void AddValuesToList()
	{
	}

	private void AddValuesToListWithMetadata()
	{
	}

	private void IncrementValues()
	{
	}

	private string ListName()
	{
		return null;
	}
}
