using System.Collections.Generic;

namespace Helpshift
{
	public class HelpshiftInbox
	{
		private static HelpshiftInbox instance;

		private static HelpshiftInboxAndroid nativeSdk;

		private HelpshiftInbox()
		{
		}

		public static HelpshiftInbox getInstance()
		{
			return null;
		}

		public List<HelpshiftInboxMessage> GetAllInboxMessages()
		{
			return null;
		}

		public HelpshiftInboxMessage GetInboxMessage(string messageIdentifier)
		{
			return null;
		}

		public void MarkInboxMessageAsRead(string messageIdentifier)
		{
		}

		public void MarkInboxMessageAsSeen(string messageIdentifier)
		{
		}

		public void DeleteInboxMessage(string messageIdentifier)
		{
		}

		public void SetInboxMessageDelegate(IHelpshiftInboxDelegate externalDelegate)
		{
		}

		public void SetInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
		{
		}
	}
}
