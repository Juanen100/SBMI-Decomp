using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DeltaDNA
{
	internal class DDNAImpl : DDNABase
	{
		private readonly EventStore eventStore;

		private readonly EngageCache engageCache;

		private readonly ActionStore actionStore;

		private bool started;

		private bool uploading;

		private DateTime lastActive;

		private GameEvent launchNotificationEvent;

		private string pushNotificationToken;

		private string androidRegistrationId;

		private ReadOnlyCollection<string> whitelistDps;

		private ReadOnlyCollection<string> whitelistEvents;

		private Dictionary<string, ReadOnlyCollection<EventTrigger>> eventTriggers;

		private ReadOnlyCollection<string> cacheImages;

		internal override bool HasStarted
		{
			get
			{
				return false;
			}
		}

		internal override bool IsUploading
		{
			get
			{
				return false;
			}
		}

		internal override string CrossGameUserID
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal override string AndroidRegistrationID
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal override string PushNotificationToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal DDNAImpl(DDNA ddna)
			: base(null)
		{
		}

		internal override void OnApplicationPause(bool pauseStatus)
		{
		}

		internal override void OnDestroy()
		{
		}

		internal override void StartSDK(bool newPlayer)
		{
		}

		internal override void StopSDK()
		{
		}

		internal override EventAction RecordEvent<T>(T gameEvent)
		{
			return null;
		}

		internal override EventAction RecordEvent(string eventName)
		{
			return null;
		}

		internal override EventAction RecordEvent(string eventName, Dictionary<string, object> eventParams)
		{
			return null;
		}

		internal override void RequestEngagement(Engagement engagement, Action<Dictionary<string, object>> callback)
		{
		}

		internal override void RequestEngagement(Engagement engagement, Action<Engagement> onCompleted, Action<Exception> onError)
		{
		}

		internal override void RecordPushNotification(Dictionary<string, object> payload)
		{
		}

		internal override void RequestSessionConfiguration()
		{
		}

		internal override void Upload()
		{
		}

		internal override void DownloadImageAssets()
		{
		}

		internal override void ClearPersistentData()
		{
		}

		internal override void ForgetMe()
		{
		}

		[DebuggerHidden]
		private IEnumerator UploadCoroutine()
		{
			return null;
		}

		[DebuggerHidden]
		private IEnumerator PostEvents(string[] events, Action<bool, int> resultCallback)
		{
			return null;
		}

		private void TriggerDefaultEvents(bool newPlayer)
		{
		}

		private void HandleSessionConfigurationCallback(Dictionary<string, object> response)
		{
		}
	}
}
