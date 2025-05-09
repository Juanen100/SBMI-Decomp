public class SoaringEvents : SoaringDelegate
{
	public SoaringArray mBannerAdEvents;

	public bool mBannerAdEventActive;

	public SoaringEvents()
	{
		mBannerAdEvents = new SoaringArray();
	}

	public void LoadEvents(SoaringArray events)
	{
		if (events == null)
		{
			return;
		}
		int num = events.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDictionary soaringDictionary = (SoaringDictionary)events.objectAtIndex(i);
			if (soaringDictionary != null)
			{
				SoaringEvent soaringEvent = new SoaringEvent(soaringDictionary);
				if (soaringEvent.HasDisplayBannerEvent() && soaringEvent.Requires == null)
				{
					mBannerAdEvents.addObject(soaringEvent);
				}
				else
				{
					Soaring.Delegate.OnRecievedEvent(this, soaringEvent);
				}
			}
		}
		HandleBannerAdEvent();
	}

	public bool AddBannerEvent(SoaringEvent ev)
	{
		if (ev == null)
		{
			return false;
		}
		if (!ev.HasDisplayBannerEvent())
		{
			return false;
		}
		mBannerAdEvents.addObject(ev);
		HandleBannerAdEvent();
		return true;
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
		if (!mBannerAdEventActive && mBannerAdEvents != null && mBannerAdEvents.count() != 0)
		{
			SoaringEvent soaringEvent = (SoaringEvent)mBannerAdEvents.objectAtIndex(0);
			mBannerAdEvents.removeObjectAtIndex(0);
			SoaringEvent.SoaringEventAction action = null;
			soaringEvent.HasDisplayBannerEvent(ref action);
			if (action == null)
			{
				SoaringDebug.Log(soaringEvent.Name + " : No Banner Action Returned");
				Soaring.Delegate.OnRecievedEvent(this, soaringEvent);
				HandleBannerAdEvent();
				return;
			}
			if (string.IsNullOrEmpty(action.Value))
			{
				SoaringDebug.Log(soaringEvent.Name + " : No Banner Name");
				Soaring.Delegate.OnRecievedEvent(this, soaringEvent);
				HandleBannerAdEvent();
				return;
			}
			SoaringContext soaringContext = new SoaringContext();
			soaringContext.Responder = this;
			soaringContext.addValue(soaringEvent, "event");
			soaringContext.addValue(action.Value, "event_banner");
			mBannerAdEventActive = true;
			Soaring.RequestSoaringAdvert(action.Value, false, soaringContext);
		}
	}

	public override void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
		if (context == null)
		{
			if (mBannerAdEventActive)
			{
				SoaringDebug.Log("No Banner Context");
				Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
				mBannerAdEventActive = false;
				HandleBannerAdEvent();
			}
			return;
		}
		switch (state)
		{
		case SoaringAdServerState.Failed:
			if (mBannerAdEventActive)
			{
				SoaringDebug.Log("Banner Failed To Be Retrieved");
				mBannerAdEventActive = false;
				HandleBannerAdEvent();
			}
			break;
		case SoaringAdServerState.Retrieved:
		{
			string text = context.soaringValue("event_banner");
			if (string.IsNullOrEmpty(text))
			{
				Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
				mBannerAdEventActive = false;
				HandleBannerAdEvent();
			}
			else if (!Soaring.SoaringDisplayAdvert(text))
			{
				SoaringDebug.Log("No Banner To Display");
				Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
				mBannerAdEventActive = false;
				HandleBannerAdEvent();
			}
			break;
		}
		case SoaringAdServerState.Closed:
		case SoaringAdServerState.Clicked:
			Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
			mBannerAdEventActive = false;
			HandleBannerAdEvent();
			break;
		}
	}
}
