namespace Helpshift
{
	public interface IHelpshiftCampaignsDelegate
	{
		void didReceiveUnreadMessagesCount(int count);

		void sessionBegan();

		void sessionEnded();
	}
}
