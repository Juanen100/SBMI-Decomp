using System.Collections.Generic;
using UnityEngine;

public class SoundArray : BaseSoundIndex
{
	private List<TFSound> sounds;

	public SoundArray(string key, int maxInstances, List<string> filenames, string character)
		: base(key, maxInstances)
	{
		sounds = new List<TFSound>();
		foreach (string filename in filenames)
		{
			sounds.Add(new TFSound(filename, character));
		}
	}

	public override TFSound GetNextSound(SoundEffectManager sfxMgr)
	{
		return sounds[Random.Range(0, sounds.Count)];
	}

	public override void Clear()
	{
		foreach (TFSound sound in sounds)
		{
			sound.Clear();
		}
	}

	public override void Init()
	{
		foreach (TFSound sound in sounds)
		{
			sound.Init();
		}
	}
}
