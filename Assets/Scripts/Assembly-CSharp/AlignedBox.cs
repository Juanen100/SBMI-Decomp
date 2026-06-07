using UnityEngine;

public class AlignedBox
{
	public float xmin;

	public float xmax;

	public float ymin;

	public float ymax;

	private static Vector2[] point;

	public float Width
	{
		get
		{
			return 0f;
		}
	}

	public float Height
	{
		get
		{
			return 0f;
		}
	}

	public AlignedBox()
	{
	}

	public AlignedBox(float xmin, float xmax, float ymin, float ymax)
	{
	}

	public string Describe()
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}

	public static bool Intersects(AlignedBox lhs, Segment rhs)
	{
		return false;
	}

	public static bool Intersects(AlignedBox lhs, AlignedBox rhs)
	{
		return false;
	}

	public static bool Contains(AlignedBox lhs, AlignedBox rhs)
	{
		return false;
	}

	public bool Contains(float x, float y)
	{
		return false;
	}

	public static AlignedBox Union(AlignedBox lhs, AlignedBox rhs)
	{
		return null;
	}

	private static bool Left(Vector2 r, Vector2 q)
	{
		return false;
	}

	public AlignedBox OffsetByVector(Vector2 offset)
	{
		return null;
	}
}
