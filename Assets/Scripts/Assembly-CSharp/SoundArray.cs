using System.Collections.Generic;

public class SoundArray : BaseSoundIndex
{
	private List<TFSound> sounds;

	public SoundArray(string key, int maxInstances, List<string> filenames, string character)
		: base(null, 0)
	{
	}

	public override TFSound GetNextSound(SoundEffectManager sfxMgr)
	{
		return null;
	}

	public override void Clear()
	{
	}

	public override void Init()
	{
	}
}
