public class SoaringUser : SoaringObjectBase
{
	protected SoaringDictionary mUserData;

	public virtual bool IsFriend
	{
		get
		{
			return false;
		}
	}

	public string UserID
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string UserTag
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string PictureUrl
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int Score
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public string UserStatus
	{
		get
		{
			return null;
		}
	}

	public string UserEmail
	{
		get
		{
			return null;
		}
	}

	public string FacebookID
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string UserGameSesssionID
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary CustomData
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary PublicData
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary PublicData_Safe
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary PrivateData
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary PrivateData_Safe
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary UserData
	{
		get
		{
			return null;
		}
	}

	public SoaringUser()
		: base(default(IsType))
	{
	}

	public void SetUserData(SoaringDictionary userData)
	{
	}

	public void SetUserData(SoaringDictionary userData, bool clearExisting)
	{
	}

	public void SetUserInfo(SoaringValue val, string key)
	{
	}

	public SoaringValue GetUserInfo(string key)
	{
		return null;
	}
}
