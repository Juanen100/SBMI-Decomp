using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetUserDataResponseDelegator : IDelegator
	{
		public readonly GetUserDataResponseDelegate responseDelegate;

		public GetUserDataResponseDelegator(GetUserDataResponseDelegate responseDelegate)
		{
		}

		public void ExecuteSuccess()
		{
		}

		public void ExecuteSuccess(Dictionary<string, object> objectDictionary)
		{
		}

		public void ExecuteError(AmazonException e)
		{
		}
	}
}
