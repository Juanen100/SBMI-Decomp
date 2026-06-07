public class MousePinchGestureRecognizer : PinchGestureRecognizer
{
	public string axis;

	private int requiredFingers;

	private float resetTime;

	protected override int GetRequiredFingerCount()
	{
		return 0;
	}

	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		return false;
	}

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		return default(GestureState);
	}
}
