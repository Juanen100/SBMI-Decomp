using System.Threading;

namespace Helpshift
{
	public class APICallInfo
	{
		public string instanceIdentifier;

		public string methodIdentifier;

		public string apiName;

		public object[] args;

		public ManualResetEvent resetEvent;

		public APICallInfo(string instanceIdentifier, string methodIdentifier, string apiName, object[] args)
		{
		}

		public APICallInfo(ManualResetEvent resetEvent)
		{
		}
	}
}
