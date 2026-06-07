public class ConstantPattern : PeriodicPattern
{
	protected float constant;

	public ConstantPattern(float constant)
	{
	}

	public override float ValueAtTime(float atTime)
	{
		return 0f;
	}
}
