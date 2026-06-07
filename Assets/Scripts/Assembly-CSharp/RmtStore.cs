using System;
using System.Collections.Generic;

public class RmtStore
{
	public class HandleProductsDelegate : SoaringDelegate
	{
		public Session session;

		public static SoaringContext CreateDelegate(Session session, string name = null, SoaringContextDelegate del = null, SoaringObjectBase passthrough = null)
		{
			return null;
		}

		public override void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
		{
		}

		public override void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
		{
		}

		public override void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
		{
		}
	}

	public delegate void StoreEventHandler(object sender, StoreEventArgs args);

	public class StoreEventArgs : EventArgs
	{
		public Dictionary<string, object> results;

		public StoreEventArgs(Dictionary<string, object> res)
		{
		}
	}

	private static string mTRANSACTION_LOG;

	public const float STORE_TIMEOUT = 15f;

	public bool rmtEnabled;

	public Dictionary<string, RmtProduct> rmtProducts;

	public Dictionary<string, SoaringPurchasable> soaringProducts;

	public bool receivedProductInfo;

	public bool receivedPurchaseInfo;

	private string txProductId;

	public static bool IsPurchasing;

	private Dictionary<string, Dictionary<string, object>> pendingTransactions;

	private static string TRANSACTION_LOG
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool RmtReady
	{
		get
		{
			return false;
		}
	}

	public event StoreEventHandler ProductInfoReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoreEventHandler PurchaseUpdateReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoreEventHandler PurchaseResponseReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoreEventHandler GetProductInfoResponseReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoreEventHandler PurchaseReceiptReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoreEventHandler PurchaseInfoReceived
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoreEventHandler PurchaseError
	{
		add
		{
		}
		remove
		{
		}
	}

	public event StoreEventHandler PurchaseDefered
	{
		add
		{
		}
		remove
		{
		}
	}

	public RmtStore(bool rmtEnabled, Dictionary<string, Dictionary<string, object>> pendingTransactions)
	{
	}

	public void OnProductInfoReceived(Dictionary<string, object> results, object userDarta)
	{
	}

	public void OnPurchaseUpdateReceived(Dictionary<string, object> results, object userDarta)
	{
	}

	public void OnPurchaseReceiptReceived(Dictionary<string, object> results, object userDarta)
	{
	}

	public void OnPurchaseResponseReceived(Dictionary<string, object> results, object userDarta)
	{
	}

	public void OnGetProductInfoResponseReceived(Dictionary<string, object> results, object userDarta)
	{
	}

	public void OnPurchaseInfoReceived(Dictionary<string, object> results, object userDarta)
	{
	}

	public void OnPurchaseError(Dictionary<string, object> results, object userDarta)
	{
	}

	public void OnPurchaseDefered(Dictionary<string, object> results, object userDarta)
	{
	}

	public static bool PreloadRmtProducts(Session session)
	{
		return false;
	}

	public static Cost CostFromCollection(Session session, List<object> sales, string field)
	{
		return null;
	}

	public static RmtStore LoadFromFilesystem(bool rmtEnabled)
	{
		return null;
	}

	public void Init(Session session)
	{
	}

	public void Start()
	{
	}

	public void Reset(Session session)
	{
	}

	public bool LoadRmtProductInfo(Catalog catalog, Dictionary<string, object> rawRmtProductInfo)
	{
		return false;
	}

	public bool LoadRmtProductInfo(SoaringPurchasable[] pPurchasables)
	{
		return false;
	}

	public void OpenTransaction(string productId)
	{
	}

	public void StartRmtPurchase(Session session)
	{
	}

	public void RecordPurchaseCompleted(Dictionary<string, object> purchaseInfo, Session session)
	{
	}

	public Dictionary<string, Dictionary<string, object>> PendingTransactions()
	{
		return null;
	}

	public void GetPurchases(Session session)
	{
	}

	public void ApplyRmtPurchases(Session session, Cost data)
	{
	}

	public void ApplyRmtPurchase(Session session, Cost data, string sale_tag, string transactionId)
	{
	}

	private void ClearTransaction(string transactionId)
	{
	}

	private static void HandleProductInfo(Session session, StoreEventArgs args)
	{
	}

	private static void HandlePurchaseInfo(Session session, StoreEventArgs args)
	{
	}

	private static void HandlePurchaseUpdate(Session session, StoreEventArgs args)
	{
	}

	private static void HandlePurchaseResponse(Session session, StoreEventArgs args)
	{
	}

	private static void HandleGetProductInfoResponse(Session session, StoreEventArgs args)
	{
	}

	private static string GetStoreName()
	{
		return null;
	}

	public void CheckPendingTransactions()
	{
	}
}
