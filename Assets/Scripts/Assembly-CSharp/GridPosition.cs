using System;
using UnityEngine;

public class GridPosition : IEquatable<GridPosition>
{
	public int row;

	public int col;

	public int X
	{
		get
		{
			return col;
		}
		set
		{
			col = value;
		}
	}

	public int Y
	{
		get
		{
			return row;
		}
		set
		{
			row = value;
		}
	}

	public GridPosition(int row, int col)
	{
		this.row = row;
		this.col = col;
	}

	public bool Within(GridPosition min, GridPosition max)
	{
		return row >= min.row && row <= max.row && col >= min.col && col <= max.col;
	}

	public override string ToString()
	{
		return "(" + row + ", " + col + ")";
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = num * 31 + row;
		return num * 31 + col;
	}

	public override bool Equals(object other)
	{
		return other is GridPosition && Equals((GridPosition)other);
	}

	public bool Equals(GridPosition other)
	{
		return row == other.row && col == other.col;
	}

	public void MakeValid(int maxRow, int maxCol)
	{
		row = Mathf.Clamp(row, 0, maxRow);
		col = Mathf.Clamp(col, 0, maxCol);
	}

	public Vector2 ToVector2()
	{
		return new Vector2(X, Y);
	}

	public static GridPosition operator +(GridPosition lhs, GridPosition rhs)
	{
		return new GridPosition(lhs.row + rhs.row, lhs.col + rhs.col);
	}

	public static GridPosition operator -(GridPosition lhs, GridPosition rhs)
	{
		return new GridPosition(lhs.row - rhs.row, lhs.col - rhs.col);
	}

	public static bool operator ==(GridPosition a, GridPosition b)
	{
		if (object.ReferenceEquals(a, b))
		{
			return true;
		}
		if ((object)a == null || (object)b == null)
		{
			return false;
		}
		return a.row == b.row && a.col == b.col;
	}

	public static bool operator !=(GridPosition a, GridPosition b)
	{
		return !(a == b);
	}
}
