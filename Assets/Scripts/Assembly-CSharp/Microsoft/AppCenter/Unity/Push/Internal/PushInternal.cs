using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Push.Internal
{
	internal class PushInternal
	{
		private static AndroidJavaClass _push;

		private static AndroidJavaClass _unityListener;

		public static void PrepareEventHandlers()
		{
		}

		private static void Initialize()
		{
		}

		public static void StartPush()
		{
		}

		private static void PostInitialize()
		{
		}

		public static AppCenterTask SetEnabledAsync(bool isEnabled)
		{
			return null;
		}

		public static AppCenterTask<bool> IsEnabledAsync()
		{
			return null;
		}

		public static void AddNativeType(List<IntPtr> nativeTypes)
		{
		}

		public static void EnableFirebaseAnalytics()
		{
		}

		internal static void ReplayUnprocessedPushNotifications()
		{
		}
	}
}
