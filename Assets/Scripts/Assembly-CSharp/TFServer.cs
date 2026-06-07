using System;
using System.Collections.Generic;
using System.Net;

public class TFServer
{
	public delegate void JsonStringHandler(string jsonResponse, object userData);

	public delegate void JsonResponseHandler(Dictionary<string, object> dict, object userData);

	public const string ERROR_KEY = "error";

	public const string NETWORK_ERROR = "Network error";

	private static string NETWORK_ERROR_JSON;

	private const bool LOG_FAILED_REQUESTS = true;

	private static string mLOG_LOCATION;

	private const ulong DEACTIVATION_PERIOD = 60uL;

	private const int STRIKE_OUT = 3;

	private static int errorCount;

	private bool loggingIn;

	private int strikes;

	private bool activeConnection;

	private ulong deactivatedTime;

	private CookieContainer cookies;

	private Dictionary<TFWebClient, JsonStringHandler> reqs;

	private bool unreachable;

	private static string LOG_LOCATION
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public CookieContainer Cookies
	{
		set
		{
		}
	}

	public bool Connected
	{
		get
		{
			return false;
		}
	}

	public TFServer()
	{
	}

	public TFServer(CookieContainer cookies)
	{
	}

	public static bool IsNetworkError(Dictionary<string, object> response)
	{
		return false;
	}

	public void SetConnected(bool val)
	{
	}

	public void PostToJSON(string url, Dictionary<string, object> postDict, JsonResponseHandler callback, bool checkConnection = false, object userData = null)
	{
	}

	public void GetToJSON(string url, JsonResponseHandler callback, bool checkConnection = false, object userData = null)
	{
	}

	public Cookie GetCookie(Uri uri, string key)
	{
		return null;
	}

	private TFWebClient RegisterCallback(JsonStringHandler callback)
	{
		return null;
	}

	private TFWebClient RegisterCallback(JsonResponseHandler callback)
	{
		return null;
	}

	private string encodePostData(Dictionary<string, object> d)
	{
		return null;
	}

	private JsonStringHandler JsCallback(JsonResponseHandler cb)
	{
		return null;
	}

	private void OnNetworkError(TFWebClient client, JsonStringHandler callback)
	{
	}

	private void OnCallComplete(TFWebClient client)
	{
	}

	private JsonStringHandler GetCallback(TFWebClient sender)
	{
		return null;
	}

	private void LogResponse(HttpWebResponse response)
	{
	}

	private void CheckConnectivity()
	{
	}

	private bool ShortCircuitRequest()
	{
		return false;
	}
}
