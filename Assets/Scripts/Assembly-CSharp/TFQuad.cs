using UnityEngine;

public class TFQuad
{
	public static void SetupQuadMesh(Mesh mesh, float width, float height, Vector2 center, bool resizeOnly = false, Rect? uvs = null)
	{
		float num = 0.5f * width;
		float num2 = 0.5f * height;
		float x = center.x;
		float y = center.y;
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f - num - x, 0f - num2 - y, 0f),
			new Vector3(0f - num - x, num2 - y, 0f),
			new Vector3(num - x, 0f - num2 - y, 0f),
			new Vector3(num - x, num2 - y, 0f)
		};
		mesh.vertices = vertices;
		if (!resizeOnly)
		{
			if (uvs.HasValue)
			{
				mesh.uv = new Vector2[4]
				{
					new Vector2(uvs.Value.xMax, uvs.Value.yMin),
					new Vector2(uvs.Value.xMax, uvs.Value.yMax),
					new Vector2(uvs.Value.xMin, uvs.Value.yMin),
					new Vector2(uvs.Value.xMin, uvs.Value.yMax)
				};
			}
			else
			{
				mesh.uv = new Vector2[4]
				{
					new Vector2(1f, 0f),
					new Vector2(1f, 1f),
					new Vector2(0f, 0f),
					new Vector2(0f, 1f)
				};
			}
			int[] triangles = new int[6] { 2, 1, 0, 1, 2, 3 };
			mesh.triangles = triangles;
		}
	}

	public static void SetupQuad(GameObject go, Material material, float width, float height, Vector2 center, Rect? uvs = null, Mesh hitMesh = null)
	{
		go.AddComponent<MeshFilter>();
		MeshFilter component = go.GetComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		MeshRenderer component2 = go.GetComponent<MeshRenderer>();
		component2.castShadows = false;
		component2.receiveShadows = false;
		component2.material = material;
		if (hitMesh == null)
		{
			Mesh mesh = (component.mesh = new Mesh());
			SetupQuadMesh(mesh, width, height, center, false, uvs);
		}
		else
		{
			component.mesh = hitMesh;
		}
	}
}
