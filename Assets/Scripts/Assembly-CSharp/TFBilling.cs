using System;
using System.Collections.Generic;
using UnityEngine;
using com.amazon.device.iap.cpt;

public class TFBilling
{
	public const string PRODUCT_INFO_REQUEST = "productInfo";

	public const string PURCHASE_UPDATE = "purchaseUpdate";

	public const string PURCHASE_COMPLETED = "completed";

	public const string PURCHASE_FAILED = "failed";

	public const string PURCHASE_STARTED = "started";

	public const string PURCHASE_DEFERED = "defered";

	public const string TECHNICAL_FAILURE = "technicalFailure";

	public const string USER_CANCEL = "userCancelled";

	public const string STATE = "state";

	public const string REASON = "reason";

	public const string DESCRIPTION = "description";

	public const string PRODUCT_ID = "productId";

	public const string TOKEN = "token";

	public const string ORDER_ID = "orderId";

	public const string USER_ID = "userId";

	public const string TRANSACTION_ID = "transactionId";

	public const string RECEIPT = "receipt";

	public const string PRODUCTS = "products";

	public const string INVALID_PRODUCTS = "invalidProductIdentifiers";

	public const string LOCALIZED_PRICE = "localizedprice";

	public const string CURRENCY_CODE = "currencyCode";

	public const string PRICE = "price";

	public const string TITLE = "title";

	public static bool BillingIsAvailable()
	{
		return InternalBillingIsAvailable();
	}

	public static void InitializeStore()
	{
		InternalInitializeStore();
	}

	public static void ResetStore()
	{
		InternalResetStore();
	}

	public static void FetchProductBillingInfo(Session session, List<string> productIds)
	{
		InternalFetchBillingInfo(session, productIds);
	}

	public static void StartRmtPurchase(string productId)
	{
		InternalStartRmtPurchase(productId);
	}

	public static void CompleteRmtPurchase(string transactionId)
	{
		InternalCompleteRmtPurchase(transactionId);
	}

	private static bool InternalBillingIsAvailable()
	{
		if (TFUtils.isAmazon())
		{
			return AmazonIAPEventListener.getInstance().isAvailable;
		}
		return GoogleIAB.areSubscriptionsSupported();
	}

	private static void InternalInitializeStore()
	{
		if (TFUtils.isAmazon())
		{
			AmazonIAPEventListener.getInstance();
			IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
		}
		else
		{
			GoogleIAB.init(SBSettings.BillingKey);
		}
	}

	private static void InternalResetStore()
	{
	}

	private static void InternalFetchBillingInfo(Session session, List<string> productIds)
	{
		string[] array = productIds.ToArray();
		if (TFUtils.isAmazon())
		{
			try
			{
				AmazonIAPEventListener.getInstance().session = session;
				for (int i = 0; i < array.Length; i++)
				{
					TFUtils.DebugLog("fetched: " + array[i]);
				}
				SkusInput skusInput = new SkusInput();
				skusInput.Skus = productIds;
				AmazonIapV2Impl.Instance.GetProductData(skusInput);
				return;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message + " : " + ex.StackTrace);
				session.TheGame.store.receivedProductInfo = false;
				RmtStore.IsPurchasing = false;
				return;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			TFUtils.DebugLog("fetched: " + array[j]);
		}
		GoogleIapListener.getInstance()._productIds = array;
		GoogleIapListener.getInstance().session = session;
		TFUtils.DebugLog(GoogleIapListener.getInstance()._productIds.Length);
		if (InternalBillingIsAvailable())
		{
			TFUtils.DebugLog("InternalBillingIsAvailable: " + InternalBillingIsAvailable());
			GoogleIAB.queryInventory(array);
		}
	}

	private static void InternalStartRmtPurchase(string productId)
	{
		if (TFUtils.isAmazon())
		{
			SkuInput skuInput = new SkuInput();
			skuInput.Sku = productId;
			AmazonIapV2Impl.Instance.Purchase(skuInput);
		}
		else
		{
			GoogleIAB.purchaseProduct(productId);
		}
	}

	private static void InternalCompleteRmtPurchase(string transactionId)
	{
		Debug.Log("InternalCompleteRmtPurchase----------------------");
	}
}
