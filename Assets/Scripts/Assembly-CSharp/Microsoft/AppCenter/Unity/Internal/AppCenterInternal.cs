using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Internal
{
	internal class AppCenterInternal
	{
		private static AndroidJavaClass _appCenter;

		public static void SetLogLevel(int logLevel)
		{
		}

		public static int GetLogLevel()
		{
			return 0;
		}

		public static bool IsConfigured()
		{
			return false;
		}

		public static void SetLogUrl(string logUrl)
		{
		}

		public static void SetUserId(string userId)
		{
		}

		public static string GetSdkVersion()
		{
			return null;
		}

		public static AppCenterTask SetEnabledAsync(bool enabled)
		{
			return null;
		}

		public static AppCenterTask<bool> IsEnabledAsync()
		{
			return null;
		}

		public static AppCenterTask<string> GetInstallIdAsync()
		{
			return null;
		}

		public static void SetCustomProperties(AndroidJavaObject properties)
		{
		}

		private static AndroidJavaObject GetAndroidApplication()
		{
			return null;
		}

		public static void SetWrapperSdk(string wrapperSdkVersion, string wrapperSdkName, string wrapperRuntimeVersion, string liveUpdateReleaseLabel, string liveUpdateDeploymentKey, string liveUpdatePackageHash)
		{
		}

		public static void Start(string appSecret, Type[] services)
		{
		}

		public static void Start(Type[] services)
		{
		}

		public static void Start(Type service)
		{
		}

		public static void StartFromLibrary(IntPtr servicesArray)
		{
		}

		public static IntPtr ServicesToNativeTypes(Type[] services)
		{
			return (IntPtr)0;
		}

		public static void SetMaxStorageSize(long size)
		{
		}
	}
}
