using System.Collections.Generic;
using UnityEngine;

public class YGFrameSprite : YGSprite
{
	public RectOffset padding;

	protected override void OnEnable()
	{
		lockAspect = false;
		base.OnEnable();
	}

	public new static int[] BuildTris()
	{
		return new int[54]
		{
			1, 4, 0, 5, 4, 1, 2, 5, 1, 6,
			5, 2, 3, 6, 2, 7, 6, 3, 5, 8,
			4, 9, 8, 5, 6, 9, 5, 10, 9, 6,
			7, 10, 6, 11, 10, 7, 9, 12, 8, 13,
			12, 9, 10, 13, 9, 14, 13, 10, 11, 14,
			10, 15, 14, 11
		};
	}

	public static Vector3[] BuildVerts(Vector2 size, RectOffset padding, Vector2 scale)
	{
		List<Vector3> list = new List<Vector3>();
		float[] array = new float[4]
		{
			0f,
			-padding.top,
			0f - size.y + (float)padding.bottom,
			0f - size.y
		};
		float[] array2 = array;
		foreach (float num in array2)
		{
			list.Add(new Vector2(0f, num * scale.y));
			list.Add(new Vector2((float)padding.left * scale.x, num * scale.y));
			list.Add(new Vector2((size.x - (float)padding.right) * scale.x, num * scale.y));
			list.Add(new Vector2(size.x * scale.x, num * scale.y));
		}
		return list.ToArray();
	}

	public static Vector2[] BuildUVs(Rect rect, RectOffset padding, Vector2 size)
	{
		List<Vector2> list = new List<Vector2>();
		float[] array = new float[4]
		{
			1f - rect.yMin / size.y,
			1f - (rect.yMin + (float)padding.top) / size.y,
			1f - (rect.yMax - (float)padding.bottom) / size.y,
			1f - rect.yMax / size.y
		};
		float[] array2 = array;
		foreach (float y in array2)
		{
			list.Add(new Vector2(rect.xMin / size.x, y));
			list.Add(new Vector2((rect.xMin + (float)padding.left) / size.x, y));
			list.Add(new Vector2((rect.xMax - (float)padding.right) / size.x, y));
			list.Add(new Vector2(rect.xMax / size.x, y));
		}
		return list.ToArray();
	}

	public override void SetSize(Vector2 s)
	{
		size = s;
		update.verts = BuildVerts(size, padding, scale);
		base.View.RefreshEvent += base.UpdateMesh;
	}

	public override void SetColor(Color c)
	{
		color = c;
		YGSprite.BuildColors(color, ref colors);
		update.colors = colors;
		base.View.RefreshEvent += base.UpdateMesh;
	}

	public override void SetAlpha(float alpha)
	{
		color.a = alpha;
		SetColor(color);
	}

	public override void AssembleMesh()
	{
		update.Reset();
		YGSprite.BuildColors(color, ref colors);
		update.verts = BuildVerts(size, padding, scale);
		update.normals = YGSprite.BuildNormals(update.vertCount);
		update.tris = BuildTris();
		update.colors = colors;
		update.uvs = BuildUVs(new Rect(0f, 0f, textureSize.x, textureSize.y), padding, textureSize);
		UpdateMesh(update);
	}
}
