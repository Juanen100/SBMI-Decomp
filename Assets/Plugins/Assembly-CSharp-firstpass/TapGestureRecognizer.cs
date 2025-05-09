using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Tap")]
public class TapGestureRecognizer : AveragedGestureRecognizer
{
	public float MoveTolerance = 5f;

	public float MaxDuration;

	public event EventDelegate<TapGestureRecognizer> OnTap;

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != RequiredFingerCount)
		{
			if (touches.Count == 0)
			{
				RaiseOnTap();
				return GestureState.Recognized;
			}
			return GestureState.Failed;
		}
		if (MaxDuration > 0f && base.ElapsedTime > MaxDuration)
		{
			return GestureState.Failed;
		}
		float num = Vector3.SqrMagnitude(touches.GetAveragePosition() - base.StartPosition);
		if (num >= MoveTolerance * MoveTolerance)
		{
			return GestureState.Failed;
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
