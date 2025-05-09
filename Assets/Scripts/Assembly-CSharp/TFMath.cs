using System;
using UnityEngine;

public class TFMath
{
	public static float ClampF(float input, float min, float max)
	{
		return Math.Max(min, Math.Min(max, input));
	}

	public static float Modulo(float dividend, float divisor)
	{
		float num = ((!(dividend < 0f)) ? 1 : (-1));
		return num * (Mathf.Abs(dividend) % divisor);
	}

	public static int Modulo(int dividend, int divisor)
	{
		int num = ((dividend >= 0) ? 1 : (-1));
		return num * (Mathf.Abs(dividend) % divisor);
	}

	public static float Quadratic(float a, float b, float c, float x)
	{
		return a * (x * x) + b * x + c;
	}
}
