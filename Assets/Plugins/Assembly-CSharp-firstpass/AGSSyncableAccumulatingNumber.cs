using UnityEngine;

public class AGSSyncableAccumulatingNumber : AGSSyncable
{
	public AGSSyncableAccumulatingNumber(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableAccumulatingNumber(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public void Increment(long delta)
	{
	}

	public void Increment(double delta)
	{
	}

	public void Increment(int delta)
	{
	}

	public void Increment(string delta)
	{
	}

	public void Decrement(long delta)
	{
	}

	public void Decrement(double delta)
	{
	}

	public void Decrement(int delta)
	{
	}

	public void Decrement(string delta)
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
