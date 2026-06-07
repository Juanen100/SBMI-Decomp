using System.Collections;
using System.Diagnostics;
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

	public delegate bool SCWebQueueCallback(SCWebQueueState state, SoaringError error, object userData, object call_data);

	public delegate void SCDownloadCallback(string id, bool success, string path);

	public class SCWebCallbackObject : SoaringObjectBase
	{
		public SCWebQueueCallback callback;

		public SCWebCallbackObject(SCWebQueueCallback cbk)
			: base(default(IsType))
		{
		}
	}

	public class SCDownloadCallbackObject : SoaringObjectBase
	{
		public SCDownloadCallback callback;

		public SCDownloadCallbackObject(SCDownloadCallback cbk)
			: base(default(IsType))
		{
		}
	}

	internal class SCWebChannel
	{
		public float LastProgress;

		public float LastProgressTimestamp;

		public float ConnectionStartTime;

		public int Retries;

		public const float Timeout = 30f;

		public const int MaxRetries = 3;

		private MList mConnectionsPending;

		private SCData mCurrentData;

		private SoaringConnection mConnection;

		private bool mShouldRetry;

		public SoaringConnection Connection
		{
			get
			{
				return null;
			}
		}

		public bool TestShouldRetry()
		{
			return false;
		}

		public void AddConnection(SCData data)
		{
		}

		public bool HasConnectionsPending()
		{
			return false;
		}

		public bool HasActiveConnection()
		{
			return false;
		}

		public bool PreformCallback(SCWebQueueState state, SoaringError error, object data, bool canRetry)
		{
			return false;
		}

		public bool SaveData()
		{
			return false;
		}

		public bool BuildConnection(bool canRetry)
		{
			return false;
		}

		public void FinalizeConnection(bool canRetry)
		{
		}

		public void Reset()
		{
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
				return null;
			}
		}

		public SoaringDictionary PostParams
		{
			get
			{
				return null;
			}
		}

		public string URL
		{
			get
			{
				return null;
			}
		}

		public string SaveLocation
		{
			get
			{
				return null;
			}
		}

		public SCData()
		{
		}

		public SCData(string url, SoaringDictionary post, SoaringDictionary gt, SCWebQueueCallback cbk, object userdata, SCWebQueueCallback v_cbk)
		{
		}

		public SCData(string url, SoaringDictionary post, SoaringDictionary gt, string save, SCWebQueueCallback cbk, object userdata, SCWebQueueCallback v_cbk)
		{
		}

		public bool PreformCallback(SCWebQueueState state, SoaringError error, object obj)
		{
			return false;
		}

		public bool PreformVerifyCallback(SCWebQueueState state, SoaringError error)
		{
			return false;
		}

		public void SetGetParams(SoaringDictionary p)
		{
		}

		public void SetPostParams(SoaringDictionary p)
		{
		}

		public void SetURL(string url)
		{
		}

		public void SetSaveLocation(string p)
		{
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
				return null;
			}
		}

		public int Channel
		{
			get
			{
				return 0;
			}
		}

		public SCPending()
		{
		}

		public SCPending(SCData connectionData, int channel)
		{
		}
	}

	public const int Channel_Core = 0;

	public const int Channel_User = 1;

	public const int Channel_Components = 2;

	public const int Channel_Analytics = 3;

	public const int Channel_Transport = 4;

	private static int Transport_Channels;

	private static int Channel_Total;

	private int mNextTransportChannel;

	public static string ReportedSDK;

	private bool IsActive;

	private float QueueUpdateTime;

	private SoaringDictionary mEventQueue;

	private MArray<SCWebChannel> mChannelList;

	private MArray<SCPending> mPendingNewConnections;

	private float mRealTimeSinceStartup;

	private int GetTransportChannel()
	{
		return 0;
	}

	public void Initialize(string sdk)
	{
	}

	public void ClearConnections()
	{
	}

	private void AddConnection(SCData data, int channel)
	{
	}

	private void Update()
	{
	}

	[DebuggerHidden]
	private IEnumerator Handle_Connections()
	{
		return null;
	}

	private bool HasActiveConnections()
	{
		return false;
	}

	public void OnApplicationPause(bool paused)
	{
	}

	public void OnApplicationQuit()
	{
	}

	public bool StartConnection(object userData, string url, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		return false;
	}

	public bool StartConnection(int channel, object userData, string url, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		return false;
	}

	public bool StartConnection(object userData, string url, string saveData, SoaringDictionary postData, SoaringDictionary urlData, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		return false;
	}

	public bool StartConnection(int channel, object userData, string url, string saveData, SoaringDictionary postData, SoaringDictionary urlData, SCWebQueueCallback callback, SCWebQueueCallback verifyCallback)
	{
		return false;
	}

	public void RegisterEventMessage(SoaringContext context)
	{
	}

	public void HandleEventMessage(string name)
	{
	}

	public void HandleEventMessage(SoaringContext context)
	{
	}

	public void ClearEventMessage(SoaringContext context)
	{
	}

	public void onExternalMessage(string message)
	{
	}

	public void onMemoryWarningMessage(string message)
	{
	}
}
