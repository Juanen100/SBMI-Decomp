public class LongPressGestureRecognizer : AveragedGestureRecognizer
{
	public float Duration;

	public float MoveTolerance;

	public event EventDelegate<LongPressGestureRecognizer> OnLongPress
	{
		add
		{
		}
		remove
		{
		}
	}

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		return default(GestureState);
	}

	protected void RaiseOnLongPress()
	{
	}
}
