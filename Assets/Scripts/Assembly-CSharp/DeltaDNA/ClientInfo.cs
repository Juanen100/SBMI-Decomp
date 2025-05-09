using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DeltaDNA
{
	internal static class ClientInfo
	{
		private static string platform;

		private static string deviceName;

		private static string deviceModel;

		private static string deviceType;

		private static string operatingSystem;

		private static string operatingSystemVersion;

		private static string manufacturer;

		private static string timezoneOffset;

		private static string countryCode;

		private static string languageCode;

		private static string locale;

		public static string Platform
		{
			get
			{
				return platform ?? (platform = GetPlatform());
			}
		}

		public static string DeviceName
		{
			get
			{
				return deviceName ?? (deviceName = GetDeviceName());
			}
		}

		public static string DeviceModel
		{
			get
			{
				return deviceModel ?? (deviceModel = GetDeviceModel());
			}
		}

		public static string DeviceType
		{
			get
			{
				return deviceType ?? (deviceType = GetDeviceType());
			}
		}

		public static string OperatingSystem
		{
			get
			{
				return operatingSystem ?? (operatingSystem = GetOperatingSystem());
			}
		}

		public static string OperatingSystemVersion
		{
			get
			{
				return operatingSystemVersion ?? (operatingSystemVersion = GetOperatingSystemVersion());
			}
		}

		public static string Manufacturer
		{
			get
			{
				return manufacturer ?? (manufacturer = GetManufacturer());
			}
		}

		public static string TimezoneOffset
		{
			get
			{
				return timezoneOffset ?? (timezoneOffset = GetCurrentTimezoneOffset());
			}
		}

		public static string CountryCode
		{
			get
			{
				return countryCode ?? (countryCode = GetCountryCode());
			}
		}

		public static string LanguageCode
		{
			get
			{
				return languageCode ?? (languageCode = GetLanguageCode());
			}
		}

		public static string Locale
		{
			get
			{
				return locale ?? (locale = GetLocale());
			}
		}

		private static string GetPlatform()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				return "ANDROID";
			case RuntimePlatform.FlashPlayer:
				return "WEB";
			case RuntimePlatform.LinuxPlayer:
				return "PC_CLIENT";
			case RuntimePlatform.MetroPlayerARM:
				return "WINDOWS_TABLET";
			case RuntimePlatform.MetroPlayerX64:
				return "WINDOWS_TABLET";
			case RuntimePlatform.MetroPlayerX86:
				return "WINDOWS_TABLET";
			case RuntimePlatform.NaCl:
				return "WEB";
			case RuntimePlatform.OSXEditor:
				return "MAC_CLIENT";
			case RuntimePlatform.OSXPlayer:
				return "MAC_CLIENT";
			case RuntimePlatform.PS3:
				return "PS3";
			case RuntimePlatform.TizenPlayer:
				return "ANDROID";
			case RuntimePlatform.WindowsEditor:
				return "PC_CLIENT";
			case RuntimePlatform.WindowsPlayer:
				return "PC_CLIENT";
			case RuntimePlatform.WP8Player:
				return "WINDOWS_MOBILE";
			case RuntimePlatform.XBOX360:
				return "XBOX360";
			default:
				Debug.LogWarning(string.Concat("Unsupported platform '", Application.platform, "' returning UNKNOWN"));
				return "UNKNOWN";
			}
		}

		private static string GetDeviceName()
		{
			string text = SystemInfo.deviceModel;
			switch (text)
			{
			case "iPhone1,1":
				return "iPhone 1G";
			case "iPhone1,2":
				return "iPhone 3G";
			case "iPhone2,1":
				return "iPhone 3GS";
			case "iPhone3,1":
				return "iPhone 4";
			case "iPhone3,2":
				return "iPhone 4";
			case "iPhone3,3":
				return "iPhone 4";
			case "iPhone4,1":
				return "iPhone 4S";
			case "iPhone5,1":
				return "iPhone 5";
			case "iPhone5,2":
				return "iPhone 5";
			case "iPhone5,3":
				return "iPhone 5C";
			case "iPhone5,4":
				return "iPhone 5C";
			case "iPhone6,1":
				return "iPhone 5S";
			case "iPhone6,2":
				return "iPhone 5S";
			case "iPhone7,2":
				return "iPhone 6";
			case "iPhone7,1":
				return "iPhone 6 Plus";
			case "iPod1,1":
				return "iPod Touch 1G";
			case "iPod2,1":
				return "iPod Touch 2G";
			case "iPod3,1":
				return "iPod Touch 3G";
			case "iPod4,1":
				return "iPod Touch 4G";
			case "iPod5,1":
				return "iPod Touch 5G";
			case "iPad1,1":
				return "iPad 1G";
			case "iPad2,1":
				return "iPad 2";
			case "iPad2,2":
				return "iPad 2";
			case "iPad2,3":
				return "iPad 2";
			case "iPad2,4":
				return "iPad 2";
			case "iPad3,1":
				return "iPad 3G";
			case "iPad3,2":
				return "iPad 3G";
			case "iPad3,3":
				return "iPad 3G";
			case "iPad3,4":
				return "iPad 4G";
			case "iPad3,5":
				return "iPad 4G";
			case "iPad3,6":
				return "iPad 4G";
			case "iPad4,1":
				return "iPad Air";
			case "iPad4,2":
				return "iPad Air";
			case "iPad4,3":
				return "iPad Air";
			case "iPad5,3":
				return "iPad Air 2";
			case "iPad5,4":
				return "iPad Air 2";
			case "iPad2,5":
				return "iPad Mini 1G";
			case "iPad2,6":
				return "iPad Mini 1G";
			case "iPad2,7":
				return "iPad Mini 1G";
			case "iPad4,4":
				return "iPad Mini 2G";
			case "iPad4,5":
				return "iPad Mini 2G";
			case "iPad4,6":
				return "iPad Mini 2G";
			case "iPad4,7":
				return "iPad Mini 3";
			case "iPad4,8":
				return "iPad Mini 3";
			case "iPad4,9":
				return "iPad Mini 3";
			case "Amazon KFSAWA":
				return "Fire HDX 8.9 (4th Gen)";
			case "Amazon KFASWI":
				return "Fire HD 7 (4th Gen)";
			case "Amazon KFARWI":
				return "Fire HD 6 (4th Gen)";
			case "Amazon KFAPWA":
			case "Amazon KFAPWI":
				return "Kindle Fire HDX 8.9 (3rd Gen)";
			case "Amazon KFTHWA":
			case "Amazon KFTHWI":
				return "Kindle Fire HDX 7 (3rd Gen)";
			case "Amazon KFSOWI":
				return "Kindle Fire HD 7 (3rd Gen)";
			case "Amazon KFJWA":
			case "Amazon KFJWI":
				return "Kindle Fire HD 8.9 (2nd Gen)";
			case "Amazon KFTT":
				return "Kindle Fire HD 7 (2nd Gen)";
			case "Amazon KFOT":
				return "Kindle Fire (2nd Gen)";
			case "Amazon Kindle Fire":
				return "Kindle Fire (1st Gen)";
			default:
				return text;
			}
		}

		private static string GetDeviceModel()
		{
			return deviceModel;
		}

		private static string GetDeviceType()
		{
			switch (SystemInfo.deviceType)
			{
			case UnityEngine.DeviceType.Console:
				return "CONSOLE";
			case UnityEngine.DeviceType.Desktop:
				return "PC";
			case UnityEngine.DeviceType.Handheld:
				return "HANDHELD";
			case UnityEngine.DeviceType.Unknown:
				return "UNKOWN";
			default:
				return "UNKNOWN";
			}
		}

		private static string GetOperatingSystem()
		{
			string text = SystemInfo.operatingSystem.ToUpper();
			if (text.Contains("WINDOWS"))
			{
				return "WINDOWS";
			}
			if (text.Contains("OSX"))
			{
				return "OSX";
			}
			if (text.Contains("MAC"))
			{
				return "OSX";
			}
			if (text.Contains("IOS"))
			{
				return "IOS";
			}
			if (text.Contains("LINUX"))
			{
				return "LINUX";
			}
			if (text.Contains("ANDROID"))
			{
				return "ANDROID";
			}
			if (text.Contains("BLACKBERRY"))
			{
				return "BLACKBERRY";
			}
			return "UNKNOWN";
		}

		private static string GetOperatingSystemVersion()
		{
			string pattern = "^\\w+";
			Regex regex = new Regex(pattern);
			string input = SystemInfo.operatingSystem;
			return regex.Replace(input, string.Empty).Trim();
		}

		private static string GetManufacturer()
		{
			return null;
		}

		private static string GetCurrentTimezoneOffset()
		{
			try
			{
				TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
				DateTime now = DateTime.Now;
				TimeSpan utcOffset = currentTimeZone.GetUtcOffset(now);
				return string.Format("{0}{1:D2}", (utcOffset.Hours < 0) ? string.Empty : "+", utcOffset.Hours);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static string GetCountryCode()
		{
			return null;
		}

		private static string GetLanguageCode()
		{
			switch (Application.systemLanguage)
			{
			case SystemLanguage.Afrikaans:
				return "af";
			case SystemLanguage.Arabic:
				return "ar";
			case SystemLanguage.Basque:
				return "eu";
			case SystemLanguage.Belarusian:
				return "be";
			case SystemLanguage.Bulgarian:
				return "bg";
			case SystemLanguage.Catalan:
				return "ca";
			case SystemLanguage.Chinese:
				return "zh";
			case SystemLanguage.Czech:
				return "cs";
			case SystemLanguage.Danish:
				return "da";
			case SystemLanguage.Dutch:
				return "nl";
			case SystemLanguage.English:
				return "en";
			case SystemLanguage.Estonian:
				return "et";
			case SystemLanguage.Faroese:
				return "fo";
			case SystemLanguage.Finnish:
				return "fi";
			case SystemLanguage.French:
				return "fr";
			case SystemLanguage.German:
				return "de";
			case SystemLanguage.Greek:
				return "el";
			case SystemLanguage.Hebrew:
				return "he";
			case SystemLanguage.Hungarian:
				return "hu";
			case SystemLanguage.Icelandic:
				return "is";
			case SystemLanguage.Indonesian:
				return "id";
			case SystemLanguage.Italian:
				return "it";
			case SystemLanguage.Japanese:
				return "ja";
			case SystemLanguage.Korean:
				return "ko";
			case SystemLanguage.Latvian:
				return "lv";
			case SystemLanguage.Lithuanian:
				return "lt";
			case SystemLanguage.Norwegian:
				return "nn";
			case SystemLanguage.Polish:
				return "pl";
			case SystemLanguage.Portuguese:
				return "pt";
			case SystemLanguage.Romanian:
				return "ro";
			case SystemLanguage.Russian:
				return "ru";
			case SystemLanguage.SerboCroatian:
				return "sr";
			case SystemLanguage.Slovak:
				return "sk";
			case SystemLanguage.Slovenian:
				return "sl";
			case SystemLanguage.Spanish:
				return "es";
			case SystemLanguage.Swedish:
				return "sv";
			case SystemLanguage.Thai:
				return "th";
			case SystemLanguage.Turkish:
				return "tr";
			case SystemLanguage.Ukrainian:
				return "uk";
			case SystemLanguage.Vietnamese:
				return "vi";
			default:
				return "en";
			}
		}

		private static string GetLocale()
		{
			if (CountryCode != null)
			{
				return string.Format("{0}_{1}", LanguageCode, CountryCode);
			}
			return string.Format("{0}_ZZ", LanguageCode);
		}
	}
}
