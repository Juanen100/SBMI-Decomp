using UnityEngine;

public class AGSSyncableNumberElement : AGSSyncableElement
{
	public AGSSyncableNumberElement(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableNumberElement(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public long AsLong()
	{
		return 0L;
	}

	public double AsDouble()
	{
		return 0.0;
	}

	public int AsInt()
	{
		return 0;
	}

	public string AsString()
	{
		return null;
	}
}
