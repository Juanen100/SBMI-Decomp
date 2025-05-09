public abstract class BaseSoundIndex : ISoundIndex
{
	private string key;

	private int maxInstances;

	public string Key
	{
		get
		{
			return key;
		}
	}

	public int MaxInstances
	{
		get
		{
			return maxInstances;
		}
	}

	public BaseSoundIndex(string key, int maxInstances)
	{
		this.key = key;
		this.maxInstances = maxInstances;
	}

	public abstract TFSound GetNextSound(SoundEffectManager sfxMgr);

	public abstract void Clear();

	public abstract void Init();
}
