public class ClearableDecorator : EntityDecorator
{
	public ulong ClearTime
	{
		get
		{
			return (ulong)Invariable["time.clear"];
		}
	}

	public float ClearTimerDuration
	{
		get
		{
			return (float)Invariable["timer_duration"];
		}
	}

	public ulong? ClearCompleteTime
	{
		get
		{
			if (!Variable.ContainsKey("clearCompleteTime"))
			{
				return null;
			}
			return (ulong?)Variable["clearCompleteTime"];
		}
		set
		{
			Variable["clearCompleteTime"] = value;
		}
	}

	public ulong ClearTimeRemaining
	{
		get
		{
			return (!ClearCompleteTime.HasValue) ? 0 : (ClearCompleteTime.Value - TFUtils.EpochTime());
		}
	}

	public Cost ClearCost
	{
		get
		{
			return (Cost)Invariable["cost"];
		}
	}

	public Cost ClearingRushCost
	{
		get
		{
			return (Cost)Invariable["clear_rush_cost"];
		}
	}

	public RewardDefinition ClearingReward
	{
		get
		{
			return (RewardDefinition)Invariable["clearing_reward"];
		}
	}

	public bool HasStartedClearing
	{
		get
		{
			if (!Variable.ContainsKey("clearCompleteTime"))
			{
				return false;
			}
			return ClearCompleteTime != 0;
		}
	}

	public ClearableDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}

	public bool IsClearing(ulong utcNow)
	{
		int result;
		if (HasStartedClearing)
		{
			ulong? clearCompleteTime = ClearCompleteTime;
			result = ((clearCompleteTime.HasValue && utcNow < clearCompleteTime.Value) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public ulong RemainingTime(ulong utcNow)
	{
		return ClearCompleteTime.Value - utcNow;
	}
}
