using UnityEngine;

public class AmazonGameCircleExampleWSAccumulatingNumber
{
	public enum AvailableAccumulatingNumberType
	{
		Int = 0,
		Long = 1,
		Double = 2,
		String = 3
	}

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

	private readonly AvailableAccumulatingNumberType type;

	private bool foldoutOpen;

	private double? valueAsDouble;

	private long? valueAsLong;

	private int? valueAsInt;

	private string valueAsString;

	public AmazonGameCircleExampleWSAccumulatingNumber(AvailableAccumulatingNumberType newType)
	{
		type = newType;
	}

	public void DrawGUI(AGSGameDataMap dataMap)
	{
		GUILayout.BeginVertical(GUI.skin.box);
		foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(foldoutOpen, SyncableVariableName());
		if (foldoutOpen)
		{
			if (!ValueAvailable())
			{
				RetrieveAccumulatingNumberValue(dataMap);
			}
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(ValueLabel());
			if (GUILayout.Button("Increment"))
			{
				IncrementValue(dataMap);
			}
			if (GUILayout.Button("Decrement"))
			{
				DecrementValue(dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	private void RetrieveAccumulatingNumberValue(AGSGameDataMap dataMap)
	{
		AGSSyncableAccumulatingNumber accumulatingNumber = dataMap.GetAccumulatingNumber(SyncableVariableName());
		switch (type)
		{
		case AvailableAccumulatingNumberType.Double:
			valueAsDouble = accumulatingNumber.AsDouble();
			break;
		case AvailableAccumulatingNumberType.Int:
			valueAsInt = accumulatingNumber.AsInt();
			break;
		case AvailableAccumulatingNumberType.Long:
			valueAsLong = accumulatingNumber.AsLong();
			break;
		case AvailableAccumulatingNumberType.String:
			valueAsString = accumulatingNumber.AsString();
			break;
		}
	}

	private void IncrementValue(AGSGameDataMap dataMap)
	{
		AGSSyncableAccumulatingNumber accumulatingNumber = dataMap.GetAccumulatingNumber(SyncableVariableName());
		switch (type)
		{
		case AvailableAccumulatingNumberType.Double:
			accumulatingNumber.Increment(0.10000000149011612);
			break;
		case AvailableAccumulatingNumberType.Int:
			accumulatingNumber.Increment(1);
			break;
		case AvailableAccumulatingNumberType.Long:
			accumulatingNumber.Increment(1L);
			break;
		case AvailableAccumulatingNumberType.String:
			accumulatingNumber.Increment("1");
			break;
		}
		RetrieveAccumulatingNumberValue(dataMap);
	}

	private void DecrementValue(AGSGameDataMap dataMap)
	{
		AGSSyncableAccumulatingNumber accumulatingNumber = dataMap.GetAccumulatingNumber(SyncableVariableName());
		switch (type)
		{
		case AvailableAccumulatingNumberType.Double:
			accumulatingNumber.Decrement(0.10000000149011612);
			break;
		case AvailableAccumulatingNumberType.Int:
			accumulatingNumber.Decrement(1);
			break;
		case AvailableAccumulatingNumberType.Long:
			accumulatingNumber.Decrement(1L);
			break;
		case AvailableAccumulatingNumberType.String:
			accumulatingNumber.Decrement("1");
			break;
		}
		RetrieveAccumulatingNumberValue(dataMap);
	}

	private string ValueLabel()
	{
		if (!ValueAvailable())
		{
			return "No value available.";
		}
		switch (type)
		{
		case AvailableAccumulatingNumberType.Double:
			return string.Format("Accumulating number value: {0,5:N1}", valueAsDouble.Value);
		case AvailableAccumulatingNumberType.Int:
			return string.Format("Accumulating number value: {0}", valueAsInt.Value);
		case AvailableAccumulatingNumberType.Long:
			return string.Format("Accumulating number value: {0}", valueAsLong.Value);
		case AvailableAccumulatingNumberType.String:
			return string.Format("Accumulating number value: {0}", valueAsString);
		default:
			return "No value available.";
		}
	}

	private bool ValueAvailable()
	{
		switch (type)
		{
		case AvailableAccumulatingNumberType.Double:
			return valueAsDouble.HasValue;
		case AvailableAccumulatingNumberType.Int:
			return valueAsInt.HasValue;
		case AvailableAccumulatingNumberType.Long:
			return valueAsLong.HasValue;
		case AvailableAccumulatingNumberType.String:
			return !string.IsNullOrEmpty(valueAsString);
		default:
			return false;
		}
	}

	private string SyncableVariableName()
	{
		return type.ToString();
	}
}
