using System;
using System.Collections.Generic;
using MiniJSON;
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
			Point = p;
			Time = t;
			EaseIO = io;
		}

		internal SplineNode(SplineNode o)
		{
			Point = o.Point;
			Time = o.Time;
			EaseIO = o.EaseIO;
		}
	}

	private float maxTime;

	private List<SplineNode> mNodes = new List<SplineNode>();

	public float MaxTime
	{
		get
		{
			return maxTime;
		}
	}

	public void Reset()
	{
		mNodes.Clear();
	}

	public void AddPoint(Vector3 pos, float timeInSeconds, Vector2 easeInOut)
	{
		mNodes.Add(new SplineNode(pos, timeInSeconds, easeInOut));
		if (timeInSeconds > maxTime)
		{
			maxTime = timeInSeconds;
		}
	}

	public static float Ease(float t, float k1, float k2)
	{
		float num = k1 * 2f / (float)Math.PI + k2 - k1 + (1f - k2) * 2f / (float)Math.PI;
		float num2 = ((t < k1) ? (k1 * (2f / (float)Math.PI) * (Mathf.Sin(t / k1 * (float)Math.PI / 2f - (float)Math.PI / 2f) + 1f)) : ((!(t < k2)) ? (2f * k1 / (float)Math.PI + k2 - k1 + (1f - k2) * (2f / (float)Math.PI) * Mathf.Sin((t - k2) / (1f - k2) * (float)Math.PI / 2f)) : (2f * k1 / (float)Math.PI + t - k1)));
		return num2 / num;
	}

	public void LoadData(string fname)
	{
		string streamingAssetsFile = TFUtils.GetStreamingAssetsFile(fname);
		string json = TFUtils.ReadAllText(streamingAssetsFile);
		Debug.Log(streamingAssetsFile);
		List<object> list = (List<object>)Json.Deserialize(json);
		foreach (object item in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)item;
			Vector3 v;
			TFUtils.LoadVector3(out v, (Dictionary<string, object>)dictionary["scale"]);
			Vector2 v2;
			TFUtils.LoadVector2(out v2, (Dictionary<string, object>)dictionary["easeIO"]);
			float timeInSeconds = TFUtils.LoadFloat(dictionary, "time");
			AddPoint(v, timeInSeconds, v2);
		}
	}

	public Vector3 GetHermiteAtTime(float timeParam)
	{
		if (timeParam >= mNodes[mNodes.Count - 2].Time)
		{
			return mNodes[mNodes.Count - 2].Point;
		}
		int i;
		for (i = 1; i < mNodes.Count - 2 && !(mNodes[i].Time > timeParam); i++)
		{
		}
		int num = i - 1;
		float t = (timeParam - mNodes[num].Time) / (mNodes[num + 1].Time - mNodes[num].Time);
		t = Ease(t, mNodes[num].EaseIO.x, mNodes[num].EaseIO.y);
		float num2 = t;
		float num3 = num2 * num2;
		float num4 = num3 * num2;
		Vector3 vector = ((num <= 0) ? mNodes[num].Point : mNodes[num - 1].Point);
		Vector3 point = mNodes[num].Point;
		Vector3 point2 = mNodes[num + 1].Point;
		Vector3 point3 = mNodes[num + 2].Point;
		float num5 = 0.5f;
		Vector3 vector2 = num5 * (point2 - vector);
		Vector3 vector3 = num5 * (point3 - point);
		float num6 = 2f * num4 - 3f * num3 + 1f;
		float num7 = -2f * num4 + 3f * num3;
		float num8 = num4 - 2f * num3 + num2;
		float num9 = num4 - num3;
		return num6 * point + num7 * point2 + num8 * vector2 + num9 * vector3;
	}
}
