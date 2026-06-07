public class SoaringJSON
{
	private const int TOKEN_NONE = 0;

	private const int TOKEN_CURLY_OPEN = 1;

	private const int TOKEN_CURLY_CLOSE = 2;

	private const int TOKEN_SQUARED_OPEN = 3;

	private const int TOKEN_SQUARED_CLOSE = 4;

	private const int TOKEN_COLON = 5;

	private const int TOKEN_COMMA = 6;

	private const int TOKEN_STRING = 7;

	private const int TOKEN_NUMBER = 8;

	private const int TOKEN_TRUE = 9;

	private const int TOKEN_FALSE = 10;

	private const int TOKEN_NULL = 11;

	private const int BUILDER_CAPACITY = 2000;

	public static SoaringDictionary jsonDecode(string json, SoaringDictionary tables)
	{
		return null;
	}

	public static SoaringDictionary jsonDecode(byte[] json, SoaringDictionary tables)
	{
		return null;
	}

	protected static SoaringDictionary parseObject(char[] json, ref int index, SoaringDictionary table)
	{
		return null;
	}

	protected static SoaringDictionary parseObjectRaw(byte[] json, ref int index, SoaringDictionary table)
	{
		return null;
	}

	protected static SoaringArray parseArray(char[] json, ref int index)
	{
		return null;
	}

	protected static SoaringArray parseArrayRaw(byte[] json, ref int index)
	{
		return null;
	}

	protected static SoaringObjectBase parseValue(char[] json, ref int index, ref bool success)
	{
		return null;
	}

	protected static SoaringObjectBase parseValueRaw(byte[] json, ref int index, ref bool success)
	{
		return null;
	}

	protected static string parseString(char[] json, ref int index)
	{
		return null;
	}

	protected static string parseStringRaw(byte[] json, ref int index)
	{
		return null;
	}

	protected static SoaringValue parseNumber(char[] json, ref int index)
	{
		return null;
	}

	protected static SoaringValue parseNumberRaw(byte[] json, ref int index)
	{
		return null;
	}

	protected static int getLastIndexOfNumber(char[] json, int index)
	{
		return 0;
	}

	protected static int getLastIndexOfNumberRaw(byte[] json, int index)
	{
		return 0;
	}

	protected static void eatWhitespace(char[] json, ref int index)
	{
	}

	protected static void eatWhitespaceRaw(byte[] json, ref int index)
	{
	}

	protected static int lookAhead(char[] json, int index)
	{
		return 0;
	}

	protected static int lookAheadRaw(byte[] json, int index)
	{
		return 0;
	}

	protected static int nextToken(char[] json, ref int index)
	{
		return 0;
	}

	protected static int nextTokenRaw(byte[] json, ref int index)
	{
		return 0;
	}
}
