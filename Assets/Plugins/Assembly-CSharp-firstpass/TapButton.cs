using Yarg;

public class TapButton : BaseButton
{
	public EventDispatcher TapEvent;

	public EventDispatcher BeginEvent;

	private float cdtime;

	private bool buttonRdy;

	private bool didBegin;

	protected override bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}

	private void ResetButtonCD()
	{
	}
}
