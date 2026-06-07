using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics
{
	public class Analytics
	{
		public const string AnalyticsSDKVersion = "2.3.0";

		public static void PrepareEventHandlers()
		{
		}

		public static void AddNativeType(List<IntPtr> nativeTypes)
		{
		}

		public static void TrackEvent(string eventName)
		{
		}

		public static void TrackEvent(string eventName, Flags flags)
		{
		}

		public static void TrackEvent(string eventName, IDictionary<string, string> properties)
		{
		}

		public static void TrackEvent(string eventName, IDictionary<string, string> properties, Flags flags)
		{
		}

		public static void TrackEvent(string eventName, EventProperties properties)
		{
		}

		public static void TrackEvent(string eventName, EventProperties properties, Flags flags)
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

		public static TransmissionTarget GetTransmissionTarget(string transmissionTargetToken)
		{
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
