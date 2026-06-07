using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroidInboxDelegate : AndroidJavaProxy
	{
		private IHelpshiftInboxDelegate externalDelegate;

		public HelpshiftAndroidInboxDelegate(IHelpshiftInboxDelegate externalDelegate)
			: base((string)null)
		{
		}

		public void inboxMessageAdded(AndroidJavaObject message)
		{
		}

		public void iconImageDownloaded(string messageIdentifier)
		{
		}

		public void coverImageDownloaded(string messageIdentifier)
		{
		}

		public void inboxMessageDeleted(string messageIdentifier)
		{
		}

		public void inboxMessageMarkedAsSeen(string messageIdentifier)
		{
		}

		public void inboxMessageMarkedAsRead(string messageIdentifier)
		{
		}
	}
}
