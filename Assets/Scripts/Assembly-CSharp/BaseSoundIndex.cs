public abstract class BaseSoundIndex : ISoundIndex
{
	private string key;

	private int maxInstances;

	public string Key
	{
		get
		{
			return null;
		}
	}

	public int MaxInstances
	{
		get
		{
			return 0;
		}
	}

	public BaseSoundIndex(string key, int maxInstances)
	{
	}

	public abstract TFSound GetNextSound(SoundEffectManager sfxMgr);

	public abstract void Clear();

	public abstract void Init();
}
