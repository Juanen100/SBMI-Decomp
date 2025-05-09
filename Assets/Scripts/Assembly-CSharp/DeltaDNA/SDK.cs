using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MiniJSON;
using UnityEngine;

namespace DeltaDNA
{
	public sealed class SDK : Singleton<SDK>
	{
		private static readonly string PF_KEY_USER_ID = "DDSDK_USER_ID";

		private static readonly string PF_KEY_FIRST_RUN = "DDSDK_FIRST_RUN";

		private static readonly string PF_KEY_HASH_SECRET = "DDSDK_HASH_SECRET";

		private static readonly string PF_KEY_CLIENT_VERSION = "DDSDK_CLIENT_VERSION";

		private static readonly string PF_KEY_PUSH_NOTIFICATION_TOKEN = "DDSDK_PUSH_NOTIFICATION_TOKEN";

		private static readonly string PF_KEY_ANDROID_REGISTRATION_ID = "DDSDK_ANDROID_REGISTRATION_ID";

		private static readonly string EV_KEY_NAME = "eventName";

		private static readonly string EV_KEY_USER_ID = "userID";

		private static readonly string EV_KEY_SESSION_ID = "sessionID";

		private static readonly string EV_KEY_TIMESTAMP = "eventTimestamp";

		private static readonly string EV_KEY_PARAMS = "eventParams";

		private static readonly string EP_KEY_PLATFORM = "platform";

		private static readonly string EP_KEY_SDK_VERSION = "sdkVersion";

		public static readonly string AUTO_GENERATED_USER_ID;

		private bool initialised;

		private IEventStore eventStore;

		private EngageArchive engageArchive;

		public Settings Settings { get; set; }

		public TransactionBuilder Transaction { get; private set; }

		public string EnvironmentKey { get; private set; }

		public string CollectURL { get; private set; }

		public string EngageURL { get; private set; }

		public string SessionID { get; private set; }

		public string Platform { get; private set; }

		public string UserID
		{
			get
			{
				string text = PlayerPrefs.GetString(PF_KEY_USER_ID, null);
				if (string.IsNullOrEmpty(text))
				{
					LogDebug("No existing User ID found.");
					return null;
				}
				return text;
			}
			private set
			{
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString(PF_KEY_USER_ID, value);
					PlayerPrefs.Save();
				}
			}
		}

		public bool IsInitialised
		{
			get
			{
				return initialised;
			}
		}

		public bool IsUploading { get; private set; }

		public string HashSecret
		{
			get
			{
				string text = PlayerPrefs.GetString(PF_KEY_HASH_SECRET, null);
				if (string.IsNullOrEmpty(text))
				{
					LogDebug("Event hashing not enabled.");
					return null;
				}
				return text;
			}
			set
			{
				LogDebug("Setting Hash Secret '" + value + "'");
				PlayerPrefs.SetString(PF_KEY_HASH_SECRET, value);
				PlayerPrefs.Save();
			}
		}

		public string ClientVersion
		{
			get
			{
				string text = PlayerPrefs.GetString(PF_KEY_CLIENT_VERSION, null);
				if (string.IsNullOrEmpty(text))
				{
					LogWarning("No client game version set.");
					return null;
				}
				return text;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					LogDebug("Setting ClientVersion '" + value + "'");
					PlayerPrefs.SetString(PF_KEY_CLIENT_VERSION, value);
					PlayerPrefs.Save();
				}
			}
		}

		public string PushNotificationToken
		{
			get
			{
				string text = PlayerPrefs.GetString(PF_KEY_PUSH_NOTIFICATION_TOKEN, null);
				if (string.IsNullOrEmpty(text))
				{
					if (ClientInfo.Platform.Contains("IOS"))
					{
						LogWarning("No Apple push notification token set, sending push notifications to iOS devices will be unavailable.");
					}
					return null;
				}
				return text;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString(PF_KEY_PUSH_NOTIFICATION_TOKEN, value);
					PlayerPrefs.Save();
				}
			}
		}

		public string AndroidRegistrationID
		{
			get
			{
				string text = PlayerPrefs.GetString(PF_KEY_ANDROID_REGISTRATION_ID, null);
				if (string.IsNullOrEmpty(text))
				{
					if (ClientInfo.Platform.Contains("ANDROID"))
					{
						LogWarning("No Android registration id set, sending push notifications to Android devices will be unavailable.");
					}
					return null;
				}
				return text;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString(PF_KEY_ANDROID_REGISTRATION_ID, value);
					PlayerPrefs.Save();
				}
			}
		}

		private SDK()
		{
			Settings = new Settings();
			Transaction = new TransactionBuilder(this);
			eventStore = new EventStore(Settings.EVENT_STORAGE_PATH.Replace("{persistent_path}", Application.persistentDataPath), Settings.DebugMode);
			engageArchive = new EngageArchive(Settings.ENGAGE_STORAGE_PATH.Replace("{persistent_path}", Application.persistentDataPath));
		}

		[Obsolete("Prefer 'StartSDK' instead, Init will be removed in a future update.")]
		public void Init(string envKey, string collectURL, string engageURL, string userID)
		{
			StartSDK(envKey, collectURL, engageURL, userID);
		}

		public void StartSDK(string envKey, string collectURL, string engageURL, string userID)
		{
			SetUserID(userID);
			EnvironmentKey = envKey;
			CollectURL = collectURL;
			EngageURL = engageURL;
			Platform = ClientInfo.Platform;
			SessionID = GetSessionID();
			initialised = true;
			TriggerDefaultEvents();
			if (Settings.BackgroundEventUpload && !IsInvoking("Upload"))
			{
				InvokeRepeating("Upload", Settings.BackgroundEventUploadStartDelaySeconds, Settings.BackgroundEventUploadRepeatRateSeconds);
			}
		}

		public void NewSession()
		{
			SessionID = GetSessionID();
		}

		public void StopSDK()
		{
			LogDebug("Stopping SDK");
			RecordEvent("gameEnded");
			CancelInvoke();
			Upload();
			initialised = false;
		}

		[Obsolete("Prefer 'RecordEvent' instead, Trigger will be removed in a future update.")]
		public void TriggerEvent(string eventName)
		{
			RecordEvent(eventName, new Dictionary<string, object>());
		}

		public void RecordEvent(string eventName)
		{
			RecordEvent(eventName, new Dictionary<string, object>());
		}

		[Obsolete("Prefer 'RecordEvent' instead, Trigger will be removed in a future update.")]
		public void TriggerEvent(string eventName, EventBuilder eventParams)
		{
			RecordEvent(eventName, (eventParams != null) ? eventParams.ToDictionary() : new Dictionary<string, object>());
		}

		public void RecordEvent(string eventName, EventBuilder eventParams)
		{
			RecordEvent(eventName, (eventParams != null) ? eventParams.ToDictionary() : new Dictionary<string, object>());
		}

		[Obsolete("Prefer 'RecordEvent' instead, Trigger will be removed in a future update.")]
		public void TriggerEvent(string eventName, Dictionary<string, object> eventParams)
		{
			RecordEvent(eventName, eventParams);
		}

		public void RecordEvent(string eventName, Dictionary<string, object> eventParams)
		{
			if (!initialised)
			{
				throw new NotStartedException("You must first start the SDK via the StartSDK method");
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary[EV_KEY_NAME] = eventName;
			dictionary[EV_KEY_USER_ID] = UserID;
			dictionary[EV_KEY_SESSION_ID] = SessionID;
			dictionary[EV_KEY_TIMESTAMP] = GetCurrentTimestamp();
			if (!eventParams.ContainsKey(EP_KEY_PLATFORM))
			{
				eventParams.Add(EP_KEY_PLATFORM, Platform);
			}
			if (!eventParams.ContainsKey(EP_KEY_SDK_VERSION))
			{
				eventParams.Add(EP_KEY_SDK_VERSION, Settings.SDK_VERSION);
			}
			dictionary[EV_KEY_PARAMS] = eventParams;
			Debug.Log("[DDSDK eventRecord] " + Json.Serialize(dictionary));
			if (!string.IsNullOrEmpty(UserID) && !eventStore.Push(Json.Serialize(dictionary)))
			{
				LogWarning("Event Store full, unable to handle event");
			}
		}

		public void RequestEngagement(string decisionPoint, Dictionary<string, object> engageParams, Action<Dictionary<string, object>> callback)
		{
			if (!initialised)
			{
				throw new NotStartedException("You must first start the SDK via the StartSDK method");
			}
			if (string.IsNullOrEmpty(EngageURL))
			{
				LogWarning("Engage URL not configured, can not make engagement.");
			}
			else if (string.IsNullOrEmpty(decisionPoint))
			{
				LogWarning("No decision point set, can not make engagement.");
			}
			else
			{
				StartCoroutine(EngageCoroutine(decisionPoint, engageParams, callback));
			}
		}

		public void Upload()
		{
			if (!initialised)
			{
				throw new NotStartedException("You must first start the SDK via the StartSDK method");
			}
			if (IsUploading)
			{
				LogWarning("Event upload already in progress, aborting");
			}
			else
			{
				StartCoroutine(UploadCoroutine());
			}
		}

		public void ClearPersistentData()
		{
			PlayerPrefs.DeleteKey(PF_KEY_USER_ID);
			PlayerPrefs.DeleteKey(PF_KEY_FIRST_RUN);
			PlayerPrefs.DeleteKey(PF_KEY_HASH_SECRET);
			PlayerPrefs.DeleteKey(PF_KEY_CLIENT_VERSION);
			PlayerPrefs.DeleteKey(PF_KEY_PUSH_NOTIFICATION_TOKEN);
			PlayerPrefs.DeleteKey(PF_KEY_ANDROID_REGISTRATION_ID);
			eventStore.Clear();
			engageArchive.Clear();
		}

		public override void OnDestroy()
		{
			if (eventStore != null && eventStore.GetType() == typeof(EventStore))
			{
				eventStore.Dispose();
			}
			if (engageArchive != null)
			{
				engageArchive.Save();
			}
			base.OnDestroy();
		}

		private void LogDebug(string message)
		{
			if (Settings.DebugMode)
			{
				Debug.Log("[DDSDK] " + message);
			}
		}

		private void LogWarning(string message)
		{
			Debug.LogWarning("[DDSDK] " + message);
		}

		private string GetSessionID()
		{
			return Guid.NewGuid().ToString();
		}

		private string GetUserID()
		{
			string text = Settings.LEGACY_SETTINGS_STORAGE_PATH.Replace("{persistent_path}", Application.persistentDataPath);
			if (File.Exists(text))
			{
				LogDebug("Found a legacy file in " + text);
				using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
				{
					try
					{
						List<byte> list = new List<byte>();
						byte[] array = new byte[1024];
						while (fileStream.Read(array, 0, array.Length) > 0)
						{
							list.AddRange(array);
						}
						byte[] array2 = list.ToArray();
						string json = Encoding.UTF8.GetString(array2, 0, array2.Length);
						Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
						if (dictionary.ContainsKey("userID"))
						{
							LogDebug("Found a legacy user id for player");
							return dictionary["userID"] as string;
						}
					}
					catch (Exception ex)
					{
						LogWarning("Problem reading legacy user id: " + ex.Message);
					}
				}
			}
			LogDebug("Creating a new user id for player");
			return Guid.NewGuid().ToString();
		}

		private string GetCurrentTimestamp()
		{
			return DateTime.UtcNow.ToString(Settings.EVENT_TIMESTAMP_FORMAT, CultureInfo.InvariantCulture);
		}

		private IEnumerator UploadCoroutine()
		{
			IsUploading = true;
			try
			{
				eventStore.Swap();
				List<string> events = eventStore.Read();
				if (events.Count <= 0)
				{
					yield break;
				}
				LogDebug("Starting event upload");
				yield return StartCoroutine(PostEvents(events.ToArray(), delegate(bool succeeded)
				{
					if (succeeded)
					{
						LogDebug("Event upload successful");
						eventStore.Clear();
					}
					else
					{
						LogWarning("Event upload failed - try again later");
					}
				}));
			}
			finally
			{
				IsUploading = false;
			}
		}

		private IEnumerator EngageCoroutine(string decisionPoint, Dictionary<string, object> engageParams, Action<Dictionary<string, object>> callback)
		{
			LogDebug("Starting engagement for '" + decisionPoint + "'");
			Dictionary<string, object> engageRequest = new Dictionary<string, object>
			{
				{ "userID", UserID },
				{ "decisionPoint", decisionPoint },
				{ "sessionID", SessionID },
				{
					"version",
					Settings.ENGAGE_API_VERSION
				},
				{
					"sdkVersion",
					Settings.SDK_VERSION
				},
				{ "platform", Platform },
				{
					"timezoneOffset",
					Convert.ToInt32(ClientInfo.TimezoneOffset)
				}
			};
			if (ClientInfo.Locale != null)
			{
				engageRequest.Add("locale", ClientInfo.Locale);
			}
			if (engageParams != null)
			{
				engageRequest.Add("parameters", engageParams);
			}
			string engageJSON = null;
			try
			{
				engageJSON = Json.Serialize(engageRequest);
			}
			catch (Exception ex)
			{
				LogWarning("Problem serialising engage request data: " + ex.Message);
				yield break;
			}
			string decisionPoint2 = default(string);
			Action<Dictionary<string, object>> callback2 = default(Action<Dictionary<string, object>>);
			yield return StartCoroutine(EngageRequest(engageJSON, delegate(string response)
			{
				bool flag = false;
				if (response != null)
				{
					LogDebug("Using live engagement: " + response);
					engageArchive[decisionPoint2] = response;
				}
				else if (engageArchive.Contains(decisionPoint2))
				{
					LogWarning("Engage request failed, using cached response.");
					flag = true;
					response = engageArchive[decisionPoint2];
				}
				else
				{
					LogWarning("Engage request failed");
				}
				Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
				if (flag)
				{
					dictionary["isCachedResponse"] = flag;
				}
				callback2(dictionary);
			}));
		}

		private IEnumerator PostEvents(string[] events, Action<bool> resultCallback)
		{
			string bulkEvent = "{\"eventList\":[" + string.Join(",", events) + "]}";
			Debug.Log("[DDSDK bulkEvents: ]" + bulkEvent);
			string url = ((HashSecret == null) ? FormatURI(Settings.COLLECT_URL_PATTERN, CollectURL, EnvironmentKey) : FormatURI(hash: GenerateHash(bulkEvent, HashSecret), uriPattern: Settings.COLLECT_HASH_URL_PATTERN, apiHost: CollectURL, envKey: EnvironmentKey));
			int attempts = 0;
			bool succeeded = false;
			int num;
			do
			{
				yield return StartCoroutine(HttpPOST(url, bulkEvent, delegate(int status, string response)
				{
					switch (status)
					{
					case 200:
					case 204:
						succeeded = true;
						return;
					case 100:
						if (string.IsNullOrEmpty(response))
						{
							succeeded = true;
							return;
						}
						break;
					}
					LogDebug("Error uploading events, Collect returned: " + status + " " + response);
				}));
				yield return new WaitForSeconds(Settings.HttpRequestRetryDelaySeconds);
				if (succeeded)
				{
					break;
				}
				attempts = (num = attempts + 1);
			}
			while (num < Settings.HttpRequestMaxRetries);
			resultCallback(succeeded);
		}

		private IEnumerator EngageRequest(string engagement, Action<string> callback)
		{
			string url = ((HashSecret == null) ? FormatURI(Settings.ENGAGE_URL_PATTERN, EngageURL, EnvironmentKey) : FormatURI(hash: GenerateHash(engagement, HashSecret), uriPattern: Settings.ENGAGE_HASH_URL_PATTERN, apiHost: EngageURL, envKey: EnvironmentKey));
			Action<string> callback2 = default(Action<string>);
			yield return StartCoroutine(HttpPOST(url, engagement, delegate(int status, string response)
			{
				if (status == 200 || status == 100)
				{
					callback2(response);
				}
				else
				{
					LogDebug("Error requesting engagement, Engage returned: " + status);
					callback2(null);
				}
			}));
		}

		private IEnumerator HttpGET(string url, Action<int, string> responseCallback = null)
		{
			LogDebug("HttpGET " + url);
			WWW www = new WWW(url);
			yield return www;
			int statusCode = 0;
			if (www.error == null)
			{
				statusCode = 200;
				if (responseCallback != null)
				{
					responseCallback(statusCode, www.text);
				}
			}
			else
			{
				statusCode = ReadWWWResponse(www.error);
				if (responseCallback != null)
				{
					responseCallback(statusCode, null);
				}
			}
		}

		private IEnumerator HttpPOST(string url, string json, Action<int, string> responseCallback = null)
		{
			LogDebug("HttpPOST " + url + " " + json);
			WWWForm form = new WWWForm();
			Hashtable headers = form.headers;
			headers["Content-Type"] = "application/json";
			byte[] bytes = Encoding.UTF8.GetBytes(json);
			WWW www = new WWW(url, bytes, headers);
			yield return www;
			int statusCode = ReadWWWStatusCode(www);
			if (www.error == null)
			{
				if (responseCallback != null)
				{
					responseCallback(statusCode, www.text);
				}
				yield break;
			}
			LogDebug("WWW.error: " + www.error);
			if (responseCallback != null)
			{
				responseCallback(statusCode, null);
			}
		}

		private static int ReadWWWResponse(string response)
		{
			MatchCollection matchCollection = Regex.Matches(response, "^.*\\s(\\d{3})\\s.*$");
			if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 0)
			{
				return Convert.ToInt32(matchCollection[0].Groups[1].Value);
			}
			return 0;
		}

		private int ReadWWWStatusCode(WWW www)
		{
			int result = 0;
			string key = "NULL";
			if (!www.responseHeaders.ContainsKey(key))
			{
				result = ((!string.IsNullOrEmpty(www.error)) ? ReadWWWResponse(www.error) : 200);
			}
			else
			{
				string input = www.responseHeaders[key];
				MatchCollection matchCollection = Regex.Matches(input, "^HTTP.*\\s(\\d{3})\\s.*$");
				if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 0)
				{
					result = Convert.ToInt32(matchCollection[0].Groups[1].Value);
				}
			}
			return result;
		}

		private static string FormatURI(string uriPattern, string apiHost, string envKey, string hash = null)
		{
			string text = uriPattern.Replace("{host}", apiHost);
			text = text.Replace("{env_key}", envKey);
			return text.Replace("{hash}", hash);
		}

		private static string GenerateHash(string data, string secret)
		{
			MD5 mD = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(data + secret);
			byte[] array = mD.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		private void SetUserID(string userID)
		{
			if (string.IsNullOrEmpty(userID))
			{
				if (string.IsNullOrEmpty(UserID))
				{
					UserID = GetUserID();
				}
			}
			else if (UserID != userID)
			{
				PlayerPrefs.DeleteKey(PF_KEY_FIRST_RUN);
				UserID = userID;
			}
		}

		private void TriggerDefaultEvents()
		{
			if (Settings.OnFirstRunSendNewPlayerEvent && PlayerPrefs.GetInt(PF_KEY_FIRST_RUN, 1) > 0)
			{
				LogDebug("Sending 'newPlayer' event");
				EventBuilder eventParams = new EventBuilder().AddParam("userCountry", ClientInfo.CountryCode);
				RecordEvent("newPlayer", eventParams);
				PlayerPrefs.SetInt(PF_KEY_FIRST_RUN, 0);
			}
			if (Settings.OnInitSendGameStartedEvent)
			{
				LogDebug("Sending 'gameStarted' event");
				EventBuilder eventParams2 = new EventBuilder().AddParam("clientVersion", ClientVersion).AddParam("pushNotificationToken", PushNotificationToken).AddParam("androidRegistrationID", AndroidRegistrationID);
				RecordEvent("gameStarted", eventParams2);
			}
			if (Settings.OnInitSendClientDeviceEvent)
			{
				LogDebug("Sending 'clientDevice' event");
				EventBuilder eventParams3 = new EventBuilder().AddParam("deviceName", ClientInfo.DeviceName).AddParam("deviceType", ClientInfo.DeviceType).AddParam("hardwareVersion", ClientInfo.DeviceModel)
					.AddParam("operatingSystem", ClientInfo.OperatingSystem)
					.AddParam("operatingSystemVersion", ClientInfo.OperatingSystemVersion)
					.AddParam("manufacturer", ClientInfo.Manufacturer)
					.AddParam("timezoneOffset", ClientInfo.TimezoneOffset)
					.AddParam("userLanguage", ClientInfo.LanguageCode);
				RecordEvent("clientDevice", eventParams3);
			}
		}
	}
}
