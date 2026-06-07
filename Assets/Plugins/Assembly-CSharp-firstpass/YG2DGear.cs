using FarseerPhysics.Dynamics;

public class YG2DGear : YG2DBody
{
	public float radius;

	public int teeth;

	public float tipPercent;

	public float toothHeight;

	protected override Body GetBody(World world)
	{
		return null;
	}
}
