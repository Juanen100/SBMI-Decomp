using UnityEngine;

public class MouseGestures : FingerGestures
{
	public int maxMouseButtons;

	public override int MaxFingers
	{
		get
		{
			return 0;
		}
	}

	protected override void Start()
	{
	}

	protected override FingerPhase GetPhase(Finger finger)
	{
		return default(FingerPhase);
	}

	protected override Vector2 GetPosition(Finger finger)
	{
		return default(Vector2);
	}
}
