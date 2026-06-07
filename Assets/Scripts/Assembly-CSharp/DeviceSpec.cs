using UnityEngine;

public class DeviceSpec
{
	public enum ResolutionDensity
	{
		Standard = 0,
		Dense = 1
	}

	private const float DPI_DENSE = 250f;

	private ResolutionDensity density;

	private float width;

	private float height;

	public float Width
	{
		get
		{
			return 0f;
		}
	}

	public float Height
	{
		get
		{
			return 0f;
		}
	}

	public Vector3 Center3
	{
		get
		{
			return default(Vector3);
		}
	}

	public ResolutionDensity Density
	{
		get
		{
			return default(ResolutionDensity);
		}
	}
}
