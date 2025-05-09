using System.Collections.Generic;
using UnityEngine;

public class SoundSet : BaseSoundIndex
{
	private List<string> keys;

	public SoundSet(string thisKey, int maxInstances, List<string> thoseKeys)
		: base(thisKey, maxInstances)
	{
		keys = new List<string>();
		foreach (string thoseKey in thoseKeys)
		{
			keys.Add(thoseKey);
		}
	}

	public override TFSound GetNextSound(SoundEffectManager sfxMgr)
	{
		string text = keys[Random.Range(0, keys.Count)];
		ISoundIndex soundIndex = sfxMgr.GetSoundIndex(text);
		return soundIndex.GetNextSound(sfxMgr);
	}

	public override void Clear()
	{
	}

	public override void Init()
	{
	}
}
