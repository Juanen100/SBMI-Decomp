using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Pinch")]
public class PinchGestureRecognizer : MultiFingerGestureRecognizer
{
	public float MinDOT = -0.7f;

	public float MinDistance = 5f;

	public float DeltaScale = 1f;

	protected float delta;

	public float Delta
	{
		get
		{
			return delta;
		}
	}

	public event EventDelegate<PinchGestureRecognizer> OnPinchBegin;

	public event EventDelegate<PinchGestureRecognizer> OnPinchMove;

	public event EventDelegate<PinchGestureRecognizer> OnPinchEnd;

	protected override int GetRequiredFingerCount()
	{
		return 2;
	}

	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		if (!base.CanBegin(touches))
		{
			return false;
		}
		FingerGestures.Finger finger = touches[0];
		FingerGestures.Finger finger2 = touches[1];
		if (!FingerGestures.AllFingersMoving(finger, finger2))
		{
			return false;
		}
		if (!FingersMovedInOppositeDirections(finger, finger2))
		{
			return false;
		}
		float f = ComputeGapDelta(finger, finger2, finger.StartPosition, finger2.StartPosition);
		if (Mathf.Abs(f) < MinDistance)
		{
			return false;
		}
		return true;
	}

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		FingerGestures.Finger finger = touches[0];
		FingerGestures.Finger finger2 = touches[1];
		base.StartPosition[0] = finger.StartPosition;
		base.StartPosition[1] = finger2.StartPosition;
		base.Position[0] = finger.Position;
		base.Position[1] = finger2.Position;
		RaiseOnPinchBegin();
		float num = ComputeGapDelta(finger, finger2, finger.StartPosition, finger2.StartPosition);
		delta = DeltaScale * (num - Mathf.Sign(num) * MinDistance);
		RaiseOnPinchMove();
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != base.RequiredFingerCount)
		{
			if (touches.Count < base.RequiredFingerCount)
			{
				RaiseOnPinchEnd();
				return GestureState.Recognized;
			}
			return GestureState.Failed;
		}
		FingerGestures.Finger finger = touches[0];
		FingerGestures.Finger finger2 = touches[1];
		base.Position[0] = finger.Position;
		base.Position[1] = finger2.Position;
		if (!FingerGestures.AllFingersMoving(finger, finger2))
		{
			return GestureState.InProgress;
		}
		float num = ComputeGapDelta(finger, finger2, finger.PreviousPosition, finger2.PreviousPosition);
		if (Mathf.Abs(num) > 0.001f)
		{
			if (!FingersMovedInOppositeDirections(finger, finger2))
			{
				return GestureState.InProgress;
			}
			delta = DeltaScale * num;
			RaiseOnPinchMove();
		}
		return GestureState.InProgress;
	}

	protected void RaiseOnPinchBegin()
	{
		if (this.OnPinchBegin != null)
		{
			this.OnPinchBegin(this);
		}
	}

	protected void RaiseOnPinchMove()
	{
		if (this.OnPinchMove != null)
		{
			this.OnPinchMove(this);
		}
	}

	protected void RaiseOnPinchEnd()
	{
		if (this.OnPinchEnd != null)
		{
			this.OnPinchEnd(this);
		}
	}

	private bool FingersMovedInOppositeDirections(FingerGestures.Finger finger0, FingerGestures.Finger finger1)
	{
		return FingerGestures.FingersMovedInOppositeDirections(finger0, finger1, MinDOT);
	}

	private float ComputeGapDelta(FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos1, Vector2 refPos2)
	{
		Vector2 vector = finger0.Position - finger1.Position;
		Vector2 vector2 = refPos1 - refPos2;
		return vector.magnitude - vector2.magnitude;
	}
}
