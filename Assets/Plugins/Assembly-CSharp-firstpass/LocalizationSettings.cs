using System;
using UnityEngine;

[Serializable]
public class LocalizationSettings : ScriptableObject
{
	public string[] sheetTitles;

	public bool useSystemLanguagePerDefault;

	public string defaultLangCode;

	public static LanguageCode GetLanguageEnum(string langCode)
	{
		return default(LanguageCode);
	}
}
