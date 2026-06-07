using System.Collections.Generic;
using UnityEngine;

public class GoogleIABEventListener : MonoBehaviour
{
	public string[] productIds;

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
