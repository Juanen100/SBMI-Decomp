using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout((LayoutKind)0, Size = 16)]
public struct Segment
{
	public Vector2 first;

	public Vector2 second;
}
