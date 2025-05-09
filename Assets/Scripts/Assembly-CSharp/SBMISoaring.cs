using System;
using System.Collections.Generic;
using UnityEngine;

public class SBMISoaring
{
	public class SBMIDailyBonusDay : SoaringObjectBase
	{
		private SoaringDictionary mData;

		public int Day
		{
			get
			{
				return GetSoaringValue("day");
			}
		}

		public int CurrencyDID
		{
			get
			{
				return GetSoaringValue("did");
			}
		}

		public int CurrencyAmount
		{
			get
			{
				return GetSoaringValue("value");
			}
		}

		public SBMIDailyBonusDay(SoaringDictionary data)
			: base(IsType.Object)
		{
			mData = data;
		}

		private int GetSoaringValue(string str)
		{
			if (mData == null || str == null)
			{
				return -1;
			}
			return mData.soaringValue(str);
		}
	}

	public class SMBICacheDelegate : SoaringDelegate
	{
		public bool IsError(bool success, SoaringError err, SoaringDictionary data)
		{
			return !success || err != null || data == null;
		}

		public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
			if (string.IsNullOrEmpty(module) || !(module == "retrieveDailyBonusCalendar"))
			{
				return;
			}
			mAlreadyCollected = false;
			mCurrentDailyBonusDay = -1;
			mDailyBonusCalendar = null;
			bool flag = false;
			if (!IsError(success, error, data))
			{
				SoaringObjectBase soaringObjectBase = data.objectWithKey("days");
				if (soaringObjectBase != null)
				{
					SoaringArray soaringArray = (SoaringArray)soaringObjectBase;
					int num = soaringArray.count();
					mDailyBonusCalendar = new SoaringArray<SBMIDailyBonusDay>(num);
					for (int i = 0; i < num; i++)
					{
						SBMIDailyBonusDay obj = new SBMIDailyBonusDay((SoaringDictionary)soaringArray.objectAtIndex(i));
						mDailyBonusCalendar.addObject(obj);
					}
				}
				soaringObjectBase = data.objectWithKey("currentDay");
				if (soaringObjectBase != null)
				{
					mCurrentDailyBonusDay = (SoaringValue)soaringObjectBase;
					if (mDailyBonusCalendar != null)
					{
						flag = true;
					}
				}
				soaringObjectBase = data.objectWithKey("alreadyCollected");
				if (soaringObjectBase != null)
				{
					mAlreadyCollected = (SoaringValue)soaringObjectBase;
				}
			}
			else
			{
				SoaringDebug.Log("Failed to retrieve daily bonus calendar: " + (string)error + " : Code: " + error.ErrorCode);
			}
			if (context != null && context.ContextResponder != null)
			{
				context.addValue(flag, "query");
				if (error != null)
				{
					context.addValue(error, "error_code");
				}
				context.ContextResponder(context);
			}
		}
	}

	public const string SBMI_Friends_Dialog_Key = "SBMI_fdk";

	public const string SBMI_CompletedQuest_Key = "SBMI_completed_quest_key";

	public const string SBMI_Friends_Reward_Key = "SBMI_friends_reward_key";

	public const string SBMI_Friends_CoinReward_Key = "SBMI_friends_coinreward_key";

	public const string SBMI_Friends_JellyReward_Key = "SBMI_friends_jellyreward_key";

	public const string SBMI_Friends_XPReward_Key = "SBMI_friends_xpreward_key";

	public const string SBMI_Friends_TimeStampReward_Key = "SBMI_friends_timestampreward_key";

	public const string SBMI_Friends_ChestsCollected_Key = "SBMI_friends_chestscollected_key";

	public const string SBMI_Friends_TimeStamp_Key = "SBMI_friends_timestamp_key";

	public const string SBMI_Friends_TreasureSpawnTimeStamp_Key = "SBMI_friends_treasurespawntimestamp_key";

	private static int mCurrentDailyBonusDay = -1;

	private static SoaringArray<SBMIDailyBonusDay> mDailyBonusCalendar;

	private static bool mAlreadyCollected;

	public static long PatchTownTimestamp
	{
		get
		{
			SoaringValue soaringValue = (SoaringValue)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_timestamp_key");
			if (soaringValue == null)
			{
				return 0L;
			}
			return soaringValue;
		}
		set
		{
			Soaring.Player.PrivateData_Safe.setValue(value, "SBMI_friends_timestamp_key");
		}
	}

	public static long PatchTownTreasureSpawnTimestamp
	{
		get
		{
			SoaringValue soaringValue = (SoaringValue)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_treasurespawntimestamp_key");
			if (soaringValue == null)
			{
				return 0L;
			}
			return soaringValue;
		}
		set
		{
			Soaring.Player.PrivateData_Safe.setValue(value, "SBMI_friends_treasurespawntimestamp_key");
		}
	}

	public static int PatchTownTreasureCollected
	{
		get
		{
			SoaringValue soaringValue = Soaring.Player.PrivateData_Safe.soaringValue("SBMI_friends_chestscollected_key");
			if (soaringValue == null)
			{
				return 0;
			}
			return soaringValue;
		}
		set
		{
			Soaring.Player.PrivateData_Safe.setValue(value, "SBMI_friends_chestscollected_key");
		}
	}

	public static void Initialize(SoaringDelegate del)
	{
		if (Soaring.IsInitialized)
		{
			return;
		}
		SoaringPlatformType soaringPlatformType = SoaringPlatformType.System;
		soaringPlatformType = ((!TFUtils.isAmazon()) ? SoaringPlatformType.Android : SoaringPlatformType.Amazon);
		SoaringMode mode = SoaringMode.Development;
		if (SBSettings.SoaringProduction)
		{
			mode = SoaringMode.Production;
		}
		else if (SBSettings.SoaringQA)
		{
			mode = SoaringMode.Testing;
		}
		Soaring.StartSoaring("5239a9edbecd6a054f000001", del, mode, soaringPlatformType);
		try
		{
			if (soaringPlatformType == SoaringPlatformType.Amazon)
			{
				CommonUtils.SetMemoryLevel(768);
				return;
			}
			SoaringPlatformAndroid soaringPlatformAndroid = (SoaringPlatformAndroid)SoaringPlatform.GetDelegate();
			CommonUtils.SetMemoryLevel((int)soaringPlatformAndroid.TotalMemory);
			SoaringDebug.Log("MEMORY: " + soaringPlatformAndroid.TotalMemory);
		}
		catch
		{
		}
	}

	public void ResetCachedData()
	{
		mDailyBonusCalendar = null;
		mCurrentDailyBonusDay = -1;
		mAlreadyCollected = false;
	}

	public static void OnInitializeSoaring()
	{
		string serverContentUrl = Soaring.ServerContentUrl;
		if (!SBSettings.IsLastRunVersion)
		{
			SoaringInternal.instance.Versions.ClearAllContent();
			SBSettings.Init();
		}
		SoaringInternal.instance.mSoaringLanguage = Language.CurrentLanguage();
		Soaring.SetGameVersion(SBSettings.LOCAL_BUNDLE_VERSION);
		Soaring.SetAdServerURL(serverContentUrl);
		string versionName = "manifest.json";
		Soaring.SetVersionedFileRepo(serverContentUrl + SBSettings.MANIFEST_FILE, serverContentUrl, TFUtils.GetPersistentAssetsPath(), versionName);
		SoaringInternal.instance.Versions.MaxActiveConnections = SBSettings.PATCHING_FILE_LIMIT;
		string text = CommonUtils.TextureLod().ToString();
		SoaringInternal.instance.Versions.SubContentCategories.addObject(text);
		RegisterModules();
	}

	private static void RegisterModules()
	{
		SoaringInternal.instance.RegisterModule(new SBMIAquireEventGiftModule());
		SoaringInternal.instance.RegisterModule(new SBMISetEventValueModule());
		SoaringInternal.instance.RegisterModule(new SBMIMoveAccountModule());
		SoaringInternal.instance.RegisterModule(new SBMIFinalizeMigrationModule());
		SoaringInternal.instance.RegisterModule(new SBMIRetrieveDailyBonusCalendarModule());
		SoaringInternal.instance.RegisterModule(new SBMIRetrieveSessionFromUserModule());
		SoaringInternal.instance.RegisterModule(new SBMIAddCredentialsToUserModule());
		SoaringInternal.instance.RegisterModule(new SBMIAddCharacterFoodModule());
		if (!SoaringInternal.IsProductionMode)
		{
			SoaringInternal.instance.RegisterModule(new SBMIResetEventValueModule());
		}
	}

	public static SoaringDictionary ConvertDictionary(Dictionary<string, object> dict)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (dict == null)
		{
			return soaringDictionary;
		}
		foreach (KeyValuePair<string, object> item in dict)
		{
			object value = item.Value;
			Type type = value.GetType();
			if (type == typeof(string))
			{
				soaringDictionary.addValue(TFUtils.TryLoadString(dict, item.Key), item.Key);
			}
			else if (type == typeof(int))
			{
				int? num = TFUtils.TryLoadInt(dict, item.Key);
				soaringDictionary.addValue((!num.HasValue) ? null : ((SoaringValue)num.Value), item.Key);
			}
			else if (type == typeof(float))
			{
				float? num2 = TFUtils.TryLoadFloat(dict, item.Key);
				soaringDictionary.addValue((!num2.HasValue) ? null : ((SoaringValue)num2.Value), item.Key);
			}
			else if (type == typeof(long) || type == typeof(ulong))
			{
				long? num3 = TFUtils.TryLoadLong(dict, item.Key);
				soaringDictionary.addValue((!num3.HasValue) ? null : ((SoaringValue)num3.Value), item.Key);
			}
			else if (type == typeof(bool))
			{
				bool? flag = TFUtils.TryLoadBool(dict, item.Key);
				soaringDictionary.addValue((!flag.HasValue) ? null : ((SoaringValue)flag.Value), item.Key);
			}
			else if (type == typeof(Dictionary<string, object>))
			{
				soaringDictionary.addValue(ConvertDictionary((Dictionary<string, object>)item.Value), item.Key);
			}
			else if (type == typeof(List<object>))
			{
				soaringDictionary.addValue(ConvertArray((List<object>)item.Value), item.Key);
			}
			else if (type == typeof(List<Dictionary<string, object>>))
			{
				soaringDictionary.addValue(ConvertArray((List<Dictionary<string, object>>)item.Value), item.Key);
			}
			else
			{
				TFUtils.ErrorLog("ConvertDictionary: Unknown Type: " + item.Key + " : " + type.ToString());
			}
		}
		return soaringDictionary;
	}

	private static SoaringArray ConvertArray(List<object> list)
	{
		SoaringArray soaringArray = new SoaringArray();
		if (list == null)
		{
			return soaringArray;
		}
		foreach (object item in list)
		{
			Type type = item.GetType();
			if (type == typeof(string))
			{
				soaringArray.addObject((string)item);
				continue;
			}
			if (type == typeof(int))
			{
				soaringArray.addObject(TFUtils.LoadIntObjectHelper(item));
				continue;
			}
			if (type == typeof(float))
			{
				float? num = TFUtils.LoadFloatObjectHelper(item);
				soaringArray.addObject((!num.HasValue) ? null : ((SoaringValue)num.Value));
				continue;
			}
			if (type == typeof(long) || type == typeof(ulong))
			{
				soaringArray.addObject(TFUtils.LoadLongObjectHelper(item));
				continue;
			}
			if (type == typeof(bool))
			{
				bool? flag = TFUtils.LoadBoolObjectHelper(item);
				soaringArray.addObject((!flag.HasValue) ? null : ((SoaringValue)flag.Value));
				continue;
			}
			if (type == typeof(Dictionary<string, object>))
			{
				soaringArray.addObject(ConvertDictionary((Dictionary<string, object>)item));
				continue;
			}
			if (type == typeof(List<object>))
			{
				soaringArray.addObject(ConvertArray((List<object>)item));
				continue;
			}
			TFUtils.ErrorLog(string.Concat("ConvertArray: Unknown Type: ", item, " : ", type.ToString()));
		}
		return soaringArray;
	}

	private static SoaringArray ConvertArray(List<Dictionary<string, object>> list)
	{
		SoaringArray soaringArray = new SoaringArray();
		if (list == null)
		{
			return soaringArray;
		}
		foreach (Dictionary<string, object> item in list)
		{
			soaringArray.addObject(ConvertDictionary(item));
		}
		return soaringArray;
	}

	public static Dictionary<string, object> ConvertDictionaryToGeneric(SoaringDictionary dict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (dict == null)
		{
			return dictionary;
		}
		SoaringObjectBase[] array = dict.allValues();
		string[] array2 = dict.allKeys();
		for (int i = 0; i < dict.count(); i++)
		{
			SoaringObjectBase soaringObjectBase = array[i];
			SoaringObjectBase.IsType type = soaringObjectBase.Type;
			switch (type)
			{
			case SoaringObjectBase.IsType.String:
				dictionary.Add(array2[i], (string)(SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Int:
				dictionary.Add(array2[i], (long)(SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Float:
				dictionary.Add(array2[i], (double)(SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Boolean:
				dictionary.Add(array2[i], (bool)(SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Dictionary:
				dictionary.Add(array2[i], ConvertDictionaryToGeneric((SoaringDictionary)soaringObjectBase));
				break;
			case SoaringObjectBase.IsType.Array:
				dictionary.Add(array2[i], ConvertArrayToGeneric((SoaringArray)soaringObjectBase));
				break;
			case SoaringObjectBase.IsType.Null:
				dictionary.Add(array2[i], null);
				break;
			default:
				TFUtils.ErrorLog("ConvertDictionary: Unknown Type: " + array2[i] + " : " + type);
				break;
			}
		}
		return dictionary;
	}

	private static List<object> ConvertArrayToGeneric(SoaringArray list)
	{
		List<object> list2 = new List<object>();
		if (list == null)
		{
			return list2;
		}
		for (int i = 0; i < list.count(); i++)
		{
			SoaringObjectBase soaringObjectBase = list.objectAtIndex(i);
			SoaringObjectBase.IsType type = soaringObjectBase.Type;
			switch (type)
			{
			case SoaringObjectBase.IsType.String:
				list2.Add((string)(SoaringValue)soaringObjectBase);
				continue;
			case SoaringObjectBase.IsType.Float:
				list2.Add((double)(SoaringValue)soaringObjectBase);
				continue;
			case SoaringObjectBase.IsType.Int:
				list2.Add((long)(SoaringValue)soaringObjectBase);
				continue;
			case SoaringObjectBase.IsType.Boolean:
				list2.Add((bool)(SoaringValue)soaringObjectBase);
				continue;
			case SoaringObjectBase.IsType.Dictionary:
				list2.Add(ConvertDictionaryToGeneric((SoaringDictionary)soaringObjectBase));
				continue;
			case SoaringObjectBase.IsType.Array:
				list2.Add(ConvertArrayToGeneric((SoaringArray)soaringObjectBase));
				continue;
			}
			TFUtils.ErrorLog(string.Concat("ConvertArray: Unknown Type: ", soaringObjectBase, " : ", type.ToString()));
		}
		return list2;
	}

	public static void SetEventValue(Session session, SoaringValue event_id, SoaringValue event_value, SoaringContext context = null)
	{
		if (context == null)
		{
			context = CheckContext();
		}
		context.addValue(event_id, "eventDid");
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "setEventValue");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		soaringDictionary.addValue(event_value, "value");
		SoaringInternal.instance.CallModule("setEventValue", soaringDictionary, context);
	}

	public static void GetEventValue(Session session, SoaringValue event_id, SoaringContext context = null)
	{
		if (context == null)
		{
			context = CheckContext();
		}
		context.addValue(event_id, "eventDid");
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "getEventValue");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		SoaringInternal.instance.CallModule("getEventValue", soaringDictionary, context);
	}

	public static void AddFoodToCharacter(SoaringValue value, SoaringValue characterDID, int day = -1, SoaringContext context = null)
	{
		if (context == null)
		{
			context = CheckContext();
		}
		context.addValue(value, "value");
		context.addValue(characterDID, "characterDid");
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "addFoodToCharacter");
			return;
		}
		if (value == null)
		{
			CallbackFailedModule(CreateInvalidParametersError("value"), context, "addFoodToCharacter");
			return;
		}
		if (characterDID == null)
		{
			CallbackFailedModule(CreateInvalidParametersError("characterDid"), context, "addFoodToCharacter");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(value, "value");
		soaringDictionary.addValue(characterDID, "characterDid");
		if (!SBSettings.SoaringProduction && day != -1)
		{
			soaringDictionary.addValue(day, "__day__");
		}
		SoaringInternal.instance.CallModule("addFoodToCharacter", soaringDictionary, context);
	}

	public static void AquireEventGift(Session session, SoaringValue event_id, SoaringValue gift_id, int purchaseCost, bool purchased = false, SoaringContext context = null)
	{
		if (context == null)
		{
			context = CheckContext();
		}
		context.addValue(event_id, "eventDid");
		context.addValue(gift_id, "giftDid");
		context.addValue(purchased, "purchased");
		context.addValue(purchaseCost, "purchaseCost");
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "acquireEventGift");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		soaringDictionary.addValue(gift_id, "giftDid");
		if (purchased)
		{
			soaringDictionary.addValue(purchased, "purchased");
		}
		SoaringInternal.instance.CallModule("acquireEventGift", soaringDictionary, context);
	}

	public static void ResetEventGifts(Session session, SoaringValue event_id, SoaringContext context = null)
	{
		if (context == null)
		{
			context = CheckContext();
		}
		context.addValue(event_id, "eventDid");
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "resetEventGifts");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		SoaringInternal.instance.CallModule("resetEventGifts", soaringDictionary, context);
	}

	public static void FinalizeMigration(string playerID, SoaringLoginType type, SoaringContext context)
	{
		if (string.IsNullOrEmpty(playerID))
		{
			CallbackFailedModule(CreateInvalidCredentialsError("Invalid Player ID"), context, "finalizeMigration");
			return;
		}
		string text = "device";
		if (type != SoaringLoginType.Soaring && type != SoaringLoginType.Device && TFUtils.isAmazon())
		{
			text = "amazon";
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(playerID, "playerId");
		soaringDictionary.addValue(text, "accountType");
		playerID = SystemInfo.deviceUniqueIdentifier;
		soaringDictionary.addValue(playerID, "croppedPlayerId");
		soaringDictionary.addValue(SoaringInternal.instance.GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary.addValue(SoaringInternal.instance.GenerateAppDataDictionary(), "appInfo");
		SoaringInternal.instance.CallModule("finalizeMigration", soaringDictionary, context);
	}

	public static void MigratePlayerToNewPlayer(string srcPlayerID, SoaringLoginType srcType, string targetPlayerID, SoaringLoginType targetType, SoaringContext context)
	{
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "moveAccount");
			return;
		}
		if (string.IsNullOrEmpty(srcPlayerID) && string.IsNullOrEmpty(targetPlayerID))
		{
			CallbackFailedModule(CreateInvalidCredentialsError("Invalid User Id"), context, "moveAccount");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(targetPlayerID))
		{
			SoaringDictionary soaringDictionary2 = new SoaringDictionary();
			soaringDictionary.addValue(soaringDictionary2, "target");
			soaringDictionary2.addValue(targetPlayerID, SoaringInternal.instance.PlatformKeyWithLoginType(targetType, true));
		}
		if (!string.IsNullOrEmpty(srcPlayerID))
		{
			SoaringDictionary soaringDictionary3 = new SoaringDictionary();
			soaringDictionary.addValue(soaringDictionary3, "source");
			soaringDictionary3.addValue(srcPlayerID, SoaringInternal.instance.PlatformKeyWithLoginType(srcType, true));
		}
		SoaringInternal.instance.CallModule("moveAccount", soaringDictionary, context);
	}

	public static void RetrieveDailyBonuseCalendar(int day = -1, SoaringContext context = null, SoaringContextDelegate context_delegate = null)
	{
		if (context == null)
		{
			context = new SoaringContext();
			context.Name = "DailyBonus";
			context.Responder = new SMBICacheDelegate();
		}
		if (context_delegate != null)
		{
			context.ContextResponder = context_delegate;
		}
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "retrieveDailyBonusCalendar");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!SBSettings.SoaringProduction && day != -1)
		{
			soaringDictionary.addValue(day, "__day__");
		}
		SoaringInternal.instance.CallModule("retrieveDailyBonusCalendar", soaringDictionary, context);
	}

	public static void RetrieveUsersSession(SoaringContext context = null)
	{
		if (context == null)
		{
			context = new SoaringContext();
			context.Name = "RetrieveUsersSession";
			context.Responder = new SMBICacheDelegate();
		}
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "retrieveSessionFromUser");
			return;
		}
		SoaringDictionary data = new SoaringDictionary();
		SoaringInternal.instance.CallModule("retrieveSessionFromUser", data, context);
	}

	public static void AddCredentialsToUsers(SoaringArray identifiers, SoaringContext context = null)
	{
		if (context == null)
		{
			context = new SoaringContext();
			context.Name = "RetrieveUsersSession";
			context.Responder = new SMBICacheDelegate();
		}
		if (!Soaring.IsAuthorized)
		{
			CallbackFailedModule(CreateInvalidAuthCodeError(), context, "addCredentialsToUser");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(identifiers, "identifiers");
		SoaringInternal.instance.CallModule("addCredentialsToUser", soaringDictionary, context);
	}

	public static SoaringArray<SBMIDailyBonusDay> GetCachedDailyBonus(ref int day, ref bool alreadyCollected)
	{
		alreadyCollected = mAlreadyCollected;
		day = mCurrentDailyBonusDay;
		return mDailyBonusCalendar;
	}

	private static void CallbackFailedModule(SoaringError error, SoaringContext context, string moduleName)
	{
		if (context == null)
		{
			context = CheckContext();
		}
		if (context != null && context.Responder != null)
		{
			context.Responder.OnComponentFinished(false, moduleName, error, null, context);
		}
		else
		{
			Soaring.Delegate.OnComponentFinished(false, moduleName, error, null, context);
		}
	}

	private static SoaringError CreateInvalidAuthCodeError()
	{
		return new SoaringError("Invalid User Auth Code.", -2);
	}

	private static SoaringError CreateInvalidCredentialsError(string str)
	{
		return new SoaringError(str, -3);
	}

	private static SoaringError CreateInvalidParametersError(string param)
	{
		return new SoaringError("Invalid Parameter: " + param, -9);
	}

	private static SoaringContext CheckContext()
	{
		SoaringContext soaringContext = "CommunityEvents";
		soaringContext.Responder = new SoaringCommunityEventDelegate();
		return soaringContext;
	}

	private static SoaringError CheckForError(string error)
	{
		if (error == null)
		{
			return null;
		}
		SoaringError soaringError = null;
		string text = error.ToLower();
		if (text.Contains("is not defined"))
		{
			return new SoaringError(error, 404);
		}
		if (text.Contains("is not active"))
		{
			return new SoaringError(error, 404);
		}
		return new SoaringError(error, -1);
	}
}
