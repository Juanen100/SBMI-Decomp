using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

public class YG2DSolidArc : YG2DBody
{
	public float degrees = 15f;

	public float angle = 45f;

	public float radius = 0.5f;

	public int sides = 8;

	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateSolidArc(world, density, degrees * ((float)Math.PI / 180f), sides, radius, Vector2.zero, angle * ((float)Math.PI / 180f));
	}
}
