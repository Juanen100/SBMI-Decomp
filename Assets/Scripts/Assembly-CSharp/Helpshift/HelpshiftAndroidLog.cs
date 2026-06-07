using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroidLog : IDexLoaderListener, IWorkerMethodDispatcher
	{
		private static AndroidJavaObject logger;

		private static HelpshiftAndroidLog helpshiftAndroidLog;

		private HelpshiftAndroidLog()
		{
		}

		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		public void onDexLoaded()
		{
		}

		private static void initLogger()
		{
		}

		public static int v(string tag, string log)
		{
			return 0;
		}

		public static int d(string tag, string log)
		{
			return 0;
		}

		public static int i(string tag, string log)
		{
			return 0;
		}

		public static int w(string tag, string log)
		{
			return 0;
		}

		public static int e(string tag, string log)
		{
			return 0;
		}
	}
}
