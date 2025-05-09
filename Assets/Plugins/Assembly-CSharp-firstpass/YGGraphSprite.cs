using System.Collections.Generic;
using UnityEngine;

public class YGGraphSprite : YGSprite
{
	public float yMin;

	public float yMax = 1f;

	public bool deskew;

	public int samplesPerPoint = 4;

	private Vector2[] points;

	private List<float> data;

	public float lineWidth = 4f;

	public float pointSpacing = 4f;

	public bool dirty = true;

	private static Vector2[] uvSet = new Vector2[4]
	{
		new Vector2(0f, 0f),
		new Vector2(0f, -1f),
		new Vector2(1f, 0f),
		new Vector2(1f, -1f)
	};

	private Mesh graphMesh;

	private int dataPoints;

	private List<float> buffer;

	private int[] triSet;

	public Color PenColor
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
		}
	}

	protected override void OnEnable()
	{
		dataPoints = Mathf.CeilToInt(size.x / pointSpacing);
		points = new Vector2[dataPoints];
		data = new List<float>(new float[dataPoints]);
		buffer = new List<float>(samplesPerPoint);
		tris = new int[dataPoints * 6];
		uvs = new Vector2[dataPoints * 4];
		verts = new Vector3[dataPoints * 4];
		graphMesh = new Mesh();
		base.OnEnable();
	}

	public override void AssembleMesh()
	{
		Vector2 vector = new Vector2(0f, lineWidth / 2f);
		if (data.Count > dataPoints)
		{
			data.RemoveRange(0, data.Count - dataPoints);
		}
		float num = size.x / (float)dataPoints;
		float num2 = (yMax - yMin) / 2f * size.y;
		Vector2 vector2 = Vector2.zero;
		bool flag = true;
		triSet = new int[6] { 1, 3, 2, 1, 2, 0 };
		float new_y = lineWidth / 2f;
		for (int i = 0; i < data.Count; i++)
		{
			Vector2 vector3 = points[i];
			vector3.Set((float)i * num, data[i] * num2);
			if (flag)
			{
				vector2 = (points[i] = vector3);
				flag = false;
				continue;
			}
			if (deskew)
			{
				vector.Set(0f, new_y);
				Quaternion quaternion = Quaternion.LookRotation(vector2 - vector3);
				vector = quaternion * vector;
			}
			int num3 = i * 4;
			verts[num3] = (vector2 - vector) * 0.01f;
			verts[num3 + 1] = (vector2 + vector) * 0.01f;
			verts[num3 + 2] = (vector3 - vector) * 0.01f;
			verts[num3 + 3] = (vector3 + vector) * 0.01f;
			uvs[num3] = uvSet[0];
			uvs[num3 + 1] = uvSet[1];
			uvs[num3 + 2] = uvSet[2];
			uvs[num3 + 3] = uvSet[3];
			num3 = i * 6;
			tris[num3] = triSet[0];
			tris[num3 + 1] = triSet[1];
			tris[num3 + 2] = triSet[2];
			tris[num3 + 3] = triSet[3];
			tris[num3 + 4] = triSet[4];
			tris[num3 + 5] = triSet[5];
			for (int j = 0; j < 6; j++)
			{
				triSet[j] += 4;
			}
			vector2 = (points[i] = vector3);
		}
		graphMesh.vertices = verts;
		graphMesh.uv = uvs;
		graphMesh.triangles = tris;
		Color[] array = new Color[verts.Length];
		YGSprite.BuildColors(color, ref array);
		graphMesh.normals = YGSprite.BuildNormals(verts.Length);
		graphMesh.colors = array;
		UpdateMesh(graphMesh);
	}

	protected virtual void UpdateMesh(Mesh source)
	{
		base.meshFilter.mesh = source;
	}

	protected override void OnDisable()
	{
	}

	public void SubmitData()
	{
		while (buffer.Count > samplesPerPoint)
		{
			float num = 0f;
			for (int i = 0; i < samplesPerPoint; i++)
			{
				num += buffer[i];
			}
			num /= (float)samplesPerPoint;
			data.Add(num);
			buffer.RemoveRange(0, samplesPerPoint);
			dirty = true;
		}
	}

	public void Add(float val)
	{
		buffer.Add(val);
		if (buffer.Count >= samplesPerPoint)
		{
			SubmitData();
		}
	}

	public void Add(IList<float> vals)
	{
		buffer.AddRange(vals);
		if (buffer.Count >= samplesPerPoint)
		{
			SubmitData();
		}
	}

	public void Clear()
	{
		buffer = new List<float>(new float[samplesPerPoint]);
		Draw();
	}

	public void Draw(float val)
	{
		Add(val);
		Draw();
	}

	public void Draw(IList<float> vals)
	{
		Add(vals);
		Draw();
	}

	public void Draw()
	{
		if (dirty)
		{
			dirty = false;
			AssembleMesh();
		}
	}

	private void PlotPoint(float x, float y, ref Vector2 point)
	{
	}
}
