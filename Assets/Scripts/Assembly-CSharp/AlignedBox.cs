#define ASSERTS_ON
using UnityEngine;

public class AlignedBox
{
	public float xmin;

	public float xmax;

	public float ymin;

	public float ymax;

	private static Vector2[] point = new Vector2[4]
	{
		Vector2.zero,
		Vector2.zero,
		Vector2.zero,
		Vector2.zero
	};

	public float Width
	{
		get
		{
			return xmax - xmin;
		}
	}

	public float Height
	{
		get
		{
			return ymax - ymin;
		}
	}

	public AlignedBox()
	{
		xmin = 0f;
		xmax = 0f;
		ymin = 0f;
		ymax = 0f;
	}

	public AlignedBox(float xmin, float xmax, float ymin, float ymax)
	{
		TFUtils.Assert(xmin <= xmax, "AlignedBox cannot have xmin > xmax");
		TFUtils.Assert(ymin <= ymax, "AlignedBox cannot have ymin > ymax");
		this.xmin = xmin;
		this.xmax = xmax;
		this.ymin = ymin;
		this.ymax = ymax;
	}

	public string Describe()
	{
		return string.Format("[{0}, {1}, {2}, {3}]", xmin, xmax, ymin, ymax);
	}

	public override string ToString()
	{
		return "AlignedBox: " + Describe();
	}

	public static bool Intersects(AlignedBox lhs, Segment rhs)
	{
		point[0].Set(lhs.xmin, lhs.ymin);
		point[1].Set(lhs.xmin, lhs.ymax);
		point[2].Set(lhs.xmax, lhs.ymax);
		point[3].Set(lhs.xmax, lhs.ymin);
		Vector2 r = rhs.second - rhs.first;
		if (Left(r, point[0] - rhs.first) && Left(r, point[1] - rhs.first) && Left(r, point[2] - rhs.first) && Left(r, point[3] - rhs.first))
		{
			return false;
		}
		r = rhs.first - rhs.second;
		if (Left(r, point[0] - rhs.second) && Left(r, point[1] - rhs.second) && Left(r, point[2] - rhs.second) && Left(r, point[3] - rhs.second))
		{
			return false;
		}
		r = point[1] - point[0];
		if (Left(r, rhs.first - point[0]) && Left(r, rhs.second - point[0]))
		{
			return false;
		}
		r = point[2] - point[1];
		if (Left(r, rhs.first - point[1]) && Left(r, rhs.second - point[1]))
		{
			return false;
		}
		r = point[3] - point[2];
		if (Left(r, rhs.first - point[2]) && Left(r, rhs.second - point[2]))
		{
			return false;
		}
		r = point[0] - point[3];
		if (Left(r, rhs.first - point[3]) && Left(r, rhs.second - point[3]))
		{
			return false;
		}
		return true;
	}

	public static bool Intersects(AlignedBox lhs, AlignedBox rhs)
	{
		if (rhs.xmax <= lhs.xmin || lhs.xmax <= rhs.xmin)
		{
			return false;
		}
		if (rhs.ymax <= lhs.ymin || lhs.ymax <= rhs.ymin)
		{
			return false;
		}
		return true;
	}

	public static bool Contains(AlignedBox lhs, AlignedBox rhs)
	{
		if (rhs.xmin <= lhs.xmin || lhs.xmax <= rhs.xmax)
		{
			return false;
		}
		if (rhs.ymin <= lhs.ymin || lhs.ymax <= rhs.ymax)
		{
			return false;
		}
		return true;
	}

	public bool Contains(float x, float y)
	{
		return x >= xmin && x <= xmax && y >= ymin && y <= ymax;
	}

	public static AlignedBox Union(AlignedBox lhs, AlignedBox rhs)
	{
		return new AlignedBox((!(lhs.xmin < rhs.xmin)) ? rhs.xmin : lhs.xmin, (!(lhs.xmax > rhs.xmax)) ? rhs.xmax : lhs.xmax, (!(lhs.ymin < rhs.ymin)) ? rhs.ymin : lhs.ymin, (!(lhs.ymax > rhs.ymax)) ? rhs.ymax : lhs.ymax);
	}

	private static bool Left(Vector2 r, Vector2 q)
	{
		return r.x * q.y - q.x * r.y >= 0f;
	}

	public AlignedBox OffsetByVector(Vector2 offset)
	{
		return new AlignedBox(xmin + offset.x, xmax + offset.x, ymin + offset.y, ymax + offset.y);
	}
}
