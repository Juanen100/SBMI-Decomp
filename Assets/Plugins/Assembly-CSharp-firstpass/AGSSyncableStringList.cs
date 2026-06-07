using UnityEngine;

public class AGSSyncableStringList : AGSSyncableList
{
	public AGSSyncableStringList(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableStringList(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableString[] GetValues()
	{
		return null;
	}
}
