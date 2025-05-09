using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using MiniJSON;
using UnityEngine;

public class TFServer
{
	public delegate void JsonStringHandler(string jsonResponse, object userData);

	public delegate void JsonResponseHandler(Dictionary<string, object> dict, object userData);

	public const string ERROR_KEY = "error";

	public const string NETWORK_ERROR = "Network error";

	private const bool LOG_FAILED_REQUESTS = true;

	private const ulong DEACTIVATION_PERIOD = 60uL;

	private const int STRIKE_OUT = 3;

	private static string NETWORK_ERROR_JSON = "{\"success\": false, \"error\": \"Network error\"}";

	private static string LOG_LOCATION = Application.persistentDataPath + Path.DirectorySeparatorChar + "error";

	private static int errorCount = 0;

	private bool loggingIn = true;

	private int strikes;

	private bool activeConnection = true;

	private ulong deactivatedTime;

	private CookieContainer cookies = new CookieContainer();

	private Dictionary<TFWebClient, JsonStringHandler> reqs = new Dictionary<TFWebClient, JsonStringHandler>();

	private bool unreachable;

	public CookieContainer Cookies
	{
		set
		{
			cookies = value;
		}
	}

	public bool Connected
	{
		get
		{
			return !unreachable;
		}
	}

	public TFServer()
	{
	}

	public TFServer(CookieContainer cookies)
	{
		this.cookies = cookies;
	}

	public static bool IsNetworkError(Dictionary<string, object> response)
	{
		return response.ContainsKey("error") && "Network error".Equals(response["error"]);
	}

	public void SetConnected(bool val)
	{
		loggingIn = val;
		unreachable = !val;
	}

	public void PostToJSON(string url, Dictionary<string, object> postDict, JsonResponseHandler callback, bool checkConnection = false, object userData = null)
	{
		if (checkConnection)
		{
			CheckConnectivity();
		}
		string s = encodePostData(postDict);
		using (TFWebClient tFWebClient = RegisterCallback(callback))
		{
			if (ShortCircuitRequest())
			{
				TFUtils.DebugLog("shortcircuiting a post to " + url);
				GetCallback(tFWebClient)(NETWORK_ERROR_JSON, userData);
			}
			else
			{
				TFUtils.DebugLog("Sending a post to " + url);
				tFWebClient.Post(new Uri(url), Encoding.UTF8.GetBytes(s), OnCallComplete, userData);
			}
		}
	}

	public void GetToJSON(string url, JsonResponseHandler callback, bool checkConnection = false, object userData = null)
	{
		if (checkConnection)
		{
			CheckConnectivity();
		}
		using (TFWebClient tFWebClient = RegisterCallback(callback))
		{
			if (ShortCircuitRequest())
			{
				TFUtils.DebugLog("Shortcircuiting a request to " + url);
				GetCallback(tFWebClient)(NETWORK_ERROR_JSON, userData);
			}
			else
			{
				TFUtils.DebugLog("Sending a request to " + url);
				tFWebClient.Get(new Uri(url), OnCallComplete, userData);
			}
		}
	}

	public Cookie GetCookie(Uri uri, string key)
	{
		return cookies.GetCookies(uri)[key];
	}

	private TFWebClient RegisterCallback(JsonStringHandler callback)
	{
		TFWebClient tFWebClient = new TFWebClient(cookies);
		WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
		TFUtils.SetDefaultHeaders(webHeaderCollection);
		tFWebClient.Headers = webHeaderCollection;
		reqs[tFWebClient] = callback;
		return tFWebClient;
	}

	private TFWebClient RegisterCallback(JsonResponseHandler callback)
	{
		TFWebClient tFWebClient = new TFWebClient(cookies);
		WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
		TFUtils.SetDefaultHeaders(webHeaderCollection);
		tFWebClient.Headers = webHeaderCollection;
		reqs[tFWebClient] = JsCallback(callback);
		return tFWebClient;
	}

	private string encodePostData(Dictionary<string, object> d)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, object> item in d)
		{
			string s = item.Value as string;
			list.Add(item.Key + "=" + WWW.EscapeURL(s));
		}
		return string.Join("&", list.ToArray());
	}

	private JsonStringHandler JsCallback(JsonResponseHandler cb)
	{
		return delegate(string jsonResponse, object userData)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(jsonResponse);
			if (dictionary == null)
			{
				TFUtils.ErrorLog("TFServer.JsCallback - Failed to parse jsonResponse: " + jsonResponse);
			}
			else if (!(bool)dictionary["success"])
			{
				cb(dictionary, userData);
			}
			else
			{
				Dictionary<string, object> dict = dictionary["data"] as Dictionary<string, object>;
				cb(dict, userData);
			}
		};
	}

	private void OnNetworkError(TFWebClient client, JsonStringHandler callback)
	{
		strikes++;
		if (strikes > 3)
		{
			deactivatedTime = TFUtils.EpochTime();
			activeConnection = false;
			strikes = 0;
		}
		WebException ex = client.Response.Error as WebException;
		if (ex != null && ex.Response != null)
		{
			LogResponse(ex.Response as HttpWebResponse);
		}
		if (callback != null)
		{
			callback(NETWORK_ERROR_JSON, client.UserData);
		}
	}

	private void OnCallComplete(TFWebClient client)
	{
		JsonStringHandler callback = GetCallback(client);
		if (callback != null)
		{
			if (client.Response.Error == null)
			{
				TFUtils.DebugLog("Got response data: " + client.Response.Data);
				callback(client.Response.Data, client.UserData);
			}
			else
			{
				TFUtils.DebugLog("Got response error: " + client.Response.Error);
				OnNetworkError(client, callback);
			}
		}
	}

	private JsonStringHandler GetCallback(TFWebClient sender)
	{
		if (reqs.ContainsKey(sender))
		{
			JsonStringHandler result = reqs[sender];
			reqs.Remove(sender);
			return result;
		}
		return null;
	}

	private void LogResponse(HttpWebResponse response)
	{
		Stream responseStream = response.GetResponseStream();
		using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
		{
			string text = streamReader.ReadToEnd();
			if (!string.IsNullOrEmpty(text))
			{
				TFUtils.DebugLog("Writing out error: " + text);
				File.WriteAllText(string.Format("{0}{1}.html", LOG_LOCATION, ++errorCount), text);
			}
		}
		responseStream.Dispose();
	}

	private void CheckConnectivity()
	{
		bool flag = true;
		if ((Application.internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork && Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork) || !loggingIn)
		{
			unreachable = true;
			activeConnection = false;
			return;
		}
		unreachable = false;
		if (TFUtils.EpochTime() > deactivatedTime + 60)
		{
			activeConnection = true;
		}
	}

	private bool ShortCircuitRequest()
	{
		return !activeConnection || unreachable;
	}
}
