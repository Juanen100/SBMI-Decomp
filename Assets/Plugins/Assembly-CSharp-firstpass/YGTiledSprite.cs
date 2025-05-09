using System.Collections.Generic;
using UnityEngine;

public class YGTiledSprite : YGSprite
{
	public Vector2 tileSize;

	public Vector2 tileScale = Vector2.one;

	public Vector2 tileOffset = Vector2.one;

	protected override void OnEnable()
	{
		if (tileSize == Vector2.zero)
		{
			tileSize = GetMainTextureSize(true);
		}
		base.OnEnable();
	}

	public static int[] BuildTris(Vector3[] verts)
	{
		List<int> list = new List<int>();
		list.AddRange(new int[3] { 1, 4, 0 });
		list.AddRange(new int[3] { 5, 4, 1 });
		list.AddRange(new int[3] { 2, 5, 1 });
		list.AddRange(new int[3] { 6, 5, 2 });
		list.AddRange(new int[3] { 3, 6, 2 });
		list.AddRange(new int[3] { 7, 6, 3 });
		list.AddRange(new int[3] { 5, 8, 4 });
		list.AddRange(new int[3] { 9, 8, 5 });
		list.AddRange(new int[3] { 6, 9, 5 });
		list.AddRange(new int[3] { 10, 9, 6 });
		list.AddRange(new int[3] { 7, 10, 6 });
		list.AddRange(new int[3] { 11, 10, 7 });
		list.AddRange(new int[3] { 9, 12, 8 });
		list.AddRange(new int[3] { 13, 12, 9 });
		list.AddRange(new int[3] { 10, 13, 9 });
		list.AddRange(new int[3] { 14, 13, 10 });
		list.AddRange(new int[3] { 11, 14, 10 });
		list.AddRange(new int[3] { 15, 14, 11 });
		return list.ToArray();
	}

	public static Vector3[] BuildVerts(Vector2 size, Vector2 tileSize, Vector2 scale)
	{
		List<Vector3> list = new List<Vector3>();
		List<float> list2 = new List<float>();
		List<float> list3 = new List<float>();
		int num = Mathf.FloorToInt(size.x / tileSize.x);
		int num2 = Mathf.FloorToInt(size.y / tileSize.y);
		for (int i = 0; (float)i < size.x; i += num)
		{
			list2.Add(i);
		}
		list2.Add(size.x);
		for (int j = 0; (float)j < size.y; j += num2)
		{
			list3.Add(j);
		}
		list3.Add(size.y);
		foreach (float item in list2)
		{
			float x = item;
			foreach (float item2 in list3)
			{
				float y = item2;
				list.Add(new Vector3(x, y));
			}
		}
		return list.ToArray();
	}

	public static Vector2[] BuildUVs(Rect rect, Vector2 size, Vector3[] verts)
	{
		List<Vector2> list = new List<Vector2>();
		float[] array = new float[4]
		{
			1f - rect.y / size.y,
			1f - rect.y / size.y,
			1f - (rect.y + rect.height) / size.y,
			1f - (rect.y + rect.height) / size.y
		};
		float[] array2 = array;
		foreach (float y in array2)
		{
			list.Add(new Vector2(rect.x / size.x, y));
			list.Add(new Vector2(rect.x / size.x, y));
			list.Add(new Vector2((rect.x + rect.width) / size.x, y));
			list.Add(new Vector2((rect.x + rect.width) / size.x, y));
		}
		return list.ToArray();
	}

	public override void AssembleMesh()
	{
		update.Reset();
		verts = BuildVerts(size, tileSize, tileScale);
		update.verts = verts;
		update.tris = BuildTris(verts);
		update.uvs = BuildUVs(new Rect(0f, 0f, textureSize.x, textureSize.y), textureSize, verts);
		update.normals = YGSprite.BuildNormals(update.vertCount);
		colors = new Color[verts.Length];
		YGSprite.BuildColors(color, ref colors);
		update.colors = colors;
		UpdateMesh(update);
	}
}
