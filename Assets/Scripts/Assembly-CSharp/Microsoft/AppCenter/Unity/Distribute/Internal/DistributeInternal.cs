using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Distribute.Internal
{
	internal class DistributeInternal
	{
		private static AndroidJavaClass _distribute;

		public static void PrepareEventHandlers()
		{
		}

		private static void Initialize()
		{
		}

		private static void StartBehavior()
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

		public static void SetInstallUrl(string installUrl)
		{
		}

		public static void SetApiUrl(string apiUrl)
		{
		}

		public static void NotifyUpdateAction(int updateAction)
		{
		}
	}
}
