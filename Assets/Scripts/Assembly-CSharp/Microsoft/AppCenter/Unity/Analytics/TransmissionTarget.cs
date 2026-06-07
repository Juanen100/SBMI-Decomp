using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Analytics
{
	public class TransmissionTarget
	{
		private readonly AndroidJavaObject _rawObject;

		public TransmissionTarget(AndroidJavaObject rawObject)
		{
		}

		internal AndroidJavaObject GetRawObject()
		{
			return null;
		}

		public void TrackEvent(string eventName)
		{
		}

		public void TrackEvent(string eventName, Flags flags)
		{
		}

		public void TrackEvent(string eventName, IDictionary<string, string> properties)
		{
		}

		public void TrackEvent(string eventName, IDictionary<string, string> properties, Flags flags)
		{
		}

		public void TrackEvent(string eventName, EventProperties properties)
		{
		}

		public void TrackEvent(string eventName, EventProperties properties, Flags flags)
		{
		}

		public AppCenterTask<bool> IsEnabledAsync()
		{
			return null;
		}

		public AppCenterTask SetEnabledAsync(bool enabled)
		{
			return null;
		}

		public TransmissionTarget GetTransmissionTarget(string childTransmissionTargetToken)
		{
			return null;
		}

		public PropertyConfigurator GetPropertyConfigurator()
		{
			return null;
		}

		public void Pause()
		{
		}

		public void Resume()
		{
		}
	}
}
