using UnityEngine;

namespace Helpshift
{
	public class HelpshiftInternalLogger : IWorkerMethodDispatcher, IDexLoaderListener
	{
		private static string TAG;

		private static HelpshiftInternalLogger internalLoggerInstance;

		private AndroidJavaObject hsInternalLogger;

		private HelpshiftInternalLogger()
		{
		}

		public static HelpshiftInternalLogger getInstance()
		{
			return null;
		}

		private void addApiCallToQueue(string apiName, object[] args)
		{
		}

		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		public void onDexLoaded()
		{
		}

		public void d(string message)
		{
		}

		public void e(string message)
		{
		}

		public void w(string message)
		{
		}

		public void f(string message)
		{
		}
	}
}
