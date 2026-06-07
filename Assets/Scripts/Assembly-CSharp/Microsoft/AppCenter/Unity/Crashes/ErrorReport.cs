using System;
using Microsoft.AppCenter.Unity.Crashes.Models;

namespace Microsoft.AppCenter.Unity.Crashes
{
	public class ErrorReport
	{
		public string Id { get; private set; }

		public DateTimeOffset AppStartTime { get; private set; }

		public DateTimeOffset AppErrorTime { get; private set; }

		public Device Device { get; private set; }

		public Microsoft.AppCenter.Unity.Crashes.Models.Exception Exception { get; private set; }

		public string ThreadName { get; private set; }

		public int ProcessId { get; private set; }

		public string ReporterKey { get; private set; }

		public string ReporterSignal { get; private set; }

		public bool IsAppKill { get; private set; }

		public ErrorReport(string id, DateTimeOffset appStartTime, DateTimeOffset appErrorTime, Microsoft.AppCenter.Unity.Crashes.Models.Exception exception, Device device, string threadName)
		{
		}

		public ErrorReport(string id, DateTimeOffset appStartTime, DateTimeOffset appErrorTime, Microsoft.AppCenter.Unity.Crashes.Models.Exception exception, int processId, string reporterKey, string reporterSignal, bool isAppKill, Device device)
		{
		}
	}
}
