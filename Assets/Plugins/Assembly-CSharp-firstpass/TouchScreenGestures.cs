using UnityEngine;

public class TouchScreenGestures : FingerGestures
{
	public int maxFingers;

	private Touch nullTouch;

	private int[] finger2touchMap;

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

	private void UpdateFingerTouchMap()
	{
	}

	private bool HasValidTouch(Finger finger)
	{
		return false;
	}

	private Touch GetTouch(Finger finger)
	{
		return default(Touch);
	}

	protected override void Update()
	{
	}
}
