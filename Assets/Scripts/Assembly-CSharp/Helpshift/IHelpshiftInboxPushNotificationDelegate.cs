namespace Helpshift
{
	public interface IHelpshiftInboxPushNotificationDelegate
	{
		void OnInboxMessagePushNotificationClicked(string messageIdentifier);
	}
}
