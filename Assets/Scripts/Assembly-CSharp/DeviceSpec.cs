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
			return width;
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
	}

	public Vector3 Center3
	{
		get
		{
			return new Vector3(width / 2f, height / 2f, 0f);
		}
	}

	public ResolutionDensity Density
	{
		get
		{
			return density;
		}
	}

	public DeviceSpec()
	{
		width = Screen.width;
		height = Screen.height;
		if (Screen.dpi >= 250f)
		{
			density = ResolutionDensity.Dense;
		}
		else
		{
			density = ResolutionDensity.Standard;
		}
	}
}
