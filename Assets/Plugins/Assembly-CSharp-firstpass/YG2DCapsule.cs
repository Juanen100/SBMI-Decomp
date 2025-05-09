using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

public class YG2DCapsule : YG2DBody
{
	public float height = 2f;

	public float radius = 0.5f;

	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateCapsule(world, height, radius, density);
	}
}
