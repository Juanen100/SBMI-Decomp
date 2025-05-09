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
		: this(gravity, height, 0f, 1f, 0f, 0f, Vector2.one)
	{
	}

	public JumpPattern(float gravity, float height, float collisionStickTime, float squishFactor, float percentOffset, float now, Vector2 inStartScale)
	{
		a = gravity / 2f;
		period = Mathf.Sqrt(-4f * height / a);
		b = (0f - a) * period;
		c = 0f;
		maximum = height;
		minimum = 0f;
		timeOffset = period * percentOffset - now;
		this.collisionStickTime = collisionStickTime;
		startScale = inStartScale;
		if (TFPerfUtils.IsNonScalingDevice())
		{
			squisher = new ConstantPattern(1f);
		}
		else
		{
			squisher = new Sinusoid(0f, squishFactor, collisionStickTime, 0f);
		}
	}

	public override float ValueAtTime(float atTime)
	{
		float val;
		Vector2 squish;
		ValueAndSquishAtTime(atTime, out val, out squish);
		return val;
	}

	public void ValueAndSquishAtTime(float atTime, out float val, out Vector2 squish)
	{
		float num = (atTime + timeOffset) % (period + collisionStickTime);
		squish = startScale;
		if (num > period)
		{
			if (!TFPerfUtils.IsNonScalingDevice())
			{
				float atTime2 = num - period;
				float num2 = squisher.ValueAtTime(atTime2);
				squish = new Vector2(startScale.x + num2 * 0.75f, startScale.y - num2);
			}
			num = 0f;
		}
		val = TFMath.Quadratic(a, b, c, num);
	}
}
