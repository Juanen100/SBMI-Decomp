using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class AssetServices : MonoBehaviour
{
	public class AssetServicesMonitor
	{
		public bool IsCompleted;

		public object Data;

		public object ServiceData;
	}

	public static GameObject mServiceObject;

	public static int mServiceCounter;

	private static AssetServices CreateService()
	{
		return null;
	}

	public static AssetServicesMonitor CreateUnloadUnusedAssetService(Action callback)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator UnloadUnusedAssets_Coroutine(Action callback, AssetServicesMonitor monitor)
	{
		return null;
	}

	public void CleanupService(AssetServicesMonitor monitor)
	{
	}
}
