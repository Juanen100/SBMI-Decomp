using System;
using MTools;
using UnityEngine;

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
				if (loginType == SoaringLoginType.Soaring || loginType == SoaringLoginType.Device)
				{
					if (soaringTag == null)
					{
						return SoaringPlatform.PlatformUserID;
					}
					return soaringTag;
				}
				return SoaringPlatform.PlatformUserAlias;
			}
		}

		public SoaringPlayerData()
			: base(IsType.Object)
		{
		}

		public override string ToJsonString()
		{
			return "{\"0\":\"" + soaringTag + "\",\"1\":\"" + ((platformID != null) ? platformID : string.Empty) + "\",\"2\":\"" + password + "\",\"4\":\"" + userID + "\",\"3\":\"" + SoaringInternal.PlatformKeyAbriviationWithLoginType(loginType, true) + "\"}";
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
			return sUserArray;
		}
	}

	public SoaringPlayerResolver()
	{
	}

	public SoaringPlayerResolver(bool retrieveID)
	{
		RetrieveID = retrieveID;
	}

	public SoaringPlayerResolver(SoaringPlayerData platform_user, SoaringPlayerData player_last, SoaringPlayerData device_player)
	{
		ResolvePlatformData = platform_user;
		ResolveLastUserData = player_last;
		ResolveDeviceData = device_player;
		RetrieveID = false;
	}

	public static bool Load(SoaringPlayer player, string loadPlayer)
	{
		bool flag = true;
		if (PlayerPrefs.HasKey("SCFWS_CaC"))
		{
			return LoadV1(player);
		}
		return LoadV2(player, loadPlayer);
	}

	private static bool LoadV1(SoaringPlayer player)
	{
		SoaringPlayer.ValidCredentials = PlayerPrefs.GetInt("SCFWS_CaC") != 0;
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		string text = PlayerPrefs.GetString("SCFWS_AuthTo", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			soaringDictionary.addValue(text, "authToken");
		}
		text = PlayerPrefs.GetString("SCFWS_Tag", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			soaringDictionary.addValue(text, "tag");
		}
		text = PlayerPrefs.GetString("SCFWS_UserID", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			soaringDictionary.addValue(text, "userId");
		}
		text = PlayerPrefs.GetString("SCFWS_Password", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			soaringDictionary.addValue(text, "password");
		}
		text = PlayerPrefs.GetString("SCFWS_Invite", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			soaringDictionary.addValue(text, "invitationCode");
		}
		player.SetUserData(soaringDictionary);
		PlayerPrefs.DeleteAll();
		return true;
	}

	private static string LoadSoaringPlayers()
	{
		SoaringDictionary soaringDictionary = null;
		string text = null;
		string text2 = null;
		int num = 0;
		try
		{
			MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringUsers", "Soaring", "dat", 1);
			if (fileStream != null)
			{
				if (fileStream.IsOpen())
				{
					text = fileStream.ReadString();
					text2 = fileStream.ReadString();
					if (!string.IsNullOrEmpty(text))
					{
						string s = fileStream.ReadString();
						byte[] array = Convert.FromBase64String(s);
						s = string.Empty;
						if (array == null)
						{
							s = "{}";
						}
						else
						{
							for (int i = 0; i < array.Length; i++)
							{
								s += (char)array[i];
							}
						}
						SoaringDebug.Log(text + "\n" + text2 + "\n " + s, LogType.Warning);
						soaringDictionary = new SoaringDictionary(s);
						num = soaringDictionary.count();
					}
				}
				else
				{
					SoaringDebug.Log("Failed To Open Users Data", LogType.Warning);
				}
			}
			else
			{
				SoaringDebug.Log("Failed To Create Users Data Reader", LogType.Warning);
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringPlayerResolver: " + ex.Message + "\n" + ex.StackTrace, LogType.Error);
			soaringDictionary = null;
			num = 0;
		}
		if (num == 0)
		{
		}
		if (num > 0)
		{
			SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectAtIndex(0);
			int num2 = soaringArray.count();
			sUserArray = new SoaringArray(num2);
			for (int j = 0; j < num2; j++)
			{
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringArray.objectAtIndex(j);
				if (soaringDictionary2 != null)
				{
					SoaringPlayerData obj = ExtractPlayerData(soaringDictionary2);
					sUserArray.addObject(obj);
				}
			}
		}
		else
		{
			sUserArray = new SoaringArray();
		}
		sProperties = text;
		return text2;
	}

	private static bool TestLoadPlatformUserID(string last_user, SoaringContext context, bool retrieveID)
	{
		if (context == null)
		{
			context = new SoaringContext();
		}
		context.Responder = new SoaringPlayerResolver(retrieveID);
		if (!string.IsNullOrEmpty(last_user))
		{
			context.addValue(last_user, "last_user");
		}
		context.addValue(sProperties, "properties");
		return SoaringPlatform.AuthenticatedPlatformUser(context);
	}

	public static void FindLoginID(SoaringContext context)
	{
		string text = LoadSoaringPlayers();
		if (!string.IsNullOrEmpty(text))
		{
			context.addValue(text, "last_user");
		}
		bool flag = true;
		if (SoaringPlatform.PlatformLoginAvailable)
		{
			if (SoaringPlatform.PlatformLoginAuthenticated)
			{
				context.addValue(SoaringPlatform.PlatformUserID, "platform_id");
			}
			else
			{
				flag = !TestLoadPlatformUserID(text, context, true);
			}
		}
		if (flag)
		{
			FindLoginIDReturn(context);
		}
	}

	private static void FindLoginIDReturn(SoaringContext context)
	{
		SoaringValue soaringValue = context.soaringValue("platform_id");
		string text = null;
		SoaringLoginType soaringLoginType = Soaring.PreferedDeviceLogin;
		if (!string.IsNullOrEmpty(soaringValue))
		{
			text = soaringValue;
		}
		else
		{
			soaringLoginType = SoaringLoginType.Device;
			text = SoaringPlatform.DeviceID;
		}
		context.removeObjectWithKey("platform_id");
		context.removeObjectWithKey("last_user");
		context.addValue(text, "id");
		context.addValue((int)soaringLoginType, "type");
		context.Responder = null;
		if (context.ContextResponder != null)
		{
			context.ContextResponder(context);
		}
	}

	private static bool LoadV2(SoaringPlayer player, string loadPlayer)
	{
		string text = LoadSoaringPlayers();
		if (!string.IsNullOrEmpty(loadPlayer))
		{
			text = loadPlayer;
		}
		if (SoaringPlatform.PlatformLoginAvailable)
		{
			if (SoaringPlatform.PlatformLoginAuthenticated)
			{
				return LoadPart2(SoaringPlatform.PlatformUserID, SoaringPlatform.PlatformUserAlias, text);
			}
			if (TestLoadPlatformUserID(text, null, false))
			{
				return false;
			}
		}
		return LoadPart2(null, null, text);
	}

	private static void SetContextData(SoaringContext context, SoaringPlayerData playerData)
	{
		if (context != null)
		{
			context.addValue(playerData, "user_data");
		}
	}

	private static SoaringPlayerData NullPlayerDataResolver(SoaringPlayerData data, string userID, SoaringLoginType loginType)
	{
		if (data != null)
		{
			return data;
		}
		data = new SoaringPlayerData();
		data.soaringTag = userID;
		data.platformID = userID;
		data.loginType = loginType;
		return data;
	}

	private static bool CanCallLogin(SoaringPlayerData playerData)
	{
		if (playerData == null)
		{
			return false;
		}
		return !string.IsNullOrEmpty(playerData.password) && !string.IsNullOrEmpty(playerData.soaringTag);
	}

	private static bool LoadPart2(string platformUserID, string platformUserAlias, string lastUser)
	{
		Debug.LogWarning("LoadPart2: " + lastUser);
		if (string.IsNullOrEmpty(platformUserAlias))
		{
			platformUserAlias = platformUserID;
		}
		bool flag = false;
		if (!string.IsNullOrEmpty(platformUserID))
		{
			flag = true;
		}
		if (SoaringInternalProperties.ForceOfflineModeUser)
		{
			if (string.IsNullOrEmpty(platformUserID))
			{
				platformUserID = "Player";
				platformUserAlias = platformUserID;
			}
			SoaringPlayerData soaringPlayerData = GetUserData(platformUserID, true);
			if (soaringPlayerData == null)
			{
				soaringPlayerData = new SoaringPlayerData();
				soaringPlayerData.soaringTag = (soaringPlayerData.platformID = platformUserID);
				soaringPlayerData.loginType = SoaringLoginType.Device;
			}
			SoaringDictionary soaringDictionary = new SoaringDictionary(4);
			soaringDictionary.addValue(soaringPlayerData.soaringTag, "tag");
			soaringDictionary.addValue("Offline", "userId");
			soaringDictionary.addValue(string.Empty, "authToken");
			SoaringInternal.instance.UpdatePlayerData(soaringDictionary);
			Soaring.Player.LoginType = soaringPlayerData.loginType;
			Soaring.Player.IsLocalAuthorized = true;
			SoaringContext soaringContext = new SoaringContext();
			soaringContext.Responder = new SoaringPlayerResolver();
			soaringContext.addValue(soaringPlayerData, "user_data");
			soaringContext.Responder.OnAuthorize(true, null, Soaring.Player, soaringContext);
			return false;
		}
		Soaring.Player.IsLocalAuthorized = false;
		SoaringContext soaringContext2 = new SoaringContext();
		if (flag)
		{
			Debug.LogError("LoadPart2: Platform: " + platformUserID);
			SoaringPlayerData userData = null;
			soaringContext2.Responder = new SoaringPlayerResolver();
			if (string.IsNullOrEmpty(lastUser))
			{
				Debug.LogError("No Last User");
				userData = GetUserData(platformUserID, true);
				SetContextData(soaringContext2, NullPlayerDataResolver(userData, platformUserID, Soaring.PreferedDeviceLogin));
				if (CanCallLogin(userData))
				{
					Soaring.Login(userData.soaringTag, userData.password, soaringContext2);
				}
				else
				{
					LookupUser(platformUserID, Soaring.PreferedDeviceLogin, soaringContext2);
				}
			}
			else if (IsSamePlayer(platformUserID, lastUser, ref userData))
			{
				Debug.LogError("IsSamePlayer");
				SetContextData(soaringContext2, NullPlayerDataResolver(userData, platformUserID, Soaring.PreferedDeviceLogin));
				if (CanCallLogin(userData))
				{
					Soaring.Login(userData.soaringTag, userData.password, soaringContext2);
				}
				else
				{
					LookupUser(platformUserID, Soaring.PreferedDeviceLogin, soaringContext2);
				}
			}
			else
			{
				if (!SoaringInternalProperties.AutoChooseUserPlayer)
				{
					SoaringPlayerData soaringPlayerData2 = null;
					if (userData != null)
					{
						soaringPlayerData2 = userData;
					}
					else
					{
						soaringPlayerData2 = new SoaringPlayerData();
						soaringPlayerData2.soaringTag = (soaringPlayerData2.platformID = platformUserID);
						soaringPlayerData2.loginType = Soaring.PreferedDeviceLogin;
					}
					SoaringPlayerData soaringPlayerData3 = GetUserData(lastUser, true);
					if (soaringPlayerData3 == null)
					{
						soaringPlayerData3 = GetUserData(lastUser, false);
					}
					SoaringPlayerData soaringPlayerData4 = CreateDevicePlayerData();
					if (soaringPlayerData3 == null)
					{
						soaringPlayerData3 = soaringPlayerData4;
					}
					Debug.LogError(soaringPlayerData2.ToJsonString());
					Debug.LogError(soaringPlayerData3.ToJsonString());
					Debug.LogError(soaringPlayerData4.ToJsonString());
					Soaring.Delegate.OnPlayerConflict(new SoaringPlayerResolver(soaringPlayerData2, soaringPlayerData3, soaringPlayerData4), soaringPlayerData2, soaringPlayerData3, soaringPlayerData4, null);
					return false;
				}
				SetContextData(soaringContext2, NullPlayerDataResolver(null, platformUserID, Soaring.PreferedDeviceLogin));
				LookupUser(platformUserID, Soaring.PreferedDeviceLogin, soaringContext2);
			}
		}
		else
		{
			soaringContext2.Responder = new SoaringPlayerResolver();
			if (string.IsNullOrEmpty(lastUser))
			{
				SoaringDebug.Log("SoaringPlayerResolve: Warning: No Valid Last User", LogType.Warning);
				SetContextData(soaringContext2, NullPlayerDataResolver(null, SoaringPlatform.DeviceID, SoaringLoginType.Device));
				LookupUser(SoaringPlatform.DeviceID, SoaringLoginType.Device, soaringContext2);
			}
			else
			{
				bool flag2 = false;
				if (SoaringInternalProperties.AutoChooseUserPlayer)
				{
					SoaringPlayerData userData2 = GetUserData(lastUser, false);
					SetContextData(soaringContext2, NullPlayerDataResolver(userData2, SoaringPlatform.DeviceID, SoaringLoginType.Device));
					if (CanCallLogin(userData2))
					{
						Soaring.Login(userData2.soaringTag, userData2.password, soaringContext2);
					}
					else
					{
						flag2 = true;
					}
				}
				else
				{
					SoaringPlayerData userData3 = GetUserData(lastUser, false);
					if (userData3 == null)
					{
						flag2 = true;
						SetContextData(soaringContext2, NullPlayerDataResolver(userData3, SoaringPlatform.DeviceID, SoaringLoginType.Device));
					}
					else
					{
						Debug.LogError(userData3.ToJsonString());
						if (userData3.platformID == SoaringPlatform.DeviceID)
						{
							SoaringDebug.Log("lastUser == SoaringPlatform.DeviceID", LogType.Error);
							SetContextData(soaringContext2, NullPlayerDataResolver(userData3, SoaringPlatform.DeviceID, SoaringLoginType.Device));
							if (CanCallLogin(userData3))
							{
								Soaring.Login(userData3.soaringTag, userData3.password, soaringContext2);
							}
							else
							{
								flag2 = true;
							}
						}
						else if (userData3.loginType == SoaringLoginType.Device || userData3.loginType == SoaringLoginType.Soaring)
						{
							SoaringDebug.Log("lastUser == SoaringLoginType." + userData3.loginType, LogType.Error);
							SetContextData(soaringContext2, NullPlayerDataResolver(userData3, SoaringPlatform.DeviceID, SoaringLoginType.Device));
							if (CanCallLogin(userData3))
							{
								Soaring.Login(userData3.soaringTag, userData3.password, soaringContext2);
							}
							else
							{
								LookupUserWithTag(userData3.soaringTag, userData3.userID, soaringContext2);
							}
						}
						else
						{
							SoaringDebug.Log("lastUser != SoaringPlatform.DeviceID: " + userData3.loginType, LogType.Error);
							SoaringPlayerData soaringPlayerData5 = GetUserData(SoaringPlatform.DeviceID, true);
							if (soaringPlayerData5 == null)
							{
								soaringPlayerData5 = CreateDevicePlayerData();
							}
							Soaring.Delegate.OnPlayerConflict(new SoaringPlayerResolver(null, userData3, soaringPlayerData5), null, userData3, soaringPlayerData5, null);
						}
					}
				}
				if (flag2)
				{
					if (lastUser == null)
					{
						lastUser = "Unknown Last user";
					}
					SoaringDebug.Log("SoaringPlayerResolve: Error: No Valid Last User: " + lastUser, LogType.Error);
					LookupUser(SoaringPlatform.DeviceID, SoaringLoginType.Device, soaringContext2);
				}
			}
		}
		return true;
	}

	public static void Save(string lastUser = null)
	{
		if (sUserArray == null)
		{
			return;
		}
		try
		{
			string writePath = ResourceUtils.GetWritePath("SoaringUsers.dat", "Soaring", 1);
			MBinaryWriter mBinaryWriter = new MBinaryWriter();
			if (!mBinaryWriter.Open(writePath, true))
			{
				SoaringDebug.Log("Failed To Save Users Data: LU: " + lastUser, LogType.Error);
				return;
			}
			string val = "0";
			string text = lastUser;
			if (string.IsNullOrEmpty(text))
			{
				text = Soaring.Player.UserTag;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = string.Empty;
			}
			mBinaryWriter.Write(val);
			mBinaryWriter.Write(text);
			SoaringDictionary soaringDictionary = new SoaringDictionary(1);
			soaringDictionary.addValue(sUserArray, "0");
			string text2 = soaringDictionary.ToJsonString();
			byte[] array = new byte[text2.Length];
			for (int i = 0; i < text2.Length; i++)
			{
				array[i] = (byte)text2[i];
			}
			text2 = Convert.ToBase64String(array);
			mBinaryWriter.Write(text2);
			mBinaryWriter.Close();
			mBinaryWriter = null;
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Failed To Save Users Data: " + ex.Message + "\n" + ex.StackTrace, LogType.Error);
		}
	}

	private static bool IsSamePlayer(string platformUserID, string lastUserTag, ref SoaringPlayerData userData)
	{
		userData = null;
		if (sUserArray == null)
		{
			return false;
		}
		int num = sUserArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringPlayerData soaringPlayerData = (SoaringPlayerData)sUserArray.objectAtIndex(i);
			if (soaringPlayerData != null)
			{
				Debug.Log(soaringPlayerData.ToJsonString() + " vs " + platformUserID + " : " + lastUserTag);
				if ((!(lastUserTag != soaringPlayerData.soaringTag) || !(lastUserTag != soaringPlayerData.platformID)) && soaringPlayerData.platformID == platformUserID)
				{
					userData = soaringPlayerData;
					return true;
				}
			}
		}
		return false;
	}

	private static SoaringPlayerData ExtractPlayerData(SoaringDictionary userData)
	{
		if (userData == null)
		{
			return null;
		}
		SoaringPlayerData soaringPlayerData = new SoaringPlayerData();
		soaringPlayerData.soaringTag = userData.soaringValue("0");
		soaringPlayerData.platformID = userData.soaringValue("1");
		soaringPlayerData.password = userData.soaringValue("2");
		soaringPlayerData.userID = userData.soaringValue("4");
		string userID = userData.soaringValue("3");
		soaringPlayerData.loginType = SoaringInternal.PlatformKeyAbriviationWithTag(userID);
		return soaringPlayerData;
	}

	public static SoaringPlayerData CreateDevicePlayerData()
	{
		SoaringPlayerData soaringPlayerData = GetUserData(SoaringPlatform.DeviceID, false);
		if (soaringPlayerData == null)
		{
			soaringPlayerData = GetUserData(SoaringPlatform.DeviceID, true);
		}
		if (soaringPlayerData == null)
		{
			soaringPlayerData = new SoaringPlayerData();
			soaringPlayerData.loginType = SoaringLoginType.Device;
			soaringPlayerData.soaringTag = (soaringPlayerData.platformID = SoaringPlatform.DeviceID);
		}
		return soaringPlayerData;
	}

	private static SoaringPlayerData GetUserData(string userID, bool checkPlatformID)
	{
		if (sUserArray == null || string.IsNullOrEmpty(userID))
		{
			return null;
		}
		int num = sUserArray.count();
		SoaringPlayerData result = null;
		for (int i = 0; i < num; i++)
		{
			SoaringPlayerData soaringPlayerData = (SoaringPlayerData)sUserArray.objectAtIndex(i);
			if (soaringPlayerData == null)
			{
				continue;
			}
			if (checkPlatformID)
			{
				if (userID != soaringPlayerData.platformID)
				{
					continue;
				}
			}
			else if (userID != soaringPlayerData.soaringTag)
			{
				continue;
			}
			result = soaringPlayerData;
			break;
		}
		return result;
	}

	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		Debug.LogWarning(GetType().Name + ": OnComponentFinished: \n" + data.ToJsonString());
		if (string.IsNullOrEmpty(module) || !module.Equals("login"))
		{
			return;
		}
		if (RetrieveID)
		{
			if (data != null)
			{
				string text = data.soaringValue("id");
				string userAlias = data.soaringValue("name");
				SoaringPlatform.SetPlatformUserData(text, userAlias);
				context.addValue(text, "platform_id");
			}
			FindLoginIDReturn(context);
		}
		else
		{
			string lastUser = context.soaringValue("last_user");
			if (data == null)
			{
				LoadPart2(null, null, lastUser);
				return;
			}
			string text2 = data.soaringValue("id");
			string text3 = data.soaringValue("name");
			SoaringPlatform.SetPlatformUserData(text2, text3);
			LoadPart2(text2, text3, lastUser);
		}
	}

	public bool BadConnection(SoaringError error)
	{
		if (error == null)
		{
			return false;
		}
		return error.ErrorCode == -6 || error.ErrorCode == -10 || error.ErrorCode == -8 || error.ErrorCode == -9 || error.ErrorCode == -7;
	}

	public static void RemovePlayer(SoaringPlayerData data)
	{
		if (data == null || string.IsNullOrEmpty(data.soaringTag))
		{
			return;
		}
		for (int i = 0; i < sUserArray.count(); i++)
		{
			SoaringPlayerData soaringPlayerData = (SoaringPlayerData)sUserArray.objectAtIndex(i);
			if (soaringPlayerData.soaringTag == data.soaringTag)
			{
				SoaringDebug.Log("REMOVING PLAYER ID: " + soaringPlayerData.ToJsonString() + " With: " + data.ToJsonString(), LogType.Error);
				sUserArray.removeObjectAtIndex(i);
				i--;
			}
		}
	}

	private static void UpdateSaveData(SoaringPlayerData data)
	{
		if (data == null)
		{
			return;
		}
		data.soaringTag = Soaring.Player.UserTag;
		data.userID = Soaring.Player.UserID;
		if (string.IsNullOrEmpty(data.soaringTag) || string.IsNullOrEmpty(data.userID))
		{
			return;
		}
		data.password = Soaring.Player.Password;
		for (int i = 0; i < sUserArray.count(); i++)
		{
			SoaringPlayerData soaringPlayerData = (SoaringPlayerData)sUserArray.objectAtIndex(i);
			if (soaringPlayerData.userID == data.userID)
			{
				if (soaringPlayerData.soaringTag != data.soaringTag || soaringPlayerData.platformID != data.platformID || soaringPlayerData.password != data.password)
				{
					SoaringDebug.Log("UPDATING PLAYER ID: " + soaringPlayerData.ToJsonString() + " TO: " + data.ToJsonString(), LogType.Error);
					sUserArray.setObjectAtIndex(data, i);
					Save();
				}
				return;
			}
		}
		sUserArray.addObject(data);
		Save();
	}

	public override void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
		if (error != null)
		{
			success = false;
		}
		SoaringPlayerData soaringPlayerData = null;
		if (context != null)
		{
			soaringPlayerData = (SoaringPlayerData)context.objectWithKey("user_data");
		}
		if (!success)
		{
			if (BadConnection(error))
			{
				SoaringInternal.instance.TriggerOfflineMode(true);
				OnAuthorize(success, error, Soaring.Player, context);
			}
			else if (CanCallLogin(soaringPlayerData))
			{
				Soaring.Login(soaringPlayerData.soaringTag, soaringPlayerData.password, context);
			}
			else
			{
				Soaring.GenerateUniqueNewUserName(context);
			}
		}
		Debug.LogError(soaringPlayerData.ToJsonString());
		Soaring.Delegate.OnLookupUser(success, error, null);
		if (success)
		{
			OnAuthorize(success, error, Soaring.Player, context);
		}
	}

	public override void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
		if (error != null || string.IsNullOrEmpty(nextTag))
		{
			success = false;
		}
		Soaring.Delegate.OnGenerateUserName(success, error, nextTag, null);
		if (!success)
		{
			OnAuthorize(success, error, Soaring.Player, context);
			return;
		}
		SoaringPlayerData soaringPlayerData = null;
		if (context != null)
		{
			soaringPlayerData = (SoaringPlayerData)context.objectWithKey("user_data");
		}
		Soaring.RegisterLiteUser(nextTag, soaringPlayerData.platformID, soaringPlayerData.loginType, context);
	}

	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (error != null)
		{
			success = false;
		}
		SoaringPlayerData soaringPlayerData = null;
		if (context != null)
		{
			soaringPlayerData = (SoaringPlayerData)context.objectWithKey("user_data");
		}
		player.LoginType = soaringPlayerData.loginType;
		if (!success)
		{
			Debug.LogError(error.ToJsonString());
			SoaringInternal.instance.TriggerOfflineMode(true);
			if (!string.IsNullOrEmpty(soaringPlayerData.userID) && !string.IsNullOrEmpty(soaringPlayerData.platformID) && !string.IsNullOrEmpty(soaringPlayerData.soaringTag))
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(soaringPlayerData.userID, "userId");
				soaringDictionary.addValue(soaringPlayerData.soaringTag, "tag");
				Soaring.Player.SetUserData(soaringDictionary);
				Debug.LogError(soaringDictionary.ToJsonString());
			}
		}
		else
		{
			UpdateSaveData(soaringPlayerData);
		}
		Soaring.Delegate.OnAuthorize(success, error, player, null);
	}

	public override void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (!success)
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
		}
		Soaring.Delegate.OnRegisterUser(success, error, player, null);
		OnAuthorize(success, error, player, context);
	}

	public void HandleLoginConflict(SoaringPlayerData playerData, SoaringContext context = null)
	{
		if (playerData == null)
		{
			SoaringDebug.Log("HandleLogingConflict: Error: Null Player Data Selected");
			playerData = CreateDevicePlayerData();
		}
		if (context == null)
		{
			context = new SoaringContext();
			context.Responder = new SoaringPlayerResolver();
		}
		SetContextData(context, NullPlayerDataResolver(playerData, playerData.platformID, playerData.loginType));
		if (CanCallLogin(playerData))
		{
			Soaring.Login(playerData.soaringTag, playerData.password, context);
		}
		else
		{
			LookupUser(playerData.platformID, playerData.loginType, context);
		}
	}

	private static void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context)
	{
		SoaringDebug.Log(platformID + " : " + loginType);
		Soaring.LookupUser(platformID, loginType, context);
	}

	private static void LookupUserWithTag(string userTag, string userID, SoaringContext context)
	{
		SoaringDebug.Log(userTag + " : Tag");
		SoaringArray soaringArray = new SoaringArray();
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringArray.addObject(soaringDictionary);
		soaringDictionary.addValue(userTag, "tag");
		Soaring.LookupUser(soaringArray, context);
	}
}
