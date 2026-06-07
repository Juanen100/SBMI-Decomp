namespace DeltaDNA
{
	public abstract class EventActionHandler
	{
		internal abstract bool Handle(EventTrigger trigger, ActionStore store);

		internal abstract string Type();
	}
}
