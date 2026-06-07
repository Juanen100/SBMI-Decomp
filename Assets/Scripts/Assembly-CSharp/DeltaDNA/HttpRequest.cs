using System.Collections.Generic;

namespace DeltaDNA
{
	internal class HttpRequest
	{
		internal enum HTTPMethodType
		{
			GET = 0,
			POST = 1
		}

		private Dictionary<string, string> headers;

		internal string URL { get; private set; }

		internal HTTPMethodType HTTPMethod { get; set; }

		internal string HTTPBody { get; set; }

		internal int TimeoutSeconds { get; set; }

		internal HttpRequest(string url)
		{
		}

		internal Dictionary<string, string> getHeaders()
		{
			return null;
		}

		internal void setHeader(string field, string value)
		{
		}

		public override string ToString()
		{
			return null;
		}
	}
}
