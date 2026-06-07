using System.Collections.Generic;
using System.Threading;

namespace Helpshift
{
	public class HelpshiftWorker
	{
		private static HelpshiftWorker hsWorker;

		private Queue<APICallInfo> callerQueue;

		private ManualResetEvent waitHandle;

		private Dictionary<string, IWorkerMethodDispatcher> listeners;

		private Thread workerThread;

		private bool shouldStop;

		private HelpshiftWorker()
		{
		}

		public static HelpshiftWorker getInstance()
		{
			return null;
		}

		public void registerClient(string identifier, IWorkerMethodDispatcher instance)
		{
		}

		public void enqueueApiCall(string instanceIdentifier, string methodIdentifier, string api, object[] args)
		{
		}

		public void synchronousWaitForApiCallQueue()
		{
		}

		public void resolveHsApiCall(APICallInfo apiInfo)
		{
		}

		public void onApplicationQuit()
		{
		}
	}
}
