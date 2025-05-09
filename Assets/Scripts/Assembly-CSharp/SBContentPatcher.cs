using UnityEngine;

public class SBContentPatcher : EventDispatcher<string>
{
	private class SoaringVersionsDelegate : SoaringDelegate
	{
		public SBContentPatcher patcher;

		public override void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
		{
			if (patcher == null || data != null)
			{
				return;
			}
			switch (state)
			{
			case SoaringState.Success:
				patcher.FireEvent("patchingDone");
				patcher.RemoveDelegate();
				patcher = null;
				break;
			case SoaringState.Fail:
				if (error == null)
				{
					patcher.FireEvent("patchingDone");
					patcher.RemoveDelegate();
					patcher = null;
				}
				else if (error.ErrorCode == 33)
				{
					SoaringDebug.Log(error, LogType.Error);
					patcher.FireEvent("patchingNotNecessary");
					patcher.RemoveDelegate();
					patcher = null;
				}
				else
				{
					SoaringDebug.Log("SBContentPatcher: " + error, LogType.Error);
					patcher.FireEvent("patchingDone");
					patcher.RemoveDelegate();
					SoaringInternal.instance.TriggerOfflineMode(true);
					patcher = null;
				}
				break;
			case SoaringState.Update:
				if (error == null)
				{
					SoaringDebug.Log("SBContentPatcher: Need Patching", LogType.Error);
					patcher.FireEvent("patchingNecessary");
				}
				break;
			}
		}
	}

	public const string PATCHING_DONE_EVENT = "patchingDone";

	public const string PATCHING_NECESSARY_EVENT = "patchingNecessary";

	public const string PATCHING_NOT_NECESSARY_EVENT = "patchingNotNecessary";

	private SoaringVersionsDelegate soaring_delegate;

	public SBContentPatcher()
	{
		soaring_delegate = new SoaringVersionsDelegate();
		soaring_delegate.patcher = this;
		Soaring.AddDelegate(soaring_delegate);
	}

	~SBContentPatcher()
	{
	}

	public void RemoveDelegate()
	{
		if (soaring_delegate != null)
		{
			Soaring.RemoveDelegate(soaring_delegate);
		}
		soaring_delegate = null;
	}

	public void LoadManifest(bool checkForUpdates)
	{
		if (SBSettings.BypassPatching)
		{
			SoaringDebug.Log("Bypass Patching");
			FireEvent("patchingNotNecessary");
		}
		else
		{
			SoaringDebug.Log("Checking for Updates");
			Soaring.CheckFilesForUpdates(checkForUpdates);
		}
	}
}
