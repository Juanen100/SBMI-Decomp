using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public static class Language
{
	public static string settingsAssetPath = "Assets/Localization/Resources/Languages/LocalizationSettings.asset";

	public static LocalizationSettings settings = (LocalizationSettings)Resources.Load("Languages/" + Path.GetFileNameWithoutExtension(settingsAssetPath), typeof(LocalizationSettings));

	public static string Backup_settingsAssetPath = "Assets/Localization/Resources/Languages/BackupLocalizationSettings.asset";

	public static LocalizationSettings backup_settings = (LocalizationSettings)Resources.Load("Languages/" + Path.GetFileNameWithoutExtension(Backup_settingsAssetPath), typeof(LocalizationSettings));

	public static List<string> supportedLanguages = new List<string>(new string[9] { "de", "en", "es", "fr", "it", "latam", "nl", "pt", "ru" });

	private static Dictionary<string, UnityEngine.Object> assets = new Dictionary<string, UnityEngine.Object>();

	private static List<string> availableLanguages;

	private static LanguageCode currentLanguage = LanguageCode.N;

	private static Dictionary<string, Hashtable> currentEntrySheets;

	private static string _persistentDataPath = string.Empty;

	private static AndroidJavaClass _pAndroidLocal = null;

	private static string _sDeviceLocal = "EN";

	private static string _sDeviceLanguage = "EN";

	private static void CreateAndroidLocal()
	{
		try
		{
			_pAndroidLocal = new AndroidJavaClass("com.fws.localization.localization");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					_sDeviceLocal = _pAndroidLocal.CallStatic<string>("GetDeviceCountry", new object[1] { androidJavaObject });
					_sDeviceLanguage = _pAndroidLocal.CallStatic<string>("GetDeviceLocal", new object[1] { androidJavaObject });
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("CreateAndroidLocal: " + ex.Message);
		}
	}

	public static string getDeviceLanguage()
	{
		if (_pAndroidLocal == null)
		{
			CreateAndroidLocal();
		}
		return _sDeviceLanguage;
	}

	public static string getDeviceLocale()
	{
		if (_pAndroidLocal == null)
		{
			CreateAndroidLocal();
		}
		return _sDeviceLocal;
	}

	public static void ResetHasInitialized()
	{
	}

	public static void Init(string persistentPath)
	{
		if (!string.IsNullOrEmpty(persistentPath))
		{
			_persistentDataPath = persistentPath;
		}
		LoadAvailableLanguages();
		bool useSystemLanguagePerDefault = settings.useSystemLanguagePerDefault;
		LanguageCode code = LocalizationSettings.GetLanguageEnum(settings.defaultLangCode);
		if (useSystemLanguagePerDefault)
		{
			LanguageCode languageCode = LanguageNameToCode(Application.systemLanguage);
			string text = getDeviceLocale().ToLower().Trim();
			string text2 = getDeviceLanguage().ToLower().Trim();
			if (text2 != null)
			{
				text2 = text2.Replace('_', '-');
			}
			Debug.Log("Local: " + text + " : Language: " + text2 + " : Code: " + languageCode);
			if ((text2.Equals("es") && !text.Equals("es")) || (text2.Contains("es-") && !text.Equals("es")))
			{
				languageCode = LanguageCode.LatAm;
			}
			if (languageCode == LanguageCode.N)
			{
				string twoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
				if (twoLetterISOLanguageName != "iv")
				{
					languageCode = LocalizationSettings.GetLanguageEnum(twoLetterISOLanguageName);
				}
			}
			if (availableLanguages.Contains(string.Concat(languageCode, string.Empty)))
			{
				code = languageCode;
			}
		}
		string text3 = PlayerPrefs.GetString("M2H_lastLanguage", string.Empty);
		if (text3 != string.Empty && availableLanguages.Contains(text3))
		{
			SwitchLanguage(text3);
		}
		else
		{
			SwitchLanguage(code);
		}
	}

	public static string LocalizedEnglishAssetName(string assetName)
	{
		if (CurrentLanguage() != LanguageCode.EN)
		{
			string newValue = CurrentLanguage().ToString().ToLower();
			assetName = assetName.Replace("en", newValue);
		}
		return assetName;
	}

	private static void LoadAvailableLanguages()
	{
		availableLanguages = new List<string>();
		Debug.Log(string.Concat("Language: Settings: ", settings, " Backup: ", backup_settings));
		if (settings.sheetTitles == null || settings.sheetTitles.Length <= 0)
		{
			Debug.Log("------- First file not found -- Trying BackupLocalizationSettings.Asset file");
			settings = backup_settings;
			if (backup_settings.sheetTitles == null || backup_settings.sheetTitles.Length <= 0)
			{
				Debug.Log("------- None available -- Can't Even use BackupLocalizationSettings.Asset file");
				return;
			}
		}
		foreach (int value in Enum.GetValues(typeof(LanguageCode)))
		{
			if (HasLanguageFile(string.Concat((LanguageCode)value, string.Empty), settings.sheetTitles[0]))
			{
				availableLanguages.Add(string.Concat((LanguageCode)value, string.Empty));
			}
		}
		Resources.UnloadUnusedAssets();
	}

	public static string[] GetLanguages()
	{
		return availableLanguages.ToArray();
	}

	public static bool ReloadLanguage(string persistantPath = null)
	{
		if (string.IsNullOrEmpty(_persistentDataPath) || currentLanguage == LanguageCode.N)
		{
			Init(persistantPath);
			return true;
		}
		return SwitchLanguage(currentLanguage);
	}

	public static bool SwitchLanguage(string langCode)
	{
		return SwitchLanguage(LocalizationSettings.GetLanguageEnum(langCode));
	}

	public static bool SwitchLanguage(LanguageCode code)
	{
		if (availableLanguages.Contains(string.Concat(code, string.Empty)))
		{
			DoSwitch(code);
			return true;
		}
		Debug.LogError(string.Concat("Could not switch from language ", currentLanguage, " to ", code));
		if (currentLanguage == LanguageCode.N)
		{
			if (availableLanguages.Count > 0)
			{
				DoSwitch(LocalizationSettings.GetLanguageEnum(availableLanguages[0]));
				Debug.LogError(string.Concat("Switched to ", currentLanguage, " instead"));
			}
			else
			{
				Debug.LogError(string.Concat("Please verify that you have the file: Resources/Languages/", code, string.Empty));
				Debug.Break();
			}
		}
		return false;
	}

	private static void DoSwitch(LanguageCode newLang)
	{
		PlayerPrefs.GetString("M2H_lastLanguage", string.Concat(newLang, string.Empty));
		currentLanguage = newLang;
		currentEntrySheets = new Dictionary<string, Hashtable>();
		XMLParser xMLParser = new XMLParser();
		string[] sheetTitles = settings.sheetTitles;
		foreach (string text in sheetTitles)
		{
			currentEntrySheets[text] = new Hashtable();
			Hashtable hashtable = (Hashtable)xMLParser.Parse(GetLanguageFileContents(text));
			ArrayList arrayList = (ArrayList)(((ArrayList)hashtable["entries"])[0] as Hashtable)["entry"];
			foreach (Hashtable item in arrayList)
			{
				string key = (string)item["@name"];
				string s = string.Concat(item["_text"], string.Empty).Trim();
				s = s.UnescapeXML();
				currentEntrySheets[text][key] = s;
			}
		}
		LocalizedAsset[] array = (LocalizedAsset[])UnityEngine.Object.FindObjectsOfType(typeof(LocalizedAsset));
		LocalizedAsset[] array2 = array;
		foreach (LocalizedAsset localizedAsset in array2)
		{
			localizedAsset.LocalizeAsset();
		}
		SendMonoMessage("ChangedLanguage", currentLanguage);
	}

	public static UnityEngine.Object GetAsset(string name)
	{
		if (!assets.ContainsKey(name))
		{
			assets[name] = Resources.Load(string.Concat("Languages/Assets/", CurrentLanguage(), "/", name));
		}
		return assets[name];
	}

	private static bool HasLanguageFile(string lang, string sheetTitle)
	{
		return (TextAsset)Resources.Load("Languages/" + lang + "_" + sheetTitle, typeof(TextAsset)) != null;
	}

	private static string GetLanguageFileContents(string sheetTitle)
	{
		string text = string.Concat("Languages/", currentLanguage, "_", sheetTitle);
		string path = _persistentDataPath + "/" + text + ".xml";
		if (File.Exists(path))
		{
			return File.ReadAllText(path);
		}
		TextAsset textAsset = (TextAsset)Resources.Load(text, typeof(TextAsset));
		return textAsset.text;
	}

	public static LanguageCode CurrentLanguage()
	{
		return currentLanguage;
	}

	public static string Get(string key)
	{
		if (key.StartsWith("!!"))
		{
			return Get(key, settings.sheetTitles[0]);
		}
		return key;
	}

	public static string Get(string key, string sheetTitle)
	{
		if (currentEntrySheets == null || !currentEntrySheets.ContainsKey(sheetTitle))
		{
			Debug.LogError("The sheet with title \"" + sheetTitle + "\" does not exist!");
			return string.Empty;
		}
		if (currentEntrySheets[sheetTitle].ContainsKey(key))
		{
			return (string)currentEntrySheets[sheetTitle][key];
		}
		return "MISSING LANG:" + key;
	}

	private static void SendMonoMessage(string methodString, params object[] parameters)
	{
		if (parameters != null && parameters.Length > 1)
		{
			Debug.LogError("We cannot pass more than one argument currently!");
		}
		GameObject[] array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((bool)gameObject && gameObject.transform.parent == null)
			{
				if (parameters != null && parameters.Length == 1)
				{
					gameObject.gameObject.BroadcastMessage(methodString, parameters[0], SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					gameObject.gameObject.BroadcastMessage(methodString, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public static LanguageCode LanguageNameToCode(SystemLanguage name)
	{
		switch (name)
		{
		case SystemLanguage.Afrikaans:
			return LanguageCode.AF;
		case SystemLanguage.Arabic:
			return LanguageCode.AR;
		case SystemLanguage.Basque:
			return LanguageCode.BA;
		case SystemLanguage.Belarusian:
			return LanguageCode.BE;
		case SystemLanguage.Bulgarian:
			return LanguageCode.BG;
		case SystemLanguage.Catalan:
			return LanguageCode.CA;
		case SystemLanguage.Chinese:
			return LanguageCode.ZH;
		case SystemLanguage.Czech:
			return LanguageCode.CS;
		case SystemLanguage.Danish:
			return LanguageCode.DA;
		case SystemLanguage.Dutch:
			return LanguageCode.NL;
		case SystemLanguage.English:
			return LanguageCode.EN;
		case SystemLanguage.Estonian:
			return LanguageCode.ET;
		case SystemLanguage.Faroese:
			return LanguageCode.FA;
		case SystemLanguage.Finnish:
			return LanguageCode.FI;
		case SystemLanguage.French:
			return LanguageCode.FR;
		case SystemLanguage.German:
			return LanguageCode.DE;
		case SystemLanguage.Greek:
			return LanguageCode.EL;
		case SystemLanguage.Hebrew:
			return LanguageCode.HE;
		case SystemLanguage.Hungarian:
			return LanguageCode.HU;
		case SystemLanguage.Icelandic:
			return LanguageCode.IS;
		case SystemLanguage.Indonesian:
			return LanguageCode.ID;
		case SystemLanguage.Italian:
			return LanguageCode.IT;
		case SystemLanguage.Japanese:
			return LanguageCode.JA;
		case SystemLanguage.Korean:
			return LanguageCode.KO;
		case SystemLanguage.Latvian:
			return LanguageCode.LA;
		case SystemLanguage.Lithuanian:
			return LanguageCode.LT;
		case SystemLanguage.Norwegian:
			return LanguageCode.NO;
		case SystemLanguage.Polish:
			return LanguageCode.PL;
		case SystemLanguage.Portuguese:
			return LanguageCode.PT;
		case SystemLanguage.Romanian:
			return LanguageCode.RO;
		case SystemLanguage.Russian:
			return LanguageCode.RU;
		case SystemLanguage.SerboCroatian:
			return LanguageCode.SH;
		case SystemLanguage.Slovak:
			return LanguageCode.SK;
		case SystemLanguage.Slovenian:
			return LanguageCode.SL;
		case SystemLanguage.Spanish:
			return LanguageCode.ES;
		case SystemLanguage.Swedish:
			return LanguageCode.SW;
		case SystemLanguage.Thai:
			return LanguageCode.TH;
		case SystemLanguage.Turkish:
			return LanguageCode.TR;
		case SystemLanguage.Ukrainian:
			return LanguageCode.UK;
		case SystemLanguage.Vietnamese:
			return LanguageCode.VI;
		default:
			switch (name)
			{
			case SystemLanguage.Hungarian:
				return LanguageCode.HU;
			case SystemLanguage.Unknown:
				return LanguageCode.N;
			default:
				return LanguageCode.N;
			}
		}
	}
}
