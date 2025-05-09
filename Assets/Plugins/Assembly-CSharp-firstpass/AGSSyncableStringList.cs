using System;
using UnityEngine;

public class AGSSyncableStringList : AGSSyncableList
{
	public AGSSyncableStringList(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableStringList(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableString[] GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject[] array = javaObject.Call<AndroidJavaObject[]>("getValues", new object[0]);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		AGSSyncableString[] array2 = new AGSSyncableString[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = new AGSSyncableString(array[i]);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return array2;
	}
}
