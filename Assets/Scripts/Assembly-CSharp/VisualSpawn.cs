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

	private float alpha;

	private Vector2 scale;

	protected float Rotation
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	protected Vector3 Direction
	{
		get
		{
			return default(Vector3);
		}
	}

	protected float Alpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	protected Vector2 Scale
	{
		get
		{
			return default(Vector2);
		}
		set
		{
		}
	}

	protected virtual void Initialize(Game game, SessionActionTracker parentAction, Vector3 offset, float rotationCwDeg, float alpha, Vector2 inScale)
	{
	}

	protected void NormalizeRotationAndPushToEdge(float widthOver2, float heightOver2)
	{
	}

	public void Parse(Dictionary<string, object> data, bool isOffsetRequired, Vector3 defaultOffset, float offsetConversionScale)
	{
	}

	public void AddToDict(ref Dictionary<string, object> dict)
	{
	}
}
