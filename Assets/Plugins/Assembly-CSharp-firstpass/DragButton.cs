using Yarg;

public class DragButton : BaseButton
{
	public EventDispatcher<YGEvent> DragEvent;

	private void SendDrag(YGEvent evt)
	{
	}

	protected override bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}
}
