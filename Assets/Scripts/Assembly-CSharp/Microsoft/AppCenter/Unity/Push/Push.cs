using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Push
{
	public class Push
	{
		public const string PushSDKVersion = "2.3.0";

		private static readonly object _lockObject;

		private static bool _needsReplay;

		private static event EventHandler<PushNotificationReceivedEventArgs> _pushNotificationReceived
		{
			add
			{
			}
			remove
			{
			}
		}

		public static event EventHandler<PushNotificationReceivedEventArgs> PushNotificationReceived
		{
			add
			{
			}
			remove
			{
			}
		}

		public static void StartPush()
		{
		}

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

		public static void EnableFirebaseAnalytics()
		{
		}

		internal static void NotifyPushNotificationReceived(PushNotificationReceivedEventArgs e)
		{
		}
	}
}
