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
		Open(resourceName);
	}

	public void Open(string resourceName)
	{
		TextAsset textAsset = Resources.Load(resourceName) as TextAsset;
		if (textAsset != null)
		{
			binaryReader = new System.IO.BinaryReader(new MemoryStream(textAsset.bytes));
		}
	}

	public void Close()
	{
		binaryReader.Close();
		binaryReader = null;
	}

	public void Read(out bool value)
	{
		value = binaryReader.ReadBoolean();
	}

	public void Read(out byte value)
	{
		value = binaryReader.ReadByte();
	}

	public void Read(out short value)
	{
		value = binaryReader.ReadInt16();
	}

	public void Read(out ushort value)
	{
		value = binaryReader.ReadUInt16();
	}

	public void Read(out int value)
	{
		value = binaryReader.ReadInt32();
	}

	public void Read(out uint value)
	{
		value = binaryReader.ReadUInt32();
	}

	public void Read(out float value)
	{
		value = binaryReader.ReadSingle();
	}

	public void Read(out double value)
	{
		value = binaryReader.ReadDouble();
	}

	public void Read(out Vector2 value)
	{
		Read(out value.x);
		Read(out value.y);
	}

	public void Read(out Vector3 value)
	{
		Read(out value.x);
		Read(out value.y);
		Read(out value.z);
	}

	public void Read(out AlignedBox value)
	{
		value = new AlignedBox();
		Read(out value.xmin);
		Read(out value.xmax);
		Read(out value.ymin);
		Read(out value.ymax);
	}

	public void Read(out string value)
	{
		value = binaryReader.ReadString();
	}
}
