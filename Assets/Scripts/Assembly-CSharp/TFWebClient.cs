using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

public class TFWebClient : IDisposable
{
	public delegate void OnNetworkError(TFWebClient client, WebException e);

	public delegate void GetCallbackHandler(TFWebClient client);

	public delegate void PutCallbackHandler(TFWebClient client);

	private class CallbackInfo
	{
		public GetCallbackHandler Callback { get; set; }

		public object UserData { get; set; }

		public Uri URI { get; set; }

		public string Method { get; set; }

		public byte[] RequestData { get; set; }

		public TFWebClient Client { get; set; }
	}

	private class TFCustomWebClient : WebClient
	{
		private CookieContainer cookies;

		public TFCustomWebClient(CookieContainer cookies)
		{
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			return null;
		}
	}

	private CookieContainer cookies;

	private WebClient client;

	private TFWebResponse response;

	private int retryCount;

	public Uri URI;

	public object UserData;

	private const int TIMEOUT = 10000;

	private const string USER_AGENT = "Innertube Explorer v0.1";

	public TFWebResponse Response
	{
		get
		{
			return null;
		}
	}

	public WebHeaderCollection Headers
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public WebHeaderCollection ResponseHeaders
	{
		get
		{
			return null;
		}
	}

	public event OnNetworkError NetworkError
	{
		add
		{
		}
		remove
		{
		}
	}

	public TFWebClient(CookieContainer cookieContainer)
	{
	}

	public void Get(Uri address, GetCallbackHandler response, object userData = null)
	{
	}

	public void Put(Uri address, byte[] saveData, GetCallbackHandler response, object userData = null)
	{
	}

	public void Post(Uri address, byte[] saveData, GetCallbackHandler response, object userData = null)
	{
	}

	public void Upload(string method, Uri address, byte[] saveData, GetCallbackHandler response, object userData = null)
	{
	}

	public void Put(Uri address, string saveData, GetCallbackHandler response, object userData = null)
	{
	}

	public void UploadLogDump(Uri address, Dictionary<string, object> postParams, GetCallbackHandler response, object userData = null)
	{
	}

	private void setURI(Uri address)
	{
	}

	private void retryRequest(CallbackInfo info)
	{
	}

	private bool IsExpectedStatus(Exception ex)
	{
		return false;
	}

	protected void OnCallComplete(object sender, AsyncCompletedEventArgs e)
	{
	}

	private void PopulateResponse(TFWebResponse response, HttpWebResponse httpRes)
	{
	}

	public void Dispose()
	{
	}
}
