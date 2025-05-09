public class SingleSound : BaseSoundIndex
{
	private TFSound sound;

	public SingleSound(string key, int maxInstances, string filename, string characterName)
		: base(key, maxInstances)
	{
		sound = new TFSound(filename, characterName);
	}

	public override TFSound GetNextSound(SoundEffectManager sfxMgr)
	{
		return sound;
	}

	public override void Clear()
	{
		sound.Clear();
	}

	public override void Init()
	{
		sound.Init();
	}
}
