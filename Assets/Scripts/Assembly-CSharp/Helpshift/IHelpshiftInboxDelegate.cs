namespace Helpshift
{
	public interface IHelpshiftInboxDelegate
	{
		void InboxMessageAdded(HelpshiftInboxMessage message);

		void IconImageDownloaded(string messageIdentifier);

		void CoverImageDownloaded(string messageIdentifier);

		void InboxMessageDeleted(string messageIdentifier);

		void InboxMessageMarkedAsSeen(string messageIdentifier);

		void InboxMessageMarkedAsRead(string messageIdentifier);
	}
}
