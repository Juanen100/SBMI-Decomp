using Yarg;

public class DownButton : TapButton
{
	private bool triggered;

	protected override bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}
}
