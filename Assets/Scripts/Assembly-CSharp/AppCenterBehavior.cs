using System;
using Microsoft.AppCenter.Unity;
using UnityEngine;

public class AppCenterBehavior : MonoBehaviour
{
	private static AppCenterBehavior _instance;

	public AppCenterSettings Settings;

	public static event Action InitializingServices
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action InitializedAppCenterAndServices
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action Started
	{
		add
		{
		}
		remove
		{
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void StartAppCenter()
	{
	}

	private bool IsStartFromAppCenterBehavior(AppCenterBehaviorAdvanced advancedSettings)
	{
		return false;
	}

	private StartupType GetStartupType(AppCenterBehaviorAdvanced advancedSettings)
	{
		return default(StartupType);
	}

	private string GetTransmissionTargetToken(AppCenterBehaviorAdvanced advancedSettings)
	{
		return null;
	}

	private string GetAppSecretString(string appSecret, string transmissionTargetToken, StartupType startupType)
	{
		return null;
	}

	private static void PrepareEventHandlers(Type[] services)
	{
	}

	private static void InvokeInitializingServices()
	{
	}

	private static void InvokeInitializedServices()
	{
	}
}
