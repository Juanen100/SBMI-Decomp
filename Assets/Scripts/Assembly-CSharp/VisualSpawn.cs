#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualSpawn : SessionActionSpawn
{
	public const string OFFSET = "offset";

	public const string ROTATION = "rotation";

	public const string ALPHA = "alpha";

	public const string SCALE = "scale";

	protected Vector3 offset;

	private float rotationCwDeg;

	private Vector3 direction;

	private float alpha = 1f;

	private Vector2 scale = Vector2.one;

	protected float Rotation
	{
		get
		{
			return rotationCwDeg;
		}
		set
		{
			rotationCwDeg = value;
			direction = new Vector3(Mathf.Sin(value * ((float)Math.PI / 180f)), Mathf.Cos(value * ((float)Math.PI / 180f)), 0f);
		}
	}

	protected Vector3 Direction
	{
		get
		{
			return direction;
		}
	}

	protected float Alpha
	{
		get
		{
			return alpha;
		}
		set
		{
			alpha = value;
		}
	}

	protected Vector2 Scale
	{
		get
		{
			return scale;
		}
		set
		{
			scale = value;
		}
	}

	protected virtual void Initialize(Game game, SessionActionTracker parentAction, Vector3 offset, float rotationCwDeg, float alpha, Vector2 inScale)
	{
		base.RegisterNewInstance(game, parentAction);
		this.offset = offset;
		Rotation = rotationCwDeg;
		Alpha = alpha;
		Scale = inScale;
	}

	protected void NormalizeRotationAndPushToEdge(float widthOver2, float heightOver2)
	{
		rotationCwDeg = TFMath.Modulo(rotationCwDeg, 360f);
		if (rotationCwDeg < 0f)
		{
			rotationCwDeg += 360f;
		}
		Rotation = rotationCwDeg;
		float num;
		float num2;
		if (rotationCwDeg < 45f || rotationCwDeg > 315f)
		{
			num = widthOver2 * Mathf.Sin(rotationCwDeg * ((float)Math.PI / 180f));
			num2 = heightOver2;
		}
		else if (rotationCwDeg < 135f)
		{
			num = widthOver2;
			num2 = heightOver2 * Mathf.Cos(rotationCwDeg * ((float)Math.PI / 180f));
		}
		else if (rotationCwDeg < 225f)
		{
			num = widthOver2 * Mathf.Sin(rotationCwDeg * ((float)Math.PI / 180f));
			num2 = 0f - heightOver2;
		}
		else
		{
			num = 0f - widthOver2;
			num2 = heightOver2 * Mathf.Cos(rotationCwDeg * ((float)Math.PI / 180f));
		}
		offset = new Vector3(offset.x + num, offset.y + num2, offset.z);
	}

	public void Parse(Dictionary<string, object> data, bool isOffsetRequired, Vector3 defaultOffset, float offsetConversionScale)
	{
		rotationCwDeg = 0f;
		if (data.ContainsKey("rotation"))
		{
			rotationCwDeg = TFUtils.LoadFloat(data, "rotation");
		}
		Rotation = rotationCwDeg;
		if (isOffsetRequired && !data.ContainsKey("offset"))
		{
			TFUtils.Assert(!isOffsetRequired || data.ContainsKey("offset"), "Offset is required for this session action. data=" + TFUtils.DebugDictToString(data));
		}
		offset = defaultOffset;
		if (data.ContainsKey("offset"))
		{
			TFUtils.LoadVector3(out offset, TFUtils.LoadDict(data, "offset"));
			offset.Scale(new Vector3(offsetConversionScale, offsetConversionScale, offsetConversionScale));
		}
		alpha = 1f;
		if (data.ContainsKey("alpha"))
		{
			alpha = TFUtils.LoadFloat(data, "alpha");
		}
		scale = Vector2.one;
		if (data.ContainsKey("scale"))
		{
			TFUtils.LoadVector2(out scale, (Dictionary<string, object>)data["scale"]);
		}
	}

	public void AddToDict(ref Dictionary<string, object> dict)
	{
		dict["rotation"] = rotationCwDeg;
		dict["alpha"] = alpha;
		dict["offset"] = offset;
		dict["scale"] = scale;
	}
}
