using Yarg;

public class DragButton : BaseButton
{
	public EventDispatcher<YGEvent> DragEvent = new EventDispatcher<YGEvent>();

	private void SendDrag(YGEvent evt)
	{
		DragEvent.FireEvent(evt);
	}

	protected override bool TouchEventHandler(YGEvent evt)
	{
		switch (evt.type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
		case YGEvent.TYPE.TOUCH_MOVE:
			SendDrag(evt);
			return true;
		case YGEvent.TYPE.RESET:
			return true;
		default:
			return false;
		}
	}
}
