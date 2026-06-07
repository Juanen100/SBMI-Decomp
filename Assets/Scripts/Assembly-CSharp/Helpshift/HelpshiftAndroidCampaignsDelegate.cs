using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroidCampaignsDelegate : AndroidJavaProxy
	{
		private IHelpshiftCampaignsDelegate externalCampaignsDelegate;

		public HelpshiftAndroidCampaignsDelegate(IHelpshiftCampaignsDelegate externalDelegate)
			: base((string)null)
		{
		}

		public void didReceiveUnreadMessagesCount(int count)
		{
		}

		public void sessionBegan()
		{
		}

		public void sessionEnded()
		{
		}
	}
}
