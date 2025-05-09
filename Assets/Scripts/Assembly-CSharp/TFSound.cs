using UnityEngine;

public class TFSound
{
	public string fileName;

	public string characterName;

	public AudioClip clip;

	public bool bundleSound;

	public TFSound(string file, string characterName)
	{
		fileName = file;
		this.characterName = characterName;
	}

	public void Init()
	{
		if (Language.CurrentLanguage() != LanguageCode.EN)
		{
			fileName = LocalizeSoundFilename(fileName, false);
		}
		if (clip != null)
		{
			if (clip.name != fileName)
			{
				if (!bundleSound)
				{
					Resources.UnloadAsset(clip);
				}
				clip = (AudioClip)Resources.Load(fileName);
				if (clip == null)
				{
					clip = (AudioClip)Resources.Load(LocalizeSoundFilename(fileName, true));
				}
			}
		}
		else
		{
			clip = (AudioClip)Resources.Load(fileName);
			if (clip == null)
			{
				clip = (AudioClip)Resources.Load(LocalizeSoundFilename(fileName, true));
			}
		}
		bundleSound = false;
		if (clip == null)
		{
			Object obj = FileSystemCoordinator.LoadAsset(fileName);
			if (obj == null)
			{
				obj = FileSystemCoordinator.LoadAsset(LocalizeSoundFilename(fileName, true));
			}
			if (obj != null)
			{
				clip = (AudioClip)obj;
				bundleSound = true;
			}
		}
	}

	public void Clear()
	{
		if (characterName != null && clip != null)
		{
			Resources.UnloadAsset(clip);
		}
		clip = null;
	}

	public string LocalizeSoundFilename(string filename, bool altFile)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string text = null;
		string text2 = null;
		switch (Language.CurrentLanguage())
		{
		case LanguageCode.EN:
			empty = "/EN/";
			empty2 = "_EN";
			break;
		case LanguageCode.FR:
			empty = "/FR/";
			empty2 = "_FR";
			break;
		case LanguageCode.DE:
			empty = "/DE/";
			empty2 = "_DE";
			break;
		case LanguageCode.IT:
			empty = "/IT/";
			empty2 = "_IT";
			break;
		case LanguageCode.ES:
			empty = "/ES/";
			empty2 = "_ES";
			break;
		case LanguageCode.NL:
			empty = "/NL/";
			empty2 = "_NL";
			break;
		case LanguageCode.RU:
			empty = "/RU/";
			empty2 = "_RU";
			break;
		case LanguageCode.PT:
			empty = "/PT/";
			empty2 = "_PT";
			break;
		case LanguageCode.LatAm:
			empty = "/LatAm/";
			empty2 = "_LatAm";
			break;
		default:
			empty = "/EN/";
			empty2 = "_EN";
			break;
		}
		if (altFile)
		{
			text = empty;
			text2 = empty2;
			empty = "EXT";
			empty2 = string.Empty;
		}
		else
		{
			text = "/EN/";
			text2 = "_EN";
		}
		return filename.Replace(text, empty).Replace(text2, empty2);
	}
}
