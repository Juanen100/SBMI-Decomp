using UnityEngine;

public class SwipeGestureRecognizer : AveragedGestureRecognizer
{
	public FingerGestures.SwipeDirection ValidDirections;

	public float MinDistance;

	public float MinVelocity;

	public float DirectionTolerance;

	private Vector2 move;

	private FingerGestures.SwipeDirection direction;

	private float velocity;

	public Vector2 Move
	{
		get
		{
			return default(Vector2);
		}
		private set
		{
		}
	}

	public FingerGestures.SwipeDirection Direction
	{
		get
		{
			return default(FingerGestures.SwipeDirection);
		}
	}

	public float Velocity
	{
		get
		{
			return 0f;
		}
	}

	public event EventDelegate<SwipeGestureRecognizer> OnSwipe
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool IsValidDirection(FingerGestures.SwipeDirection dir)
	{
		return false;
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
