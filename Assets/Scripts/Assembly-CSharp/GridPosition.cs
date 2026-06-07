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
			return 0;
		}
		set
		{
		}
	}

	public int Y
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public GridPosition(int row, int col)
	{
	}

	public static GridPosition operator +(GridPosition lhs, GridPosition rhs)
	{
		return null;
	}

	public static GridPosition operator -(GridPosition lhs, GridPosition rhs)
	{
		return null;
	}

	public static bool operator ==(GridPosition a, GridPosition b)
	{
		return false;
	}

	public static bool operator !=(GridPosition a, GridPosition b)
	{
		return false;
	}

	public bool Within(GridPosition min, GridPosition max)
	{
		return false;
	}

	public override string ToString()
	{
		return null;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	public override bool Equals(object other)
	{
		return false;
	}

	public bool Equals(GridPosition other)
	{
		return false;
	}

	public void MakeValid(int maxRow, int maxCol)
	{
	}

	public Vector2 ToVector2()
	{
		return default(Vector2);
	}
}
