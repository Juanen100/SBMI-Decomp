using System;
using UnityEngine;

public class Sinusoid : PeriodicPattern
{
	public Sinusoid(float minimum, float maximum, float period, float timeOffset)
	{
		timeOffset += period / 2f;
		Initialize(minimum, maximum, period, timeOffset);
	}

	public override float ValueAtTime(float atTime)
	{
		float num = maximum - minimum;
		float num2 = num / 2f;
		float num3 = minimum + num2;
		float num4 = (float)Math.PI * 2f / period;
		float num5 = Mathf.Cos((atTime + timeOffset) * num4);
		return num3 + num5 * num2;
	}
}
