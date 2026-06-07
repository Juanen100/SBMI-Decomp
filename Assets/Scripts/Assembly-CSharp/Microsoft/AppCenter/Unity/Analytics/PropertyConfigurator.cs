using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Analytics
{
	public class PropertyConfigurator
	{
		private readonly AndroidJavaObject _rawObject;

		public PropertyConfigurator(AndroidJavaObject rawObject)
		{
		}

		internal AndroidJavaObject GetRawObject()
		{
			return null;
		}

		public void SetAppName(string appName)
		{
		}

		public void SetAppVersion(string appVersion)
		{
		}

		public void SetAppLocale(string appLocale)
		{
		}

		public void SetEventProperty(string key, string value)
		{
		}

		public void SetEventProperty(string key, DateTime value)
		{
		}

		public void SetEventProperty(string key, long value)
		{
		}

		public void SetEventProperty(string key, double value)
		{
		}

		public void SetEventProperty(string key, bool value)
		{
		}

		public void SetUserId(string userId)
		{
		}

		public void RemoveEventProperty(string key)
		{
		}

		public void CollectDeviceId()
		{
		}
	}
}
