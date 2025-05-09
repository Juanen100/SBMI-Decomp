using UnityEngine;

public class AGSSyncableNumberElement : AGSSyncableElement
{
	public AGSSyncableNumberElement(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableNumberElement(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public long AsLong()
	{
		return javaObject.Call<long>("asLong", new object[0]);
	}

	public double AsDouble()
	{
		return javaObject.Call<double>("asDouble", new object[0]);
	}

	public int AsInt()
	{
		return javaObject.Call<int>("asInt", new object[0]);
	}

	public string AsString()
	{
		return javaObject.Call<string>("asString", new object[0]);
	}
}
