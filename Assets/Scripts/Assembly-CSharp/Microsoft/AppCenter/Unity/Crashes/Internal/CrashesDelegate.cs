using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
	public class CrashesDelegate : AndroidJavaProxy
	{
		private static Crashes.UserConfirmationHandler shouldAwaitUserConfirmationHandler;

		private static Crashes.ShouldProcessErrorReportHandler shouldProcessErrorReportHandler;

		private static AndroidJavaClass _errorAttachmentLog;

		private static readonly CrashesDelegate instance;

		public static event Crashes.SendingErrorReportHandler SendingErrorReport
		{
			add
			{
			}
			remove
			{
			}
		}

		public static event Crashes.SentErrorReportHandler SentErrorReport
		{
			add
			{
			}
			remove
			{
			}
		}

		public static event Crashes.FailedToSendErrorReportHandler FailedToSendErrorReport
		{
			add
			{
			}
			remove
			{
			}
		}

		private static event Crashes.GetErrorAttachmentsHandler GetErrorAttachments
		{
			add
			{
			}
			remove
			{
			}
		}

		private CrashesDelegate()
			: base((string)null)
		{
		}

		public static void SetDelegate()
		{
		}

		public void onBeforeSending(AndroidJavaObject report)
		{
		}

		public void onSendingFailed(AndroidJavaObject report, AndroidJavaObject exception)
		{
		}

		public void onSendingSucceeded(AndroidJavaObject report)
		{
		}

		public bool shouldProcess(AndroidJavaObject report)
		{
			return false;
		}

		public bool shouldAwaitUserConfirmation()
		{
			return false;
		}

		private AndroidJavaObject AttachmentWithText(string text, string fileName)
		{
			return null;
		}

		private AndroidJavaObject AttachmentWithBinary(byte[] text, string fileName, string contentType)
		{
			return null;
		}

		public AndroidJavaObject getErrorAttachments(AndroidJavaObject report)
		{
			return null;
		}

		public static void SetShouldAwaitUserConfirmationHandler(Crashes.UserConfirmationHandler handler)
		{
		}

		public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
		{
		}

		public static void SetGetErrorAttachmentsHandler(Crashes.GetErrorAttachmentsHandler handler)
		{
		}
	}
}
