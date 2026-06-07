public class SoaringValue : SoaringObjectBase
{
	protected long IntVal;

	protected double FloatVal;

	protected string StringVal;

	public SoaringValue(int val)
		: base(default(IsType))
	{
	}

	public SoaringValue(long val)
		: base(default(IsType))
	{
	}

	public SoaringValue(float val)
		: base(default(IsType))
	{
	}

	public SoaringValue(double val)
		: base(default(IsType))
	{
	}

	public SoaringValue(string val)
		: base(default(IsType))
	{
	}

	public SoaringValue(bool val)
		: base(default(IsType))
	{
	}

	public static implicit operator SoaringValue(int b)
	{
		return null;
	}

	public static implicit operator SoaringValue(long b)
	{
		return null;
	}

	public static implicit operator SoaringValue(float b)
	{
		return null;
	}

	public static implicit operator SoaringValue(double b)
	{
		return null;
	}

	public static implicit operator SoaringValue(string b)
	{
		return null;
	}

	public static implicit operator SoaringValue(bool b)
	{
		return null;
	}

	public static implicit operator int(SoaringValue b)
	{
		return 0;
	}

	public static implicit operator long(SoaringValue b)
	{
		return 0L;
	}

	public static implicit operator float(SoaringValue b)
	{
		return 0f;
	}

	public static implicit operator double(SoaringValue b)
	{
		return 0.0;
	}

	public static implicit operator bool(SoaringValue b)
	{
		return false;
	}

	public static implicit operator string(SoaringValue b)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}

	public override string ToJsonString()
	{
		return null;
	}

	public string ProtectString(string initial)
	{
		return null;
	}
}
