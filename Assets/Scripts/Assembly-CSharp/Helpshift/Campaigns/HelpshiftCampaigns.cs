using System;
using System.Collections.Generic;

namespace Helpshift.Campaigns
{
	public class HelpshiftCampaigns
	{
		private static HelpshiftCampaigns instance;

		private static HelpshiftCampaignsAndroid nativeSdk;

		private HelpshiftCampaigns()
		{
		}

		public static HelpshiftCampaigns getInstance()
		{
			return null;
		}

		public bool AddProperty(string key, int value)
		{
			return false;
		}

		public bool AddProperty(string key, long value)
		{
			return false;
		}

		public bool AddProperty(string key, string value)
		{
			return false;
		}

		public bool AddProperty(string key, bool value)
		{
			return false;
		}

		public bool AddProperty(string key, DateTime value)
		{
			return false;
		}

		public string[] AddProperties(Dictionary<string, object> value)
		{
			return null;
		}

		public void ShowInbox(Dictionary<string, object> configMap)
		{
		}

		public void ShowMessage(string messageIdentifier, Dictionary<string, object> configMap)
		{
		}

		[Obsolete]
		public int GetCountOfUnreadMessages()
		{
			return 0;
		}

		public void RequestUnreadMessagesCount()
		{
		}

		public void SetInboxMessagesDelegate(IHelpshiftInboxDelegate inboxDelegate)
		{
		}

		public void SetCampaignsDelegate(IHelpshiftCampaignsDelegate campaignsDelegate)
		{
		}
	}
}
