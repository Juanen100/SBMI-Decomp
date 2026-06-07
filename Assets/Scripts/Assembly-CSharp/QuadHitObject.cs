using UnityEngine;

public class QuadHitObject
{
	private Vector2 center;

	private float height;

	private float width;

	public Vector2 Center
	{
		get
		{
			return default(Vector2);
		}
		set
		{
		}
	}

	public float Height
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float Width
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public QuadHitObject(Vector2 center, float width, float height)
	{
	}

	public void Initialize(Vector2 center, float width, float height)
	{
	}

	public virtual bool Intersects(Transform transform, Ray ray, Vector2 offset)
	{
		return false;
	}
}
