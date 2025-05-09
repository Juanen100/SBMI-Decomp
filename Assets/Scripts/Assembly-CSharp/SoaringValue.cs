public class SoaringValue : SoaringObjectBase
{
	protected long IntVal;

	protected double FloatVal;

	protected string StringVal;

	public SoaringValue(int val)
		: base(IsType.Int)
	{
		IntVal = val;
	}

	public SoaringValue(long val)
		: base(IsType.Int)
	{
		IntVal = val;
	}

	public SoaringValue(float val)
		: base(IsType.Float)
	{
		FloatVal = val;
	}

	public SoaringValue(double val)
		: base(IsType.Float)
	{
		FloatVal = val;
	}

	public SoaringValue(string val)
		: base(IsType.String)
	{
		StringVal = val;
	}

	public SoaringValue(bool val)
		: base(IsType.Boolean)
	{
		IntVal = (val ? 1 : 0);
	}

	public override string ToString()
	{
		string text = null;
		if (mType == IsType.Int)
		{
			return IntVal.ToString();
		}
		if (mType == IsType.Float)
		{
			return FloatVal.ToString();
		}
		if (mType == IsType.String)
		{
			return StringVal;
		}
		if (mType == IsType.Boolean)
		{
			return (IntVal == 0L) ? "false" : "true";
		}
		return string.Empty;
	}

	public override string ToJsonString()
	{
		string text = null;
		if (mType == IsType.Int)
		{
			return IntVal.ToString();
		}
		if (mType == IsType.Float)
		{
			return FloatVal.ToString();
		}
		if (mType == IsType.String)
		{
			return "\"" + ProtectString(StringVal) + "\"";
		}
		if (mType == IsType.Boolean)
		{
			return (IntVal == 0L) ? "false" : "true";
		}
		return "\"\"";
	}

	public string ProtectString(string initial)
	{
		if (string.IsNullOrEmpty(initial))
		{
			return initial;
		}
		return initial.Replace("\"", "\\\"");
	}

	public static implicit operator SoaringValue(int b)
	{
		return new SoaringValue(b);
	}

	public static implicit operator SoaringValue(long b)
	{
		return new SoaringValue(b);
	}

	public static implicit operator SoaringValue(float b)
	{
		return new SoaringValue(b);
	}

	public static implicit operator SoaringValue(double b)
	{
		return new SoaringValue(b);
	}

	public static implicit operator SoaringValue(string b)
	{
		return new SoaringValue(b);
	}

	public static implicit operator SoaringValue(bool b)
	{
		return new SoaringValue(b);
	}

	public static implicit operator int(SoaringValue b)
	{
		int result = 0;
		if (b == null)
		{
			return result;
		}
		if (b.mType == IsType.Int)
		{
			result = (int)b.IntVal;
		}
		return result;
	}

	public static implicit operator long(SoaringValue b)
	{
		long result = 0L;
		if (b == null)
		{
			return result;
		}
		if (b.mType == IsType.Int)
		{
			result = b.IntVal;
		}
		return result;
	}

	public static implicit operator float(SoaringValue b)
	{
		float result = float.NaN;
		if (b == null)
		{
			return result;
		}
		if (b.mType == IsType.Float)
		{
			result = (float)b.FloatVal;
		}
		return result;
	}

	public static implicit operator double(SoaringValue b)
	{
		double result = double.NaN;
		if (b == null)
		{
			return result;
		}
		if (b.mType == IsType.Float)
		{
			result = b.FloatVal;
		}
		return result;
	}

	public static implicit operator bool(SoaringValue b)
	{
		bool result = false;
		if (b == null)
		{
			return result;
		}
		if (b.mType == IsType.Boolean)
		{
			result = ((b.IntVal != 0L) ? true : false);
		}
		return result;
	}

	public static implicit operator string(SoaringValue b)
	{
		if (b == null)
		{
			return string.Empty;
		}
		return b.ToString();
	}
}
