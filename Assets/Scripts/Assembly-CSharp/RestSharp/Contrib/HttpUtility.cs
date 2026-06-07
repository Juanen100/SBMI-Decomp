using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace RestSharp.Contrib
{
	public sealed class HttpUtility
	{
		private sealed class HttpQSCollection : NameValueCollection
		{
			public override string ToString()
			{
				return null;
			}
		}

		public static void HtmlAttributeEncode(string s, TextWriter output)
		{
		}

		public static string HtmlAttributeEncode(string s)
		{
			return null;
		}

		public static string UrlDecode(string str)
		{
			return null;
		}

		private static char[] GetChars(MemoryStream b, Encoding e)
		{
			return null;
		}

		private static void WriteCharBytes(IList buf, char ch, Encoding e)
		{
		}

		public static string UrlDecode(string s, Encoding e)
		{
			return null;
		}

		public static string UrlDecode(byte[] bytes, Encoding e)
		{
			return null;
		}

		private static int GetInt(byte b)
		{
			return 0;
		}

		private static int GetChar(byte[] bytes, int offset, int length)
		{
			return 0;
		}

		private static int GetChar(string str, int offset, int length)
		{
			return 0;
		}

		public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
		{
			return null;
		}

		public static byte[] UrlDecodeToBytes(byte[] bytes)
		{
			return null;
		}

		public static byte[] UrlDecodeToBytes(string str)
		{
			return null;
		}

		public static byte[] UrlDecodeToBytes(string str, Encoding e)
		{
			return null;
		}

		public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
		{
			return null;
		}

		public static string UrlEncode(string str)
		{
			return null;
		}

		public static string UrlEncode(string s, Encoding Enc)
		{
			return null;
		}

		public static string UrlEncode(byte[] bytes)
		{
			return null;
		}

		public static string UrlEncode(byte[] bytes, int offset, int count)
		{
			return null;
		}

		public static byte[] UrlEncodeToBytes(string str)
		{
			return null;
		}

		public static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			return null;
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes)
		{
			return null;
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			return null;
		}

		public static string UrlEncodeUnicode(string str)
		{
			return null;
		}

		public static byte[] UrlEncodeUnicodeToBytes(string str)
		{
			return null;
		}

		public static string HtmlDecode(string s)
		{
			return null;
		}

		public static void HtmlDecode(string s, TextWriter output)
		{
		}

		public static string HtmlEncode(string s)
		{
			return null;
		}

		public static void HtmlEncode(string s, TextWriter output)
		{
		}

		public static string UrlPathEncode(string s)
		{
			return null;
		}

		public static NameValueCollection ParseQueryString(string query)
		{
			return null;
		}

		public static NameValueCollection ParseQueryString(string query, Encoding encoding)
		{
			return null;
		}

		internal static void ParseQueryString(string query, Encoding encoding, NameValueCollection result)
		{
		}
	}
}
