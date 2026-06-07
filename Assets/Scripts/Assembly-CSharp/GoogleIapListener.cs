using System.Collections.Generic;

public class GoogleIapListener : GoogleIABEventListener
{
	public static GoogleIapListener googleIapListener;

	public string[] _productIds;

	public Session session;

	public string _productId;

	public static GoogleIapListener getInstance()
	{
		return null;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void billingSupportedEvent()
	{
	}

	private void billingNotSupportedEvent(string error)
	{
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
	}

	private void listToDictionary(List<GoogleSkuInfo> skus)
	{
	}

	private void queryInventoryFailedEvent(string error)
	{
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
	}

	private void purchaseFailedEvent(string error)
	{
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
	}

	private void consumePurchaseFailedEvent(string error)
	{
	}
}
