using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using Ionic.Zlib;

public class TFWebClient : IDisposable
{
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
			this.cookies = cookies;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			HttpWebRequest httpWebRequest = base.GetWebRequest(address) as HttpWebRequest;
			httpWebRequest.CookieContainer = cookies;
			httpWebRequest.Timeout = 10000;
			httpWebRequest.UserAgent = "Innertube Explorer v0.1";
			return httpWebRequest;
		}
	}

	public delegate void OnNetworkError(TFWebClient client, WebException e);

	public delegate void GetCallbackHandler(TFWebClient client);

	public delegate void PutCallbackHandler(TFWebClient client);

	private const int TIMEOUT = 10000;

	private const string USER_AGENT = "Innertube Explorer v0.1";

	private CookieContainer cookies;

	private WebClient client;

	private TFWebResponse response;

	private int retryCount;

	public Uri URI;

	public object UserData;

	public TFWebResponse Response
	{
		get
		{
			return response;
		}
	}

	public WebHeaderCollection Headers
	{
		get
		{
			return client.Headers;
		}
		set
		{
			client.Headers = value;
		}
	}

	public WebHeaderCollection ResponseHeaders
	{
		get
		{
			return client.ResponseHeaders;
		}
	}

	public event OnNetworkError NetworkError;

	public TFWebClient(CookieContainer cookieContainer)
	{
		cookies = cookieContainer;
		client = new TFCustomWebClient(cookies);
		client.UploadDataCompleted += OnCallComplete;
		client.DownloadDataCompleted += OnCallComplete;
		TFUtils.SetDefaultHeaders(Headers);
	}

	public void Get(Uri address, GetCallbackHandler response, object userData = null)
	{
		if (address == null)
		{
			TFUtils.ErrorLog("TFWebClient.Get - null address");
			return;
		}
		if (response == null)
		{
			TFUtils.ErrorLog("TFWebClient.Get - null response");
			return;
		}
		setURI(address);
		UserData = userData;
		CallbackInfo callbackInfo = new CallbackInfo();
		callbackInfo.Client = this;
		callbackInfo.Callback = response;
		callbackInfo.URI = address;
		callbackInfo.UserData = userData;
		callbackInfo.Method = "GET";
		client.DownloadDataAsync(address, callbackInfo);
	}

	public void Put(Uri address, byte[] saveData, GetCallbackHandler response, object userData = null)
	{
		Upload("PUT", address, saveData, response, userData);
	}

	public void Post(Uri address, byte[] saveData, GetCallbackHandler response, object userData = null)
	{
		Upload("POST", address, saveData, response, userData);
	}

	public void Upload(string method, Uri address, byte[] saveData, GetCallbackHandler response, object userData = null)
	{
		setURI(address);
		UserData = userData;
		CallbackInfo callbackInfo = new CallbackInfo();
		callbackInfo.Callback = response;
		callbackInfo.URI = address;
		callbackInfo.UserData = userData;
		callbackInfo.Method = method;
		callbackInfo.RequestData = saveData;
		client.UploadDataAsync(address, method, saveData, callbackInfo);
	}

	public void Put(Uri address, string saveData, GetCallbackHandler response, object userData = null)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(saveData);
		Put(address, bytes, response, userData);
	}

	public void UploadLogDump(Uri address, Dictionary<string, object> postParams, GetCallbackHandler response, object userData = null)
	{
		setURI(address);
		UserData = postParams;
		CallbackInfo callbackInfo = new CallbackInfo();
		callbackInfo.Callback = response;
		callbackInfo.URI = address;
		callbackInfo.UserData = postParams;
		callbackInfo.Method = "POST";
		callbackInfo.RequestData = null;
		HttpWebResponse httpWebResponse = TFFormPost.PostForm(address, "Innertube Explorer v0.1", postParams, cookies);
		Stream responseStream = httpWebResponse.GetResponseStream();
		StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
		TFUtils.DebugLog("Log Dump Complete: " + streamReader.ReadToEnd());
		httpWebResponse.Close();
		streamReader.Close();
	}

	private void setURI(Uri address)
	{
		if (client.BaseAddress == null || client.BaseAddress.Length == 0)
		{
			URI = address;
		}
		else
		{
			URI = new Uri(new Uri(client.BaseAddress), address);
		}
	}

	private void retryRequest(CallbackInfo info)
	{
		retryCount++;
		client.CancelAsync();
		TFUtils.DebugLog(string.Format("Retrying {0} for the {1} attempt", info.URI, retryCount));
		if (info.Method == "GET")
		{
			Get(info.URI, info.Callback, info.UserData);
		}
		else
		{
			Upload(info.Method, info.URI, info.RequestData, info.Callback, info.UserData);
		}
	}

	private bool IsExpectedStatus(Exception ex)
	{
		if (ex.GetType().IsAssignableFrom(typeof(WebException)))
		{
			WebException ex2 = (WebException)ex;
			if (ex2 != null)
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)ex2.Response;
				TFUtils.DebugLog(ex);
				if (httpWebResponse != null)
				{
					if (httpWebResponse.StatusCode >= HttpStatusCode.InternalServerError)
					{
						return false;
					}
				}
				else if (ex2.Response == null)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	protected void OnCallComplete(object sender, AsyncCompletedEventArgs e)
	{
		try
		{
			WebClient webClient = (WebClient)sender;
			response = new TFWebResponse();
			CallbackInfo callbackInfo = (CallbackInfo)e.UserState;
			response.Error = e.Error;
			if (e.Error == null && !e.Cancelled)
			{
				response.StatusCode = HttpStatusCode.OK;
				if (e is DownloadDataCompletedEventArgs)
				{
					DownloadDataCompletedEventArgs e2 = (DownloadDataCompletedEventArgs)e;
					try
					{
						response.Data = TFUtils.Unzip(e2.Result);
					}
					catch (ZlibException)
					{
						response.Data = Encoding.UTF8.GetString(e2.Result);
					}
				}
				else if (e is UploadDataCompletedEventArgs)
				{
					UploadDataCompletedEventArgs e3 = (UploadDataCompletedEventArgs)e;
					response.Data = Encoding.UTF8.GetString(e3.Result);
				}
				response.Headers = webClient.ResponseHeaders;
				callbackInfo.Callback(this);
			}
			else
			{
				if (e.Error == null)
				{
					return;
				}
				if (retryCount < SBSettings.NETWORK_RETRY_COUNT && !IsExpectedStatus(e.Error))
				{
					TFUtils.DebugLog("Going to retry ");
					TFUtils.DebugLog(e);
					retryRequest(callbackInfo);
					return;
				}
				TFUtils.DebugLog("Going to call network error" + URI);
				if (e.Error.GetType().Name == "WebException")
				{
					WebException ex2 = (WebException)e.Error;
					TFUtils.DebugLog(ex2);
					HttpWebResponse httpWebResponse = (HttpWebResponse)ex2.Response;
					if (httpWebResponse != null)
					{
						PopulateResponse(response, httpWebResponse);
					}
					else
					{
						response.StatusCode = HttpStatusCode.ServiceUnavailable;
						response.NetworkDown = true;
					}
					if (this.NetworkError != null)
					{
						TFUtils.DebugLog("Calling network error");
						this.NetworkError(this, ex2);
					}
				}
				else
				{
					TFUtils.DebugLog("Server returned error");
					TFUtils.DebugLog(e.Error);
					response.NetworkDown = true;
					response.StatusCode = HttpStatusCode.ServiceUnavailable;
				}
				callbackInfo.Callback(this);
			}
		}
		catch (Exception message)
		{
			TFUtils.DebugLog(message);
		}
	}

	private void PopulateResponse(TFWebResponse response, HttpWebResponse httpRes)
	{
		response.StatusCode = httpRes.StatusCode;
		response.Headers = httpRes.Headers;
		try
		{
			byte[] array = new byte[4096];
			Stream responseStream = httpRes.GetResponseStream();
			int num = 0;
			MemoryStream memoryStream = new MemoryStream();
			while ((num = responseStream.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, num);
			}
			Encoding uTF = Encoding.UTF8;
			response.Data = uTF.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
		}
		catch (Exception message)
		{
			TFUtils.DebugLog(message);
		}
	}

	public void Dispose()
	{
		client.Dispose();
	}
}
