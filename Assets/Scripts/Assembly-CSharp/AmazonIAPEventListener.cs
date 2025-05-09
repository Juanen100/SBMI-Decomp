using System;
using System.Collections.Generic;
using UnityEngine;
using com.amazon.device.iap.cpt;

public class AmazonIAPEventListener : MonoBehaviour
{
	public Session session;

	public bool isAvailable;

	private string userId;

	public static AmazonIAPEventListener amazonIapListener;

	public static string kSuccessKey = "SUCCESSFUL";

	public static string kNotSupportedKey = "NOT_SUCCESSFUL";

	public static string kFailedKey = "FAILED";

	public static AmazonIAPEventListener getInstance()
	{
		if (null == amazonIapListener)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "AmazonIAPEventListener";
			amazonIapListener = gameObject.AddComponent<AmazonIAPEventListener>();
		}
		return amazonIapListener;
	}

	private void OnEnable()
	{
		SoaringDebug.Log("AmazonIAPListener: OnEnable");
		IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
		instance.AddGetUserDataResponseListener(onGetUserDataResponse);
		instance.AddPurchaseResponseListener(onPurchaseResponse);
		instance.AddGetPurchaseUpdatesResponseListener(onPurchaseUpdateResponse);
		instance.AddGetProductDataResponseListener(onProductDataResponse);
		instance.GetUserData();
	}

	private void OnDisable()
	{
		SoaringDebug.Log("AmazonIAPListener: OnDisable");
		IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
		instance.RemoveGetUserDataResponseListener(onGetUserDataResponse);
		instance.RemovePurchaseResponseListener(onPurchaseResponse);
		instance.RemoveGetPurchaseUpdatesResponseListener(onPurchaseUpdateResponse);
		instance.RemoveGetProductDataResponseListener(onProductDataResponse);
	}

	private void onSdkAvailableEvent(bool isTestMode)
	{
		Debug.Log("onSdkAvailableEvent. isTestMode: " + isTestMode);
		isAvailable = isTestMode;
	}

	private void onGetUserDataResponse(GetUserDataResponse args)
	{
		string requestId = args.RequestId;
		string marketplace = args.AmazonUserData.Marketplace;
		string status = args.Status;
		userId = args.AmazonUserData.UserId;
		SoaringDebug.Log("onGetUserDataResponse: " + userId + " : " + requestId + " : " + marketplace + " : " + status);
	}

	private void onPurchaseResponse(PurchaseResponse args)
	{
		string status = args.Status;
		Debug.Log("onPurchaseResponse: " + status + " : " + userId);
		if (status == kSuccessKey)
		{
			onPurchaseSuccessfulEventv2(args.PurchaseReceipt);
		}
		else
		{
			onPurchaseFailedEventv2(status);
		}
	}

	private void onPurchaseSuccessfulEventv2(PurchaseReceipt receipt)
	{
		if (session == null)
		{
			Debug.LogError("AmazonIapEventListener: Invalid Session");
			return;
		}
		try
		{
			Debug.Log("onPurchaseSuccessfulEventv2: " + receipt);
			TFUtils.DebugLog("userId:" + userId);
			if (userId == null)
			{
				userId = "DefaultTestUser";
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("userId", userId);
			dictionary.Add("productId", receipt.Sku);
			dictionary.Add("receipt", receipt.ReceiptId);
			session.TheGame.store.RecordPurchaseCompleted(dictionary, session);
			TFUtils.DebugLog("---------purchaseSucceededEvent-----1-------");
			string iapBundleName = TFUtils.TryLoadString(dictionary, "productId");
			int amount = session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
			session.analytics.LogCompleteInAppPurchase(iapBundleName, amount);
			session.ChangeState("Playing");
			TFUtils.DebugLog("---------purchaseSucceededEvent-----2-------");
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("AmazonIAPListener: " + ex.Message, LogType.Error);
		}
	}

	private void onPurchaseFailedEventv2(string reason)
	{
		Debug.Log("onPurchaseFailedEventv2: " + reason);
		RmtStore.IsPurchasing = false;
	}

	private void onPurchaseUpdateResponse(GetPurchaseUpdatesResponse args)
	{
		if (args.Status == kSuccessKey)
		{
			onPurchaseUpdatesRequestSuccessfulEventV2(args.Receipts);
			return;
		}
		Debug.Log("onPurchaseUpdateResponse Failed");
		RmtStore.IsPurchasing = false;
	}

	private void onPurchaseUpdatesRequestSuccessfulEventV2(List<PurchaseReceipt> receipts)
	{
		try
		{
			Debug.Log("onPurchaseUpdateResponse Success. revoked skus: " + receipts.Count);
			foreach (PurchaseReceipt receipt in receipts)
			{
				Debug.Log(receipt);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("AmazonIapEventListener: " + ex.Message);
		}
		if (session == null)
		{
			Debug.LogError("AmazonIapEventListener: Invalid Session");
		}
		else
		{
			session.TheGame.store.receivedProductInfo = true;
		}
	}

	private void onProductDataResponse(GetProductDataResponse args)
	{
		string status = args.Status;
		if (status == kSuccessKey)
		{
			Debug.Log("onProductDataResponse: success");
			string requestId = args.RequestId;
			Dictionary<string, ProductData> productDataMap = args.ProductDataMap;
			if (productDataMap.Count > 0)
			{
				listToDictionary_v2(productDataMap);
			}
		}
		else
		{
			Debug.Log("onProductDataResponse: failed");
			RmtStore.IsPurchasing = false;
		}
	}

	private void listToDictionary_v2(Dictionary<string, ProductData> availableItems)
	{
		if (session == null)
		{
			SoaringDebug.Log("AmazonIAPEventListener: Invalid Session", LogType.Error);
			return;
		}
		session.TheGame.store.rmtProducts = new Dictionary<string, RmtProduct>();
		foreach (string key in availableItems.Keys)
		{
			ProductData productData = availableItems[key];
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string sku = productData.Sku;
			TFUtils.DebugLog("sku.title " + productData.Title);
			dictionary.Add("title", sku);
			dictionary.Add("price", productData.Price);
			dictionary.Add("localizedprice", productData.Price);
			dictionary.Add("description", productData.Description);
			dictionary.Add("productId", sku);
			TFUtils.DebugLog("productId " + sku);
			RmtProduct rmtProduct = new RmtProduct(dictionary);
			session.TheGame.store.rmtProducts[rmtProduct.productId] = rmtProduct;
			TFUtils.DebugLog("--------" + rmtProduct.productId);
			TFUtils.DebugLog("--------" + rmtProduct.localizedprice);
		}
		session.TheGame.store.receivedProductInfo = true;
	}
}
