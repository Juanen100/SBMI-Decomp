using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroidInboxPushNotificationDelegate : AndroidJavaProxy
	{
		private IHelpshiftInboxPushNotificationDelegate externalDelegate;

		public HelpshiftAndroidInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
			: base((string)null)
		{
		}

		public void onInboxMessagePushNotificationClicked(string messageIdentifier)
		{
		}
	}
}
