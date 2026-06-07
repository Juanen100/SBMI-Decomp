using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 1)]
public struct LandmarkAction
{
	public const string UNPURCHASED = "unpurchased";

	public const string INACTIVE = "inactive";

	public const string ACTIVE = "active";
}
