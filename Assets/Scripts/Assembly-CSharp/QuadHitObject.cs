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
			return center;
		}
		set
		{
			center = value;
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
		}
	}

	public float Width
	{
		get
		{
			return width;
		}
		set
		{
			width = value;
		}
	}

	public QuadHitObject(Vector2 center, float width, float height)
	{
		this.center = center;
		this.width = width;
		this.height = height;
	}

	public void Initialize(Vector2 center, float width, float height)
	{
		this.center = center;
		this.width = width;
		this.height = height;
	}

	public virtual bool Intersects(Transform transform, Ray ray, Vector2 offset)
	{
		float num = 0.5f * width;
		float num2 = 0.5f * height;
		float enter;
		if (new Plane(-ray.direction, transform.position).Raycast(ray, out enter))
		{
			Vector3 vector = transform.InverseTransformPoint(ray.origin + ray.direction * enter) + new Vector3(center.x + offset.x, center.y + offset.y, 0f);
			return 0f - num < vector.x && vector.x < num && 0f - num2 < vector.y && vector.y < num2;
		}
		return false;
	}
}
