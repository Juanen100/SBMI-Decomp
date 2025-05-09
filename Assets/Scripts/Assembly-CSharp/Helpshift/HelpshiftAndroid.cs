using System;
using System.Collections.Generic;
using System.Linq;
using HSMiniJSON;
using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroid
	{
		private AndroidJavaClass jc;

		private AndroidJavaObject currentActivity;

		private AndroidJavaObject application;

		private AndroidJavaObject hsPlugin;

		public HelpshiftAndroid()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
				application = currentActivity.Call<AndroidJavaObject>("getApplication", new object[0]);
				hsPlugin = new AndroidJavaClass("com.helpshift.Helpshift");
			}
		}

		private AndroidJavaObject convertToJavaHashMap(Dictionary<string, object> configD)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			if (configD != null)
			{
				Dictionary<string, object> dictionary = configD.Where((KeyValuePair<string, object> kv) => kv.Value != null).ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value);
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				array[0] = (array[1] = null);
				foreach (KeyValuePair<string, object> item in dictionary)
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", item.Key))
					{
						array[0] = androidJavaObject2;
						if ((item.Value != null && item.Value.Equals("yes")) || item.Value.Equals("no"))
						{
							string text = ((!item.Value.Equals("yes")) ? "false" : "true");
							array[1] = new AndroidJavaObject("java.lang.Boolean", text);
						}
						else if (item.Value != null)
						{
							if (item.Value.GetType().ToString() == "System.String")
							{
								array[1] = new AndroidJavaObject("java.lang.String", item.Value);
							}
							else if (item.Value.GetType().ToString() == "System.String[]")
							{
								string[] array2 = (string[])item.Value;
								AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.util.ArrayList");
								IntPtr methodID2 = AndroidJNIHelper.GetMethodID(androidJavaObject3.GetRawClass(), "add", "(Ljava/lang/String;)Z");
								object[] array3 = new object[1];
								string[] array4 = array2;
								foreach (string text2 in array4)
								{
									if (text2 != null)
									{
										array3[0] = new AndroidJavaObject("java.lang.String", text2);
										AndroidJNI.CallBooleanMethod(androidJavaObject3.GetRawObject(), methodID2, AndroidJNIHelper.CreateJNIArgArray(array3));
									}
								}
								array[1] = new AndroidJavaObject("java.util.ArrayList", androidJavaObject3);
							}
							else
							{
								if (item.Key == "hs-custom-metadata")
								{
									Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item.Value;
									AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("java.util.HashMap");
									IntPtr methodID3 = AndroidJNIHelper.GetMethodID(androidJavaObject4.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
									object[] array5 = new object[2];
									array5[0] = (array5[1] = null);
									foreach (KeyValuePair<string, object> item2 in dictionary2)
									{
										array5[0] = new AndroidJavaObject("java.lang.String", item2.Key);
										if (item2.Value.GetType().ToString() == "System.String")
										{
											array5[1] = new AndroidJavaObject("java.lang.String", item2.Value);
										}
										else if (item2.Value.GetType().ToString() == "System.Int32")
										{
											array5[1] = new AndroidJavaObject("java.lang.Integer", item2.Value);
										}
										else if (item2.Value.GetType().ToString() == "System.Double")
										{
											array5[1] = new AndroidJavaObject("java.lang.Double", item2.Value);
										}
										else if (item2.Key == "hs-tags" && item2.Value.GetType().ToString() == "System.String[]")
										{
											string[] array6 = (string[])item2.Value;
											AndroidJavaObject androidJavaObject5 = new AndroidJavaObject("java.util.ArrayList");
											IntPtr methodID4 = AndroidJNIHelper.GetMethodID(androidJavaObject5.GetRawClass(), "add", "(Ljava/lang/String;)Z");
											object[] array7 = new object[1];
											string[] array8 = array6;
											foreach (string text3 in array8)
											{
												if (text3 != null)
												{
													array7[0] = new AndroidJavaObject("java.lang.String", text3);
													AndroidJNI.CallBooleanMethod(androidJavaObject5.GetRawObject(), methodID4, AndroidJNIHelper.CreateJNIArgArray(array7));
												}
											}
											array5[1] = new AndroidJavaObject("java.util.ArrayList", androidJavaObject5);
										}
										if (array5[1] != null)
										{
											AndroidJNI.CallObjectMethod(androidJavaObject4.GetRawObject(), methodID3, AndroidJNIHelper.CreateJNIArgArray(array5));
										}
									}
									array[1] = new AndroidJavaObject("java.util.HashMap", androidJavaObject4);
								}
								if (item.Key == "withTagsMatching")
								{
									Dictionary<string, object> dictionary3 = (Dictionary<string, object>)item.Value;
									AndroidJavaObject androidJavaObject6 = new AndroidJavaObject("java.util.HashMap");
									IntPtr methodID5 = AndroidJNIHelper.GetMethodID(androidJavaObject6.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
									object[] array9 = new object[2];
									array9[0] = (array9[1] = null);
									foreach (KeyValuePair<string, object> item3 in dictionary3)
									{
										array9[0] = new AndroidJavaObject("java.lang.String", item3.Key);
										if (item3.Key == "operator" && item3.Value.GetType().ToString() == "System.String")
										{
											array9[1] = new AndroidJavaObject("java.lang.String", item3.Value);
										}
										else if (item3.Key == "tags" && item3.Value.GetType().ToString() == "System.String[]")
										{
											string[] array10 = (string[])item3.Value;
											AndroidJavaObject androidJavaObject7 = new AndroidJavaObject("java.util.ArrayList");
											IntPtr methodID6 = AndroidJNIHelper.GetMethodID(androidJavaObject7.GetRawClass(), "add", "(Ljava/lang/String;)Z");
											object[] array11 = new object[1];
											string[] array12 = array10;
											foreach (string text4 in array12)
											{
												if (text4 != null)
												{
													array11[0] = new AndroidJavaObject("java.lang.String", text4);
													AndroidJNI.CallBooleanMethod(androidJavaObject7.GetRawObject(), methodID6, AndroidJNIHelper.CreateJNIArgArray(array11));
												}
											}
											array9[1] = new AndroidJavaObject("java.util.ArrayList", androidJavaObject7);
										}
										if (array9[1] != null)
										{
											AndroidJNI.CallObjectMethod(androidJavaObject6.GetRawObject(), methodID5, AndroidJNIHelper.CreateJNIArgArray(array9));
										}
									}
									array[1] = new AndroidJavaObject("java.util.HashMap", androidJavaObject6);
								}
							}
						}
						if (array[1] != null)
						{
							AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						}
					}
				}
			}
			return androidJavaObject;
		}

		private AndroidJavaObject convertMetadataToJavaHashMap(Dictionary<string, object> metaMap)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			if (metaMap != null)
			{
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				array[0] = (array[1] = null);
				foreach (KeyValuePair<string, object> item in metaMap)
				{
					array[0] = new AndroidJavaObject("java.lang.String", item.Key);
					if (item.Value.GetType().ToString() == "System.String")
					{
						if ((item.Value != null && item.Value.Equals("yes")) || item.Value.Equals("no"))
						{
							string text = ((!item.Value.Equals("yes")) ? "false" : "true");
							array[1] = new AndroidJavaObject("java.lang.Boolean", text);
						}
						else
						{
							array[1] = new AndroidJavaObject("java.lang.String", item.Value);
						}
					}
					else if (item.Key == "hs-tags" && item.Value.GetType().ToString() == "System.String[]")
					{
						string[] array2 = (string[])item.Value;
						AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.util.ArrayList");
						IntPtr methodID2 = AndroidJNIHelper.GetMethodID(androidJavaObject2.GetRawClass(), "add", "(Ljava/lang/String;)Z");
						object[] array3 = new object[1];
						string[] array4 = array2;
						foreach (string text2 in array4)
						{
							if (text2 != null)
							{
								array3[0] = new AndroidJavaObject("java.lang.String", text2);
								AndroidJNI.CallBooleanMethod(androidJavaObject2.GetRawObject(), methodID2, AndroidJNIHelper.CreateJNIArgArray(array3));
							}
						}
						array[1] = new AndroidJavaObject("java.util.ArrayList", androidJavaObject2);
					}
					if (array[1] != null)
					{
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
					}
				}
			}
			Debug.Log("Returning the Hashmap : " + androidJavaObject);
			return androidJavaObject;
		}

		private void hsApiCall(string api, params object[] args)
		{
			hsPlugin.CallStatic(api, args);
		}

		private void hsApiCall(string api)
		{
			hsPlugin.CallStatic(api);
		}

		public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
		{
			hsApiCall("install", application, apiKey, domain, appId, convertToJavaHashMap(configMap));
		}

		public void install()
		{
			hsApiCall("install", application);
		}

		public int getNotificationCount(bool isAsync)
		{
			if(Application.platform != RuntimePlatform.Android)
				return 0;
			return hsPlugin.CallStatic<int>("getNotificationCount", new object[1] { isAsync });
		}

		public void setNameAndEmail(string userName, string email)
		{
			hsApiCall("setNameAndEmail", userName, email);
		}

		public void setUserIdentifier(string identifier)
		{
			hsApiCall("setUserIdentifier", identifier);
		}

		public void registerDeviceToken(string deviceToken)
		{
			hsApiCall("registerDeviceToken", currentActivity, deviceToken);
		}

		public void leaveBreadCrumb(string breadCrumb)
		{
			hsApiCall("leaveBreadCrumb", breadCrumb);
		}

		public void clearBreadCrumbs()
		{
			hsApiCall("clearBreadCrumbs");
		}

		public void login(string identifier, string userName, string email)
		{
			hsApiCall("login", identifier, userName, email);
		}

		public void logout()
		{
			hsApiCall("logout");
		}

		public void showConversation(Dictionary<string, object> configMap)
		{
			hsApiCall("showConversation", currentActivity, convertToJavaHashMap(configMap));
		}

		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
		{
			hsApiCall("showFAQSection", currentActivity, sectionPublishId, convertToJavaHashMap(configMap));
		}

		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
		{
			hsApiCall("showSingleFAQ", currentActivity, questionPublishId, convertToJavaHashMap(configMap));
		}

		public void showFAQs(Dictionary<string, object> configMap)
		{
			hsApiCall("showFAQs", currentActivity, convertToJavaHashMap(configMap));
		}

		public void showConversation()
		{
			hsApiCall("showConversation");
		}

		public void showFAQSection(string sectionPublishId)
		{
			hsApiCall("showFAQSection", sectionPublishId);
		}

		public void showSingleFAQ(string questionPublishId)
		{
			hsApiCall("showSingleFAQ", questionPublishId);
		}

		public void showFAQs()
		{
			hsApiCall("showFAQs");
		}

		public void showConversationWithMeta(Dictionary<string, object> configMap)
		{
			hsApiCall("showConversationWithMeta", convertMetadataToJavaHashMap(configMap));
		}

		public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
		{
			hsApiCall("showFAQSectionWithMeta", sectionPublishId, convertMetadataToJavaHashMap(configMap));
		}

		public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
		{
			hsApiCall("showSingleFAQWithMeta", questionPublishId, convertMetadataToJavaHashMap(configMap));
		}

		public void showFAQsWithMeta(Dictionary<string, object> configMap)
		{
			hsApiCall("showFAQsWithMeta", convertMetadataToJavaHashMap(configMap));
		}

		public void updateMetaData(Dictionary<string, object> metaData)
		{
			hsApiCall("setMetaData", Json.Serialize(metaData));
		}

		public void handlePushNotification(string issueId)
		{
			hsApiCall("handlePush", currentActivity, issueId);
		}

		public void showAlertToRateAppWithURL(string url)
		{
			hsApiCall("showAlertToRateApp", url);
		}

		public void registerSessionDelegates()
		{
			hsApiCall("registerSessionDelegates");
		}

		public void registerForPushWithGcmId(string gcmId)
		{
			hsApiCall("registerGcmKey", gcmId, currentActivity);
		}

		public void setSDKLanguage(string locale)
		{
			hsApiCall("setSDKLanguage", locale);
		}
	}
}
