using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Swipe")]
public class SwipeGestureRecognizer : AveragedGestureRecognizer
{
	public FingerGestures.SwipeDirection ValidDirections = FingerGestures.SwipeDirection.All;

	public float MinDistance = 1f;

	public float MinVelocity = 1f;

	public float DirectionTolerance = 0.2f;

	private Vector2 move;

	private FingerGestures.SwipeDirection direction;

	private float velocity;

	public Vector2 Move
	{
		get
		{
			return move;
		}
		private set
		{
			move = value;
		}
	}

	public FingerGestures.SwipeDirection Direction
	{
		get
		{
			return direction;
		}
	}

	public float Velocity
	{
		get
		{
			return velocity;
		}
	}

	public event EventDelegate<SwipeGestureRecognizer> OnSwipe;

	public bool IsValidDirection(FingerGestures.SwipeDirection dir)
	{
		if (dir == FingerGestures.SwipeDirection.None)
		{
			return false;
		}
		return (ValidDirections & dir) == dir;
	}

	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		if (!base.CanBegin(touches))
		{
			return false;
		}
		if (touches.GetAverageDistanceFromStart() < 0.5f)
		{
			return false;
		}
		return true;
	}

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
		direction = FingerGestures.SwipeDirection.None;
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != RequiredFingerCount)
		{
			if (touches.Count < RequiredFingerCount && direction != FingerGestures.SwipeDirection.None)
			{
				if (this.OnSwipe != null)
				{
					this.OnSwipe(this);
				}
				return GestureState.Recognized;
			}
			return GestureState.Failed;
		}
		base.Position = touches.GetAveragePosition();
		Move = base.Position - base.StartPosition;
		float magnitude = Move.magnitude;
		if (magnitude < MinDistance)
		{
			return GestureState.InProgress;
		}
		if (base.ElapsedTime > 0f)
		{
			velocity = magnitude / base.ElapsedTime;
		}
		else
		{
			velocity = 0f;
		}
		if (velocity < MinVelocity)
		{
			return GestureState.Failed;
		}
		FingerGestures.SwipeDirection swipeDirection = FingerGestures.GetSwipeDirection(Move.normalized, DirectionTolerance);
		if (!IsValidDirection(swipeDirection) || (direction != FingerGestures.SwipeDirection.None && swipeDirection != direction))
		{
			return GestureState.Failed;
		}
		direction = swipeDirection;
		return GestureState.InProgress;
	}
}
