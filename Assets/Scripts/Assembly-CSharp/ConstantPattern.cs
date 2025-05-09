public class ConstantPattern : PeriodicPattern
{
	protected float constant;

	public ConstantPattern(float constant)
	{
		this.constant = constant;
	}

	public override float ValueAtTime(float atTime)
	{
		return constant;
	}
}
