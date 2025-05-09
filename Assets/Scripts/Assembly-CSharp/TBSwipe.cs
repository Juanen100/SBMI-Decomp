using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Swipe")]
public class TBSwipe : TBComponent
{
	public bool swipeLeft = true;

	public bool swipeRight = true;

	public bool swipeUp = true;

	public bool swipeDown = true;

	public float minVelocity;

	public Message swipeMessage = new Message("OnSwipe");

	public Message swipeLeftMessage = new Message("OnSwipeLeft", false);

	public Message swipeRightMessage = new Message("OnSwipeRight", false);

	public Message swipeUpMessage = new Message("OnSwipeUp", false);

	public Message swipeDownMessage = new Message("OnSwipeDown", false);

	private FingerGestures.SwipeDirection direction;

	private float velocity;

	public FingerGestures.SwipeDirection Direction
	{
		get
		{
			return direction;
		}
		protected set
		{
			direction = value;
		}
	}

	public float Velocity
	{
		get
		{
			return velocity;
		}
		protected set
		{
			velocity = value;
		}
	}

	public event EventHandler<TBSwipe> OnSwipe;

	public bool IsValid(FingerGestures.SwipeDirection direction)
	{
		switch (direction)
		{
		case FingerGestures.SwipeDirection.Left:
			return swipeLeft;
		case FingerGestures.SwipeDirection.Right:
			return swipeRight;
		case FingerGestures.SwipeDirection.Up:
			return swipeUp;
		case FingerGestures.SwipeDirection.Down:
			return swipeDown;
		default:
			return false;
		}
	}

	private Message GetMessageForSwipeDirection(FingerGestures.SwipeDirection direction)
	{
		switch (direction)
		{
		case FingerGestures.SwipeDirection.Left:
			return swipeLeftMessage;
		case FingerGestures.SwipeDirection.Right:
			return swipeRightMessage;
		case FingerGestures.SwipeDirection.Up:
			return swipeUpMessage;
		default:
			return swipeDownMessage;
		}
	}

	public bool RaiseSwipe(int fingerIndex, Vector2 fingerPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if (velocity < minVelocity)
		{
			return false;
		}
		if (!IsValid(direction))
		{
			return false;
		}
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		Direction = direction;
		Velocity = velocity;
		if (this.OnSwipe != null)
		{
			this.OnSwipe(this);
		}
		Send(swipeMessage);
		Send(GetMessageForSwipeDirection(direction));
		return true;
	}
}
