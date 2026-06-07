public class SBContentPatcher : EventDispatcher<string>
{
	private class SoaringVersionsDelegate : SoaringDelegate
	{
		public SBContentPatcher patcher;

		public override void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
		{
		}
	}

	public const string PATCHING_DONE_EVENT = "patchingDone";

	public const string PATCHING_NECESSARY_EVENT = "patchingNecessary";

	public const string PATCHING_NOT_NECESSARY_EVENT = "patchingNotNecessary";

	private SoaringVersionsDelegate soaring_delegate;

	~SBContentPatcher()
	{
	}

	public void RemoveDelegate()
	{
	}

	public void LoadManifest(bool checkForUpdates)
	{
	}
}
