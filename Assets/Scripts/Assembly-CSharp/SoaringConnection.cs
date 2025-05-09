public class SoaringConnection
{
	protected string mError;

	protected int mErrorCode;

	protected SCWebQueue.SCData mProperties;

	public virtual bool HasError
	{
		get
		{
			return mError != null;
		}
	}

	public virtual string Error
	{
		get
		{
			return mError;
		}
	}

	public virtual int ErrorCode
	{
		get
		{
			return mErrorCode;
		}
	}

	public virtual float Progress
	{
		get
		{
			return 0f;
		}
	}

	public virtual long Length
	{
		get
		{
			return 0L;
		}
	}

	public virtual bool IsValid
	{
		get
		{
			return false;
		}
	}

	public virtual string ContentAsText
	{
		get
		{
			return null;
		}
	}

	public virtual byte[] Content
	{
		get
		{
			return null;
		}
	}

	public virtual int CacheVersion
	{
		get
		{
			return -1;
		}
	}

	public static void BeginUpdates()
	{
	}

	public static void EndUpdates()
	{
	}

	public virtual bool Create(SCWebQueue.SCData properties)
	{
		mProperties = properties;
		return false;
	}

	public virtual bool IsDone()
	{
		return false;
	}

	public virtual bool SaveData()
	{
		return false;
	}
}
