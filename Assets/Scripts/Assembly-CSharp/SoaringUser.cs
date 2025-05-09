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
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("userId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			if (mUserData != null)
			{
				mUserData.setValue(value, "userId");
			}
		}
	}

	public string UserTag
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("tag");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			if (mUserData != null)
			{
				mUserData.setValue(value, "tag");
			}
		}
	}

	public string PictureUrl
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("pictureUrl");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			if (mUserData != null)
			{
				mUserData.setValue(value, "pictureUrl");
			}
		}
	}

	public int Score
	{
		get
		{
			if (mUserData == null)
			{
				return 0;
			}
			SoaringValue soaringValue = mUserData.soaringValue("score");
			if (soaringValue == null)
			{
				soaringValue = 0;
			}
			return soaringValue;
		}
		set
		{
			if (mUserData != null)
			{
				mUserData.setValue(value, "score");
			}
		}
	}

	public string UserStatus
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("status");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	public string UserEmail
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			string text = mUserData.soaringValue("email");
			if (string.IsNullOrEmpty(text))
			{
				SoaringArray soaringArray = (SoaringArray)mUserData.objectWithKey("emails");
				if (soaringArray != null && soaringArray.count() != 0)
				{
					SoaringValue soaringValue = (SoaringValue)soaringArray.objectAtIndex(0);
					if (soaringValue != null)
					{
						text = soaringValue;
					}
				}
				if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}
	}

	public string FacebookID
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			SoaringValue soaringValue = mUserData.soaringValue("facebookId");
			if (soaringValue == null)
			{
				soaringValue = string.Empty;
			}
			return soaringValue;
		}
		set
		{
			if (mUserData != null)
			{
				mUserData.setValue(value, "facebookId");
			}
		}
	}

	public string Name
	{
		get
		{
			if (mUserData == null)
			{
				return string.Empty;
			}
			SoaringValue soaringValue = mUserData.soaringValue("name");
			if (soaringValue == null)
			{
				soaringValue = string.Empty;
			}
			return soaringValue;
		}
		set
		{
			if (mUserData != null)
			{
				mUserData.setValue(value, "name");
			}
		}
	}

	public string UserGameSesssionID
	{
		get
		{
			string text = null;
			SoaringDictionary publicData = PublicData;
			if (publicData == null)
			{
				publicData = CustomData;
				if (CustomData != null)
				{
					text = publicData.soaringValue("gameSessionId");
				}
				if (text == null)
				{
					text = string.Empty;
				}
			}
			else
			{
				text = publicData.soaringValue("gameSessionId");
				if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}
	}

	public SoaringDictionary CustomData
	{
		get
		{
			if (mUserData == null)
			{
				return null;
			}
			return (SoaringDictionary)mUserData.objectWithKey("custom");
		}
	}

	public SoaringDictionary PublicData
	{
		get
		{
			if (mUserData == null)
			{
				return null;
			}
			SoaringDictionary customData = CustomData;
			if (customData == null)
			{
				return null;
			}
			return (SoaringDictionary)customData.objectWithKey("public");
		}
	}

	public SoaringDictionary PublicData_Safe
	{
		get
		{
			if (mUserData == null)
			{
				return null;
			}
			SoaringDictionary soaringDictionary = CustomData;
			if (soaringDictionary == null)
			{
				soaringDictionary = new SoaringDictionary();
				mUserData.addValue(new SoaringDictionary(), "custom");
			}
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("public");
			if (soaringDictionary2 == null)
			{
				soaringDictionary2 = new SoaringDictionary();
				soaringDictionary.addValue(soaringDictionary2, "public");
			}
			return soaringDictionary2;
		}
	}

	public SoaringDictionary PrivateData
	{
		get
		{
			if (mUserData == null)
			{
				return null;
			}
			SoaringDictionary customData = CustomData;
			if (customData == null)
			{
				return null;
			}
			return (SoaringDictionary)customData.objectWithKey("private");
		}
	}

	public SoaringDictionary PrivateData_Safe
	{
		get
		{
			if (mUserData == null)
			{
				return null;
			}
			SoaringDictionary soaringDictionary = CustomData;
			if (soaringDictionary == null)
			{
				soaringDictionary = new SoaringDictionary();
				mUserData.addValue(new SoaringDictionary(), "custom");
			}
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("private");
			if (soaringDictionary2 == null)
			{
				soaringDictionary2 = new SoaringDictionary();
				soaringDictionary.addValue(soaringDictionary2, "private");
			}
			return soaringDictionary2;
		}
	}

	public SoaringDictionary UserData
	{
		get
		{
			return mUserData;
		}
	}

	public SoaringUser()
		: base(IsType.Object)
	{
	}

	public void SetUserData(SoaringDictionary userData)
	{
		SetUserData(userData, false);
	}

	public void SetUserData(SoaringDictionary userData, bool clearExisting)
	{
		if (userData == null)
		{
			return;
		}
		if (mUserData == null || clearExisting)
		{
			mUserData = userData;
			return;
		}
		int num = userData.count();
		string[] array = userData.allKeys();
		SoaringObjectBase[] array2 = userData.allValues();
		for (int i = 0; i < num; i++)
		{
			mUserData.setValue(array2[i], array[i]);
		}
	}

	public void SetUserInfo(SoaringValue val, string key)
	{
		if (!string.IsNullOrEmpty(key) && val != null)
		{
			if (mUserData == null)
			{
				mUserData = new SoaringDictionary();
			}
			mUserData.addValue(val, key);
		}
	}

	public SoaringValue GetUserInfo(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return null;
		}
		if (mUserData == null)
		{
			return null;
		}
		return mUserData.soaringValue(key);
	}
}
