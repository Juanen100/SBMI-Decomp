using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class GoogleIapListener : GoogleIABEventListener
{
	public static GoogleIapListener googleIapListener;

	public string[] _productIds;

	public Session session;

	public string _productId;

	public static GoogleIapListener getInstance()
	{
		if (null == googleIapListener)
		{
			GameObject gameObject = new GameObject();
			googleIapListener = gameObject.AddComponent<GoogleIapListener>();
		}
		return googleIapListener;
	}

	private void OnEnable()
	{
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	}

	private void OnDisable()
	{
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}

	private void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");
		if (GoogleIAB.areSubscriptionsSupported())
		{
			GoogleIAB.queryInventory(_productIds);
		}
	}

	private void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
		if (purchases.Count > 0)
		{
			string[] array = new string[purchases.Count];
			int num = 0;
			foreach (GooglePurchase purchase in purchases)
			{
				array[num++] = purchase.productId;
				TFUtils.DebugLog("--------consume---------------" + purchase.productId);
			}
			GoogleIAB.consumeProducts(array);
		}
		Utils.logObject(purchases);
		Utils.logObject(skus);
		if (!session.TheGame.store.receivedProductInfo)
		{
			listToDictionary(skus);
		}
	}

	private void listToDictionary(List<GoogleSkuInfo> skus)
	{
		session.TheGame.store.rmtProducts = new Dictionary<string, RmtProduct>();
		foreach (GoogleSkuInfo sku in skus)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string productId = sku.productId;
			TFUtils.DebugLog("sku.productId " + sku.productId);
			dictionary.Add("title", productId);
			dictionary.Add("price", sku.price);
			dictionary.Add("localizedprice", sku.price);
			dictionary.Add("description", sku.description);
			dictionary.Add("productId", productId);
			RmtProduct rmtProduct = new RmtProduct(dictionary);
			session.TheGame.store.rmtProducts[rmtProduct.productId] = rmtProduct;
			TFUtils.DebugLog("--------" + rmtProduct.productId);
			TFUtils.DebugLog("--------" + rmtProduct.localizedprice);
		}
		session.TheGame.store.receivedProductInfo = true;
	}

	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		GoogleIAB.consumeProduct(purchase.productId);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("transactionId", purchase.orderId);
		dictionary.Add("productId", purchase.productId);
		dictionary.Add("receipt", purchase.purchaseToken);
		session.TheGame.store.RecordPurchaseCompleted(dictionary, session);
		TFUtils.DebugLog("---------purchaseSucceededEvent-----1-------");
		string iapBundleName = TFUtils.TryLoadString(dictionary, "productId");
		int amount = session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
		session.analytics.LogCompleteInAppPurchase(iapBundleName, amount);
		session.ChangeState("Playing");
		TFUtils.DebugLog("---------purchaseSucceededEvent-----2-------");
	}

	private void purchaseFailedEvent(string error)
	{
		Debug.Log("purchaseFailedEvent: " + error);
		RmtStore.IsPurchasing = false;
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
		RmtStore.IsPurchasing = false;
	}
}
