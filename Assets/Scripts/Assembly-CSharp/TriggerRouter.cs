using System.Collections.Generic;

public class TriggerRouter
{
	public const bool DEBUG_LOG_TRIGGERS = false;

	private List<ITriggerObserver> observers;

	public TriggerRouter(List<ITriggerObserver> observers)
	{
	}

	public void RouteTrigger(ITrigger trigger, Game game)
	{
	}
}
