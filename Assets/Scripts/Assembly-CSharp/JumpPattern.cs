using UnityEngine;

public class JumpPattern : PeriodicPattern
{
	private float a;

	private float b;

	private float c;

	private float collisionStickTime;

	private PeriodicPattern squisher;

	private Vector2 startScale;

	public JumpPattern(float gravity, float height)
	{
	}

	public JumpPattern(float gravity, float height, float collisionStickTime, float squishFactor, float percentOffset, float now, Vector2 inStartScale)
	{
	}

	public override float ValueAtTime(float atTime)
	{
		return 0f;
	}

	public void ValueAndSquishAtTime(float atTime, out float val, out Vector2 squish)
	{
		val = default(float);
		squish = default(Vector2);
	}
}
