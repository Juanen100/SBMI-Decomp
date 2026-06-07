public class SoaringError : SoaringObjectBase
{
	private string mError;

	private int mErrorCode;

	public string Error
	{
		get
		{
			return null;
		}
	}

	public int ErrorCode
	{
		get
		{
			return 0;
		}
	}

	public SoaringError()
		: base(default(IsType))
	{
	}

	public SoaringError(string error, int code)
		: base(default(IsType))
	{
	}

	public bool InvalidErrorCode()
	{
		return false;
	}

	public static implicit operator SoaringError(int b)
	{
		return null;
	}

	public static implicit operator SoaringError(string b)
	{
		return null;
	}

	public static implicit operator string(SoaringError b)
	{
		return null;
	}

	public static implicit operator int(SoaringError b)
	{
		return 0;
	}

	public override string ToJsonString()
	{
		return null;
	}
}
