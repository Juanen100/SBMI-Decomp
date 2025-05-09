using Yarg;

public class TapButton : BaseButton
{
	public EventDispatcher TapEvent = new EventDispatcher();

	public EventDispatcher BeginEvent = new EventDispatcher();

	private float cdtime = 0.1f;

	private bool buttonRdy = true;

	private bool didBegin;

	protected override bool TouchEventHandler(YGEvent evt)
	{
		if (base.enabled)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.TAP:
				didBegin = false;
				if (buttonRdy)
				{
					TapEvent.FireEvent();
					buttonRdy = false;
					Invoke("ResetButtonCD", cdtime);
				}
				return true;
			case YGEvent.TYPE.TOUCH_BEGIN:
				didBegin = true;
				BeginEvent.FireEvent();
				return true;
			case YGEvent.TYPE.TOUCH_END:
				if (didBegin)
				{
					didBegin = false;
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private void ResetButtonCD()
	{
		buttonRdy = true;
	}
}
