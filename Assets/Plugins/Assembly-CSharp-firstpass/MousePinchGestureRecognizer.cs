using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Mouse Pinch")]
public class MousePinchGestureRecognizer : PinchGestureRecognizer
{
	public string axis = "Mouse ScrollWheel";

	private int requiredFingers = 2;

	private float resetTime;

	protected override int GetRequiredFingerCount()
	{
		return requiredFingers;
	}

	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		if (!CheckCanBeginDelegate(touches))
		{
			return false;
		}
		float f = Input.GetAxis(axis);
		if (Mathf.Abs(f) < 0.0001f)
		{
			return false;
		}
		return true;
	}

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.StartPosition[0] = (base.StartPosition[1] = Input.mousePosition);
		base.Position[0] = (base.Position[1] = Input.mousePosition);
		delta = 0f;
		RaiseOnPinchBegin();
		delta = DeltaScale * Input.GetAxis(axis);
		resetTime = Time.time + 0.1f;
		RaiseOnPinchMove();
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		float num = Input.GetAxis(axis);
		if (Mathf.Abs(num) < 0.001f)
		{
			if (resetTime <= Time.time)
			{
				RaiseOnPinchEnd();
				return GestureState.Recognized;
			}
			return GestureState.InProgress;
		}
		resetTime = Time.time + 0.1f;
		base.Position[0] = (base.Position[1] = Input.mousePosition);
		delta = DeltaScale * num;
		RaiseOnPinchMove();
		return GestureState.InProgress;
	}
}
