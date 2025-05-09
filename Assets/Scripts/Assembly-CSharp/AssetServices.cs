using System;
using System.Collections;
using UnityEngine;

public class AssetServices : MonoBehaviour
{
	public class AssetServicesMonitor
	{
		public volatile bool IsCompleted;

		public object Data;

		public object ServiceData;
	}

	public static GameObject mServiceObject;

	public static volatile int mServiceCounter;

	private static AssetServices CreateService()
	{
		if (mServiceObject == null)
		{
			mServiceObject = new GameObject("AssetServices");
		}
		AssetServices assetServices = mServiceObject.AddComponent<AssetServices>();
		if (assetServices != null)
		{
			mServiceCounter++;
		}
		return assetServices;
	}

	public static AssetServicesMonitor CreateUnloadUnusedAssetService(Action callback)
	{
		AssetServices assetServices = CreateService();
		AssetServicesMonitor assetServicesMonitor = new AssetServicesMonitor();
		if (assetServices == null)
		{
			if (callback != null)
			{
				callback();
			}
			assetServicesMonitor.IsCompleted = true;
			return assetServicesMonitor;
		}
		assetServices.StartCoroutine(assetServices.UnloadUnusedAssets_Coroutine(callback, assetServicesMonitor));
		return assetServicesMonitor;
	}

	private IEnumerator UnloadUnusedAssets_Coroutine(Action callback, AssetServicesMonitor monitor)
	{
		yield return Resources.UnloadUnusedAssets();
		if (callback != null)
		{
			callback();
		}
		CleanupService(monitor);
	}

	public void CleanupService(AssetServicesMonitor monitor)
	{
		monitor.IsCompleted = true;
		mServiceCounter--;
		if (mServiceCounter <= 0)
		{
			UnityEngine.Object.Destroy(mServiceObject);
			mServiceObject = null;
		}
	}
}
