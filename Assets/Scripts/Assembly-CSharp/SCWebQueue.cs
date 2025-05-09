using System;
using System.Collections;
using MTools;
using UnityEngine;

public class SCWebQueue : MonoBehaviour
{
	public enum SCWebQueueState
	{
		Failed = 0,
		Finished = 1,
		Updated = 2
	}

	public class SCWebCallbackObject : SoaringObjectBase
	{
		public SCWebQueueCallback callback;

		public SCWebCallbackObject(SCWebQueueCallback cbk)
			: base(IsType.Object)
		{
			callback = cbk;
		}
	}

	public class SCDownloadCallbackObject : SoaringObjectBase
	{
		public SCDownloadCallback callback;

		public SCDownloadCallbackObject(SCDownloadCallback cbk)
			: base(IsType.Object)
		{
			callback = cbk;
		}
	}

	internal class SCWebChannel
	{
		public const float Timeout = 30f;

		public const int MaxRetries = 3;

		public float LastProgress;

		public float LastProgressTimestamp;

		public float ConnectionStartTime;

		public int Retries;

		private MList mConnectionsPending;

		private SCData mCurrentData;

		private SoaringConnection mConnection;

		private bool mShouldRetry;

		public SoaringConnection Connection
		{
			get
			{
				return mConnection;
			}
		}

		public SCWebChannel()
		{
			mConnectionsPending = new MList();
		}

		public bool TestShouldRetry()
		{
			return mShouldRetry;
		}

		public void AddConnection(SCData data)
		{
			if (data != null)
			{
				mConnectionsPending.PushBack(data);
			}
		}

		public bool HasConnectionsPending()
		{
			return mConnectionsPending.count() != 0;
		}

		public bool HasActiveConnection()
		{
			return mCurrentData != null;
		}

		public bool PreformCallback(SCWebQueueState state, SoaringError error, object data, bool canRetry)
		{
			bool result = false;
			if (state == SCWebQueueState.Failed)
			{
				if (data != null)
				{
					SoaringDebug.Log("SCWebQueue: Failed: " + data.ToString());
				}
				if (canRetry && Retries < 3)
				{
					return true;
				}
			}
			if (mCurrentData != null)
			{
				result = mCurrentData.PreformCallback(state, error, data);
			}
			return result;
		}

		public bool SaveData()
		{
			if (mCurrentData == null)
			{
				return false;
			}
			if (mConnection == null)
			{
				return false;
			}
			return mConnection.SaveData();
		}

		public bool BuildConnection(bool canRetry)
		{
			if (!mShouldRetry && !HasConnectionsPending())
			{
				SoaringDebug.Log("SCWebQueue: Failed to build: No Pending Connections");
				return false;
			}
			mShouldRetry = false;
			if (!canRetry || mCurrentData == null)
			{
				mCurrentData = (SCData)mConnectionsPending.PopFront();
				Retries = 0;
			}
			else
			{
				Retries++;
			}
			if (mCurrentData == null)
			{
				return false;
			}
			if (!mCurrentData.PreformVerifyCallback(SCWebQueueState.Updated, null))
			{
				SoaringError soaringError = new SoaringError("Failed Call Verification.", -10);
				PreformCallback(SCWebQueueState.Failed, soaringError, soaringError.Error, false);
				FinalizeConnection(false);
				return false;
			}
			mConnection = new SoaringUnityConnection();
			if (!mConnection.Create(mCurrentData))
			{
				SoaringError soaringError2 = new SoaringError("Invalid Connection Data.", -9);
				PreformCallback(SCWebQueueState.Failed, soaringError2, soaringError2.Error, false);
				FinalizeConnection(false);
				return false;
			}
			LastProgress = 0f;
			LastProgressTimestamp = Time.realtimeSinceStartup;
			ConnectionStartTime = LastProgressTimestamp;
			return true;
		}

		public void FinalizeConnection(bool canRetry)
		{
			if (!canRetry)
			{
				if (mConnection != null && mCurrentData != null)
				{
					SoaringDebug.Log("Finalize: Retries: " + Retries + " Time: " + (Time.realtimeSinceStartup - ConnectionStartTime) + "\nUrl: " + mCurrentData.URL);
				}
				else
				{
					SoaringDebug.Log("Finalize: Retries: " + Retries + " Time: " + (Time.realtimeSinceStartup - ConnectionStartTime));
				}
				mCurrentData = null;
				Retries = 0;
			}
			LastProgress = -1f;
			mConnection = null;
			mConnection = null;
			mShouldRetry = canRetry;
		}

		public void Reset()
		{
			FinalizeConnection(false);
			if (mConnectionsPending != null)
			{
				mConnectionsPending = new MList();
			}
		}
	}

	public class SCData
	{
		private object mUserData;

		private SoaringDictionary mGetParams;

		private SoaringDictionary mPostParams;

		private string mURL;

		private string mSaveLocation;

		private SCWebQueueCallback mCallback;

		private SCWebQueueCallback mVerifyCallback;

		public SoaringDictionary GetParams
		{
			get
			{
				return mGetParams;
			}
		}

		public SoaringDictionary PostParams
		{
			get
			{
				return mPostParams;
			}
		}

		public string URL
		{
			get
			{
				return mURL;
			}
		}

		public string SaveLocation
		{
			get
			{
				return mSaveLocation;
			}
		}

		public SCData()
		{
		}

		public SCData(string url, SoaringDictionary post, SoaringDictionary gt, SCWebQueueCallback cbk, object userdata, SCWebQueueCallback v_cbk)
		{
			mUserData = userdata;
			mCallback = cbk;
			mVerifyCallback = v_cbk;
			SetGetParams(gt);
			SetPostParams(post);
			SetURL(url);
		}

		public SCData(string url, SoaringDictionary post, SoaringDictionary gt, string save, SCWebQueueCallback cbk, object userdata, SCWebQueueCallback v_cbk)
		{
			mUserData = userdata;
			mCallback = cbk;
			mVerifyCallback = v_cbk;
			SetGetParams(gt);
			SetPostParams(post);
			SetSaveLocation(save);
			SetURL(url);
		}

		public bool PreformCallback(SCWebQueueState state, SoaringError error, object obj)
		{
			bool result = false;
			if (mCallback != null)
			{
				result = mCallback(state, error, mUserData, obj);
			}
			return result;
		}

		public bool PreformVerifyCallback(SCWebQueueState state, SoaringError error)
		{
			bool result = true;
			if (mVerifyCallback != null)
			{
				result = mVerifyCallback(state, error, mUserData, null);
			}
			return result;
		}

		public void SetGetParams(SoaringDictionary p)
		{
			mGetParams = p;
		}

		public void SetPostParams(SoaringDictionary p)
		{
			mPostParams = p;
		}

		public void SetURL(string url)
		{
			mURL = url;
		}

		public void SetSaveLocation(string p)
		{
			mSaveLocation = p;
		}
	}

	internal class SCPending
	{
		private SCData mConnectionData;

		private int mChannel;

		public SCData Data
		{
			get
			{
				return mConnectionData;
			}
		}

		public int Channel
		{
			get
			{
				return mChannel;
			}
		}

		public SCPending()
		{
		}

		public SCPending(SCData connectionData, int channel)
		{
			mConnectionData = connectionData;
			mChannel = channel;
		}
	}

	public delegate bool SCWebQueueCallback(SCWebQueueState state, SoaringError error, object userData, object call_data);

	public delegate void SCDownloadCallback(string id, bool success, string path);

	public const int Channel_Core = 0;

	public const int Channel_User = 1;

	public const int Channel_Components = 2;

	public const int Channel_Analytics = 3;

	public const int Channel_Transport = 4;

	private static int Transport_Channels = 10;

	private static int Channel_Total = Transport_Channels + 4;

	private int mNextTransportChannel = 4;

	public static string ReportedSDK = "0";

	private bool IsActive;

	private float QueueUpdateTime = 0.1f;

	private SoaringDictionary mEventQueue = new SoaringDictionary();

	private MArray<SCWebChannel> mChannelList;

	private MArray<SCPending> mPendingNewConnections;

	private float mRealTimeSinceStartup;

	private int GetTransportChannel()
	{
		int result = mNextTransportChannel;
		mNextTransportChannel++;
		if (mNextTransportChannel >= 4 + Transport_Channels)
		{
			mNextTransportChannel = 4;
		}
		return result;
	}

	public void Initialize(string sdk)
	{
		mRealTimeSinceStartup = Time.realtimeSinceStartup;
		if (mChannelList == null)
		{
			ReportedSDK = sdk;
			mChannelList = new MArray<SCWebChannel>(Channel_Total);
			mPendingNewConnections = new MArray<SCPending>(Channel_Total);
			for (int i = 0; i < Channel_Total; i++)
			{
				mChannelList.addObject(new SCWebChannel());
			}
		}
	}

	public void ClearConnections()
	{
		if (mPendingNewConnections != null)
		{
			mPendingNewConnections.clear();
		}
		int num = mChannelList.count();
		for (int i = 0; i < num; i++)
		{
			SCWebChannel sCWebChannel = mChannelList[i];
			if (sCWebChannel != null)
			{
				sCWebChannel.Reset();
			}
		}
	}

	private void AddConnection(SCData data, int channel)
	{
		mPendingNewConnections.addObject(new SCPending(data, channel));
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		SoaringInternal.instance.Update(realtimeSinceStartup - mRealTimeSinceStartup);
		mRealTimeSinceStartup = realtimeSinceStartup;
		if (!IsActive && mChannelList != null && mPendingNewConnections != null && mChannelList.count() != 0 && mPendingNewConnections.count() != 0)
		{
			StartCoroutine(Handle_Connections());
		}
	}

	private IEnumerator Handle_Connections()
	{
		if (IsActive)
		{
			yield break;
		}
		IsActive = true;
		while (true)
		{
			int nCount = mPendingNewConnections.count();
			if (nCount != 0)
			{
				for (int nItter = 0; nItter < nCount; nItter++)
				{
					SCPending pending = mPendingNewConnections.objectAtIndex(nItter);
					int nChannel = pending.Channel;
					if (nChannel == 4)
					{
						nChannel = GetTransportChannel();
					}
					mChannelList[nChannel].AddConnection(pending.Data);
				}
				mPendingNewConnections.clear();
			}
			if (!HasActiveConnections())
			{
				break;
			}
			nCount = mChannelList.count();
			for (int i = 0; i < nCount; i++)
			{
				SCWebChannel channel = mChannelList[i];
				if (!channel.HasActiveConnection() || channel.TestShouldRetry())
				{
					if (!channel.HasConnectionsPending() && !channel.TestShouldRetry())
					{
						continue;
					}
					bool didBuildConnection = false;
					try
					{
						didBuildConnection = channel.BuildConnection(channel.Retries < 3);
					}
					catch (Exception ex)
					{
						channel.FinalizeConnection(false);
						SoaringDebug.Log("SCWebQueue: " + ex.Message);
						didBuildConnection = false;
					}
					if (!didBuildConnection)
					{
						SoaringDebug.Log("SCWebQueue: Failed to build web connection: Retries: " + channel.Retries + " of " + 3);
						continue;
					}
				}
				if (channel.Connection == null)
				{
					channel.PreformCallback(SCWebQueueState.Updated, null, 1f, false);
					SoaringError error = new SoaringError("Connection Is Invalid or Null", -8);
					channel.PreformCallback(SCWebQueueState.Failed, error, error.Error, true);
					channel.FinalizeConnection(true);
				}
				else if (channel.Connection.HasError)
				{
					SoaringError error2 = new SoaringError("Could not connect to the server.", -7);
					SoaringDebug.Log("SCWebQueue: " + channel.Connection.Error, LogType.Warning);
					channel.PreformCallback(SCWebQueueState.Failed, error2, error2.Error, true);
					channel.FinalizeConnection(true);
				}
				else if (channel.Connection.IsDone())
				{
					channel.PreformCallback(SCWebQueueState.Updated, null, 1f, false);
					string connectionText = null;
					if (!channel.SaveData())
					{
						connectionText = channel.Connection.ContentAsText;
					}
					channel.PreformCallback(SCWebQueueState.Finished, null, connectionText, false);
					channel.FinalizeConnection(false);
				}
				else if (channel.LastProgress == channel.Connection.Progress)
				{
					float testValue = Time.realtimeSinceStartup + 0.01f - channel.LastProgressTimestamp;
					if (testValue > 30f)
					{
						SoaringError error3 = new SoaringError("Connection Timed Out", -6);
						channel.PreformCallback(SCWebQueueState.Failed, error3, error3.Error, true);
						channel.FinalizeConnection(true);
					}
				}
				else
				{
					channel.LastProgress = channel.Connection.Progress;
					channel.LastProgressTimestamp = Time.realtimeSinceStartup;
					channel.PreformCallback(SCWebQueueState.Updated, null, channel.LastProgress, false);
				}
			}
			yield return new WaitForSeconds(QueueUpdateTime);
		}
		IsActive = false;
		yield return null;
	}

	private bool HasActiveConnections()
	{
		bool result = false;
		for (int i = 0; i < mChannelList.count(); i++)
		{
			if (mChannelList[i].HasConnectionsPending() || mChannelList[i].HasActiveConnection())
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void OnApplicationPause(bool paused)
	{
		SoaringInternal.instance.HandleOnApplicationPaused(paused);
	}

	public void OnApplicationQuit()
	{
		SoaringInternal.instance.HandleOnApplicationQuit();
	}

	public bool StartConnection(object userData, string url, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		return StartConnection(0, userData, url, null, null, null, callback, verifyCallback);
	}

	public bool StartConnection(int channel, object userData, string url, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		return StartConnection(channel, userData, url, null, null, null, callback, verifyCallback);
	}

	public bool StartConnection(object userData, string url, string saveData, SoaringDictionary postData, SoaringDictionary urlData, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		return StartConnection(0, userData, url, saveData, postData, urlData, callback, verifyCallback);
	}

	public bool StartConnection(int channel, object userData, string url, string saveData, SoaringDictionary postData, SoaringDictionary urlData, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		if (string.IsNullOrEmpty(url) || callback == null)
		{
			return false;
		}
		SCData data = new SCData(url, postData, urlData, saveData, callback, userData, verifyCallback);
		AddConnection(data, channel);
		return true;
	}

	public void RegisterEventMessage(SoaringContext context)
	{
		if (context == null)
		{
			SoaringDebug.Log("RegisterEventMessage: No Context", LogType.Warning);
			return;
		}
		if (string.IsNullOrEmpty(context.Name))
		{
			SoaringDebug.Log("RegisterEventMessage: No User Name", LogType.Warning);
			return;
		}
		SoaringDebug.Log("RegisterEventMessage: " + context.Name);
		mEventQueue.addValue(context, context.Name);
	}

	public void HandleEventMessage(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			SoaringDebug.Log("SCWebQueue: No Event Name", LogType.Warning);
		}
		else
		{
			HandleEventMessage((SoaringContext)mEventQueue.objectWithKey(name));
		}
	}

	public void HandleEventMessage(SoaringContext context)
	{
		if (context == null)
		{
			SoaringDebug.Log("SCWebQueue: Invalid Message Name: " + base.name, LogType.Error);
			return;
		}
		if (context.ContextResponder != null)
		{
			context.ContextResponder(context);
		}
		else if (context.Responder != null)
		{
			context.Responder.OnComponentFinished(true, context.Name, null, null, context);
		}
		mEventQueue.removeObjectWithKey(context.Name);
	}

	public void ClearEventMessage(SoaringContext context)
	{
		if (context != null)
		{
			mEventQueue.removeObjectWithKey(context.Name);
		}
	}

	public void onExternalMessage(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			SoaringDebug.Log("onExternalMessage: Null Message", LogType.Warning);
			return;
		}
		SoaringDebug.Log("onExternalMessage: " + message);
		SoaringDictionary soaringDictionary = new SoaringDictionary(message);
		SoaringDebug.Log("onExternalMessage: Json: " + soaringDictionary.ToJsonString());
		string text = soaringDictionary.soaringValue("call");
		if (string.IsNullOrEmpty(text))
		{
			SoaringDebug.Log("SCWebQueue: No Call Name:\n" + soaringDictionary, LogType.Error);
			return;
		}
		SoaringContext soaringContext = (SoaringContext)mEventQueue.objectWithKey(text);
		if (soaringContext == null)
		{
			SoaringDebug.Log("SCWebQueue: Invalid Message Name: " + text, LogType.Error);
			return;
		}
		mEventQueue.removeObjectWithKey(soaringContext.Name);
		if (soaringContext.Responder != null)
		{
			soaringContext.Responder.OnComponentFinished(true, soaringContext.Name, null, soaringDictionary, soaringContext);
		}
		else if (soaringContext.ContextResponder != null)
		{
			soaringContext.addValue(soaringDictionary, "message");
			soaringContext.ContextResponder(soaringContext);
		}
		SoaringDebug.Log("onExternalMessage: " + message + " done");
	}

	public void onMemoryWarningMessage(string message)
	{
		SoaringDebug.Log("onMemoryWarningMessage: " + message, LogType.Warning);
	}
}
