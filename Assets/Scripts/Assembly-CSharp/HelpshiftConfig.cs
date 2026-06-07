using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HelpshiftConfig : ScriptableObject
{
	private static HelpshiftConfig instance;

	private const string helpshiftConfigAssetName = "HelpshiftConfig";

	private const string helpshiftConfigPath = "Helpshift/Resources";

	public const string pluginVersion = "4.1.0";

	[SerializeField]
	private string apiKey;

	[SerializeField]
	private string domainName;

	[SerializeField]
	private string iosAppId;

	[SerializeField]
	private string androidAppId;

	[SerializeField]
	private int contactUsOption;

	[SerializeField]
	private bool gotoConversation;

	[SerializeField]
	private bool presentFullScreen;

	[SerializeField]
	private int enableInAppNotification;

	[SerializeField]
	private bool requireEmail;

	[SerializeField]
	private bool hideNameAndEmail;

	[SerializeField]
	private bool enablePrivacy;

	[SerializeField]
	private bool showSearchOnNewConversation;

	[SerializeField]
	private int showConversationResolutionQuestion;

	[SerializeField]
	private int enableDefaultFallbackLanguage;

	[SerializeField]
	private bool disableEntryExitAnimations;

	[SerializeField]
	private string conversationPrefillText;

	[SerializeField]
	private bool enableInboxPolling;

	[SerializeField]
	private bool enableLogging;

	[SerializeField]
	private bool enableTypingIndicator;

	[SerializeField]
	private int screenOrientation;

	[SerializeField]
	private bool showConversationInfoScreen;

	[SerializeField]
	private string supportedFileFormats;

	private string[] contactUsOptions;

	[SerializeField]
	private string unityGameObject;

	[SerializeField]
	private string notificationIcon;

	[SerializeField]
	private string largeNotificationIcon;

	[SerializeField]
	private string notificationSound;

	[SerializeField]
	private string customFont;

	[SerializeField]
	private string supportNotificationChannel;

	[SerializeField]
	private string campaignsNotificationChannel;

	public static HelpshiftConfig Instance
	{
		get
		{
			return null;
		}
	}

	public bool GotoConversation
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public int ContactUs
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public bool PresentFullScreenOniPad
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool EnableInAppNotification
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool RequireEmail
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool HideNameAndEmail
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool EnablePrivacy
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool ShowSearchOnNewConversation
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool ShowConversationResolutionQuestion
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool EnableDefaultFallbackLanguage
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool DisableEntryExitAnimations
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public string ConversationPrefillText
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string ApiKey
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string DomainName
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string AndroidAppId
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string iOSAppId
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string UnityGameObject
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string NotificationIcon
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string LargeNotificationIcon
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string NotificationSound
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string CustomFont
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string SupportNotificationChannel
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string CampaignsNotificationChannel
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool EnableInboxPolling
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool EnableLogging
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool EnableTypingIndicator
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public int ScreenOrientation
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public string SupportedFileFormats
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool ShowConversationInfoScreen
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public Dictionary<string, object> InstallConfig
	{
		get
		{
			return null;
		}
	}

	public Dictionary<string, object> ApiConfig
	{
		get
		{
			return null;
		}
	}

	public void SaveConfig()
	{
	}

	public Dictionary<string, object> getApiConfig()
	{
		return null;
	}

	public Dictionary<string, object> getInstallConfig()
	{
		return null;
	}
}
