using UnityEngine;

public class PinchGestureRecognizer : MultiFingerGestureRecognizer
{
	public float MinDOT;

	public float MinDistance;

	public float DeltaScale;

	protected float delta;

	public float Delta
	{
		get
		{
			return 0f;
		}
	}

	public event EventDelegate<PinchGestureRecognizer> OnPinchBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<PinchGestureRecognizer> OnPinchMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<PinchGestureRecognizer> OnPinchEnd
	{
		add
		{
		}
		remove
		{
		}
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

	protected void RaiseOnPinchBegin()
	{
	}

	protected void RaiseOnPinchMove()
	{
	}

	protected void RaiseOnPinchEnd()
	{
	}

	private bool FingersMovedInOppositeDirections(FingerGestures.Finger finger0, FingerGestures.Finger finger1)
	{
		return false;
	}

	private float ComputeGapDelta(FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos1, Vector2 refPos2)
	{
		return 0f;
	}
}
