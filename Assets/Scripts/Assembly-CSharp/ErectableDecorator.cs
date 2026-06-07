using System.Collections.Generic;

public class ErectableDecorator : EntityDecorator
{
	public Cost BuildRushCost
	{
		get
		{
			return null;
		}
	}

	public ulong ErectionTime
	{
		get
		{
			return 0uL;
		}
	}

	public float ErectionTimerDuration
	{
		get
		{
			return 0f;
		}
	}

	public ulong? ErectionCompleteTime
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public double RaisingTimeRemaining
	{
		get
		{
			return 0.0;
		}
		set
		{
		}
	}

	public RewardDefinition CompletionReward
	{
		get
		{
			return null;
		}
	}

	public RewardDefinition UpgradeReward
	{
		get
		{
			return null;
		}
	}

	public int? UpgradeLevel
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ulong UpgradeTime
	{
		get
		{
			return 0uL;
		}
	}

	public ulong? UpgradeCompleteTime
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ulong? UpgradeStartedTime
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ulong? UpgradeFinishTime
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ErectableDecorator(Entity toDecorate)
		: base(null)
	{
	}

	public bool IsErecting(ulong utcNow)
	{
		return false;
	}

	public bool IsUpgrading(ulong utcNow)
	{
		return false;
	}

	public void IncrementUpgradeLevel()
	{
	}

	public override void DeserializeDecorator(Dictionary<string, object> data)
	{
	}

	public override void SerializeDecorator(ref Dictionary<string, object> data)
	{
	}

	public static void Serialize(ref Dictionary<string, object> data, ulong completeTime)
	{
	}
}
