using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetProductDataResponseDelegator : IDelegator
	{
		public readonly GetProductDataResponseDelegate responseDelegate;

		public GetProductDataResponseDelegator(GetProductDataResponseDelegate responseDelegate)
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
