using UnityEngine;

public class TBSwipe : TBComponent
{
	public bool swipeLeft;

	public bool swipeRight;

	public bool swipeUp;

	public bool swipeDown;

	public float minVelocity;

	public Message swipeMessage;

	public Message swipeLeftMessage;

	public Message swipeRightMessage;

	public Message swipeUpMessage;

	public Message swipeDownMessage;

	private FingerGestures.SwipeDirection direction;

	private float velocity;

	public FingerGestures.SwipeDirection Direction
	{
		get
		{
			return default(FingerGestures.SwipeDirection);
		}
		protected set
		{
		}
	}

	public float Velocity
	{
		get
		{
			return 0f;
		}
		protected set
		{
		}
	}

	public event EventHandler<TBSwipe> OnSwipe
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool IsValid(FingerGestures.SwipeDirection direction)
	{
		return false;
	}

	private Message GetMessageForSwipeDirection(FingerGestures.SwipeDirection direction)
	{
		return null;
	}

	public bool RaiseSwipe(int fingerIndex, Vector2 fingerPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		return false;
	}
}
