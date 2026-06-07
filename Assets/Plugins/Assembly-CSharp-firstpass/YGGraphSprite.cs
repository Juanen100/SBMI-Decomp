using System.Collections.Generic;
using UnityEngine;

public class YGGraphSprite : YGSprite
{
	public float yMin;

	public float yMax;

	public bool deskew;

	public int samplesPerPoint;

	private Vector2[] points;

	private List<float> data;

	public float lineWidth;

	public float pointSpacing;

	public bool dirty;

	private static Vector2[] uvSet;

	private Mesh graphMesh;

	private int dataPoints;

	private List<float> buffer;

	private int[] triSet;

	public Color PenColor
	{
		get
		{
			return default(Color);
		}
		set
		{
		}
	}

	protected override void OnEnable()
	{
	}

	public override void AssembleMesh()
	{
	}

	protected virtual void UpdateMesh(Mesh source)
	{
	}

	protected override void OnDisable()
	{
	}

	public void SubmitData()
	{
	}

	public void Add(float val)
	{
	}

	public void Add(IList<float> vals)
	{
	}

	public void Clear()
	{
	}

	public void Draw(float val)
	{
	}

	public void Draw(IList<float> vals)
	{
	}

	public void Draw()
	{
	}

	private void PlotPoint(float x, float y, ref Vector2 point)
	{
	}
}
