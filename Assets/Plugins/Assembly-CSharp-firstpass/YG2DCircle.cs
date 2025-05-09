using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

public class YG2DCircle : YG2DBody
{
	public float radius = 0.5f;

	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateCircle(world, radius, density);
	}
}
