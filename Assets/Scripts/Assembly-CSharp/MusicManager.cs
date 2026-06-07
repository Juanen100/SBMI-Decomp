using System.Collections.Generic;
using UnityEngine;

public class MusicManager
{
	public static string MUSIC_ENABLED;

	private static string MUSIC_FILE;

	private Dictionary<string, string> tracks;

	private GameObject currentMusicGo;

	private AudioClip currentTrack;

	private bool enabled;

	private string currentTrackName;

	public bool Enabled
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public MusicManager(Dictionary<string, string> tracks)
	{
	}

	public static MusicManager CreateMusicManager()
	{
		return null;
	}

	public void PlayTrack(string trackName)
	{
	}

	private void PlayCurrentTrack()
	{
	}

	public void StopTrack()
	{
	}

	public void ToggleOnOff()
	{
	}

	public void Mute(bool setMute)
	{
	}

	private static MusicManager CreateMusicManagerFromSpread()
	{
		return null;
	}
}
