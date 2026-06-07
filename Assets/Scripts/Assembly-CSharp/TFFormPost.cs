using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public static class TFFormPost
{
	private static readonly Encoding encoding;

	public static HttpWebResponse PostForm(Uri postUri, string userAgent, Dictionary<string, object> postParameters, CookieContainer cookies)
	{
		return null;
	}

	private static byte[] GetFormData(Dictionary<string, object> postParameters, string boundary)
	{
		return null;
	}
}
