public abstract class PeriodicPattern
{
	protected float period;

	protected float minimum;

	protected float maximum;

	protected float timeOffset;

	public PeriodicPattern()
	{
	}

	protected void Initialize(float minimum, float maximum, float period, float timeOffset)
	{
	}

	public abstract float ValueAtTime(float atTime);
}
