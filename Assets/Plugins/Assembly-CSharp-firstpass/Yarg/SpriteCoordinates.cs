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
				return null;
			}
		}

		public SpriteCoordinates()
		{
		}

		public SpriteCoordinates(string asset)
		{
		}

		public bool Reload(Dictionary<string, AtlasCoords> frames)
		{
			return false;
		}

		public void SetMesh(Mesh mesh)
		{
		}
	}
}
