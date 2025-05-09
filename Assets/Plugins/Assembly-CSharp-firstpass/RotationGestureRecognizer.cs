using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Rotation")]
public class RotationGestureRecognizer : MultiFingerGestureRecognizer
{
	public float MinDOT = -0.7f;

	public float MinRotation = 1f;

	private float totalRotation;

	private float rotationDelta;

	public float TotalRotation
	{
		get
		{
			return totalRotation;
		}
	}

	public float RotationDelta
	{
		get
		{
			return rotationDelta;
		}
	}

	public event EventDelegate<RotationGestureRecognizer> OnRotationBegin;

	public event EventDelegate<RotationGestureRecognizer> OnRotationMove;

	public event EventDelegate<RotationGestureRecognizer> OnRotationEnd;

	private bool FingersMovedInOppositeDirections(FingerGestures.Finger finger0, FingerGestures.Finger finger1)
	{
		return FingerGestures.FingersMovedInOppositeDirections(finger0, finger1, MinDOT);
	}

	private static float SignedAngularGap(FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos0, Vector2 refPos1)
	{
		Vector2 normalized = (finger0.Position - finger1.Position).normalized;
		Vector2 normalized2 = (refPos0 - refPos1).normalized;
		return 57.29578f * FingerGestures.SignedAngle(normalized2, normalized);
	}

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
		float f = SignedAngularGap(finger, finger2, finger.StartPosition, finger2.StartPosition);
		if (Mathf.Abs(f) < MinRotation)
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
		float num = SignedAngularGap(finger, finger2, finger.StartPosition, finger2.StartPosition);
		totalRotation = Mathf.Sign(num) * MinRotation;
		rotationDelta = 0f;
		if (this.OnRotationBegin != null)
		{
			this.OnRotationBegin(this);
		}
		rotationDelta = num - totalRotation;
		totalRotation = num;
		if (this.OnRotationMove != null)
		{
			this.OnRotationMove(this);
		}
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != base.RequiredFingerCount)
		{
			if (touches.Count < base.RequiredFingerCount)
			{
				if (this.OnRotationEnd != null)
				{
					this.OnRotationEnd(this);
				}
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
		rotationDelta = SignedAngularGap(finger, finger2, finger.PreviousPosition, finger2.PreviousPosition);
		totalRotation += rotationDelta;
		if (this.OnRotationMove != null)
		{
			this.OnRotationMove(this);
		}
		return GestureState.InProgress;
	}
}
