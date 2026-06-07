using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helpshift
{
	public class HelpshiftCampaignsAndroid : IWorkerMethodDispatcher, IDexLoaderListener
	{
		private AndroidJavaObject hsCampaignsClass;

		private AndroidJavaObject currentActivity;

		private AndroidJavaObject application;

		private AndroidJavaObject convertToJavaHashMap(Dictionary<string, object> configD)
		{
			return null;
		}

		private void addHSApiCallToQueue(string methodIdentifier, string api, object[] args)
		{
		}

		private void synchronousWaitForHSApiCallQueue()
		{
		}

		public void onDexLoaded()
		{
		}

		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		private void hsCampaignsApiCall(string api, params object[] args)
		{
		}

		private void hsCampaignsApiCall(string api)
		{
		}

		private bool hsCampaignsApiCallAndReturnBool(string api, params object[] args)
		{
			return false;
		}

		private int hsCampaignsApiCallAndReturnInt(string api, params object[] args)
		{
			return 0;
		}

		private AndroidJavaObject hsCampaignsApiCallAndReturnObject(string api, params object[] args)
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

		public int GetCountOfUnreadMessages()
		{
			return 0;
		}

		public void RequestUnreadMessagesCount()
		{
		}

		public void ShowMessage(string messageIdentifier, Dictionary<string, object> configMap)
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
