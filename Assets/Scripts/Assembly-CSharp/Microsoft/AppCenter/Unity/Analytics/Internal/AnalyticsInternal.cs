using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
	internal class AnalyticsInternal
	{
		private static AndroidJavaClass _analytics;

		public static void PrepareEventHandlers()
		{
		}

		private static void PostInitialize()
		{
		}

		public static void AddNativeType(List<IntPtr> nativeTypes)
		{
		}

		public static void TrackEvent(string eventName)
		{
		}

		public static void TrackEvent(string eventName, int flags)
		{
		}

		public static void TrackEventWithProperties(string eventName, IDictionary<string, string> properties)
		{
		}

		public static void TrackEventWithProperties(string eventName, EventProperties properties)
		{
		}

		public static void TrackEventWithProperties(string eventName, IDictionary<string, string> properties, int flags)
		{
		}

		public static void TrackEventWithProperties(string eventName, EventProperties properties, int flags)
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

		public static AndroidJavaObject GetTransmissionTarget(string transmissionTargetToken, out bool success)
		{
			success = default(bool);
			return null;
		}

		public static void Pause()
		{
		}

		public static void Resume()
		{
		}
	}
}
