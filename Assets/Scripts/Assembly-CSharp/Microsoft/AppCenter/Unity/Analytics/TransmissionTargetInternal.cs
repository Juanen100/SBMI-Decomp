using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Analytics
{
	public class TransmissionTargetInternal
	{
		public static void TrackEvent(AndroidJavaObject transmissionTarget, string eventName)
		{
		}

		public static void TrackEvent(AndroidJavaObject transmissionTarget, string eventName, int flags)
		{
		}

		public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, IDictionary<string, string> properties)
		{
		}

		public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, EventProperties properties)
		{
		}

		public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, IDictionary<string, string> properties, int flags)
		{
		}

		public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, EventProperties properties, int flags)
		{
		}

		public static AppCenterTask SetEnabledAsync(AndroidJavaObject transmissionTarget, bool enabled)
		{
			return null;
		}

		public static AppCenterTask<bool> IsEnabledAsync(AndroidJavaObject transmissionTarget)
		{
			return null;
		}

		public static AndroidJavaObject GetTransmissionTarget(AndroidJavaObject transmissionTargetParent, string transmissionTargetToken, out bool success)
		{
			success = default(bool);
			return null;
		}

		public static AndroidJavaObject GetPropertyConfigurator(AndroidJavaObject transmissionTarget)
		{
			return null;
		}

		public static void Pause(AndroidJavaObject transmissionTarget)
		{
		}

		public static void Resume(AndroidJavaObject transmissionTarget)
		{
		}
	}
}
