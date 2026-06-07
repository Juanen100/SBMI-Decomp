using System.Collections.Generic;

public class AmazonGameCircleExampleWSNumberListElementCache
{
	private int valueAsInt;

	private long valueAsLong;

	private double valueAsDouble;

	private string valueAsString;

	private Dictionary<string, string> metadata;

	private const string listElementLabel = "Int {0} : Long {1} : Double {2,5:N1} : String {3}";

	private const string metadataLabel = "Metadata";

	private const string noMetadataAvailableLabel = "No metadata";

	public AmazonGameCircleExampleWSNumberListElementCache(int intVal, long longVal, double doubleVal, string stringVal, Dictionary<string, string> elementMetadata)
	{
	}

	public void DrawElement()
	{
	}
}
