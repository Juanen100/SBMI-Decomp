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
			return null;
		}
	}

	public string MessageID
	{
		get
		{
			return null;
		}
	}

	public string SenderID
	{
		get
		{
			return null;
		}
	}

	public int SenderDate
	{
		get
		{
			return 0;
		}
	}

	public string MessageBody
	{
		get
		{
			return null;
		}
	}

	public string RecipientUserID
	{
		get
		{
			return null;
		}
	}

	public SoaringMessage()
		: base(default(IsType))
	{
	}

	public SoaringMessage(string to, string body, string category)
		: base(default(IsType))
	{
	}

	public void SetMessageSendData(int date)
	{
	}

	public void SetMessageID(string id)
	{
	}

	public void SetSenderID(string id)
	{
	}

	public void AddRecipientUserID(string userID)
	{
	}

	public void AddRecipientInviteCode(string ic)
	{
	}

	public void AddRecipientTag(string tag)
	{
	}

	public void SetCategory(string cat)
	{
	}

	public void SetTextBody(string text)
	{
	}

	public override string ToJsonString()
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
