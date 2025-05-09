using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

public class YG2DEllipse : YG2DBody
{
	public Vector2 size = new Vector2(2f, 1f);

	public int edges = 16;

	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateEllipse(world, size.x, size.y, edges, density);
	}
}
