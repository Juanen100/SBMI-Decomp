using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AppCenter.Unity.Crashes.Models;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes
{
	public class Crashes
	{
		public delegate bool UserConfirmationHandler();

		public enum ConfirmationResult
		{
			DontSend = 0,
			Send = 1,
			AlwaysSend = 2
		}

		public delegate bool ShouldProcessErrorReportHandler(ErrorReport errorReport);

		public delegate ErrorAttachmentLog[] GetErrorAttachmentsHandler(ErrorReport errorReport);

		public delegate void SendingErrorReportHandler(ErrorReport errorReport);

		public delegate void SentErrorReportHandler(ErrorReport errorReport);

		public delegate void FailedToSendErrorReportHandler(ErrorReport errorReport, Microsoft.AppCenter.Unity.Crashes.Models.Exception exception);

		public const string CrashesSDKVersion = "2.3.0";

		private static bool _reportUnhandledExceptions;

		private static readonly object _objectLock;

		private static Queue<System.Exception> _unhandledExceptions;

		public static UserConfirmationHandler ShouldAwaitUserConfirmation
		{
			set
			{
			}
		}

		public static ShouldProcessErrorReportHandler ShouldProcessErrorReport
		{
			set
			{
			}
		}

		public static GetErrorAttachmentsHandler GetErrorAttachments
		{
			set
			{
			}
		}

		public static event SendingErrorReportHandler SendingErrorReport
		{
			add
			{
			}
			remove
			{
			}
		}

		public static event SentErrorReportHandler SentErrorReport
		{
			add
			{
			}
			remove
			{
			}
		}

		public static event FailedToSendErrorReportHandler FailedToSendErrorReport
		{
			add
			{
			}
			remove
			{
			}
		}

		public static void PrepareEventHandlers()
		{
		}

		public static void Initialize()
		{
		}

		public static void AddNativeType(List<IntPtr> nativeTypes)
		{
		}

		public static void TrackError(System.Exception exception, IDictionary<string, string> properties = null)
		{
		}

		public static void OnHandleLog(string logString, string stackTrace, LogType type)
		{
		}

		public static void OnHandleUnresolvedException(object sender, UnhandledExceptionEventArgs args)
		{
		}

		public static AppCenterTask<string> GetMinidumpDirectoryAsync()
		{
			return null;
		}

		public static AppCenterTask<bool> HasReceivedMemoryWarningInLastSessionAsync()
		{
			return null;
		}

		public static AppCenterTask<bool> IsEnabledAsync()
		{
			return null;
		}

		public static AppCenterTask SetEnabledAsync(bool enabled)
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

		public static void DisableMachExceptionHandler()
		{
		}

		public static AppCenterTask<ErrorReport> GetLastSessionCrashReportAsync()
		{
			return null;
		}

		public static void ReportUnhandledExceptions(bool enabled)
		{
		}

		public static bool IsReportingUnhandledExceptions()
		{
			return false;
		}

		public static void NotifyUserConfirmation(ConfirmationResult answer)
		{
		}

		public static void StartCrashes()
		{
		}

		private static void SubscribeToUnhandledExceptions()
		{
		}

		private static void UnsubscribeFromUnhandledExceptions()
		{
		}

		private static void HandleAppCenterInitialized()
		{
		}

		[DebuggerHidden]
		private static IEnumerator SendUnhandledExceptionReports()
		{
			return null;
		}

		private static WrapperException CreateWrapperException(System.Exception exception)
		{
			return null;
		}

		private static WrapperException CreateWrapperException(string logString, string stackTrace, LogType type)
		{
			return null;
		}

		private static string GetExceptionWrapperSdkName()
		{
			return null;
		}
	}
}
