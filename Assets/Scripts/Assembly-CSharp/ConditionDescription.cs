using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 24)]
public struct ConditionDescription
{
	public uint Id;

	public uint OccuranceCount;

	public uint OccurancesRequired;

	public bool IsPassed;

	public string Description;
}
