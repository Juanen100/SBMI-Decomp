using UnityEngine;

namespace Microsoft.AppCenter.Unity.Push.Internal
{
	internal class PushDelegate : AndroidJavaProxy
	{
		public PushDelegate()
			: base((string)null)
		{
		}

		private void onPushNotificationReceived(AndroidJavaObject activity, AndroidJavaObject pushNotification)
		{
		}
	}
}
