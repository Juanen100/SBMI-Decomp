using UnityEngine;

public class AGSLatestStringList : AGSSyncableStringList
{
	public AGSLatestStringList(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSLatestStringList(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}
}
