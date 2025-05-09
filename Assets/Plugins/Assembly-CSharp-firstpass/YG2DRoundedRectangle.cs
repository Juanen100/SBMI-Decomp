using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

public class YG2DRoundedRectangle : YG2DBody
{
	public Vector2 size = new Vector2(2f, 0.75f);

	public float xRadius = 0.2f;

	public float yRadius = 0.2f;

	public int segments = 2;

	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateRoundedRectangle(world, size.x, size.y, xRadius, yRadius, segments, density);
	}
}
