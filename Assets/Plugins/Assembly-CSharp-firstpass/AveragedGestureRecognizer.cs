using UnityEngine;

public abstract class AveragedGestureRecognizer : GestureRecognizer
{
	public int RequiredFingerCount = 1;

	private Vector2 startPos = Vector2.zero;

	private Vector2 pos = Vector2.zero;

	public Vector2 StartPosition
	{
		get
		{
			return startPos;
		}
		protected set
		{
			startPos = value;
		}
	}

	public Vector2 Position
	{
		get
		{
			return pos;
		}
		protected set
		{
			pos = value;
		}
	}

	protected override int GetRequiredFingerCount()
	{
		return RequiredFingerCount;
	}
}
