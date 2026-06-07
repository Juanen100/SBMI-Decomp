public class ClearableDecorator : EntityDecorator
{
	public ulong ClearTime
	{
		get
		{
			return 0uL;
		}
	}

	public float ClearTimerDuration
	{
		get
		{
			return 0f;
		}
	}

	public ulong? ClearCompleteTime
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ulong ClearTimeRemaining
	{
		get
		{
			return 0uL;
		}
	}

	public Cost ClearCost
	{
		get
		{
			return null;
		}
	}

	public Cost ClearingRushCost
	{
		get
		{
			return null;
		}
	}

	public RewardDefinition ClearingReward
	{
		get
		{
			return null;
		}
	}

	public bool HasStartedClearing
	{
		get
		{
			return false;
		}
	}

	public ClearableDecorator(Entity toDecorate)
		: base(null)
	{
	}

	public bool IsClearing(ulong utcNow)
	{
		return false;
	}

	public ulong RemainingTime(ulong utcNow)
	{
		return 0uL;
	}
}
