public class SBGUIScrollListElement : SBGUIElement
{
	public EventDispatcher VisibleEvent;

	public EventDispatcher InvisibleEvent;

	protected virtual void OnBecameVisible()
	{
	}

	protected virtual void OnBecameInvisible()
	{
	}

	public virtual void Deactivate()
	{
	}
}
