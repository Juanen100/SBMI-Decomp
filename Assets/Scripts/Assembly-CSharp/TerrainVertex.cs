using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout((LayoutKind)0, Size = 20)]
public struct TerrainVertex
{
	public Vector3 position;

	public Vector2 texcoord;
}
