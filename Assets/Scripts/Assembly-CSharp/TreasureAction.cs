using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 1)]
public struct TreasureAction
{
	public const string BURIED = "buried";

	public const string UNCOVERING = "uncovering";

	public const string CLAIMING = "claiming";

	public const string DELETING = "deleting";
}
