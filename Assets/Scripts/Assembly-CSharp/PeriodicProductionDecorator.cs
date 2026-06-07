public class PeriodicProductionDecorator : EntityDecorator
{
	public const string PRODUCTION_RUSHABLE = "rent_rushable";

	public bool RentRushable
	{
		get
		{
			return false;
		}
	}

	public ulong RentProductionTime
	{
		get
		{
			return 0uL;
		}
	}

	public float RentTimerDuration
	{
		get
		{
			return 0f;
		}
	}

	public Cost RentRushCost
	{
		get
		{
			return null;
		}
	}

	public ulong ProductReadyTime
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public RewardDefinition Product
	{
		get
		{
			return null;
		}
	}

	public bool HasProduct
	{
		get
		{
			return false;
		}
	}

	public PeriodicProductionDecorator(Entity toDecorate)
		: base(null)
	{
	}

	private void RequireProduction()
	{
	}
}
