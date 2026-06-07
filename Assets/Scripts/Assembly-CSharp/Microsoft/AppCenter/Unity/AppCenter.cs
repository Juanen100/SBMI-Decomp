using System;
using System.Reflection;

namespace Microsoft.AppCenter.Unity
{
	public class AppCenter
	{
		public delegate void SetMaxStorageSizeCompletionHandler(bool result);

		private static AppCenterTask<string> _secretTask;

		private static AppCenterTask<string> _logUrlTask;

		private static AppCenterTask<long> _storageSizeTask;

		public static LogLevel LogLevel
		{
			get
			{
				return default(LogLevel);
			}
			set
			{
			}
		}

		public static bool Configured
		{
			get
			{
				return false;
			}
		}

		public static Type Analytics
		{
			get
			{
				return null;
			}
		}

		public static Type Crashes
		{
			get
			{
				return null;
			}
		}

		public static Type Distribute
		{
			get
			{
				return null;
			}
		}

		public static Type Push
		{
			get
			{
				return null;
			}
		}

		private static Assembly AppCenterAssembly
		{
			get
			{
				return null;
			}
		}

		public static AppCenterTask SetEnabledAsync(bool enabled)
		{
			return null;
		}

		public static void StartFromLibrary(Type[] servicesArray)
		{
		}

		public static AppCenterTask<bool> IsEnabledAsync()
		{
			return null;
		}

		public static AppCenterTask<Guid?> GetInstallIdAsync()
		{
			return null;
		}

		public static string GetSdkVersion()
		{
			return null;
		}

		public static AppCenterTask<string> GetLogUrlAsync()
		{
			return null;
		}

		public static AppCenterTask<long> GetStorageSizeAsync()
		{
			return null;
		}

		public static void SetLogUrl(string logUrl)
		{
		}

		public static void CacheStorageSize(long storageSize)
		{
		}

		public static void CacheLogUrl(string logUrl)
		{
		}

		public static void SetCustomProperties(CustomProperties customProperties)
		{
		}

		public static void SetWrapperSdk()
		{
		}

		public static AppCenterTask<string> GetSecretForPlatformAsync()
		{
			return null;
		}

		public static string ParseAndSaveSecretForPlatform(string secrets)
		{
			return null;
		}

		public static void SetUserId(string userId)
		{
		}

		private static string GetPlatformIdentifier()
		{
			return null;
		}
	}
}
