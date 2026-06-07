using System.Collections.Generic;

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

	public const string SIGNITURE = "pro_signiture";

	public const string DATA = "pro_data";

	public const string RESPONSE = "pro_response";

	public static bool BillingIsAvailable()
	{
		return false;
	}

	public static void InitializeStore()
	{
	}

	public static void ResetStore()
	{
	}

	public static void FetchProductBillingInfo(Session session, List<string> productIds)
	{
	}

	public static void StartRmtPurchase(string productId)
	{
	}

	public static void CompleteRmtPurchase(string transactionId)
	{
	}

	private static bool InternalBillingIsAvailable()
	{
		return false;
	}

	private static void InternalInitializeStore()
	{
	}

	private static void InternalResetStore()
	{
	}

	private static void InternalFetchBillingInfo(Session session, List<string> productIds)
	{
	}

	private static void InternalStartRmtPurchase(string productId)
	{
	}

	private static void InternalCompleteRmtPurchase(string transactionId)
	{
	}
}
