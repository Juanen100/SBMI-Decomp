using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

public class YG2DNGon : YG2DBody
{
	public float size = 1f;

	public int sides = 5;

	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateEllipse(world, size, size, sides, density);
	}
}
