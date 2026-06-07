using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Language
{
	public static string settingsAssetPath;

	public static LocalizationSettings settings;

	public static string Backup_settingsAssetPath;

	public static LocalizationSettings backup_settings;

	public static List<string> supportedLanguages;

	private static Dictionary<string, Object> assets;

	private static List<string> availableLanguages;

	private static LanguageCode currentLanguage;

	private static Dictionary<string, Hashtable> currentEntrySheets;

	private static string _persistentDataPath;

	private static AndroidJavaClass _pAndroidLocal;

	private static string _sDeviceLocal;

	private static string _sDeviceLanguage;

	private static void CreateAndroidLocal()
	{
	}

	public static string getDeviceLanguage()
	{
		return null;
	}

	public static string getDeviceLocale()
	{
		return null;
	}

	public static void ResetHasInitialized()
	{
	}

	public static void Init(string persistentPath)
	{
	}

	public static string LocalizedEnglishAssetName(string assetName)
	{
		return null;
	}

	private static void LoadAvailableLanguages()
	{
	}

	public static string[] GetLanguages()
	{
		return null;
	}

	public static bool ReloadLanguage(string persistantPath = null)
	{
		return false;
	}

	public static bool SwitchLanguage(string langCode)
	{
		return false;
	}

	public static bool SwitchLanguage(LanguageCode code)
	{
		return false;
	}

	private static void DoSwitch(LanguageCode newLang)
	{
	}

	public static Object GetAsset(string name)
	{
		return null;
	}

	private static bool HasLanguageFile(string lang, string sheetTitle)
	{
		return false;
	}

	private static string GetLanguageFileContents(string sheetTitle)
	{
		return null;
	}

	public static LanguageCode CurrentLanguage()
	{
		return default(LanguageCode);
	}

	public static string Get(string key)
	{
		return null;
	}

	public static string Get(string key, string sheetTitle)
	{
		return null;
	}

	private static void SendMonoMessage(string methodString, params object[] parameters)
	{
	}

	public static LanguageCode LanguageNameToCode(SystemLanguage name)
	{
		return default(LanguageCode);
	}
}
