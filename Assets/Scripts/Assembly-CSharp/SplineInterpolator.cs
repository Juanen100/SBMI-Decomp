using System.Collections.Generic;
using UnityEngine;

public class SplineInterpolator
{
	internal class SplineNode
	{
		internal Vector3 Point;

		internal float Time;

		internal Vector2 EaseIO;

		internal SplineNode(Vector3 p, float t, Vector2 io)
		{
		}

		internal SplineNode(SplineNode o)
		{
		}
	}

	private float maxTime;

	private List<SplineNode> mNodes;

	public float MaxTime
	{
		get
		{
			return 0f;
		}
	}

	public void Reset()
	{
	}

	public void AddPoint(Vector3 pos, float timeInSeconds, Vector2 easeInOut)
	{
	}

	public static float Ease(float t, float k1, float k2)
	{
		return 0f;
	}

	public void LoadData(string fname)
	{
	}

	public Vector3 GetHermiteAtTime(float timeParam)
	{
		return default(Vector3);
	}
}
