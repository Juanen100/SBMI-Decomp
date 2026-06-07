using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Distribute
{
	public class Distribute
	{
		public const string DistributeSDKVersion = "2.3.0";

		public static ReleaseAvailableCallback ReleaseAvailable { get; set; }

		public static void PrepareEventHandlers()
		{
		}

		public static void AddNativeType(List<IntPtr> nativeTypes)
		{
		}

		public static AppCenterTask<bool> IsEnabledAsync()
		{
			return null;
		}

		public static AppCenterTask SetEnabledAsync(bool enabled)
		{
			return null;
		}

		public static void SetInstallUrl(string installUrl)
		{
		}

		public static void SetApiUrl(string apiUrl)
		{
		}

		public static void NotifyUpdateAction(UpdateAction updateAction)
		{
		}
	}
}
