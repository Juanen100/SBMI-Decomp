using System.IO;
using UnityEngine;

public class BinaryWriter : Writer
{
	private System.IO.BinaryWriter binaryWriter;

	public BinaryWriter()
	{
	}

	public BinaryWriter(string localPath)
	{
		Open(localPath);
	}

	public void Open(string localPath)
	{
	}

	public void Close()
	{
		binaryWriter.Close();
		binaryWriter = null;
	}

	public void Write(bool value)
	{
		binaryWriter.Write(value);
	}

	public void Write(byte value)
	{
		binaryWriter.Write(value);
	}

	public void Write(short value)
	{
		binaryWriter.Write(value);
	}

	public void Write(ushort value)
	{
		binaryWriter.Write(value);
	}

	public void Write(int value)
	{
		binaryWriter.Write(value);
	}

	public void Write(uint value)
	{
		binaryWriter.Write(value);
	}

	public void Write(float value)
	{
		binaryWriter.Write(value);
	}

	public void Write(double value)
	{
		binaryWriter.Write(value);
	}

	public void Write(Vector2 value)
	{
		Write(value.x);
		Write(value.y);
	}

	public void Write(Vector3 value)
	{
		Write(value.x);
		Write(value.y);
		Write(value.z);
	}

	public void Write(AlignedBox value)
	{
		Write(value.xmin);
		Write(value.xmax);
		Write(value.ymin);
		Write(value.ymax);
	}

	public void Write(string value)
	{
		binaryWriter.Write(value);
	}
}
