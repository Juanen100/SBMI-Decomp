using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

public class YG2DRectangle : YG2DBody
{
	public Vector2 size = new Vector2(64f, 64f);

	protected override Body GetBody(World world)
	{
		Vector2 vector = size * 0.01f;
		return BodyFactory.CreateRectangle(world, vector.x, vector.y, density);
	}
}
