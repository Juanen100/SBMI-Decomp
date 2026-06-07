using FarseerPhysics.Dynamics;
using UnityEngine;

public class YG2DRoundedRectangle : YG2DBody
{
	public Vector2 size;

	public float xRadius;

	public float yRadius;

	public int segments;

	protected override Body GetBody(World world)
	{
		return null;
	}
}
