using System.Collections.Generic;

public class Path<Position>
{
	private class PathNode
	{
		public PathNode next;

		public Position position;

		public PathNode(Position position)
		{
			this.position = position;
		}
	}

	private PathNode head;

	private PathNode current;

	public Position Current
	{
		get
		{
			return current.position;
		}
	}

	public void Add(Position position)
	{
		PathNode pathNode = new PathNode(position);
		pathNode.next = head;
		head = pathNode;
	}

	public void Begin()
	{
		current = head;
	}

	public bool Next()
	{
		current = current.next;
		return Done();
	}

	public bool Done()
	{
		return null == current;
	}

	public IEnumerator<Position> GetEnumerator()
	{
		for (PathNode node = head; node != null; node = node.next)
		{
			yield return node.position;
		}
	}
}
