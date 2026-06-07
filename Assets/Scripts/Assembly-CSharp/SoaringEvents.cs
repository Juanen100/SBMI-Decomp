public class SoaringEvents : SoaringDelegate
{
	public SoaringArray mBannerAdEvents;

	public bool mBannerAdEventActive;

	public void LoadEvents(SoaringArray events)
	{
	}

	public bool AddBannerEvent(SoaringEvent ev)
	{
		return false;
	}

	public bool HandleEventsHandled(SoaringEvent ev, bool handleActions = true)
	{
		return false;
	}

	public bool HandleEventsActionsHandled(SoaringEvent.SoaringEventAction ac)
	{
		return false;
	}

	protected void HandleBannerAdEvent()
	{
	}

	public override void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
	}
}
