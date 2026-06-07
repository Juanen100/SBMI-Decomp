using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AmazonCommon
{
	public class MiniJSON
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

		protected static int lastErrorIndex;

		protected static string lastDecode;

		public static object jsonDecode(string json)
		{
			return null;
		}

		public static string jsonEncode(object json)
		{
			return null;
		}

		public static bool lastDecodeSuccessful()
		{
			return false;
		}

		public static int getLastErrorIndex()
		{
			return 0;
		}

		public static string getLastErrorSnippet()
		{
			return null;
		}

		protected static Hashtable parseObject(char[] json, ref int index)
		{
			return null;
		}

		protected static ArrayList parseArray(char[] json, ref int index)
		{
			return null;
		}

		protected static object parseValue(char[] json, ref int index, ref bool success)
		{
			return null;
		}

		protected static string parseString(char[] json, ref int index)
		{
			return null;
		}

		protected static double parseNumber(char[] json, ref int index)
		{
			return 0.0;
		}

		protected static int getLastIndexOfNumber(char[] json, int index)
		{
			return 0;
		}

		protected static void eatWhitespace(char[] json, ref int index)
		{
		}

		protected static int lookAhead(char[] json, int index)
		{
			return 0;
		}

		protected static int nextToken(char[] json, ref int index)
		{
			return 0;
		}

		protected static bool serializeObjectOrArray(object objectOrArray, StringBuilder builder)
		{
			return false;
		}

		protected static bool serializeObject(Hashtable anObject, StringBuilder builder)
		{
			return false;
		}

		protected static bool serializeDictionary(Dictionary<string, string> dict, StringBuilder builder)
		{
			return false;
		}

		protected static bool serializeDictionary(Dictionary<string, double> dict, StringBuilder builder)
		{
			return false;
		}

		protected static bool serializeArray(ArrayList anArray, StringBuilder builder)
		{
			return false;
		}

		protected static bool serializeValue(object value, StringBuilder builder)
		{
			return false;
		}

		protected static void serializeString(string aString, StringBuilder builder)
		{
		}

		protected static void serializeNumber(double number, StringBuilder builder)
		{
		}
	}
}
