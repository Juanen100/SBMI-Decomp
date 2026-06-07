public class SoaringPlayerResolver : SoaringDelegate
{
	public class SoaringPlayerData : SoaringObjectBase
	{
		public string soaringTag;

		public string platformID;

		public string password;

		public string userID;

		public SoaringLoginType loginType;

		public string playerAlias
		{
			get
			{
				return null;
			}
		}

		public SoaringPlayerData()
			: base(default(IsType))
		{
		}

		public override string ToJsonString()
		{
			return null;
		}
	}

	public const string Soaring_LastUser_Key = "last_user";

	public const string Soaring_PlatformUser_Key = "platform_id";

	public const string Soaring_ExternalLogin_Key = "login";

	private const string SoaringSoaringTagKey = "0";

	private const string SoaringUserPlatformKey = "1";

	private const string SoaringUserPasswordKey = "2";

	private const string SoaringLoginTypeKey = "3";

	private const string SoaringUserIDKey = "4";

	private static SoaringArray sUserArray;

	private static string sProperties;

	public SoaringPlayerData ResolvePlatformData;

	public SoaringPlayerData ResolveLastUserData;

	public SoaringPlayerData ResolveDeviceData;

	public bool RetrieveID;

	public static SoaringArray UsersArray
	{
		get
		{
			return null;
		}
	}

	public SoaringPlayerResolver()
	{
	}

	public SoaringPlayerResolver(bool retrieveID)
	{
	}

	public SoaringPlayerResolver(SoaringPlayerData platform_user, SoaringPlayerData player_last, SoaringPlayerData device_player)
	{
	}

	public static bool Load(SoaringPlayer player, string loadPlayer)
	{
		return false;
	}

	private static bool LoadV1(SoaringPlayer player)
	{
		return false;
	}

	private static string LoadSoaringPlayers()
	{
		return null;
	}

	private static bool TestLoadPlatformUserID(string last_user, SoaringContext context, bool retrieveID)
	{
		return false;
	}

	public static void FindLoginID(SoaringContext context)
	{
	}

	private static void FindLoginIDReturn(SoaringContext context)
	{
	}

	private static bool LoadV2(SoaringPlayer player, string loadPlayer)
	{
		return false;
	}

	private static void SetContextData(SoaringContext context, SoaringPlayerData playerData)
	{
	}

	private static SoaringPlayerData NullPlayerDataResolver(SoaringPlayerData data, string userID, SoaringLoginType loginType)
	{
		return null;
	}

	private static bool CanCallLogin(SoaringPlayerData playerData)
	{
		return false;
	}

	private static bool LoadPart2(string platformUserID, string platformUserAlias, string lastUser)
	{
		return false;
	}

	public static void Save(string lastUser = null)
	{
	}

	private static bool IsSamePlayer(string platformUserID, string lastUserTag, ref SoaringPlayerData userData)
	{
		return false;
	}

	private static SoaringPlayerData ExtractPlayerData(SoaringDictionary userData)
	{
		return null;
	}

	public static SoaringPlayerData CreateDevicePlayerData()
	{
		return null;
	}

	private static SoaringPlayerData GetUserData(string userID, bool checkPlatformID)
	{
		return null;
	}

	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public bool BadConnection(SoaringError error)
	{
		return false;
	}

	public static void RemovePlayer(SoaringPlayerData data)
	{
	}

	private static void UpdateSaveData(SoaringPlayerData data)
	{
	}

	public override void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
	}

	public override void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
	}

	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	public override void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	public void HandleLoginConflict(SoaringPlayerData playerData, SoaringContext context = null)
	{
	}

	private static void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context)
	{
	}

	private static void LookupUserWithTag(string userTag, string userID, SoaringContext context)
	{
	}
}
