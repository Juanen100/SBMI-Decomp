using UnityEngine;

public abstract class MultiFingerGestureRecognizer : GestureRecognizer
{
	private Vector2[] pos;

	private Vector2[] startPos;

	protected Vector2[] StartPosition
	{
		get
		{
			return startPos;
		}
		set
		{
			startPos = value;
		}
	}

	protected Vector2[] Position
	{
		get
		{
			return pos;
		}
		set
		{
			pos = value;
		}
	}

	public int RequiredFingerCount
	{
		get
		{
			return GetRequiredFingerCount();
		}
	}

	protected override void Start()
	{
		base.Start();
		OnFingerCountChanged(GetRequiredFingerCount());
	}

	protected void OnFingerCountChanged(int fingerCount)
	{
		StartPosition = new Vector2[fingerCount];
		Position = new Vector2[fingerCount];
	}

	public Vector2 GetPosition(int index)
	{
		return pos[index];
	}

	public Vector2 GetStartPosition(int index)
	{
		return startPos[index];
	}
}
