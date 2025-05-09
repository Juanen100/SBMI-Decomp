public class SBGUIScrollListElement : SBGUIElement
{
	public EventDispatcher VisibleEvent = new EventDispatcher();

	public EventDispatcher InvisibleEvent = new EventDispatcher();

	protected virtual void OnBecameVisible()
	{
		VisibleEvent.FireEvent();
		VisibleEvent.ClearListeners();
	}

	protected virtual void OnBecameInvisible()
	{
		InvisibleEvent.FireEvent();
	}

	public virtual void Deactivate()
	{
		VisibleEvent.ClearListeners();
		InvisibleEvent.ClearListeners();
		MuteButtons(false);
		SetParent(null);
		SetActive(false);
	}
}
