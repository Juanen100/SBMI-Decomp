using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;
using com.amazon.device.iap.cpt;

public class RmtStore
{
	public class HandleProductsDelegate : SoaringDelegate
	{
		public Session session;

		public static SoaringContext CreateDelegate(Session session, string name = null, SoaringContextDelegate del = null, SoaringObjectBase passthrough = null)
		{
			SoaringContext soaringContext = new SoaringContext();
			soaringContext.Name = name;
			HandleProductsDelegate handleProductsDelegate = new HandleProductsDelegate();
			handleProductsDelegate.session = session;
			soaringContext.Responder = handleProductsDelegate;
			soaringContext.ContextResponder = del;
			if (passthrough != null)
			{
				soaringContext.addValue(passthrough, "passthrough");
			}
			return soaringContext;
		}

		public override void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
		{
			session.TheGame.store.LoadRmtProductInfo(purchasables);
			List<string> list = new List<string>();
			int num = ((purchasables != null) ? purchasables.Length : 0);
			for (int i = 0; i < num; i++)
			{
				list.Add(purchasables[i].ProductID);
			}
			TFBilling.FetchProductBillingInfo(session, list);
		}

		public override void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
		{
			if (success)
			{
				session.TheGame.store.receivedPurchaseInfo = true;
			}
			if (purchases == null)
			{
				return;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int num = purchases.Length;
			for (int i = 0; i < num; i++)
			{
				SoaringPurchase soaringPurchase = purchases[i];
				if (dictionary.ContainsKey(soaringPurchase.ResourceType))
				{
					Dictionary<int, int> dictionary3;
					Dictionary<int, int> dictionary2 = (dictionary3 = dictionary);
					int resourceType;
					int key = (resourceType = soaringPurchase.ResourceType);
					resourceType = dictionary3[resourceType];
					dictionary2[key] = resourceType + soaringPurchase.Amount;
				}
				else
				{
					dictionary.Add(soaringPurchase.ResourceType, soaringPurchase.Amount);
				}
			}
			Cost data = new Cost(dictionary);
			session.TheGame.store.ApplyRmtPurchases(session, data);
		}

		public override void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
		{
			if (context == null)
			{
				return;
			}
			SoaringDictionary soaringDictionary = (SoaringDictionary)context["passthrough"];
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary["cost"];
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			string[] array = soaringDictionary2.allKeys();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (string.IsNullOrEmpty(array[i]))
				{
					SoaringDebug.Log("Invalid CostKey: " + i);
				}
				else
				{
					dictionary.Add(int.Parse(array[i]), (SoaringValue)soaringDictionary2[array[i]]);
				}
			}
			string text = (SoaringValue)soaringDictionary["transaction_id"];
			string text2 = string.Empty;
			if (soaringDictionary.containsKey("receipt"))
			{
				text2 = (SoaringValue)soaringDictionary["receipt"];
			}
			if ((error != null || !success) && Soaring.IsOnline)
			{
				if (error != null)
				{
					SoaringDebug.Log("OnRecieptValidated: " + error, LogType.Error);
				}
				else
				{
					SoaringDebug.Log("OnRecieptValidated: Unknown Error", LogType.Error);
				}
				session.TheGame.store.ClearTransaction(text);
			}
			else if (success)
			{
				session.TheGame.store.ApplyRmtPurchase(session, new Cost(dictionary), (SoaringValue)soaringDictionary["sale_tag"], text);
				SoaringPurchasable soaringPurchasable = session.TheGame.store.soaringProducts[(SoaringValue)soaringDictionary["productId"]];
				RmtProduct value = null;
				if (!session.TheGame.store.rmtProducts.TryGetValue(soaringPurchasable.ProductID, out value))
				{
					SoaringDebug.Log("No Remote Product Found For Analytics: " + soaringPurchasable.ProductID, LogType.Error);
				}
				if (TFUtils.isAmazon())
				{
					NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput();
					notifyFulfillmentInput.FulfillmentResult = "FULFILLED";
					notifyFulfillmentInput.ReceiptId = text2;
					AmazonIapV2Impl.Instance.NotifyFulfillment(notifyFulfillmentInput);
				}
				session.TheGame.analytics.LogSoaringIAPPurchaseComplete(soaringPurchasable.ProductID);
				AnalyticsWrapper.LogPurchaseComplete(session.TheGame, soaringPurchasable, text2, text);
			}
			else
			{
				SoaringDebug.Log("OnRecieptValidated: Critical Error: Reciept Valied To Validated, Offline Mode", LogType.Error);
			}
		}
	}

	public class StoreEventArgs : EventArgs
	{
		public Dictionary<string, object> results;

		public StoreEventArgs(Dictionary<string, object> res)
		{
			results = res;
		}
	}

	public delegate void StoreEventHandler(object sender, StoreEventArgs args);

	public const float STORE_TIMEOUT = 15f;

	private static string TRANSACTION_LOG = Application.persistentDataPath + Path.DirectorySeparatorChar + "txn.json";

	public bool rmtEnabled;

	public Dictionary<string, RmtProduct> rmtProducts;

	public Dictionary<string, SoaringPurchasable> soaringProducts = new Dictionary<string, SoaringPurchasable>();

	public bool receivedProductInfo;

	public bool receivedPurchaseInfo;

	private string txProductId;

	public static bool IsPurchasing = false;

	private Dictionary<string, Dictionary<string, object>> pendingTransactions;

	public bool RmtReady
	{
		get
		{
			return rmtEnabled && rmtProducts != null;
		}
	}

	public event StoreEventHandler ProductInfoReceived;

	public event StoreEventHandler PurchaseUpdateReceived;

	public event StoreEventHandler PurchaseResponseReceived;

	public event StoreEventHandler GetProductInfoResponseReceived;

	public event StoreEventHandler PurchaseReceiptReceived;

	public event StoreEventHandler PurchaseInfoReceived;

	public event StoreEventHandler PurchaseError;

	public event StoreEventHandler PurchaseDefered;

	public RmtStore(bool rmtEnabled, Dictionary<string, Dictionary<string, object>> pendingTransactions)
	{
		this.rmtEnabled = rmtEnabled;
		if (this.rmtEnabled)
		{
			string bundleIdentifier = SBSettings.BundleIdentifier;
			if (bundleIdentifier != null && (bundleIdentifier.StartsWith("com.tinyfunstudios.") || bundleIdentifier.StartsWith("com.kungfufactory.")))
			{
				this.rmtEnabled = false;
			}
		}
		this.pendingTransactions = pendingTransactions;
	}

	public void OnProductInfoReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.ProductInfoReceived != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.ProductInfoReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No ProductInfoReceived Function");
		}
	}

	public void OnPurchaseUpdateReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseUpdateReceived != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.PurchaseUpdateReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No OnPurchaseUpdateReceived Function");
		}
	}

	public void OnPurchaseReceiptReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseReceiptReceived != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.PurchaseReceiptReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No OnPurchaseReceiptReceived Function");
		}
	}

	public void OnPurchaseResponseReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseResponseReceived != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.PurchaseResponseReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No OnPurchaseResponseReceived Function");
		}
	}

	public void OnGetProductInfoResponseReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.GetProductInfoResponseReceived != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.GetProductInfoResponseReceived(this, args);
		}
	}

	public void OnPurchaseInfoReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseInfoReceived != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.PurchaseInfoReceived(this, args);
		}
	}

	public void OnPurchaseError(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseError != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.PurchaseError(this, args);
		}
	}

	public void OnPurchaseDefered(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseDefered != null)
		{
			StoreEventArgs args = new StoreEventArgs(results);
			this.PurchaseDefered(this, args);
		}
	}

	public static bool PreloadRmtProducts(Session session)
	{
		Soaring.RequestProducts(GetStoreName(), "en", HandleProductsDelegate.CreateDelegate(session, "RequestProducts"));
		return true;
	}

	public static Cost CostFromCollection(Session session, List<object> sales, string field)
	{
		Cost result = new Cost();
		foreach (Dictionary<string, object> sale in sales)
		{
			if (TFUtils.LoadBoolAsInt(sale, "confirmed"))
			{
				string text = (string)sale["product_id"];
				Dictionary<string, object> offerByCode = session.TheGame.catalog.GetOfferByCode(text);
				if (offerByCode != null)
				{
					Dictionary<string, object> dict = (Dictionary<string, object>)offerByCode[field];
					result += Cost.FromDict(dict);
				}
				else
				{
					TFUtils.ErrorLog("Failed to find offer for " + text);
				}
			}
			else
			{
				string text2 = (string)sale["product_id"];
				TFUtils.DebugLog("Skipping unconfirmed sale of " + text2);
			}
		}
		return result;
	}

	public static RmtStore LoadFromFilesystem(bool rmtEnabled)
	{
		if (!TFUtils.FileIsExists(TRANSACTION_LOG))
		{
			using (FileStream fileStream = File.Create(TRANSACTION_LOG))
			{
				fileStream.Close();
			}
		}
		string[] array = File.ReadAllLines(TRANSACTION_LOG);
		Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)Json.Deserialize(array[i]);
			dictionary.Add((string)dictionary2["transactionId"], dictionary2);
		}
		return new RmtStore(rmtEnabled, dictionary);
	}

	public void Init(Session session)
	{
		TFUtils.DebugLog("Initializing RMT Store");
		this.ProductInfoReceived = (StoreEventHandler)Delegate.Combine(this.ProductInfoReceived, (StoreEventHandler)delegate(object sender, StoreEventArgs args)
		{
			HandleProductInfo(session, args);
		});
		this.PurchaseInfoReceived = (StoreEventHandler)Delegate.Combine(this.PurchaseInfoReceived, (StoreEventHandler)delegate(object sender, StoreEventArgs args)
		{
			HandlePurchaseInfo(session, args);
		});
		this.PurchaseUpdateReceived = (StoreEventHandler)Delegate.Combine(this.PurchaseUpdateReceived, (StoreEventHandler)delegate(object sender, StoreEventArgs args)
		{
			HandlePurchaseUpdate(session, args);
		});
		this.PurchaseResponseReceived = (StoreEventHandler)Delegate.Combine(this.PurchaseResponseReceived, (StoreEventHandler)delegate(object sender, StoreEventArgs args)
		{
			HandlePurchaseResponse(session, args);
		});
		this.GetProductInfoResponseReceived = (StoreEventHandler)Delegate.Combine(this.GetProductInfoResponseReceived, (StoreEventHandler)delegate(object sender, StoreEventArgs args)
		{
			HandleGetProductInfoResponse(session, args);
		});
		session.RegisterExternalCallback("productInfo", OnProductInfoReceived);
		session.RegisterExternalCallback("purchaseUpdate", OnPurchaseUpdateReceived);
	}

	public void Start()
	{
		TFBilling.InitializeStore();
	}

	public void Reset(Session session)
	{
		TFBilling.ResetStore();
		session.unregisterExternalCallback("productInfo", OnProductInfoReceived);
		session.unregisterExternalCallback("purchaseUpdate", OnPurchaseUpdateReceived);
		this.ProductInfoReceived = null;
		this.PurchaseInfoReceived = null;
		this.PurchaseUpdateReceived = null;
		this.PurchaseResponseReceived = null;
		this.GetProductInfoResponseReceived = null;
		txProductId = null;
		IsPurchasing = false;
		pendingTransactions.Clear();
		receivedProductInfo = false;
		receivedPurchaseInfo = false;
	}

	public bool LoadRmtProductInfo(Catalog catalog, Dictionary<string, object> rawRmtProductInfo)
	{
		List<object> list = (List<object>)rawRmtProductInfo["products"];
		TFUtils.DebugLog("Premium product info: " + list.Count);
		if (list.Count == 0)
		{
			return false;
		}
		rmtProducts = new Dictionary<string, RmtProduct>();
		foreach (Dictionary<string, object> item in list)
		{
			RmtProduct rmtProduct = new RmtProduct(item);
			rmtProducts[rmtProduct.productId] = rmtProduct;
		}
		return true;
	}

	public bool LoadRmtProductInfo(SoaringPurchasable[] pPurchasables)
	{
		soaringProducts = new Dictionary<string, SoaringPurchasable>();
		if (pPurchasables == null)
		{
			return false;
		}
		int num = pPurchasables.Length;
		for (int i = 0; i < num; i++)
		{
			SoaringPurchasable soaringPurchasable = pPurchasables[i];
			soaringProducts.Add(soaringPurchasable.ProductID, soaringPurchasable);
		}
		return true;
	}

	public void OpenTransaction(string productId)
	{
		txProductId = productId;
		IsPurchasing = true;
	}

	public void StartRmtPurchase(Session session)
	{
		TFUtils.DebugLog("Purchasing product: " + txProductId);
		if (session.TheGame.store.RmtReady)
		{
			IsPurchasing = true;
			TFBilling.StartRmtPurchase(txProductId);
		}
		else
		{
			TFUtils.DebugLog("Skipping premium purchase, since premium is not supported");
		}
	}

	public void RecordPurchaseCompleted(Dictionary<string, object> purchaseInfo, Session session)
	{
		try
		{
			string text = null;
			string text2 = (string)purchaseInfo["productId"];
			text = ((!purchaseInfo.ContainsKey("transactionId")) ? text2 : ((string)purchaseInfo["transactionId"]));
			string value = (string)purchaseInfo["receipt"];
			string playerId = session.ThePlayer.playerId;
			if (!soaringProducts.ContainsKey(text2))
			{
				TFUtils.ErrorLog("missing offer " + text2);
				return;
			}
			SoaringPurchasable soaringPurchasable = soaringProducts[text2];
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["transactionId"] = text;
			dictionary["productId"] = text2;
			dictionary["playerId"] = playerId;
			dictionary["receipt"] = value;
			dictionary["data"] = new Cost(new Dictionary<int, int> { { soaringPurchasable.ResourceType, soaringPurchasable.Amount } }).ToDict();
			dictionary["type"] = soaringPurchasable.ResourceType.ToString();
			if (purchaseInfo.ContainsKey("userId"))
			{
				dictionary["userId"] = purchaseInfo["userId"];
			}
			string text3 = Json.Serialize(dictionary);
			File.AppendAllText(TRANSACTION_LOG, text3);
			pendingTransactions[text] = dictionary;
			TFUtils.DebugLog("json for purchase: " + text3);
			OnPurchaseResponseReceived(dictionary, null);
			OnPurchaseReceiptReceived(dictionary, null);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
		}
	}

	public Dictionary<string, Dictionary<string, object>> PendingTransactions()
	{
		return pendingTransactions;
	}

	public void GetPurchases(Session session)
	{
		Soaring.RequestPurchases(GetStoreName(), HandleProductsDelegate.CreateDelegate(session, "GetPurchases"));
	}

	public void ApplyRmtPurchases(Session session, Cost data)
	{
		session.TheGame.resourceManager.SetPurchasedResources(data);
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			ResourceManager.ApplyPurchasesToGameState(data, gameState);
		};
		session.TheGame.LockedGameStateChange(writer);
	}

	public void ApplyRmtPurchase(Session session, Cost data, string sale_tag, string transactionId)
	{
		if (!session.TheGame.store.RmtReady)
		{
			TFUtils.WarningLog("Failed to apply premium purchase, since premium is not ready");
			return;
		}
		session.TheGame.resourceManager.PurchaseResourcesWithHardCurrency(0, new Cost(), session.TheGame);
		session.TheGame.resourceManager.SetPurchasedResources(data);
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			ResourceManager.ApplyPurchasesToGameState(data, gameState);
			((Dictionary<string, object>)gameState["farm"])["last_action"] = sale_tag;
		};
		session.TheGame.LockedGameStateChange(writer);
		ClearTransaction(transactionId);
	}

	private void ClearTransaction(string transactionId)
	{
		if (pendingTransactions.ContainsKey(transactionId))
		{
			pendingTransactions.Remove(transactionId);
		}
		else
		{
			TFUtils.DebugLog("Did not find transaction for id: " + transactionId + " in in-memory log");
		}
		string[] array = File.ReadAllLines(TRANSACTION_LOG);
		List<string> list = new List<string>();
		bool flag = false;
		foreach (string text in array)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(text);
			if (dictionary["transactionId"].Equals(transactionId))
			{
				flag = true;
			}
			else
			{
				list.Add(text);
			}
		}
		File.WriteAllText(TRANSACTION_LOG, string.Join("\n", list.ToArray()));
		if (!flag)
		{
			TFUtils.DebugLog("Did not find transaction for id: " + transactionId + " in on-disk log");
		}
		TFBilling.CompleteRmtPurchase(transactionId);
		TFUtils.DebugLog("Closed transaction for " + transactionId);
		txProductId = null;
	}

	private static void HandleProductInfo(Session session, StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		bool flag = session.TheGame.store.LoadRmtProductInfo(session.TheGame.catalog, results);
		TFUtils.DebugLog("Got premium product info:");
		if (!flag)
		{
			TFUtils.WarningLog("Ignoring invalid response for product info with no products");
		}
		session.TheGame.store.receivedProductInfo = flag;
	}

	private static void HandlePurchaseInfo(Session session, StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		if (TFServer.IsNetworkError(results))
		{
			TFUtils.DebugLog("Failed to load purchases. Continuing.");
			session.TheGame.store.receivedPurchaseInfo = true;
			IsPurchasing = false;
		}
		else
		{
			Dictionary<string, object> dictionary = results;
			Cost data = CostFromCollection(session, (List<object>)dictionary["sales"], "data");
			session.TheGame.store.ApplyRmtPurchases(session, data);
			session.TheGame.store.receivedPurchaseInfo = true;
		}
	}

	private static void HandlePurchaseUpdate(Session session, StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		TFUtils.DebugLog("Got purchase update: " + results);
		string text = (string)results["state"];
		string iapBundleName = TFUtils.TryLoadString(results, "productId");
		int num = 1;
		if (results.ContainsKey("data"))
		{
			Dictionary<int, int> dictionary = results["data"] as Dictionary<int, int>;
			if (dictionary != null && dictionary.ContainsKey(ResourceManager.HARD_CURRENCY))
			{
				num = dictionary[ResourceManager.HARD_CURRENCY];
			}
			TFUtils.DebugLog("Found purchase amount! " + num);
		}
		int amount = session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
		switch (text)
		{
		case "started":
			break;
		case "completed":
			session.TheGame.store.RecordPurchaseCompleted(results, session);
			session.analytics.LogCompleteInAppPurchase(iapBundleName, amount);
			break;
		case "failed":
			session.TheGame.store.OnPurchaseError(results, null);
			session.analytics.LogFailInAppPurchase(iapBundleName, amount);
			IsPurchasing = false;
			break;
		case "defered":
			session.TheGame.store.OnPurchaseDefered(results, null);
			break;
		default:
			session.TheGame.store.OnPurchaseError(results, null);
			IsPurchasing = false;
			break;
		}
	}

	private static void HandlePurchaseResponse(Session session, StoreEventArgs args)
	{
		IsPurchasing = false;
		Dictionary<string, object> results = args.results;
		if (TFServer.IsNetworkError(results))
		{
			TFUtils.ErrorLog("Network access is required to complete RMT");
			session.TheGame.store.OnPurchaseError(results, null);
			return;
		}
		Dictionary<string, object> dictionary = results;
		Cost cost = Cost.FromDict((Dictionary<string, object>)dictionary["data"]);
		foreach (string key in dictionary.Keys)
		{
			TFUtils.DebugLog("key:" + key + " value:" + dictionary[key]);
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (dictionary.ContainsKey("sale_tag"))
		{
			text = TFUtils.LoadString(dictionary, "sale_tag");
		}
		if (dictionary.ContainsKey("transactionId"))
		{
			text2 = TFUtils.LoadString(dictionary, "transactionId");
		}
		string userID = null;
		if (dictionary.ContainsKey("userId"))
		{
			userID = TFUtils.LoadString(dictionary, "userId");
		}
		string text3 = TFUtils.LoadString(dictionary, "receipt").Replace("\r", string.Empty);
		text3 = text3.Replace("\n", string.Empty);
		string text4 = TFUtils.LoadString(dictionary, "productId");
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		soaringDictionary.addValue(text2, "transaction_id");
		soaringDictionary.addValue(text, "sale_tag");
		soaringDictionary.addValue(text4, "productId");
		soaringDictionary.addValue(text3, "receipt");
		Dictionary<int, int> resourceAmounts = cost.ResourceAmounts;
		foreach (KeyValuePair<int, int> item in resourceAmounts)
		{
			soaringDictionary2.addValue(item.Value + session.TheGame.resourceManager.Resources[item.Key].AmountPurchased, item.Key.ToString());
		}
		soaringDictionary.addValue(soaringDictionary2, "cost");
		Soaring.ValidatePurchasableReciept(text3, session.TheGame.store.soaringProducts[text4], GetStoreName(), SBSettings.UseProductionIAP, userID, HandleProductsDelegate.CreateDelegate(session, "ValidatePurchasableReciept", null, soaringDictionary));
	}

	private static void HandleGetProductInfoResponse(Session session, StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		TFUtils.DebugLog("purchaseResponse " + results);
		if (TFServer.IsNetworkError(results))
		{
			TFUtils.ErrorLog("Network access is required to complete RMT");
			session.TheGame.store.OnPurchaseError(results, null);
			return;
		}
		TFUtils.DebugLog("args " + args);
		Dictionary<string, object> dictionary = results;
		TFUtils.DebugLog("0000");
		foreach (string key in dictionary.Keys)
		{
			TFUtils.DebugLog("------ key:" + key + " value:" + dictionary[key]);
		}
		List<object> list = (List<object>)dictionary["products"];
		TFUtils.DebugLog("length " + list.Count);
		List<string> list2 = new List<string>();
		foreach (object item in list)
		{
			TFUtils.DebugLog("------ " + item);
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item;
			list2.Add(dictionary2["code"].ToString());
			foreach (string key2 in dictionary2.Keys)
			{
				TFUtils.DebugLog("------ key:" + key2 + " value:" + dictionary2[key2]);
			}
		}
		if (TFUtils.isAmazon())
		{
			SkusInput skusInput = new SkusInput();
			skusInput.Skus = list2;
			AmazonIapV2Impl.Instance.GetProductData(skusInput);
		}
		else
		{
			GoogleIAB.queryInventory(list2.ToArray());
		}
	}

	private static string GetStoreName()
	{
		return SBSettings.StoreName;
	}
}
