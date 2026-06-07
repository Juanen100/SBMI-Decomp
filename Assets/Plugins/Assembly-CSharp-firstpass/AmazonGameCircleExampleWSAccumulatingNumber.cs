public class AmazonGameCircleExampleWSAccumulatingNumber
{
	public enum AvailableAccumulatingNumberType
	{
		Int = 0,
		Long = 1,
		Double = 2,
		String = 3
	}

	private readonly AvailableAccumulatingNumberType type;

	private bool foldoutOpen;

	private double? valueAsDouble;

	private long? valueAsLong;

	private int? valueAsInt;

	private string valueAsString;

	private const string incrementButtonLabel = "Increment";

	private const string decrementButtonLabel = "Decrement";

	private const string accumulatingNumberValueLabel = "Accumulating number value: {0}";

	private const string accumulatingNumberDoubleValueLabel = "Accumulating number value: {0,5:N1}";

	private const string noAccumulatingNumberLabel = "No value available.";

	private const string unableToParseValueAsStringError = "Unable to parse accumulating number.";

	private const double doubleIncrementValue = 0.10000000149011612;

	private const int intIncrementValue = 1;

	private const long longIncrementValue = 1L;

	private const string stringIncrementValue = "1";

	public AmazonGameCircleExampleWSAccumulatingNumber(AvailableAccumulatingNumberType newType)
	{
	}

	public void DrawGUI(AGSGameDataMap dataMap)
	{
	}

	private void RetrieveAccumulatingNumberValue(AGSGameDataMap dataMap)
	{
	}

	private void IncrementValue(AGSGameDataMap dataMap)
	{
	}

	private void DecrementValue(AGSGameDataMap dataMap)
	{
	}

	private string ValueLabel()
	{
		return null;
	}

	private bool ValueAvailable()
	{
		return false;
	}

	private string SyncableVariableName()
	{
		return null;
	}
}
