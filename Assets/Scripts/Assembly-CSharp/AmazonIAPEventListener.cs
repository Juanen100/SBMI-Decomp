using System.Collections.Generic;
using UnityEngine;
using com.amazon.device.iap.cpt;

public class AmazonIAPEventListener : MonoBehaviour
{
	public Session session;

	public bool isAvailable;

	private string userId;

	public static AmazonIAPEventListener amazonIapListener;

	public static string kSuccessKey;

	public static string kNotSupportedKey;

	public static string kFailedKey;

	public static AmazonIAPEventListener getInstance()
	{
		return null;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void onSdkAvailableEvent(bool isTestMode)
	{
	}

	private void onGetUserDataResponse(GetUserDataResponse args)
	{
	}

	private void onPurchaseResponse(PurchaseResponse args)
	{
	}

	private void onPurchaseSuccessfulEventv2(PurchaseReceipt receipt)
	{
	}

	private void onPurchaseFailedEventv2(string reason)
	{
	}

	private void onPurchaseUpdateResponse(GetPurchaseUpdatesResponse args)
	{
	}

	private void onPurchaseUpdatesRequestSuccessfulEventV2(List<PurchaseReceipt> receipts)
	{
	}

	private void onProductDataResponse(GetProductDataResponse args)
	{
	}

	private void listToDictionary_v2(Dictionary<string, ProductData> availableItems)
	{
	}
}
