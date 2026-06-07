using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RestSharp.Contrib
{
	internal class HttpEncoder
	{
		private static char[] hexChars;

		private static object entitiesLock;

		private static SortedDictionary<string, char> entities;

		private static HttpEncoder defaultEncoder;

		private static HttpEncoder currentEncoder;

		private static IDictionary<string, char> Entities
		{
			get
			{
				return null;
			}
		}

		public static HttpEncoder Current
		{
			get
			{
				return null;
			}
		}

		public static HttpEncoder Default
		{
			get
			{
				return null;
			}
		}

		static HttpEncoder()
		{
		}

		internal static void HeaderNameValueEncode(string headerName, string headerValue, out string encodedHeaderName, out string encodedHeaderValue)
		{
			encodedHeaderName = null;
			encodedHeaderValue = null;
		}

		private static void StringBuilderAppend(string s, ref StringBuilder sb)
		{
		}

		private static string EncodeHeaderString(string input)
		{
			return null;
		}

		internal static string UrlPathEncode(string value)
		{
			return null;
		}

		internal static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			return null;
		}

		internal static string HtmlEncode(string s)
		{
			return null;
		}

		internal static string HtmlAttributeEncode(string s)
		{
			return null;
		}

		internal static string HtmlDecode(string s)
		{
			return null;
		}

		internal static bool NotEncoded(char c)
		{
			return false;
		}

		internal static void UrlEncodeChar(char c, Stream result, bool isUnicode)
		{
		}

		internal static void UrlPathEncodeChar(char c, Stream result)
		{
		}

		private static void InitEntities()
		{
		}
	}
}
