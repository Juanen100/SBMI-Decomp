using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseResponseDelegator : IDelegator
	{
		public readonly PurchaseResponseDelegate responseDelegate;

		public PurchaseResponseDelegator(PurchaseResponseDelegate responseDelegate)
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
