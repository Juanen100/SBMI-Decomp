using UnityEngine;

public class AGSSyncableStringElement : AGSSyncableElement
{
	public AGSSyncableStringElement(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableStringElement(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public string GetValue()
	{
		return javaObject.Call<string>("getValue", new object[0]);
	}
}
