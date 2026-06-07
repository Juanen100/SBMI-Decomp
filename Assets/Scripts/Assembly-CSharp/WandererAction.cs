using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 1)]
public struct WandererAction
{
	public const string SPAWN = "spawn";

	public const string IDLE = "idle";

	public const string WANDERING = "wandering";

	public const string CLICKED = "clicked";

	public const string FLEEING = "fleeing";

	public const string HIDDEN = "hidden";

	public const string CHEERING = "cheering";
}
