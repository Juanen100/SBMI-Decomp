using UnityEngine;

public class TouchScreenGestures : FingerGestures
{
	public int maxFingers = 5;

	private Touch nullTouch = default(Touch);

	private int[] finger2touchMap;

	public override int MaxFingers
	{
		get
		{
			return maxFingers;
		}
	}

	protected override void Start()
	{
		finger2touchMap = new int[MaxFingers];
		base.Start();
	}

	protected override FingerPhase GetPhase(Finger finger)
	{
		if (HasValidTouch(finger))
		{
			switch (GetTouch(finger).phase)
			{
			case TouchPhase.Began:
				return FingerPhase.Began;
			case TouchPhase.Moved:
				return FingerPhase.Moved;
			case TouchPhase.Stationary:
				return FingerPhase.Stationary;
			default:
				return FingerPhase.Ended;
			}
		}
		return FingerPhase.None;
	}

	protected override Vector2 GetPosition(Finger finger)
	{
		return GetTouch(finger).position;
	}

	private void UpdateFingerTouchMap()
	{
		for (int i = 0; i < finger2touchMap.Length; i++)
		{
			finger2touchMap[i] = -1;
		}
		for (int j = 0; j < Input.touchCount; j++)
		{
			int fingerId = Input.touches[j].fingerId;
			if (fingerId < finger2touchMap.Length)
			{
				finger2touchMap[fingerId] = j;
			}
		}
	}

	private bool HasValidTouch(Finger finger)
	{
		return finger2touchMap[finger.Index] != -1;
	}

	private Touch GetTouch(Finger finger)
	{
		int num = finger2touchMap[finger.Index];
		if (num == -1)
		{
			return nullTouch;
		}
		return Input.touches[num];
	}

	protected override void Update()
	{
		UpdateFingerTouchMap();
		base.Update();
	}
}
