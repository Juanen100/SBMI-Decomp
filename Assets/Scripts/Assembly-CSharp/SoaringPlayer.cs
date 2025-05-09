public class SoaringPlayer : SoaringUser
{
	private SoaringArray<SoaringUser> mFriends = new SoaringArray<SoaringUser>();

	private bool mCanSaveUserCredentials;

	public static bool ValidCredentials;

	public SoaringUser[] Friends
	{
		get
		{
			return mFriends.array();
		}
	}

	public SoaringLoginType LoginType { get; set; }

	public bool IsLocalAuthorized { get; set; }

	public bool HasFriend
	{
		get
		{
			if (mFriends == null)
			{
				return false;
			}
			int num = mFriends.count();
			for (int i = 0; i <= num; i++)
			{
				if (mFriends[i] != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	public string AuthToken
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("authToken");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	public string GameCenterID
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("gamecenterId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	public string GoogleID
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("googleId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	public string AmazonID
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("amazonId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	public string Password
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("password");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	public string InviteCode
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("invitationCode");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	public bool LightUser
	{
		get
		{
			if (mUserData == null)
			{
				return true;
			}
			return mUserData.soaringValue("autoCreated");
		}
	}

	public bool CanSaveUserCredentials
	{
		get
		{
			return mCanSaveUserCredentials;
		}
		set
		{
			mCanSaveUserCredentials = value;
		}
	}

	public void SetFriendsData(SoaringArray<SoaringUser> users)
	{
		if (users != null)
		{
			mFriends = users;
		}
	}

	public bool Load(string userID = null)
	{
		return SoaringPlayerResolver.Load(this, userID);
	}

	public void Save()
	{
		SoaringPlayerResolver.Save();
	}

	public void ClearSavedCredentials()
	{
		bool flag = mCanSaveUserCredentials;
		mCanSaveUserCredentials = false;
		Save();
		mCanSaveUserCredentials = flag;
	}
}
