using UnityEngine;

public class TBFingerUp : TBComponent
{
	public Message message;

	private float timeHeldDown;

	public float TimeHeldDown
	{
		get
		{
			return 0f;
		}
		private set
		{
		}
	}

	public event EventHandler<TBFingerUp> OnFingerUp
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool RaiseFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		return false;
	}
}
