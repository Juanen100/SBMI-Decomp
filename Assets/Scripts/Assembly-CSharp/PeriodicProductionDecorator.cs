using System;

public class PeriodicProductionDecorator : EntityDecorator
{
	public const string PRODUCTION_RUSHABLE = "rent_rushable";

	public bool RentRushable
	{
		get
		{
			RequireProduction();
			return (bool)Invariable["rent_rushable"];
		}
	}

	public ulong RentProductionTime
	{
		get
		{
			RequireProduction();
			return (ulong)Invariable["time.production"];
		}
	}

	public float RentTimerDuration
	{
		get
		{
			RequireProduction();
			return (float)Invariable["rent_timer_duration"];
		}
	}

	public Cost RentRushCost
	{
		get
		{
			return (Cost)Invariable["rent_rush_cost"];
		}
	}

	public ulong ProductReadyTime
	{
		get
		{
			RequireProduction();
			if (!Variable.ContainsKey("product.ready"))
			{
				Variable["product.ready"] = TFUtils.EpochTime() + RentProductionTime;
			}
			return (ulong)Variable["product.ready"];
		}
		set
		{
			RequireProduction();
			Variable["product.ready"] = value;
		}
	}

	public RewardDefinition Product
	{
		get
		{
			RequireProduction();
			return (RewardDefinition)Invariable["product"];
		}
	}

	public bool HasProduct
	{
		get
		{
			return Invariable["product"] != null;
		}
	}

	public PeriodicProductionDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}

	private void RequireProduction()
	{
		if (Invariable["product"] == null)
		{
			throw new InvalidOperationException("Building does not produce rent");
		}
	}
}
