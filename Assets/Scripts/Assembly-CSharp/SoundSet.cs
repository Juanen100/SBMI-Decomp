using System.Collections.Generic;

public class SoundSet : BaseSoundIndex
{
	private List<string> keys;

	public SoundSet(string thisKey, int maxInstances, List<string> thoseKeys)
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
