using System;
using System.Collections.Generic;
using System.Net;

public class TFWebResponse
{
	public HttpStatusCode StatusCode;

	public string Data;

	public WebHeaderCollection Headers;

	public bool NetworkDown;

	public Exception Error;

	public Dictionary<string, object> GetAsJSONDict()
	{
		return null;
	}
}
