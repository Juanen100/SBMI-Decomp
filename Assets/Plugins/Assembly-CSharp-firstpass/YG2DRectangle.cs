using FarseerPhysics.Dynamics;
using UnityEngine;

public class YG2DRectangle : YG2DBody
{
	public Vector2 size;

	protected override Body GetBody(World world)
	{
		return null;
	}
}
