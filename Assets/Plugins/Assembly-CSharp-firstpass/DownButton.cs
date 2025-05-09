using Yarg;

public class DownButton : TapButton
{
	private bool triggered;

	protected override bool TouchEventHandler(YGEvent evt)
	{
		switch (evt.type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			if (!triggered)
			{
				TapEvent.FireEvent();
				triggered = true;
			}
			return true;
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
		case YGEvent.TYPE.RESET:
			triggered = false;
			return true;
		default:
			return false;
		}
	}
}
