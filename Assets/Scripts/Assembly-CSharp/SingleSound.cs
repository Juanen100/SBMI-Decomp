public class SingleSound : BaseSoundIndex
{
	private TFSound sound;

	public SingleSound(string key, int maxInstances, string filename, string characterName)
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
