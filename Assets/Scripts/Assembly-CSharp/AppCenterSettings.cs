using System;
using Microsoft.AppCenter.Unity;
using UnityEngine;

[Serializable]
public class AppCenterSettings : ScriptableObject
{
	[AppSecret]
	public string iOSAppSecret;

	[AppSecret]
	public string AndroidAppSecret;

	[AppSecret]
	public string AmazonAppSecret;

	[AppSecret]
	public string UWPAppSecret;

	public bool UseAnalytics;

	public bool UseCrashes;

	public bool UseDistribute;

	public CustomUrlProperty CustomApiUrl;

	public CustomUrlProperty CustomInstallUrl;

	public bool EnableDistributeForDebuggableBuild;

	public bool UsePush;

	public bool EnableFirebaseAnalytics;

	public LogLevel InitialLogLevel;

	public CustomUrlProperty CustomLogUrl;

	public MaxStorageSizeProperty MaxStorageSize;

	public string AppSecret
	{
		get
		{
			return null;
		}
	}

	public Type[] Services
	{
		get
		{
			return null;
		}
	}
}
