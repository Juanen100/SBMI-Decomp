public class TreasureEntity : EntityDecorator
{
	public override EntityType Type
	{
		get
		{
			return default(EntityType);
		}
	}

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

	public bool Quickclear
	{
		get
		{
			return false;
		}
	}

	public TreasureSpawner TreasureTiming
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

	public float RaisingTimeRemaining
	{
		get
		{
			return 0f;
		}
		set
		{
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

	public TreasureEntity(Entity toDecorate)
		: base(null)
	{
	}

	public bool IsClearing(ulong utcNow)
	{
		return false;
	}
}
