#define ASSERTS_ON
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class SoundEffectManager
{
	public const int START_POOL_SIZE = 6;

	public static int sAudioSourceID = 0;

	public static string SOUND_ENABLED = "sound_enabled";

	public static SoundEffectManager soundEffectManager;

	private static string SOUND_FILE = TFUtils.GetStreamingAssetsFileInDirectory("Sound", "SoundEffects.json");

	private TFPool<GameObject> audioSourcePool;

	private List<GameObject> cleanupList;

	private Dictionary<string, ISoundIndex> sounds;

	private Dictionary<string, int> soundInstances;

	private HashSet<string> characterSet;

	private bool enabled;

	private ISoundIndex defaultSound;

	public bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			enabled = value;
			int value2 = (enabled ? 1 : 0);
			PlayerPrefs.SetInt(SOUND_ENABLED, value2);
			PlayerPrefs.Save();
		}
	}

	public SoundEffectManager(Dictionary<string, ISoundIndex> sounds)
	{
		audioSourcePool = TFPool<GameObject>.CreatePool(6, CreateAudioSource);
		cleanupList = new List<GameObject>(6);
		characterSet = new HashSet<string>();
		soundInstances = new Dictionary<string, int>();
		this.sounds = sounds;
		enabled = false;
		TFUtils.Assert(this.sounds.ContainsKey("Silence"), "You should include a sound named Silence in this file to use as the default sound!");
		defaultSound = this.sounds["Silence"];
	}

	public static GameObject CreateAudioSource()
	{
		GameObject gameObject = new GameObject("AudioSource_" + sAudioSourceID++);
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		return gameObject;
	}

	public static SoundEffectManager CreateSoundEffectManager()
	{
		if (soundEffectManager == null)
		{
			soundEffectManager = CreateSoundEffectManagerFromSpread();
			if (soundEffectManager != null)
			{
				return soundEffectManager;
			}
			Debug.Log("Loading " + SOUND_FILE + "...");
			string text = TFUtils.ReadAllText(SOUND_FILE);
			TFUtils.Assert(!string.IsNullOrEmpty(text), "Empty file for sound data - SoundEffects.json.");
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(text);
			TFUtils.Assert(dictionary != null, "Invalid json data for SoundEffects.json.");
			Dictionary<string, ISoundIndex> dictionary2 = new Dictionary<string, ISoundIndex>();
			foreach (Dictionary<string, object> item in dictionary["sound_effects"] as List<object>)
			{
				ISoundIndex soundIndex = SoundIndexFactory.FromDict(item);
				if (dictionary2.ContainsKey(soundIndex.Key))
				{
					TFUtils.Assert(false, "Found duplicate entry for sound entry '" + soundIndex.Key + "'!");
				}
				else
				{
					dictionary2.Add(soundIndex.Key, soundIndex);
				}
			}
			soundEffectManager = new SoundEffectManager(dictionary2);
		}
		return soundEffectManager;
	}

	private static SoundEffectManager CreateSoundEffectManagerFromSpread()
	{
		string text = "Sound";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, ISoundIndex> dictionary2 = new Dictionary<string, ISoundIndex>();
		string text2 = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "set sounds");
			}
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "type");
			if (stringCell != "sound_effects")
			{
				continue;
			}
			dictionary.Clear();
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "file");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
			{
				dictionary.Add("file", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "character");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
			{
				dictionary.Add("character", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "set sound 1");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
			{
				dictionary.Add("set", new List<string> { stringCell });
				for (int j = 2; j <= num2; j++)
				{
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "set sound " + j);
					if (string.IsNullOrEmpty(stringCell) || stringCell == text2)
					{
						break;
					}
					((List<string>)dictionary["set"]).Add(stringCell);
				}
			}
			ISoundIndex soundIndex = SoundIndexFactory.FromDict(dictionary);
			if (dictionary2.ContainsKey(soundIndex.Key))
			{
				TFUtils.Assert(false, "Found duplicate entry for sound entry '" + soundIndex.Key + "'!");
			}
			else
			{
				dictionary2.Add(soundIndex.Key, soundIndex);
			}
		}
		return new SoundEffectManager(dictionary2);
	}

	public ISoundIndex GetSoundIndex(string key)
	{
		ISoundIndex value = null;
		if (!sounds.TryGetValue(key, out value))
		{
			TFUtils.ErrorLog("Could not find sound effect with key " + key);
			return defaultSound;
		}
		return value;
	}

	public void Clear()
	{
		foreach (ISoundIndex value in sounds.Values)
		{
			value.Clear();
		}
	}

	public void InitAudio()
	{
		foreach (ISoundIndex value in sounds.Values)
		{
			value.Init();
		}
	}

	private void CleanUpFinishedAudio()
	{
		cleanupList.Clear();
		foreach (GameObject item in audioSourcePool.ActiveSet)
		{
			if (!item.GetComponent<AudioSource>().isPlaying)
			{
				cleanupList.Add(item);
			}
		}
		foreach (GameObject cleanup in cleanupList)
		{
			if (cleanup.GetComponent<AudioSource>().name != null)
			{
				characterSet.Remove(cleanup.GetComponent<AudioSource>().name);
			}
			if (soundInstances.ContainsKey(cleanup.name))
			{
				Dictionary<string, int> dictionary2;
				Dictionary<string, int> dictionary = (dictionary2 = soundInstances);
				string name;
				string key = (name = cleanup.name);
				int num = dictionary2[name];
				dictionary[key] = num - 1;
			}
			audioSourcePool.Release(cleanup);
		}
		soundInstances.Clear();
	}

	public void StartSoundEffectsManager()
	{
		if (PlayerPrefs.HasKey(SOUND_ENABLED))
		{
			bool flag = PlayerPrefs.GetInt(SOUND_ENABLED) == 1;
			enabled = flag;
		}
		else
		{
			Enabled = true;
		}
	}

	public AudioSource PlaySound(string soundId)
	{
		return PlaySound(soundId, 0f);
	}

	public AudioSource PlaySound(string soundId, float delaySeconds)
	{
		if (!Enabled || soundId == null)
		{
			return null;
		}
		if (!sounds.ContainsKey(soundId))
		{
			TFUtils.ErrorLog("Cannot find sound effect: " + soundId);
			return null;
		}
		CleanUpFinishedAudio();
		BaseSoundIndex baseSoundIndex = sounds[soundId] as BaseSoundIndex;
		if (baseSoundIndex != null)
		{
			int value = 0;
			if (soundInstances.TryGetValue(soundId, out value) && value >= baseSoundIndex.MaxInstances)
			{
				return null;
			}
		}
		GameObject gameObject = audioSourcePool.Create(CreateAudioSource);
		gameObject.name = soundId;
		TFSound nextSound = sounds[soundId].GetNextSound(this);
		if (nextSound.clip == null)
		{
			nextSound.Init();
			if (nextSound.clip == null)
			{
				TFUtils.ErrorLog("Sound clip " + soundId + " did not initialize properly (Clip should not be null).\nFilename=" + nextSound.fileName);
				audioSourcePool.Release(gameObject);
				return null;
			}
		}
		if (nextSound.characterName != null)
		{
			if (characterSet.Contains(nextSound.characterName))
			{
				TFUtils.DebugLog("Character already has VO: " + soundId);
				audioSourcePool.Release(gameObject);
				return null;
			}
			characterSet.Add(nextSound.characterName);
		}
		gameObject.GetComponent<AudioSource>().name = nextSound.characterName;
		gameObject.GetComponent<AudioSource>().clip = nextSound.clip;
		gameObject.GetComponent<AudioSource>().volume = 1f;
		int frequency = gameObject.GetComponent<AudioSource>().clip.frequency;
		ulong num = (ulong)(delaySeconds * (float)frequency);
		if (num != 0L)
		{
			gameObject.GetComponent<AudioSource>().PlayDelayed((ulong)(delaySeconds * (float)frequency));
		}
		else
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
		if (soundInstances.ContainsKey(soundId))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = (dictionary2 = soundInstances);
			string key2;
			string key = (key2 = soundId);
			int num2 = dictionary2[key2];
			dictionary[key] = num2 + 1;
		}
		else
		{
			soundInstances.Add(soundId, 1);
		}
		return gameObject.GetComponent<AudioSource>();
	}

	public void ToggleOnOff()
	{
		Enabled = !Enabled;
	}
}
