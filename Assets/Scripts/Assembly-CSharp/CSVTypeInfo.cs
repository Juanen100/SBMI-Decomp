using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 16)]
public struct CSVTypeInfo
{
	public TypeID id;

	public string colName;
}
