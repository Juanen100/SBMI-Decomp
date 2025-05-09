using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

public class YG2DGear : YG2DBody
{
	public float radius = 0.5f;

	public int teeth = 9;

	public float tipPercent = 0.1f;

	public float toothHeight = 0.25f;

	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateGear(world, radius, teeth, tipPercent, toothHeight, density);
	}
}
