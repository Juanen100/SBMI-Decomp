public interface ISoundIndex
{
	string Key { get; }

	TFSound GetNextSound(SoundEffectManager sfxMgr);

	void Clear();

	void Init();
}
