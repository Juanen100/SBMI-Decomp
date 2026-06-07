using System.Collections.Generic;
using UnityEngine;

namespace Helpshift
{
	public class HelpshiftDexLoader : IWorkerMethodDispatcher
	{
		private static HelpshiftDexLoader dexLoader;

		private static bool isDexLoaded;

		private HashSet<IDexLoaderListener> listeners;

		private AndroidJavaObject application;

		private AndroidJavaClass helpshiftLoaderClass;

		private AndroidJavaClass unityApiDelegateClass;

		private HelpshiftDexLoader()
		{
		}

		public static HelpshiftDexLoader getInstance()
		{
			return null;
		}

		public void loadDex(IDexLoaderListener listener, AndroidJavaObject application)
		{
		}

		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		private void loadHelpshiftDex(AndroidJavaClass helpshiftLoaderClass)
		{
		}

		public void registerListener(IDexLoaderListener listener)
		{
		}

		public AndroidJavaClass getHSDexLoaderJavaClass()
		{
			return null;
		}
	}
}
