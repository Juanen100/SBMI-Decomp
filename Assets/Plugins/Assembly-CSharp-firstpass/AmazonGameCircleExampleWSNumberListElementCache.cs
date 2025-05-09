using System.Collections.Generic;
using UnityEngine;

public class AmazonGameCircleExampleWSNumberListElementCache
{
	private const string listElementLabel = "Int {0} : Long {1} : Double {2,5:N1} : String {3}";

	private const string metadataLabel = "Metadata";

	private const string noMetadataAvailableLabel = "No metadata";

	private int valueAsInt;

	private long valueAsLong;

	private double valueAsDouble;

	private string valueAsString = 0.ToString();

	private Dictionary<string, string> metadata;

	public AmazonGameCircleExampleWSNumberListElementCache(int intVal, long longVal, double doubleVal, string stringVal, Dictionary<string, string> elementMetadata)
	{
		valueAsInt = intVal;
		valueAsLong = longVal;
		valueAsDouble = doubleVal;
		valueAsString = stringVal;
		metadata = elementMetadata;
	}

	public void DrawElement()
	{
		GUILayout.BeginVertical(GUI.skin.box);
		string text = string.Format("Int {0} : Long {1} : Double {2,5:N1} : String {3}", valueAsInt, valueAsLong, valueAsDouble, valueAsString);
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(text);
		if (metadata != null && metadata.Count > 0)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Metadata");
			foreach (KeyValuePair<string, string> metadatum in metadata)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(metadatum.ToString());
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No metadata");
		}
		GUILayout.EndVertical();
	}
}
