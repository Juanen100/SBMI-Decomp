using System.Collections.Generic;
using UnityEngine;

public class Scene
{
	private class DistanceCompare : IComparer<Simulated>
	{
		private Vector2 point;

		public DistanceCompare(Vector2 point)
		{
		}

		public int Compare(Simulated lhs, Simulated rhs)
		{
			return 0;
		}
	}

	private class Node
	{
		public Node firstChild;

		public Node nextSibling;

		public AlignedBox box;

		public List<Simulated> entities;

		public List<Simulated> blockerEntities;

		public Node(AlignedBox box)
		{
		}

		public void AddChild(Node child)
		{
		}

		public void AddSibling(Node sibling)
		{
		}
	}

	private Terrain terrain;

	private int depth;

	private Node root;

	public Scene(Terrain terrain, int depth)
	{
	}

	public void OnUpdate(List<Simulated> simulateds)
	{
	}

	public void Add(Simulated entity)
	{
	}

	public void Remove(Simulated entity)
	{
	}

	public void Find(AlignedBox box, ref List<Simulated> result)
	{
	}

	public void FindPlacementBlockers(AlignedBox box, ref List<Simulated> result)
	{
	}

	public void Find(Ray ray, ref List<Simulated> result)
	{
	}

	private Node Filter(Simulated entity)
	{
		return null;
	}

	private Node Filter(Node node, Simulated simulated)
	{
		return null;
	}

	private Node FilterDown(Node node, Simulated simulated)
	{
		return null;
	}

	private void FindPlacementBlockers(Node node, AlignedBox box, ref List<Simulated> result)
	{
	}

	private void Find(Node node, AlignedBox box, ref List<Simulated> result)
	{
	}

	private void Find(Node node, Ray ray, Segment segment, ref List<Simulated> result)
	{
	}

	private void Generate(Node parent, int depth)
	{
	}
}
