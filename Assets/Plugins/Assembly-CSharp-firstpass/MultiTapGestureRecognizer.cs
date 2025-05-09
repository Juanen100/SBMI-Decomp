using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Tap (Multi)")]
public class MultiTapGestureRecognizer : AveragedGestureRecognizer
{
	public int RequiredTaps;

	public bool RaiseEventOnEachTap;

	public float MaxDelayBetweenTaps = 0.25f;

	public float MaxDuration;

	public float MoveTolerance = 5f;

	private int taps;

	private bool down;

	private bool wasDown;

	private float lastDownTime;

	private float lastTapTime;

	public int Taps
	{
		get
		{
			return taps;
		}
	}

	public event EventDelegate<MultiTapGestureRecognizer> OnTap;

	private bool HasTimedOut()
	{
		if (MaxDelayBetweenTaps > 0f && Time.time - lastTapTime > MaxDelayBetweenTaps)
		{
			return true;
		}
		if (MaxDuration > 0f && base.ElapsedTime > MaxDuration)
		{
			return true;
		}
		return false;
	}

	protected override void Reset()
	{
		taps = 0;
		down = false;
		wasDown = false;
		base.Reset();
	}

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
		lastTapTime = Time.time;
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		wasDown = down;
		down = false;
		if (touches.Count == RequiredFingerCount)
		{
			down = true;
			lastDownTime = Time.time;
		}
		else if (touches.Count == 0)
		{
			down = false;
		}
		else if (touches.Count < RequiredFingerCount)
		{
			if (Time.time - lastDownTime > 0.25f)
			{
				return GestureState.Failed;
			}
		}
		else if (!Young(touches))
		{
			return GestureState.Failed;
		}
		if (HasTimedOut())
		{
			if (RequiredTaps == 0 && Taps > 0)
			{
				if (!RaiseEventOnEachTap)
				{
					RaiseOnTap();
				}
				return GestureState.Recognized;
			}
			return GestureState.Failed;
		}
		if (down)
		{
			float num = Vector3.SqrMagnitude(touches.GetAveragePosition() - base.StartPosition);
			if (num >= MoveTolerance * MoveTolerance)
			{
				return GestureState.Failed;
			}
		}
		if (wasDown != down && !down)
		{
			taps++;
			lastTapTime = Time.time;
			if (RequiredTaps > 0 && taps >= RequiredTaps)
			{
				RaiseOnTap();
				return GestureState.Recognized;
			}
			if (RaiseEventOnEachTap)
			{
				RaiseOnTap();
			}
		}
		return GestureState.InProgress;
	}

	protected void RaiseOnTap()
	{
		if (this.OnTap != null)
		{
			this.OnTap(this);
		}
	}
}
