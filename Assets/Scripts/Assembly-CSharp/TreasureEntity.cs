public class TreasureEntity : EntityDecorator
{
	public override EntityType Type
	{
		get
		{
			return EntityType.TREASURE;
		}
	}

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

	public bool Quickclear
	{
		get
		{
			return (bool)Invariable["quick_clear"];
		}
	}

	public TreasureSpawner TreasureTiming
	{
		get
		{
			if (!Variable.ContainsKey("treasure_timing"))
			{
				return null;
			}
			return (TreasureSpawner)Variable["treasure_timing"];
		}
		set
		{
			Variable["treasure_timing"] = value;
		}
	}

	public ulong ClearTimeRemaining
	{
		get
		{
			return (!ClearCompleteTime.HasValue) ? 0 : (ClearCompleteTime.Value - TFUtils.EpochTime());
		}
	}

	public float RaisingTimeRemaining
	{
		get
		{
			if (Variable.ContainsKey("raising_time"))
			{
				return (float)Variable["raising_time"];
			}
			return 0f;
		}
		set
		{
			Variable["raising_time"] = value;
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

	public TreasureEntity(Entity toDecorate)
		: base(toDecorate)
	{
		new StructureDecorator(this);
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
}
