using UnityEngine;
using Yarg;

public class TwoShadeButton : BaseButton
{
	private enum BUTTON_STATE
	{
		IDLE = 0,
		ACTIVATE = 1
	}

	public Color idle = new Color(1f, 1f, 1f, 0.5f);

	public Color activate = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	private BUTTON_STATE buttonState;

	protected override bool TouchEventHandler(YGEvent evt)
	{
		switch (evt.type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			if (buttonState != BUTTON_STATE.ACTIVATE)
			{
				buttonState = BUTTON_STATE.ACTIVATE;
				base.parent.SetColor(activate);
			}
			ResetHighlightState();
			return true;
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
		case YGEvent.TYPE.RESET:
			ResetHighlightState();
			return true;
		default:
			return false;
		}
	}

	public void ResetHighlightState()
	{
		if (buttonState != BUTTON_STATE.IDLE)
		{
			buttonState = BUTTON_STATE.IDLE;
		}
		base.parent.SetColor(idle);
	}

	protected override void OnDisable()
	{
		if (base.parent != null)
		{
			ResetHighlightState();
		}
		base.OnDisable();
	}
}
