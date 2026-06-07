using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroid : IWorkerMethodDispatcher, IDexLoaderListener
	{
		private AndroidJavaClass jc;

		private AndroidJavaObject currentActivity;

		private AndroidJavaObject application;

		private AndroidJavaObject hsHelpshiftClass;

		private AndroidJavaObject hsSupportClass;

		private AndroidJavaClass hsUnityAPIDelegate;

		private HelpshiftInternalLogger hsInternalLogger;

		private void unityHSApiCall(string api, params object[] args)
		{
		}

		private void hsApiCall(string api, params object[] args)
		{
		}

		private void hsApiCall(string api)
		{
		}

		private void hsSupportApiCall(string api, params object[] args)
		{
		}

		private void hsSupportApiCall(string api)
		{
		}

		private void addHSApiCallToQueue(string methodIdentifier, string api, object[] args)
		{
		}

		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		public void onDexLoaded()
		{
		}

		public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
		{
		}

		public void install()
		{
		}

		public int getNotificationCount(bool isAsync)
		{
			return 0;
		}

		public void requestUnreadMessagesCount(bool isAsync)
		{
		}

		[Obsolete]
		public void setNameAndEmail(string userName, string email)
		{
		}

		[Obsolete]
		public void setUserIdentifier(string identifier)
		{
		}

		public void registerDeviceToken(string deviceToken)
		{
		}

		public void leaveBreadCrumb(string breadCrumb)
		{
		}

		public void clearBreadCrumbs()
		{
		}

		[Obsolete]
		public void login(string identifier, string userName, string email)
		{
		}

		public void login(HelpshiftUser helpshiftUser)
		{
		}

		public void clearAnonymousUser()
		{
		}

		public void logout()
		{
		}

		public void showConversation(Dictionary<string, object> configMap)
		{
		}

		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showFAQs(Dictionary<string, object> configMap)
		{
		}

		public void showConversation()
		{
		}

		public void showFAQSection(string sectionPublishId)
		{
		}

		public void showSingleFAQ(string questionPublishId)
		{
		}

		public void showFAQs()
		{
		}

		public void showConversationWithMeta(Dictionary<string, object> configMap)
		{
		}

		public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showFAQsWithMeta(Dictionary<string, object> configMap)
		{
		}

		public void updateMetaData(Dictionary<string, object> metaData)
		{
		}

		private Dictionary<string, object> cleanConfig(Dictionary<string, object> configMap)
		{
			return null;
		}

		public void handlePushNotification(string issueId)
		{
		}

		public void handlePushNotification(Dictionary<string, object> pushNotificationData)
		{
		}

		public void showAlertToRateAppWithURL(string url)
		{
		}

		public void registerDelegates()
		{
		}

		[Obsolete]
		public void registerForPushWithGcmId(string gcmId)
		{
		}

		public void setSDKLanguage(string locale)
		{
		}

		public void showDynamicForm(string title, Dictionary<string, object>[] flows)
		{
		}

		public bool isConversationActive()
		{
			return false;
		}

		public void checkIfConversationActive()
		{
		}

		public void onApplicationQuit()
		{
		}

		private string jsonifyHelpshiftUser(HelpshiftUser helpshiftUser)
		{
			return null;
		}
	}
}
