using System.Collections.Generic;
using System.Diagnostics;

public class Path<Position>
{
	private class PathNode
	{
		public PathNode next;

		public Position position;

		public PathNode(Position position)
		{
		}
	}

	private PathNode head;

	private PathNode current;

	public Position Current
	{
		get
		{
			return default(Position);
		}
	}

	public void Add(Position position)
	{
	}

	public void Begin()
	{
	}

	public bool Next()
	{
		return false;
	}

	public bool Done()
	{
		return false;
	}

	[DebuggerHidden]
	public IEnumerator<Position> GetEnumerator()
	{
		return null;
	}
}
