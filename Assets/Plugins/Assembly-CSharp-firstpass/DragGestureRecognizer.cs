using UnityEngine;

public class DragGestureRecognizer : AveragedGestureRecognizer
{
	public float MoveTolerance;

	private Vector2 delta;

	private Vector2 lastPos;

	public Vector2 MoveDelta
	{
		get
		{
			return default(Vector2);
		}
		private set
		{
		}
	}

	public event EventDelegate<DragGestureRecognizer> OnDragBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<DragGestureRecognizer> OnDragMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<DragGestureRecognizer> OnDragStationary
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<DragGestureRecognizer> OnDragEnd
	{
		add
		{
		}
		remove
		{
		}
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

	protected void RaiseOnDragBegin()
	{
	}

	protected void RaiseOnDragMove()
	{
	}

	protected void RaiseOnDragStationary()
	{
	}

	protected void RaiseOnDragEnd()
	{
	}
}
