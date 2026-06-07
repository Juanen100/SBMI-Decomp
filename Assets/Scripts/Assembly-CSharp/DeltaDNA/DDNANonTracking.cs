using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeltaDNA
{
	internal class DDNANonTracking : DDNABase
	{
		private bool started;

		private bool uploading;

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

		internal override string CrossGameUserID { get; set; }

		internal override string PushNotificationToken { get; set; }

		internal override string AndroidRegistrationID { get; set; }

		internal DDNANonTracking(DDNA ddna)
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
		private IEnumerator Send(HttpRequest request, Action onSuccess)
		{
			return null;
		}
	}
}
