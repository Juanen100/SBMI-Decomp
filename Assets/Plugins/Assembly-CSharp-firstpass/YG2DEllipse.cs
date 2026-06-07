using FarseerPhysics.Dynamics;
using UnityEngine;

public class YG2DEllipse : YG2DBody
{
	public Vector2 size;

	public int edges;

	protected override Body GetBody(World world)
	{
		return null;
	}
}
