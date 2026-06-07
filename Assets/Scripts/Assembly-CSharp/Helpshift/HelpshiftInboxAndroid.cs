using System.Collections.Generic;
using UnityEngine;

namespace Helpshift
{
	public class HelpshiftInboxAndroid : IWorkerMethodDispatcher, IDexLoaderListener
	{
		private AndroidJavaObject hsInboxJavaInstance;

		public void onDexLoaded()
		{
		}

		private void addHSApiCallToQueue(string methodIdentifier, string api, object[] args)
		{
		}

		private void hsInboxApiCall(string api, object[] args)
		{
		}

		public void resolveAndCallApi(string methodIdentifier, string apiName, object[] args)
		{
		}

		private void synchronousWaitForHSApiCallQueue()
		{
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
