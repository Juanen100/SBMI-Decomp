using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	public class DDNA : Singleton<DDNA>
	{
		internal const string PF_KEY_USER_ID = "DDSDK_USER_ID";

		internal const string PF_KEY_FIRST_SESSION = "DDSDK_FIRST_SESSION";

		internal const string PF_KEY_LAST_SESSION = "DDSDK_LAST_SESSION";

		internal const string PF_KEY_CROSS_GAME_USER_ID = "DDSDK_CROSS_GAME_USER_ID";

		internal const string PF_KEY_ADVERTISING_ID = "DDSDK_ADVERTISING_ID";

		internal const string PF_KEY_FORGET_ME = "DDSDK_FORGET_ME";

		internal const string PF_KEY_FORGOTTEN = "DDSK_FORGOTTEN";

		internal const string PF_KEY_ACTIONS_SALT = "DDSDK_ACTIONS_SALT";

		private static object _lock;

		private DDNABase delegated;

		private string collectURL;

		private string engageURL;

		public Settings Settings { get; set; }

		public AndroidNotifications AndroidNotifications { get; private set; }

		public IosNotifications IosNotifications { get; private set; }

		public EngageFactory EngageFactory
		{
			get
			{
				return null;
			}
		}

		public string EnvironmentKey { get; set; }

		public string CollectURL
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string EngageURL
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string SessionID { get; set; }

		public string UserID
		{
			get
			{
				return null;
			}
			private set
			{
			}
		}

		public bool HasStarted
		{
			get
			{
				return false;
			}
		}

		public bool IsUploading
		{
			get
			{
				return false;
			}
		}

		public string HashSecret { get; set; }

		public string ClientVersion { get; set; }

		public string Platform { get; set; }

		public string CrossGameUserID
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string AndroidRegistrationID
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string PushNotificationToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public event Action OnNewSession
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<bool> OnSessionConfigured
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnSessionConfigurationFailed
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnImageCachePopulated
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnImageCachingFailed
		{
			add
			{
			}
			remove
			{
			}
		}

		protected DDNA()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		internal void Awake()
		{
		}

		public void StartSDK()
		{
		}

		public void StartSDK(string userID)
		{
		}

		public void StartSDK(Configuration config)
		{
		}

		public void StartSDK(Configuration config, string userID)
		{
		}

		[Obsolete]
		public void StartSDK(string envKey, string collectURL, string engageURL)
		{
		}

		[Obsolete]
		public void StartSDK(string envKey, string collectURL, string engageURL, string userID)
		{
		}

		public void NewSession()
		{
		}

		public void StopSDK()
		{
		}

		public EventAction RecordEvent<T>(T gameEvent) where T : GameEvent<T>
		{
			return null;
		}

		public EventAction RecordEvent(string eventName)
		{
			return null;
		}

		public EventAction RecordEvent(string eventName, Dictionary<string, object> eventParams)
		{
			return null;
		}

		public void RequestEngagement(Engagement engagement, Action<Dictionary<string, object>> callback)
		{
		}

		public void RequestEngagement(Engagement engagement, Action<Engagement> onCompleted, Action<Exception> onError)
		{
		}

		public void RecordPushNotification(Dictionary<string, object> payload)
		{
		}

		public void RequestSessionConfiguration()
		{
		}

		public void Upload()
		{
		}

		public void DownloadImageAssets()
		{
		}

		public void ClearPersistentData()
		{
		}

		public void ForgetMe()
		{
		}

		public void UseCollectTimestamp(bool useCollect)
		{
		}

		public void SetTimestampFunc(Func<DateTime?> TimestampFunc)
		{
		}

		public void SetLoggingLevel(Logger.Level level)
		{
		}

		public override void OnDestroy()
		{
		}

		private void OnApplicationPause(bool pauseStatus)
		{
		}

		internal virtual ImageMessageStore GetImageMessageStore()
		{
			return null;
		}

		internal string ResolveEngageURL(string httpBody)
		{
			return null;
		}

		internal void NotifyOnSessionConfigured(bool cached)
		{
		}

		internal void NotifyOnSessionConfigurationFailed()
		{
		}

		internal void NotifyOnImageCachePopulated()
		{
		}

		internal void NotifyOnImageCachingFailed(string cause)
		{
		}

		private string GenerateSessionID()
		{
			return null;
		}

		private string GenerateUserID()
		{
			return null;
		}

		internal static string GenerateHash(string data, string secret)
		{
			return null;
		}

		internal static string FormatURI(string uriPattern, string apiHost, string envKey, string hash)
		{
			return null;
		}
	}
}
