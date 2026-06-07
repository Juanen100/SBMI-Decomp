using Microsoft.AppCenter.Unity;
using Microsoft.AppCenter.Unity.Crashes;
using Microsoft.AppCenter.Unity.Crashes.Models;
using UnityEngine;

public class JavaObjectsConverter
{
	public static ErrorReport ConvertErrorReport(AndroidJavaObject errorReport)
	{
		return null;
	}

	public static Exception ConvertException(AndroidJavaObject throwable)
	{
		return null;
	}

	private static Device ConvertDevice(AndroidJavaObject device)
	{
		return null;
	}

	private static int GetIntValue(AndroidJavaObject javaObject, string getterName)
	{
		return 0;
	}
}
