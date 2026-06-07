using System.Collections.Generic;
using UnityEngine;

public sealed class GraphTexture
{
	private Texture2D blankTexture;

	public Rect limits;

	public Vector2 offset;

	private Vector2 plotScale;

	private Color penColor;

	private int i;

	private bool offScale;

	public Texture2D texture { get; private set; }

	public GraphTexture(Vector2 size, Color bgColor, Color graphColor)
	{
	}

	public void Draw(List<float> data)
	{
	}

	private void PlotPoint(Vector2 point, Texture2D tex)
	{
	}

	private void Circle(Texture2D tex, int cx, int cy, int r, Color col)
	{
	}
}
