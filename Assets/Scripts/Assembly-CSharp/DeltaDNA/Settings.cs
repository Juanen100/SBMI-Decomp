namespace DeltaDNA
{
	public class Settings
	{
		internal static readonly string SDK_VERSION;

		internal static readonly string ENGAGE_API_VERSION;

		internal static readonly string EVENT_STORAGE_PATH;

		internal static readonly string ENGAGE_STORAGE_PATH;

		internal static readonly string ACTIONS_STORAGE_PATH;

		internal static readonly string LEGACY_SETTINGS_STORAGE_PATH;

		internal static readonly string EVENT_TIMESTAMP_FORMAT;

		internal static readonly string USERID_URL_PATTERN;

		internal static readonly string COLLECT_URL_PATTERN;

		internal static readonly string COLLECT_HASH_URL_PATTERN;

		internal static readonly string ENGAGE_URL_PATTERN;

		internal static readonly string ENGAGE_HASH_URL_PATTERN;

		private bool _debugMode;

		public bool OnFirstRunSendNewPlayerEvent { get; set; }

		public bool OnInitSendClientDeviceEvent { get; set; }

		public bool OnInitSendGameStartedEvent { get; set; }

		public bool DebugMode
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float HttpRequestRetryDelaySeconds { get; set; }

		public int HttpRequestMaxRetries { get; set; }

		public int HttpRequestCollectTimeoutSeconds { get; set; }

		public int HttpRequestEngageTimeoutSeconds { get; set; }

		public bool BackgroundEventUpload { get; set; }

		public int BackgroundEventUploadStartDelaySeconds { get; set; }

		public int BackgroundEventUploadRepeatRateSeconds { get; set; }

		public bool UseEventStore { get; set; }

		public int SessionTimeoutSeconds { get; set; }

		public int EngageCacheExpirySeconds { get; set; }

		public bool AdvertiserGdprUserConsent { get; set; }

		public bool AdvertiserGdprAgeRestrictedUser { get; set; }

		public bool MultipleActionsForEventTriggerEnabled { get; set; }

		internal Settings()
		{
		}
	}
}
