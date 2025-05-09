using System.Collections.Generic;
using DeltaDNA;

public class SBMIDeltaDNA
{
	public abstract class Object
	{
		protected EventBuilder m_pEventBuilder = new EventBuilder();

		public string m_sKey { get; protected set; }

		public void AddToDict(EventBuilder pEventBuilder, string sOverrideKey = null, bool bNested = true)
		{
			if (bNested)
			{
				string text = (string.IsNullOrEmpty(sOverrideKey) ? m_sKey : sOverrideKey);
				if (m_pEventBuilder != null && pEventBuilder != null && !string.IsNullOrEmpty(text))
				{
					pEventBuilder.AddParam(text, m_pEventBuilder.ToDictionary());
				}
				return;
			}
			Dictionary<string, object> dictionary = m_pEventBuilder.ToDictionary();
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				pEventBuilder.AddParam(item.Key, item.Value);
			}
		}
	}

	public class DeviceObject : Object
	{
		public DeviceObject(string sObjectKey, string sDeviceName, string sDeviceType, string sHardwareVersion, string sOS, string sOSVersion, string sManufacturer, string sTimezoneOffset, string sUserLanguage)
		{
			base.m_sKey = sObjectKey;
			m_pEventBuilder.AddParam("deviceName", sDeviceName);
			m_pEventBuilder.AddParam("deviceType", sDeviceType);
			m_pEventBuilder.AddParam("hardwareVersion", sHardwareVersion);
			m_pEventBuilder.AddParam("operatingSystem", sOS);
			m_pEventBuilder.AddParam("operatingSystemVersion", sOSVersion);
			m_pEventBuilder.AddParam("manufacturer", sManufacturer);
			m_pEventBuilder.AddParam("timezoneOffset", sTimezoneOffset);
			m_pEventBuilder.AddParam("userLanguage", sUserLanguage);
		}
	}

	public class PlayerObject : Object
	{
		public PlayerObject(string sObjectKey, int nLevel, int nXP, int nHardCurrency, int nSoftCurrency)
		{
			base.m_sKey = sObjectKey;
			m_pEventBuilder.AddParam("userLevel", nLevel);
			m_pEventBuilder.AddParam("userXP", nXP);
			m_pEventBuilder.AddParam("userJelly", nHardCurrency);
			m_pEventBuilder.AddParam("userCoins", nSoftCurrency);
		}
	}

	public class MissionObject : Object
	{
		public MissionObject(string sObjectKey, string sMissionName, string sMissionType, int nMissionID)
		{
			SetBaseData(sObjectKey, sMissionName, sMissionType, nMissionID);
		}

		public MissionObject(string sObjectKey, string sMissionName, string sMissionType, int nMissionID, ulong ulMissionDuration)
		{
			SetBaseData(sObjectKey, sMissionName, sMissionType, nMissionID);
			m_pEventBuilder.AddParam("missionDuration", ulMissionDuration);
		}

		private void SetBaseData(string sObjectKey, string sMissionName, string sMissionType, int nMissionID)
		{
			base.m_sKey = sObjectKey;
			m_pEventBuilder.AddParam("missionName", sMissionName);
			m_pEventBuilder.AddParam("missionID", nMissionID.ToString());
			m_pEventBuilder.AddParam("missionType", sMissionType);
		}
	}

	public class RewardObject : Object
	{
		public RewardObject(string sObjectKey, string sRewardName, Reward pReward, Game pGame, int nRealCurrencyAmount = -1, string sRealCurrencyType = null, string sTypeOverride = null)
		{
			base.m_sKey = sObjectKey;
			ProductBuilder productBuilder = new ProductBuilder();
			if (pReward != null)
			{
				Dictionary<int, int> resourceAmounts = pReward.ResourceAmounts;
				Dictionary<int, int> buildingAmounts = pReward.BuildingAmounts;
				List<int> costumeUnlocks = pReward.CostumeUnlocks;
				List<int> recipeUnlocks = pReward.RecipeUnlocks;
				List<int> clearedLands = pReward.ClearedLands;
				foreach (KeyValuePair<int, int> item in resourceAmounts)
				{
					Resource resource = ((!pGame.resourceManager.Resources.ContainsKey(item.Key)) ? null : pGame.resourceManager.Resources[item.Key]);
					if (resource != null)
					{
						productBuilder.AddItem(resource.Name, string.IsNullOrEmpty(sTypeOverride) ? "resource" : sTypeOverride, item.Value);
					}
				}
				foreach (KeyValuePair<int, int> item2 in buildingAmounts)
				{
					Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, item2.Key, true);
					if (blueprint != null)
					{
						productBuilder.AddItem((string)blueprint.Invariable["name"], string.IsNullOrEmpty(sTypeOverride) ? "building" : sTypeOverride, item2.Value);
					}
				}
				int count = costumeUnlocks.Count;
				for (int i = 0; i < count; i++)
				{
					CostumeManager.Costume costume = pGame.costumeManager.GetCostume(costumeUnlocks[i]);
					if (costume != null)
					{
						productBuilder.AddItem(costume.m_sName, string.IsNullOrEmpty(sTypeOverride) ? "costume" : sTypeOverride, 1);
					}
				}
				count = recipeUnlocks.Count;
				for (int j = 0; j < count; j++)
				{
					CraftingRecipe recipeById = pGame.craftManager.GetRecipeById(recipeUnlocks[j]);
					if (recipeById != null)
					{
						productBuilder.AddItem(recipeById.recipeName, string.IsNullOrEmpty(sTypeOverride) ? "recipe" : sTypeOverride, 1);
					}
				}
				count = clearedLands.Count;
				for (int k = 0; k < count; k++)
				{
					productBuilder.AddItem(clearedLands[k].ToString(), string.IsNullOrEmpty(sTypeOverride) ? "expansion" : sTypeOverride, 1);
				}
			}
			if (!string.IsNullOrEmpty(sRealCurrencyType) && nRealCurrencyAmount > 0)
			{
				productBuilder.AddRealCurrency(sRealCurrencyType, nRealCurrencyAmount);
			}
			m_pEventBuilder.AddParam(sRewardName, productBuilder);
		}
	}

	public class TransactionObject : Object
	{
		public TransactionObject(string sObjectKey, string sTransactorID, string sTransactionServer, string sTransactionReceipt, string sProductID, string sTransactionID, bool bIsInitiator)
		{
			base.m_sKey = sObjectKey;
			m_pEventBuilder.AddParam("transactorID", sTransactorID);
			m_pEventBuilder.AddParam("transactionServer", sTransactionServer);
			m_pEventBuilder.AddParam("transactionReceipt", sTransactionReceipt);
			m_pEventBuilder.AddParam("productID", sProductID);
			m_pEventBuilder.AddParam("transactionID", sTransactionID);
			m_pEventBuilder.AddParam("isInitiator", bIsInitiator);
		}
	}

	private static bool _bDEBUG_LOG;

	public static void LogMissionStart(Game pGame, MissionObject pMission)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		pMission.AddToDict(eventBuilder, null, false);
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogMissionStart: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("missionStarted", eventBuilder);
	}

	public static void LogMissionComplete(Game pGame, MissionObject pMission, RewardObject pReward)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		pMission.AddToDict(eventBuilder, null, false);
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder);
		}
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogMissionComplete: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("missionCompleted", eventBuilder);
	}

	public static void LogLevelUp(Game pGame, RewardObject pReward, int nLevel)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("levelUpName", nLevel.ToString());
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder);
		}
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogLevelUp: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("levelUp", eventBuilder);
	}

	public static void LogWishGranted(Game pGame, string sWishName, RewardObject pReward)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("wishName", sWishName);
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder);
		}
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogWishGranted: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("wishGranted", eventBuilder);
	}

	public static void LogUIInteraction(Game pGame, string sUIName, string sType, string sAction)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("UIName", sUIName);
		eventBuilder.AddParam("UIAction", sAction);
		eventBuilder.AddParam("UIType", sType);
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogUIInteraction: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("uiInteraction", eventBuilder);
	}

	public static void LogFeatureUnlocked(Game pGame, string sFeatureName, string sFeatureType)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("featureName", sFeatureName);
		eventBuilder.AddParam("featureType", sFeatureType);
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogFeatureUnlocked: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("featureUnlocked", eventBuilder);
	}

	public static void LogShopEntered(Game pGame, string sShopName)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("shopName", sShopName);
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogShopEntered: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("shopEntered", eventBuilder);
	}

	public static void LogTransaction(Game pGame, RewardObject pSpent, RewardObject pRecieved, TransactionObject pTransaction, string sTransactionType, string sTransactionName)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("transactionType", sTransactionType);
		eventBuilder.AddParam("transactionName", sTransactionName);
		if (pSpent != null)
		{
			pSpent.AddToDict(eventBuilder, null, false);
		}
		if (pRecieved != null)
		{
			pRecieved.AddToDict(eventBuilder, null, false);
		}
		if (pTransaction != null)
		{
			pTransaction.AddToDict(eventBuilder, null, false);
		}
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogTransaction: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("transaction", eventBuilder);
	}

	public static void LogTransaction(Game pGame, int jellyfishJellyCost, int itemDID, string itemName, string itemType, string sTransactionType, string sTransactionName)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("transactionType", sTransactionType);
		eventBuilder.AddParam("transactionName", sTransactionName);
		eventBuilder.AddParam("productsSpent", new ProductBuilder().AddItem("Jellyfish Jelly", "resource", jellyfishJellyCost));
		eventBuilder.AddParam("productsReceived", new ProductBuilder().AddItem(itemName, itemType, 1));
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogTransaction: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("transaction", eventBuilder);
	}

	public static void LogItemCollected(Game pGame, RewardObject pReward)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder);
		}
		if (_bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string text = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogItemCollected: " + text);
		}
		Singleton<SDK>.Instance.RecordEvent("itemCollected", eventBuilder);
	}
}
