using UnityEngine;

public abstract class MultiFingerGestureRecognizer : GestureRecognizer
{
	private Vector2[] pos;

	private Vector2[] startPos;

	protected Vector2[] StartPosition
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	protected Vector2[] Position
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int RequiredFingerCount
	{
		get
		{
			return 0;
		}
	}

	protected override void Start()
	{
	}

	protected void OnFingerCountChanged(int fingerCount)
	{
	}

	public Vector2 GetPosition(int index)
	{
		return default(Vector2);
	}

	public Vector2 GetStartPosition(int index)
	{
		return default(Vector2);
	}
}
