using System.Collections.Generic;

public class GooglePurchase
{
	public enum GooglePurchaseState
	{
		Purchased = 0,
		Canceled = 1,
		Refunded = 2
	}

	public string packageName { get; private set; }

	public string orderId { get; private set; }

	public string productId { get; private set; }

	public string developerPayload { get; private set; }

	public string type { get; private set; }

	public long purchaseTime { get; private set; }

	public GooglePurchaseState purchaseState { get; private set; }

	public string purchaseToken { get; private set; }

	public string signature { get; private set; }

	public string originalJson { get; private set; }

	public GooglePurchase(Dictionary<string, object> dict)
	{
	}

	public static List<GooglePurchase> fromList(List<object> items)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
