using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager
{
	public static int sAudioSourceID;

	public const int START_POOL_SIZE = 6;

	public static string SOUND_ENABLED;

	public static SoundEffectManager soundEffectManager;

	private static string SOUND_FILE;

	private List<AudioSource> audioSourcePool;

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
			return false;
		}
		set
		{
		}
	}

	public SoundEffectManager(Dictionary<string, ISoundIndex> sounds)
	{
	}

	public static AudioSource CreateAudioSource()
	{
		return null;
	}

	public static SoundEffectManager CreateSoundEffectManager()
	{
		return null;
	}

	private static SoundEffectManager CreateSoundEffectManagerFromSpread()
	{
		return null;
	}

	public ISoundIndex GetSoundIndex(string key)
	{
		return null;
	}

	public void Clear()
	{
	}

	public void InitAudio()
	{
	}

	public void StartSoundEffectsManager()
	{
	}

	public AudioSource PlaySound(string soundId)
	{
		return null;
	}

	public AudioSource PlaySound(string soundId, float delaySeconds)
	{
		return null;
	}

	private AudioSource GetAudioSource()
	{
		return null;
	}

	public void ToggleOnOff()
	{
	}
}
