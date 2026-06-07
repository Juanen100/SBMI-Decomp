namespace Microsoft.AppCenter.Unity
{
	public class Device
	{
		public string SdkName { get; private set; }

		public string SdkVersion { get; private set; }

		public string Model { get; private set; }

		public string OemName { get; private set; }

		public string OsName { get; private set; }

		public string OsVersion { get; private set; }

		public string OsBuild { get; private set; }

		public int OsApiLevel { get; private set; }

		public string Locale { get; private set; }

		public int TimeZoneOffset { get; private set; }

		public string ScreenSize { get; private set; }

		public string AppVersion { get; private set; }

		public string CarrierName { get; private set; }

		public string CarrierCountry { get; private set; }

		public string AppBuild { get; private set; }

		public string AppNamespace { get; private set; }

		public Device(string sdkName, string sdkVersion, string model, string oemName, string osName, string osVersion, string osBuild, int osApiLevel, string locale, int timeZoneOffset, string screenSize, string appVersion, string carrierName, string carrierCountry, string appBuild, string appNamespace)
		{
		}
	}
}
