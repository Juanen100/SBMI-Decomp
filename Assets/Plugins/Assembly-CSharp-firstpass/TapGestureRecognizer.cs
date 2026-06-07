public class TapGestureRecognizer : AveragedGestureRecognizer
{
	public float MoveTolerance;

	public float MaxDuration;

	public event EventDelegate<TapGestureRecognizer> OnTap
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

	protected void RaiseOnTap()
	{
	}
}
