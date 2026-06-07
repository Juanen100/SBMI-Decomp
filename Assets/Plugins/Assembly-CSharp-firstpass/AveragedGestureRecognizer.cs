using UnityEngine;

public abstract class AveragedGestureRecognizer : GestureRecognizer
{
	public int RequiredFingerCount;

	private Vector2 startPos;

	private Vector2 pos;

	public Vector2 StartPosition
	{
		get
		{
			return default(Vector2);
		}
		protected set
		{
		}
	}

	public Vector2 Position
	{
		get
		{
			return default(Vector2);
		}
		protected set
		{
		}
	}

	protected override int GetRequiredFingerCount()
	{
		return 0;
	}
}
