using UnityEngine;
using Yarg;

public class TwoShadeButton : BaseButton
{
	private enum BUTTON_STATE
	{
		IDLE = 0,
		ACTIVATE = 1
	}

	public Color idle;

	public Color activate;

	private BUTTON_STATE buttonState;

	protected override bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}

	public void ResetHighlightState()
	{
	}

	protected override void OnDisable()
	{
	}
}
