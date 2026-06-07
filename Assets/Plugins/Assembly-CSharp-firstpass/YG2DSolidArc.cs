using FarseerPhysics.Dynamics;

public class YG2DSolidArc : YG2DBody
{
	public float degrees;

	public float angle;

	public float radius;

	public int sides;

	protected override Body GetBody(World world)
	{
		return null;
	}
}
