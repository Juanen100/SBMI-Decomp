using System;
using System.Globalization;

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
		SoaringDictionary result = null;
		if (json != null)
		{
			char[] json2 = json.ToCharArray();
			int index = 0;
			result = parseObject(json2, ref index, tables);
		}
		return result;
	}

	public static SoaringDictionary jsonDecode(byte[] json, SoaringDictionary tables)
	{
		SoaringDictionary result = null;
		if (json != null)
		{
			int index = 0;
			result = parseObjectRaw(json, ref index, tables);
		}
		return result;
	}

	protected static SoaringDictionary parseObject(char[] json, ref int index, SoaringDictionary table)
	{
		if (table == null)
		{
			table = new SoaringDictionary();
		}
		nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			switch (lookAhead(json, index))
			{
			case 0:
				return null;
			case 6:
				nextToken(json, ref index);
				continue;
			case 2:
				nextToken(json, ref index);
				return table;
			}
			string text = parseString(json, ref index);
			if (text == null)
			{
				return null;
			}
			int num = nextToken(json, ref index);
			if (num != 5)
			{
				return null;
			}
			bool success = true;
			SoaringObjectBase val = parseValue(json, ref index, ref success);
			if (!success)
			{
				return null;
			}
			table.addValue(val, text);
		}
		return table;
	}

	protected static SoaringDictionary parseObjectRaw(byte[] json, ref int index, SoaringDictionary table)
	{
		if (table == null)
		{
			table = new SoaringDictionary();
		}
		nextTokenRaw(json, ref index);
		bool flag = false;
		while (!flag)
		{
			switch (lookAheadRaw(json, index))
			{
			case 0:
				return null;
			case 6:
				nextTokenRaw(json, ref index);
				continue;
			case 2:
				nextTokenRaw(json, ref index);
				return table;
			}
			string text = parseStringRaw(json, ref index);
			if (text == null)
			{
				return null;
			}
			int num = nextTokenRaw(json, ref index);
			if (num != 5)
			{
				return null;
			}
			bool success = true;
			SoaringObjectBase val = parseValueRaw(json, ref index, ref success);
			if (!success)
			{
				return null;
			}
			table.addValue(val, text);
		}
		return table;
	}

	protected static SoaringArray parseArray(char[] json, ref int index)
	{
		SoaringArray soaringArray = new SoaringArray();
		nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			switch (lookAhead(json, index))
			{
			case 0:
				return null;
			case 6:
				nextToken(json, ref index);
				continue;
			case 4:
				break;
			default:
			{
				bool success = true;
				SoaringObjectBase obj = parseValue(json, ref index, ref success);
				if (!success)
				{
					return null;
				}
				soaringArray.addObject(obj);
				continue;
			}
			}
			nextToken(json, ref index);
			break;
		}
		return soaringArray;
	}

	protected static SoaringArray parseArrayRaw(byte[] json, ref int index)
	{
		SoaringArray soaringArray = new SoaringArray();
		nextTokenRaw(json, ref index);
		bool flag = false;
		while (!flag)
		{
			switch (lookAheadRaw(json, index))
			{
			case 0:
				return null;
			case 6:
				nextTokenRaw(json, ref index);
				continue;
			case 4:
				break;
			default:
			{
				bool success = true;
				SoaringObjectBase obj = parseValueRaw(json, ref index, ref success);
				if (!success)
				{
					return null;
				}
				soaringArray.addObject(obj);
				continue;
			}
			}
			nextTokenRaw(json, ref index);
			break;
		}
		return soaringArray;
	}

	protected static SoaringObjectBase parseValue(char[] json, ref int index, ref bool success)
	{
		switch (lookAhead(json, index))
		{
		case 7:
			return new SoaringValue(parseString(json, ref index));
		case 8:
			return parseNumber(json, ref index);
		case 1:
			return parseObject(json, ref index, null);
		case 3:
			return parseArray(json, ref index);
		case 9:
			nextToken(json, ref index);
			return new SoaringValue(true);
		case 10:
			nextToken(json, ref index);
			return new SoaringValue(false);
		case 11:
			nextToken(json, ref index);
			return new SoaringNullValue();
		default:
			success = false;
			return null;
		}
	}

	protected static SoaringObjectBase parseValueRaw(byte[] json, ref int index, ref bool success)
	{
		switch (lookAheadRaw(json, index))
		{
		case 7:
			return new SoaringValue(parseStringRaw(json, ref index));
		case 8:
			return parseNumberRaw(json, ref index);
		case 1:
			return parseObjectRaw(json, ref index, null);
		case 3:
			return parseArrayRaw(json, ref index);
		case 9:
			nextTokenRaw(json, ref index);
			return new SoaringValue(true);
		case 10:
			nextTokenRaw(json, ref index);
			return new SoaringValue(false);
		case 11:
			nextTokenRaw(json, ref index);
			return new SoaringNullValue();
		default:
			success = false;
			return null;
		}
	}

	protected static string parseString(char[] json, ref int index)
	{
		string text = string.Empty;
		eatWhitespace(json, ref index);
		char c = json[index++];
		bool flag = false;
		while (!flag && index != json.Length)
		{
			c = json[index++];
			switch (c)
			{
			case '"':
				flag = true;
				break;
			case '\\':
				if (index != json.Length)
				{
					switch (json[index++])
					{
					case '"':
						text += '"';
						continue;
					case '\\':
						text += '\\';
						continue;
					case '/':
						text += '/';
						continue;
					case 'b':
						text += '\b';
						continue;
					case 'f':
						text += '\f';
						continue;
					case 'n':
						text += '\n';
						continue;
					case 'r':
						text += '\r';
						continue;
					case 't':
						text += '\t';
						continue;
					case 'u':
						break;
					default:
						continue;
					}
					int num = json.Length - index;
					if (num >= 4)
					{
						char[] array = new char[4];
						Array.Copy(json, index, array, 0, 4);
						uint utf = uint.Parse(new string(array), NumberStyles.HexNumber);
						text += char.ConvertFromUtf32((int)utf);
						index += 4;
						continue;
					}
				}
				break;
			default:
				text += c;
				continue;
			}
			break;
		}
		if (!flag)
		{
			return null;
		}
		return text;
	}

	protected static string parseStringRaw(byte[] json, ref int index)
	{
		string text = string.Empty;
		eatWhitespaceRaw(json, ref index);
		char c = (char)json[index++];
		bool flag = false;
		while (!flag && index != json.Length)
		{
			c = (char)json[index++];
			switch (c)
			{
			case '"':
				flag = true;
				break;
			case '\\':
				if (index != json.Length)
				{
					switch ((char)json[index++])
					{
					case '"':
						text += '"';
						continue;
					case '\\':
						text += '\\';
						continue;
					case '/':
						text += '/';
						continue;
					case 'b':
						text += '\b';
						continue;
					case 'f':
						text += '\f';
						continue;
					case 'n':
						text += '\n';
						continue;
					case 'r':
						text += '\r';
						continue;
					case 't':
						text += '\t';
						continue;
					case 'u':
						break;
					default:
						continue;
					}
					int num = json.Length - index;
					if (num >= 4)
					{
						char[] array = new char[4];
						Array.Copy(json, index, array, 0, 4);
						uint utf = uint.Parse(new string(array), NumberStyles.HexNumber);
						text += char.ConvertFromUtf32((int)utf);
						index += 4;
						continue;
					}
				}
				break;
			default:
				text += c;
				continue;
			}
			break;
		}
		if (!flag)
		{
			return null;
		}
		return text;
	}

	protected static SoaringValue parseNumber(char[] json, ref int index)
	{
		eatWhitespace(json, ref index);
		int lastIndexOfNumber = getLastIndexOfNumber(json, index);
		int num = lastIndexOfNumber - index + 1;
		char[] array = new char[num];
		SoaringValue soaringValue = null;
		Array.Copy(json, index, array, 0, num);
		index = lastIndexOfNumber + 1;
		double num2 = double.Parse(new string(array));
		if (num2 == (double)(long)num2)
		{
			return new SoaringValue((long)num2);
		}
		return new SoaringValue(num2);
	}

	protected static SoaringValue parseNumberRaw(byte[] json, ref int index)
	{
		eatWhitespaceRaw(json, ref index);
		int lastIndexOfNumberRaw = getLastIndexOfNumberRaw(json, index);
		int num = lastIndexOfNumberRaw - index + 1;
		char[] array = new char[num];
		SoaringValue soaringValue = null;
		Array.Copy(json, index, array, 0, num);
		index = lastIndexOfNumberRaw + 1;
		double num2 = double.Parse(new string(array));
		if (num2 == (double)(long)num2)
		{
			return new SoaringValue((long)num2);
		}
		return new SoaringValue(num2);
	}

	protected static int getLastIndexOfNumber(char[] json, int index)
	{
		int i;
		for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
		{
		}
		return i - 1;
	}

	protected static int getLastIndexOfNumberRaw(byte[] json, int index)
	{
		int i;
		for (i = index; i < json.Length && "0123456789+-.eE".IndexOf((char)json[i]) != -1; i++)
		{
		}
		return i - 1;
	}

	protected static void eatWhitespace(char[] json, ref int index)
	{
		while (index < json.Length && " \t\n\r".IndexOf(json[index]) != -1)
		{
			index++;
		}
	}

	protected static void eatWhitespaceRaw(byte[] json, ref int index)
	{
		while (index < json.Length && " \t\n\r".IndexOf((char)json[index]) != -1)
		{
			index++;
		}
	}

	protected static int lookAhead(char[] json, int index)
	{
		int index2 = index;
		return nextToken(json, ref index2);
	}

	protected static int lookAheadRaw(byte[] json, int index)
	{
		int index2 = index;
		return nextTokenRaw(json, ref index2);
	}

	protected static int nextToken(char[] json, ref int index)
	{
		eatWhitespace(json, ref index);
		if (index == json.Length)
		{
			return 0;
		}
		char c = json[index];
		index++;
		switch (c)
		{
		case '{':
			return 1;
		case '}':
			return 2;
		case '[':
			return 3;
		case ']':
			return 4;
		case ',':
			return 6;
		case '"':
			return 7;
		case '-':
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
			return 8;
		case ':':
			return 5;
		default:
		{
			index--;
			int num = json.Length - index;
			if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
			{
				index += 5;
				return 10;
			}
			if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
			{
				index += 4;
				return 9;
			}
			if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
			{
				index += 4;
				return 11;
			}
			return 0;
		}
		}
	}

	protected static int nextTokenRaw(byte[] json, ref int index)
	{
		eatWhitespaceRaw(json, ref index);
		if (index == json.Length)
		{
			return 0;
		}
		char c = (char)json[index];
		index++;
		switch (c)
		{
		case '{':
			return 1;
		case '}':
			return 2;
		case '[':
			return 3;
		case ']':
			return 4;
		case ',':
			return 6;
		case '"':
			return 7;
		case '-':
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
			return 8;
		case ':':
			return 5;
		default:
		{
			index--;
			int num = json.Length - index;
			if (num >= 5 && json[index] == 102 && json[index + 1] == 97 && json[index + 2] == 108 && json[index + 3] == 115 && json[index + 4] == 101)
			{
				index += 5;
				return 10;
			}
			if (num >= 4 && json[index] == 116 && json[index + 1] == 114 && json[index + 2] == 117 && json[index + 3] == 101)
			{
				index += 4;
				return 9;
			}
			if (num >= 4 && json[index] == 110 && json[index + 1] == 117 && json[index + 2] == 108 && json[index + 3] == 108)
			{
				index += 4;
				return 11;
			}
			return 0;
		}
		}
	}
}
