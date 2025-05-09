using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/FingerUp")]
public class TBFingerUp : TBComponent
{
	public Message message = new Message("OnFingerUp");

	private float timeHeldDown;

	public float TimeHeldDown
	{
		get
		{
			return timeHeldDown;
		}
		private set
		{
			timeHeldDown = value;
		}
	}

	public event EventHandler<TBFingerUp> OnFingerUp;

	public bool RaiseFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		TimeHeldDown = timeHeldDown;
		if (this.OnFingerUp != null)
		{
			this.OnFingerUp(this);
		}
		Send(message);
		return true;
	}
}
