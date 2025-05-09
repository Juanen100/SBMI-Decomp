public class SoaringMessage : SoaringObjectBase
{
	private string mSenderID;

	private string mMessageID;

	private SoaringArray mUsers;

	private string mCategory;

	private string mBody;

	private int mSendDate;

	public string Category
	{
		get
		{
			return mCategory;
		}
	}

	public string MessageID
	{
		get
		{
			return mMessageID;
		}
	}

	public string SenderID
	{
		get
		{
			return mSenderID;
		}
	}

	public int SenderDate
	{
		get
		{
			return mSendDate;
		}
	}

	public string MessageBody
	{
		get
		{
			return mBody;
		}
	}

	public string RecipientUserID
	{
		get
		{
			string result = string.Empty;
			int num = mUsers.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)mUsers.objectAtIndex(i);
				string text = soaringDictionary.soaringValue("userId");
				if (!string.IsNullOrEmpty(text))
				{
					result = text;
					break;
				}
			}
			return result;
		}
	}

	public SoaringMessage()
		: base(IsType.Object)
	{
		mUsers = new SoaringArray();
		mCategory = string.Empty;
		mBody = string.Empty;
	}

	public SoaringMessage(string to, string body, string category)
		: base(IsType.Object)
	{
		mUsers = new SoaringArray();
		AddRecipientTag(to);
		SetCategory(category);
		mBody = body;
	}

	public void SetMessageSendData(int date)
	{
		mSendDate = date;
	}

	public void SetMessageID(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			mMessageID = id;
		}
	}

	public void SetSenderID(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			mSenderID = id;
		}
	}

	public void AddRecipientUserID(string userID)
	{
		if (!string.IsNullOrEmpty(userID))
		{
			SoaringDictionary soaringDictionary = new SoaringDictionary(1);
			soaringDictionary.addValue(userID, "userId");
			mUsers.addObject(soaringDictionary);
		}
	}

	public void AddRecipientInviteCode(string ic)
	{
		if (!string.IsNullOrEmpty(ic))
		{
			SoaringDictionary soaringDictionary = new SoaringDictionary(1);
			soaringDictionary.addValue(ic, "ic");
			mUsers.addObject(soaringDictionary);
		}
	}

	public void AddRecipientTag(string tag)
	{
		if (!string.IsNullOrEmpty(tag))
		{
			SoaringDictionary soaringDictionary = new SoaringDictionary(1);
			soaringDictionary.addValue(tag, "tag");
			mUsers.addObject(soaringDictionary);
		}
	}

	public void SetCategory(string cat)
	{
		if (!string.IsNullOrEmpty(cat))
		{
			mCategory = cat;
		}
	}

	public void SetTextBody(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			mBody = text;
		}
	}

	public override string ToJsonString()
	{
		return "{\n\"to\" : " + mUsers.ToJsonString() + ",\n\"category\" : \"" + mCategory + "\",\n\"body\" : \"" + mBody + "\"\n}";
	}

	public override string ToString()
	{
		return string.Format("[SoaringMessage]");
	}
}
