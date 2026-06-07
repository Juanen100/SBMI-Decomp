using System.IO;
using UnityEngine;

public class BinaryReader : Reader
{
	private System.IO.BinaryReader binaryReader;

	public BinaryReader()
	{
	}

	public BinaryReader(string resourceName)
	{
	}

	public void Open(string resourceName)
	{
	}

	public void Close()
	{
	}

	public void Read(out bool value)
	{
		value = default(bool);
	}

	public void Read(out byte value)
	{
		value = default(byte);
	}

	public void Read(out short value)
	{
		value = default(short);
	}

	public void Read(out ushort value)
	{
		value = default(ushort);
	}

	public void Read(out int value)
	{
		value = default(int);
	}

	public void Read(out uint value)
	{
		value = default(uint);
	}

	public void Read(out float value)
	{
		value = default(float);
	}

	public void Read(out double value)
	{
		value = default(double);
	}

	public void Read(out Vector2 value)
	{
		value = default(Vector2);
	}

	public void Read(out Vector3 value)
	{
		value = default(Vector3);
	}

	public void Read(out AlignedBox value)
	{
		value = null;
	}

	public void Read(out string value)
	{
		value = null;
	}
}
