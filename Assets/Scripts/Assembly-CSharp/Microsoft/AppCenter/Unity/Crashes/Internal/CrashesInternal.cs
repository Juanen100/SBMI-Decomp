using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
	internal class CrashesInternal
	{
		private static AndroidJavaClass _crashes;

		private static AndroidJavaClass _wrapperSdkExceptionManager;

		public static void AddNativeType(List<IntPtr> nativeTypes)
		{
		}

		public static void TrackException(AndroidJavaObject exception)
		{
		}

		public static void TrackException(AndroidJavaObject exception, IDictionary<string, string> properties)
		{
		}

		public static AppCenterTask<bool> HasReceivedMemoryWarningInLastSessionAsync()
		{
			return null;
		}

		public static AppCenterTask SetEnabledAsync(bool isEnabled)
		{
			return null;
		}

		public static AppCenterTask<bool> IsEnabledAsync()
		{
			return null;
		}

		public static void GenerateTestCrash()
		{
		}

		public static AppCenterTask<bool> HasCrashedInLastSessionAsync()
		{
			return null;
		}

		public static AppCenterTask<ErrorReport> GetLastSessionCrashReportAsync()
		{
			return null;
		}

		public static void DisableMachExceptionHandler()
		{
		}

		public static void SetUserConfirmationHandler(Crashes.UserConfirmationHandler handler)
		{
		}

		public static void NotifyWithUserConfirmation(Crashes.ConfirmationResult answer)
		{
		}

		public static AppCenterTask<string> GetMinidumpDirectoryAsync()
		{
			return null;
		}

		public static void StartCrashes()
		{
		}

		private static int ToJavaConfirmationResult(Crashes.ConfirmationResult answer)
		{
			return 0;
		}
	}
}
