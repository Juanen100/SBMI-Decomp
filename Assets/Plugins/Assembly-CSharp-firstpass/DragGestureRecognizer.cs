using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Drag")]
public class DragGestureRecognizer : AveragedGestureRecognizer
{
	public float MoveTolerance = 5f;

	private Vector2 delta = Vector2.zero;

	private Vector2 lastPos = Vector2.zero;

	public Vector2 MoveDelta
	{
		get
		{
			return delta;
		}
		private set
		{
			delta = value;
		}
	}

	public event EventDelegate<DragGestureRecognizer> OnDragBegin;

	public event EventDelegate<DragGestureRecognizer> OnDragMove;

	public event EventDelegate<DragGestureRecognizer> OnDragStationary;

	public event EventDelegate<DragGestureRecognizer> OnDragEnd;

	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		if (!base.CanBegin(touches))
		{
			return false;
		}
		if (touches.GetAverageDistanceFromStart() < MoveTolerance)
		{
			return false;
		}
		return true;
	}

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
		MoveDelta = Vector2.zero;
		lastPos = base.Position;
		RaiseOnDragBegin();
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != RequiredFingerCount)
		{
			if (touches.Count < RequiredFingerCount)
			{
				RaiseOnDragEnd();
				return GestureState.Recognized;
			}
			return GestureState.Failed;
		}
		base.Position = touches.GetAveragePosition();
		MoveDelta = base.Position - lastPos;
		if (MoveDelta.sqrMagnitude > 0f)
		{
			RaiseOnDragMove();
			lastPos = base.Position;
		}
		else
		{
			RaiseOnDragStationary();
		}
		return GestureState.InProgress;
	}

	protected void RaiseOnDragBegin()
	{
		if (this.OnDragBegin != null)
		{
			this.OnDragBegin(this);
		}
	}

	protected void RaiseOnDragMove()
	{
		if (this.OnDragMove != null)
		{
			this.OnDragMove(this);
		}
	}

	protected void RaiseOnDragStationary()
	{
		if (this.OnDragStationary != null)
		{
			this.OnDragStationary(this);
		}
	}

	protected void RaiseOnDragEnd()
	{
		if (this.OnDragEnd != null)
		{
			this.OnDragEnd(this);
		}
	}
}
