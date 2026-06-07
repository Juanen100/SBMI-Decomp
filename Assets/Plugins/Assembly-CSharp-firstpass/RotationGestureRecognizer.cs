using UnityEngine;

public class RotationGestureRecognizer : MultiFingerGestureRecognizer
{
	public float MinDOT;

	public float MinRotation;

	private float totalRotation;

	private float rotationDelta;

	public float TotalRotation
	{
		get
		{
			return 0f;
		}
	}

	public float RotationDelta
	{
		get
		{
			return 0f;
		}
	}

	public event EventDelegate<RotationGestureRecognizer> OnRotationBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<RotationGestureRecognizer> OnRotationMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<RotationGestureRecognizer> OnRotationEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	private bool FingersMovedInOppositeDirections(FingerGestures.Finger finger0, FingerGestures.Finger finger1)
	{
		return false;
	}

	private static float SignedAngularGap(FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos0, Vector2 refPos1)
	{
		return 0f;
	}

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
