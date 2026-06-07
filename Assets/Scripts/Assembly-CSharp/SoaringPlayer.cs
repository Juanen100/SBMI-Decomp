public class SoaringPlayer : SoaringUser
{
	private SoaringArray<SoaringUser> mFriends;

	private bool mCanSaveUserCredentials;

	public static bool ValidCredentials;

	public SoaringUser[] Friends
	{
		get
		{
			return null;
		}
	}

	public SoaringLoginType LoginType { get; set; }

	public bool IsLocalAuthorized { get; set; }

	public bool HasFriend
	{
		get
		{
			return false;
		}
	}

	public string AuthToken
	{
		get
		{
			return null;
		}
	}

	public string GameCenterID
	{
		get
		{
			return null;
		}
	}

	public string GoogleID
	{
		get
		{
			return null;
		}
	}

	public string AmazonID
	{
		get
		{
			return null;
		}
	}

	public string Password
	{
		get
		{
			return null;
		}
	}

	public string InviteCode
	{
		get
		{
			return null;
		}
	}

	public bool LightUser
	{
		get
		{
			return false;
		}
	}

	public bool CanSaveUserCredentials
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public void SetFriendsData(SoaringArray<SoaringUser> users)
	{
	}

	public bool Load(string userID = null)
	{
		return false;
	}

	public void Save()
	{
	}

	public void ClearSavedCredentials()
	{
	}
}
