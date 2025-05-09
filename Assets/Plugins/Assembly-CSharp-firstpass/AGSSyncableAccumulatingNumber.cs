using UnityEngine;

public class AGSSyncableAccumulatingNumber : AGSSyncable
{
	public AGSSyncableAccumulatingNumber(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableAccumulatingNumber(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public void Increment(long delta)
	{
		javaObject.Call("increment", delta);
	}

	public void Increment(double delta)
	{
		javaObject.Call("increment", delta);
	}

	public void Increment(int delta)
	{
		javaObject.Call("increment", delta);
	}

	public void Increment(string delta)
	{
		javaObject.Call("increment", delta);
	}

	public void Decrement(long delta)
	{
		javaObject.Call("decrement", delta);
	}

	public void Decrement(double delta)
	{
		javaObject.Call("decrement", delta);
	}

	public void Decrement(int delta)
	{
		javaObject.Call("decrement", delta);
	}

	public void Decrement(string delta)
	{
		javaObject.Call("decrement", delta);
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
