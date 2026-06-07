using System;
using System.Reflection;
using Microsoft.AppCenter.Unity;
using UnityEngine;

[Serializable]
public class AppCenterSettingsAdvanced : ScriptableObject
{
	[AppSecret]
	public string TransmissionTargetToken;

	public StartupType AppCenterStartupType;

	public bool StartAndroidNativeSDKFromAppCenterBehavior;

	public bool StartIOSNativeSDKFromAppCenterBehavior;

	private static Assembly AppCenterAssembly
	{
		get
		{
			return null;
		}
	}

	public StartupType GetStartupType()
	{
		return default(StartupType);
	}
}
