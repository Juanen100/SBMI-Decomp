public class UiSpawnMixin
{
	private SessionActionTracker parentAction;

	private SBGUIScreen containingScreen;

	public void OnRegisterNewInstance(SessionActionTracker parentAction, SBGUIScreen containingScreen)
	{
		this.parentAction = parentAction;
		this.containingScreen = containingScreen;
		this.containingScreen.OnPutIntoCache.AddListener(FailOnStash);
	}

	public void Destroy()
	{
		containingScreen.OnPutIntoCache.RemoveListener(FailOnStash);
	}

	private void FailOnStash()
	{
		if (parentAction.Status == SessionActionTracker.StatusCode.STARTED)
		{
			parentAction.MarkFailed();
		}
	}
}
