using UnityEngine;

public interface Reader
{
	void Read(out bool value);

	void Read(out byte value);

	void Read(out short value);

	void Read(out ushort value);

	void Read(out int value);

	void Read(out uint value);

	void Read(out float value);

	void Read(out double value);

	void Read(out Vector2 value);

	void Read(out Vector3 value);

	void Read(out AlignedBox value);

	void Read(out string value);
}
