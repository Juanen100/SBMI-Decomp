using System.Collections.Generic;
using UnityEngine;

public class Scene
{
	private class DistanceCompare : IComparer<Simulated>
	{
		private Vector2 point;

		public DistanceCompare(Vector2 point)
		{
			this.point = point;
		}

		public int Compare(Simulated lhs, Simulated rhs)
		{
			if ((lhs.Position - point).sqrMagnitude < (rhs.Position - point).sqrMagnitude)
			{
				return -1;
			}
			return 1;
		}
	}

	private class Node
	{
		public Node firstChild;

		public Node nextSibling;

		public AlignedBox box;

		public List<Simulated> entities = new List<Simulated>();

		public List<Simulated> blockerEntities = new List<Simulated>();

		public Node(AlignedBox box)
		{
			this.box = box;
		}

		public void AddChild(Node child)
		{
			if (firstChild == null)
			{
				firstChild = child;
			}
			else
			{
				firstChild.AddSibling(child);
			}
		}

		public void AddSibling(Node sibling)
		{
			if (nextSibling == null)
			{
				nextSibling = sibling;
			}
			else
			{
				nextSibling.AddSibling(sibling);
			}
		}
	}

	private Terrain terrain;

	private int depth;

	private Node root;

	public Scene(Terrain terrain, int depth)
	{
		this.terrain = terrain;
		this.depth = depth;
		AlignedBox worldExtent = this.terrain.WorldExtent;
		float num = worldExtent.xmax - worldExtent.xmin;
		float num2 = worldExtent.ymax - worldExtent.ymin;
		float num3 = 0.5f * (worldExtent.xmax + worldExtent.xmin);
		float num4 = 0.5f * (worldExtent.ymax + worldExtent.ymin);
		root = new Node(new AlignedBox(num3 - num, num3 + num, num4 - num2, num4 + num2));
		Generate(root, 1);
	}

	public void OnUpdate(List<Simulated> simulateds)
	{
		foreach (Simulated simulated in simulateds)
		{
			AlignedBox box = simulated.Box;
			if (simulated.prevSceneBox.xmin == box.xmin && simulated.prevSceneBox.xmax == box.xmax && simulated.prevSceneBox.ymin == box.ymin && simulated.prevSceneBox.ymin == box.ymin)
			{
				continue;
			}
			simulated.prevSceneBox.xmin = simulated.Box.xmin;
			simulated.prevSceneBox.xmax = simulated.Box.xmax;
			simulated.prevSceneBox.ymin = simulated.Box.ymin;
			simulated.prevSceneBox.ymax = simulated.Box.ymax;
			Node node = simulated.Variable["scene.node"] as Node;
			if (node != null)
			{
				if (AlignedBox.Contains(node.box, simulated.Box))
				{
					Node node2 = FilterDown(node, simulated);
					if (node != node2)
					{
						node.entities.Remove(simulated);
						node2.entities.Add(simulated);
						if (node.blockerEntities.Contains(simulated))
						{
							node.blockerEntities.Remove(simulated);
							node2.blockerEntities.Add(simulated);
						}
						simulated.Variable["scene.node"] = node2;
					}
				}
				else
				{
					node.entities.Remove(simulated);
					node.blockerEntities.Remove(simulated);
					simulated.Variable["scene.node"] = null;
					Filter(root, simulated);
				}
			}
			else
			{
				Filter(simulated);
			}
		}
	}

	public void Add(Simulated entity)
	{
		entity.Variable["scene.node"] = null;
	}

	public void Remove(Simulated entity)
	{
		Node node = entity.Variable["scene.node"] as Node;
		if (node != null)
		{
			node.blockerEntities.Remove(entity);
			node.entities.Remove(entity);
		}
	}

	public void Find(AlignedBox box, ref List<Simulated> result)
	{
		result.Clear();
		Find(root, box, ref result);
	}

	public void FindPlacementBlockers(AlignedBox box, ref List<Simulated> result)
	{
		result.Clear();
		FindPlacementBlockers(root, box, ref result);
	}

	public void Find(Ray ray, ref List<Simulated> result)
	{
		result.Clear();
		Vector3 point;
		if (terrain.ComputeIntersection(ray, out point))
		{
			Segment segment = default(Segment);
			segment.first = new Vector2(ray.origin.x, ray.origin.y);
			segment.second = new Vector2(point.x, point.y);
			Find(root, ray, segment, ref result);
		}
		result.Sort(new DistanceCompare(new Vector2(ray.origin.x, ray.origin.y)));
	}

	private Node Filter(Simulated entity)
	{
		Node node = entity.Variable["scene.node"] as Node;
		if (node != null)
		{
			node.entities.Remove(entity);
			node.blockerEntities.Remove(entity);
			entity.Variable["scene.node"] = null;
		}
		return Filter(root, entity);
	}

	private Node Filter(Node node, Simulated simulated)
	{
		for (Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			if (AlignedBox.Contains(node2.box, simulated.Box))
			{
				return Filter(node2, simulated);
			}
		}
		node.entities.Add(simulated);
		if (simulated.entity.HasDecorator<StructureDecorator>() && simulated.entity.GetDecorator<StructureDecorator>().ShouldBlockPlacement)
		{
			node.blockerEntities.Add(simulated);
		}
		simulated.Variable["scene.node"] = node;
		simulated.prevSceneBox.xmin = simulated.Box.xmin;
		simulated.prevSceneBox.xmax = simulated.Box.xmax;
		simulated.prevSceneBox.ymin = simulated.Box.ymin;
		simulated.prevSceneBox.ymax = simulated.Box.ymax;
		return node;
	}

	private Node FilterDown(Node node, Simulated simulated)
	{
		for (Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			if (AlignedBox.Contains(node2.box, simulated.Box))
			{
				return FilterDown(node2, simulated);
			}
		}
		return node;
	}

	private void FindPlacementBlockers(Node node, AlignedBox box, ref List<Simulated> result)
	{
		if (node == null)
		{
			return;
		}
		if (AlignedBox.Intersects(node.box, box))
		{
			foreach (Simulated blockerEntity in node.blockerEntities)
			{
				if (AlignedBox.Intersects(blockerEntity.Box, box))
				{
					result.Add(blockerEntity);
				}
			}
		}
		for (Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			FindPlacementBlockers(node2, box, ref result);
		}
	}

	private void Find(Node node, AlignedBox box, ref List<Simulated> result)
	{
		if (node == null)
		{
			return;
		}
		if (AlignedBox.Intersects(node.box, box))
		{
			foreach (Simulated entity in node.entities)
			{
				if (AlignedBox.Intersects(entity.Box, box))
				{
					result.Add(entity);
				}
			}
		}
		for (Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			Find(node2, box, ref result);
		}
	}

	private void Find(Node node, Ray ray, Segment segment, ref List<Simulated> result)
	{
		if (node == null)
		{
			return;
		}
		if (AlignedBox.Intersects(node.box, segment))
		{
			foreach (Simulated entity in node.entities)
			{
				if (AlignedBox.Intersects(entity.Box, segment) && entity.Intersects(ray))
				{
					result.Add(entity);
				}
			}
		}
		for (Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			Find(node2, ray, segment, ref result);
		}
	}

	private void Generate(Node parent, int depth)
	{
		if (depth < this.depth)
		{
			float num = 0.5f * (parent.box.xmax + parent.box.xmin);
			float num2 = 0.25f * (parent.box.xmax - parent.box.xmin);
			float num3 = 0.5f * num2;
			float num4 = 0.5f * (parent.box.ymax + parent.box.ymin);
			float num5 = 0.25f * (parent.box.ymax - parent.box.ymin);
			float num6 = 0.5f * num5;
			Node node = new Node(new AlignedBox(num + num3 - num2, num + num3 + num2, num4 + num6 - num5, num4 + num6 + num5));
			parent.AddChild(node);
			Generate(node, depth + 1);
			Node node2 = new Node(new AlignedBox(num - num3 - num2, num - num3 + num2, num4 + num6 - num5, num4 + num6 + num5));
			parent.AddChild(node2);
			Generate(node2, depth + 1);
			Node node3 = new Node(new AlignedBox(num - num3 - num2, num - num3 + num2, num4 - num6 - num5, num4 - num6 + num5));
			parent.AddChild(node3);
			Generate(node3, depth + 1);
			Node node4 = new Node(new AlignedBox(num + num3 - num2, num + num3 + num2, num4 - num6 - num5, num4 - num6 + num5));
			parent.AddChild(node4);
			Generate(node4, depth + 1);
		}
	}
}
