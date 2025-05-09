using UnityEngine;

public interface Writer
{
	void Write(bool value);

	void Write(byte value);

	void Write(short value);

	void Write(ushort value);

	void Write(int value);

	void Write(uint value);

	void Write(float value);

	void Write(double value);

	void Write(Vector2 value);

	void Write(Vector3 value);

	void Write(AlignedBox value);

	void Write(string value);
}
