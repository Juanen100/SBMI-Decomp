using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetPurchaseUpdatesResponseDelegator : IDelegator
	{
		public readonly GetPurchaseUpdatesResponseDelegate responseDelegate;

		public GetPurchaseUpdatesResponseDelegator(GetPurchaseUpdatesResponseDelegate responseDelegate)
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
