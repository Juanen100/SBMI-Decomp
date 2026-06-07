using System.Collections.Generic;

public class SBMISoaring
{
	public class SBMIDailyBonusDay : SoaringObjectBase
	{
		private SoaringDictionary mData;

		public int Day
		{
			get
			{
				return 0;
			}
		}

		public int CurrencyDID
		{
			get
			{
				return 0;
			}
		}

		public int CurrencyAmount
		{
			get
			{
				return 0;
			}
		}

		public SBMIDailyBonusDay(SoaringDictionary data)
			: base(default(IsType))
		{
		}

		private int GetSoaringValue(string str)
		{
			return 0;
		}
	}

	public class SMBICacheDelegate : SoaringDelegate
	{
		public bool IsError(bool success, SoaringError err, SoaringDictionary data)
		{
			return false;
		}

		public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
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

	private static int mCurrentDailyBonusDay;

	private static SoaringArray<SBMIDailyBonusDay> mDailyBonusCalendar;

	private static bool mAlreadyCollected;

	public static long PatchTownTimestamp
	{
		get
		{
			return 0L;
		}
		set
		{
		}
	}

	public static long PatchTownTreasureSpawnTimestamp
	{
		get
		{
			return 0L;
		}
		set
		{
		}
	}

	public static int PatchTownTreasureCollected
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public static void Initialize(SoaringDelegate del)
	{
	}

	public void ResetCachedData()
	{
	}

	public static void OnInitializeSoaring()
	{
	}

	private static void RegisterModules()
	{
	}

	public static SoaringDictionary ConvertDictionary(Dictionary<string, object> dict)
	{
		return null;
	}

	private static SoaringArray ConvertArray(List<object> list)
	{
		return null;
	}

	private static SoaringArray ConvertArray(List<Dictionary<string, object>> list)
	{
		return null;
	}

	public static Dictionary<string, object> ConvertDictionaryToGeneric(SoaringDictionary dict)
	{
		return null;
	}

	private static List<object> ConvertArrayToGeneric(SoaringArray list)
	{
		return null;
	}

	public static void SetEventValue(Session session, SoaringValue event_id, SoaringValue event_value, SoaringContext context = null)
	{
	}

	public static void GetEventValue(Session session, SoaringValue event_id, SoaringContext context = null)
	{
	}

	public static void AddFoodToCharacter(SoaringValue value, SoaringValue characterDID, int day = -1, SoaringContext context = null)
	{
	}

	public static void ValidateUpsightRewardSignature(Session session, string signatureData, SoaringContext context = null)
	{
	}

	public static void AquireEventGift(Session session, SoaringValue event_id, SoaringValue gift_id, int purchaseCost, bool purchased = false, SoaringContext context = null)
	{
	}

	public static void ResetEventGifts(Session session, SoaringValue event_id, SoaringContext context = null)
	{
	}

	public static void FinalizeMigration(string playerID, SoaringLoginType type, SoaringContext context)
	{
	}

	public static void MigratePlayerToNewPlayer(string srcPlayerID, SoaringLoginType srcType, string targetPlayerID, SoaringLoginType targetType, SoaringContext context)
	{
	}

	public static void RetrieveDailyBonuseCalendar(int day = -1, SoaringContext context = null, SoaringContextDelegate context_delegate = null)
	{
	}

	public static void RetrieveUsersSession(SoaringContext context = null)
	{
	}

	public static void AddCredentialsToUsers(SoaringArray identifiers, SoaringContext context = null)
	{
	}

	public static SoaringArray<SBMIDailyBonusDay> GetCachedDailyBonus(ref int day, ref bool alreadyCollected)
	{
		return null;
	}

	private static void CallbackFailedModule(SoaringError error, SoaringContext context, string moduleName)
	{
	}

	private static SoaringError CreateInvalidAuthCodeError()
	{
		return null;
	}

	private static SoaringError CreateInvalidCredentialsError(string str)
	{
		return null;
	}

	private static SoaringError CreateInvalidParametersError(string param)
	{
		return null;
	}

	private static SoaringContext CheckContext()
	{
		return null;
	}

	private static SoaringError CheckForError(string error)
	{
		return null;
	}
}
