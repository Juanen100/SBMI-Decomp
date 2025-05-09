#define ASSERTS_ON
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class MusicManager
{
	public static string MUSIC_ENABLED = "music_enabled";

	private static string MUSIC_FILE = TFUtils.GetStreamingAssetsFileInDirectory("Sound", "Music.json");

	private Dictionary<string, string> tracks;

	private GameObject currentMusicGo;

	private AudioClip currentTrack;

	private bool enabled;

	private string currentTrackName;

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
			PlayerPrefs.SetInt(MUSIC_ENABLED, value2);
			PlayerPrefs.Save();
		}
	}

	public MusicManager(Dictionary<string, string> tracks)
	{
		this.tracks = tracks;
		if (PlayerPrefs.HasKey(MUSIC_ENABLED))
		{
			enabled = PlayerPrefs.GetInt(MUSIC_ENABLED) == 1;
		}
		else
		{
			Enabled = true;
		}
	}

	public static MusicManager CreateMusicManager()
	{
		MusicManager musicManager = CreateMusicManagerFromSpread();
		if (musicManager != null)
		{
			return musicManager;
		}
		string text = TFUtils.ReadAllText(MUSIC_FILE);
		TFUtils.Assert(!string.IsNullOrEmpty(text), "Empty file for music data - Music.json.");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(text);
		TFUtils.Assert(dictionary != null, "Invalid json data for Music.json.");
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		foreach (Dictionary<string, object> item in dictionary["music"] as List<object>)
		{
			string key = (string)item["name"];
			string value = (string)item["file"];
			dictionary2.Add(key, value);
		}
		return new MusicManager(dictionary2);
	}

	public void PlayTrack(string trackName)
	{
		if (Enabled && !(currentTrackName == trackName))
		{
			string text = tracks[trackName];
			TFUtils.DebugLog("MusicManager: Playing track(" + trackName + "), file(" + text + ")");
			if (currentMusicGo != null)
			{
				Object.Destroy(currentMusicGo);
			}
			currentMusicGo = new GameObject("CurrentMusicGo");
			currentMusicGo.AddComponent(typeof(AudioSource));
			AudioClip audioClip = (AudioClip)Resources.Load(text);
			TFUtils.Assert(audioClip != null, "Could not find file " + text);
			currentMusicGo.GetComponent<AudioSource>().loop = true;
			currentMusicGo.GetComponent<AudioSource>().clip = audioClip;
			currentMusicGo.GetComponent<AudioSource>().Play();
			currentTrackName = trackName;
		}
	}

	private void PlayCurrentTrack()
	{
		if (currentMusicGo != null)
		{
			currentMusicGo.GetComponent<AudioSource>().Play();
		}
		else
		{
			PlayTrack("InGame");
		}
	}

	public void StopTrack()
	{
		if (currentMusicGo != null)
		{
			currentMusicGo.GetComponent<AudioSource>().Stop();
		}
	}

	public void ToggleOnOff()
	{
		if (Enabled)
		{
			StopTrack();
			Enabled = !Enabled;
		}
		else
		{
			Enabled = !Enabled;
			PlayCurrentTrack();
		}
	}

	private static MusicManager CreateMusicManagerFromSpread()
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
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "type");
			if (!(stringCell != "music"))
			{
				dictionary.Add(instance.GetStringCell(sheetIndex, rowIndex, "name"), instance.GetStringCell(sheetIndex, rowIndex, "file"));
			}
		}
		return new MusicManager(dictionary);
	}
}
