using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeltaDNA
{
	internal abstract class DDNABase
	{
		protected static Func<DateTime?> TimestampFunc;

		protected readonly DDNA ddna;

		protected readonly GameObject gameObject;

		internal ImageMessageStore ImageMessageStore { get; set; }

		internal EngageFactory EngageFactory { get; set; }

		protected string EnvironmentKey
		{
			get
			{
				return null;
			}
		}

		protected string CollectURL
		{
			get
			{
				return null;
			}
		}

		protected string EngageURL
		{
			get
			{
				return null;
			}
		}

		protected string Platform
		{
			get
			{
				return null;
			}
		}

		protected string HashSecret
		{
			get
			{
				return null;
			}
		}

		protected string ClientVersion
		{
			get
			{
				return null;
			}
		}

		protected Settings Settings
		{
			get
			{
				return null;
			}
		}

		protected string UserID
		{
			get
			{
				return null;
			}
		}

		protected string SessionID
		{
			get
			{
				return null;
			}
		}

		internal abstract bool HasStarted { get; }

		internal abstract bool IsUploading { get; }

		internal abstract string CrossGameUserID { get; set; }

		internal abstract string AndroidRegistrationID { get; set; }

		internal abstract string PushNotificationToken { get; set; }

		internal DDNABase(DDNA ddna)
		{
		}

		internal abstract void OnApplicationPause(bool pauseStatus);

		internal abstract void OnDestroy();

		internal abstract void StartSDK(bool newPlayer);

		internal abstract void StopSDK();

		internal abstract EventAction RecordEvent<T>(T gameEvent) where T : GameEvent<T>;

		internal abstract EventAction RecordEvent(string eventName);

		internal abstract EventAction RecordEvent(string eventName, Dictionary<string, object> eventParams);

		internal abstract void RequestEngagement(Engagement engagement, Action<Dictionary<string, object>> callback);

		internal abstract void RequestEngagement(Engagement engagement, Action<Engagement> onCompleted, Action<Exception> onError);

		internal abstract void RecordPushNotification(Dictionary<string, object> payload);

		internal abstract void RequestSessionConfiguration();

		internal abstract void Upload();

		internal abstract void DownloadImageAssets();

		internal abstract void ClearPersistentData();

		internal abstract void ForgetMe();

		protected Coroutine StartCoroutine(IEnumerator routine)
		{
			return null;
		}

		protected void InvokeRepeating(string methodName, float time, float repeatRate)
		{
		}

		protected bool IsInvoking(string methodName)
		{
			return false;
		}

		protected void CancelInvoke()
		{
		}

		protected void NewSession()
		{
		}

		internal void UseCollectTimestamp(bool useCollect)
		{
		}

		internal void SetTimestampFunc(Func<DateTime?> TimestampFunc)
		{
		}

		protected static string GetCurrentTimestamp()
		{
			return null;
		}

		private static DateTime? DefaultTimestampFunc()
		{
			return null;
		}
	}
}
