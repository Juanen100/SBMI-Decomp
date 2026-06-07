using System;
using System.Collections.Generic;

namespace Helpshift
{
	public class HelpshiftSdk
	{
		public const string HS_RATE_ALERT_CLOSE = "HS_RATE_ALERT_CLOSE";

		public const string HS_RATE_ALERT_FEEDBACK = "HS_RATE_ALERT_FEEDBACK";

		public const string HS_RATE_ALERT_SUCCESS = "HS_RATE_ALERT_SUCCESS";

		public const string HS_RATE_ALERT_FAIL = "HS_RATE_ALERT_FAIL";

		public const string HSTAGSKEY = "hs-tags";

		public const string HSCUSTOMMETADATAKEY = "hs-custom-metadata";

		public const string HSCUSTOMISSUEFIELDKEY = "hs-custom-issue-field";

		public const string HSTAGSMATCHINGKEY = "withTagsMatching";

		public const string CONTACT_US_ALWAYS = "always";

		public const string CONTACT_US_NEVER = "never";

		public const string CONTACT_US_AFTER_VIEWING_FAQS = "after_viewing_faqs";

		public const string CONTACT_US_AFTER_MARKING_ANSWER_UNHELPFUL = "after_marking_answer_unhelpful";

		public const string HSUserAcceptedTheSolution = "User accepted the solution";

		public const string HSUserRejectedTheSolution = "User rejected the solution";

		public const string HSUserSentScreenShot = "User sent a screenshot";

		public const string HSUserReviewedTheApp = "User reviewed the app";

		public const string HsFlowTypeDefault = "defaultFlow";

		public const string HsFlowTypeConversation = "conversationFlow";

		public const string HsFlowTypeFaqs = "faqsFlow";

		public const string HsFlowTypeFaqSection = "faqSectionFlow";

		public const string HsFlowTypeSingleFaq = "singleFaqFlow";

		public const string HsFlowTypeNested = "dynamicFormFlow";

		public const string HsCustomContactUsFlows = "customContactUsFlows";

		public const string HsFlowType = "type";

		public const string HsFlowConfig = "config";

		public const string HsFlowData = "data";

		public const string HsFlowTitle = "title";

		private static HelpshiftSdk instance;

		private static HelpshiftAndroid nativeSdk;

		private HelpshiftSdk()
		{
		}

		public static HelpshiftSdk getInstance()
		{
			return null;
		}

		public void install(string apiKey, string domainName, string appId, Dictionary<string, object> config)
		{
		}

		public void install(string apiKey, string domainName, string appId)
		{
		}

		public void install()
		{
		}

		[Obsolete]
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

		[Obsolete]
		public void login(string identifier, string name, string email)
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

		public void registerDeviceToken(string deviceToken)
		{
		}

		public void leaveBreadCrumb(string breadCrumb)
		{
		}

		public void clearBreadCrumbs()
		{
		}

		public void showConversation(Dictionary<string, object> configMap)
		{
		}

		public void showConversation()
		{
		}

		public void showConversationWithMeta(Dictionary<string, object> configMap)
		{
		}

		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showFAQSection(string sectionPublishId)
		{
		}

		public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showSingleFAQ(string questionPublishId)
		{
		}

		public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
		{
		}

		public void showFAQs(Dictionary<string, object> configMap)
		{
		}

		public void showFAQs()
		{
		}

		public void showFAQsWithMeta(Dictionary<string, object> configMap)
		{
		}

		public void updateMetaData(Dictionary<string, object> metaData)
		{
		}

		[Obsolete]
		public void handlePushNotification(string issueId)
		{
		}

		public void handlePushNotification(Dictionary<string, object> pushNotificationData)
		{
		}

		public void showAlertToRateAppWithURL(string url)
		{
		}

		public void setSDKLanguage(string locale)
		{
		}

		public void registerDelegates()
		{
		}

		[Obsolete]
		public void registerForPush(string gcmId)
		{
		}

		public void showDynamicForm(string title, Dictionary<string, object>[] flows)
		{
		}

		public void onApplicationQuit()
		{
		}

		[Obsolete]
		public bool isConversationActive()
		{
			return false;
		}

		public void checkIfConversationActive()
		{
		}
	}
}
