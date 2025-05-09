public class SoaringError : SoaringObjectBase
{
	private string mError;

	private int mErrorCode = -1;

	public string Error
	{
		get
		{
			return mError;
		}
	}

	public int ErrorCode
	{
		get
		{
			return mErrorCode;
		}
	}

	public SoaringError()
		: base(IsType.Object)
	{
	}

	public SoaringError(string error, int code)
		: base(IsType.Object)
	{
		mError = error;
		mErrorCode = code;
	}

	public bool InvalidErrorCode()
	{
		return ErrorCode == -1;
	}

	public override string ToJsonString()
	{
		return "{\n\"code\":" + mErrorCode + ",\n\"message\":\"" + mError + "\"\n}";
	}

	public static implicit operator SoaringError(int b)
	{
		return new SoaringError(null, b);
	}

	public static implicit operator SoaringError(string b)
	{
		return new SoaringError(b, -1);
	}

	public static implicit operator string(SoaringError b)
	{
		if (b == null)
		{
			return null;
		}
		return b.Error;
	}

	public static implicit operator int(SoaringError b)
	{
		if (b == null)
		{
			return -1;
		}
		return b.ErrorCode;
	}
}
