using System;
using System.Collections.Generic;
using System.Net;
using MiniJSON;

public class TFWebResponse
{
	public HttpStatusCode StatusCode;

	public string Data;

	public WebHeaderCollection Headers;

	public bool NetworkDown;

	public Exception Error;

	public Dictionary<string, object> GetAsJSONDict()
	{
		if (Data != null && StatusCode == HttpStatusCode.OK)
		{
			object obj = Json.Deserialize(Data);
			if (obj.GetType() == typeof(Dictionary<string, object>))
			{
				return (Dictionary<string, object>)Json.Deserialize(Data);
			}
		}
		return null;
	}
}
