public class MultiTapGestureRecognizer : AveragedGestureRecognizer
{
	public int RequiredTaps;

	public bool RaiseEventOnEachTap;

	public float MaxDelayBetweenTaps;

	public float MaxDuration;

	public float MoveTolerance;

	private int taps;

	private bool down;

	private bool wasDown;

	private float lastDownTime;

	private float lastTapTime;

	public int Taps
	{
		get
		{
			return 0;
		}
	}

	public event EventDelegate<MultiTapGestureRecognizer> OnTap
	{
		add
		{
		}
		remove
		{
		}
	}

	private bool HasTimedOut()
	{
		return false;
	}

	protected override void Reset()
	{
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
