using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yarg
{
	[Serializable]
	public class SpriteCoordinates
	{
		public string name;

		[HideInInspector]
		public Rect coords;

		[HideInInspector]
		public Vector3[] verts;

		[HideInInspector]
		public Vector3[] normals;

		[HideInInspector]
		public Color[] color;

		[HideInInspector]
		public int[] tris;

		[HideInInspector]
		public Vector2[] uvs;

		public YGSprite.MeshUpdate MeshUpdate
		{
			get
			{
				return new YGSprite.MeshUpdate(this);
			}
		}

		public SpriteCoordinates()
		{
		}

		public SpriteCoordinates(string asset)
		{
			name = asset;
		}

		public bool Reload(Dictionary<string, AtlasCoords> frames)
		{
			if (!frames.ContainsKey(name))
			{
				Debug.LogError("couldn't reload " + name);
				return false;
			}
			coords = frames[name].frame;
			return true;
		}

		public void SetMesh(Mesh mesh)
		{
			verts = mesh.vertices;
			normals = mesh.normals;
			color = mesh.colors;
			tris = mesh.triangles;
			uvs = mesh.uv;
		}
	}
}
