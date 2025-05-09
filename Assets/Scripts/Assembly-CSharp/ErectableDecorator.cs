using System.Collections.Generic;

public class ErectableDecorator : EntityDecorator
{
	public Cost BuildRushCost
	{
		get
		{
			return (Cost)Invariable["build_rush_cost"];
		}
	}

	public ulong ErectionTime
	{
		get
		{
			return (ulong)Invariable["time.build"];
		}
	}

	public float ErectionTimerDuration
	{
		get
		{
			return (float)Invariable["build_timer_duration"];
		}
	}

	public ulong? ErectionCompleteTime
	{
		get
		{
			if (Variable.ContainsKey("buildCompleteTime"))
			{
				return (ulong?)Variable["buildCompleteTime"];
			}
			return null;
		}
		set
		{
			Variable["buildCompleteTime"] = value;
		}
	}

	public double RaisingTimeRemaining
	{
		get
		{
			if (Variable.ContainsKey("raising_time"))
			{
				return (double)Variable["raising_time"];
			}
			return 0.0;
		}
		set
		{
			Variable["raising_time"] = value;
		}
	}

	public RewardDefinition CompletionReward
	{
		get
		{
			return (RewardDefinition)Invariable["completion_reward"];
		}
	}

	public ErectableDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}

	public bool IsErecting(ulong utcNow)
	{
		int result;
		if (Variable.ContainsKey("buildCompleteTime"))
		{
			ulong? erectionCompleteTime = ErectionCompleteTime;
			result = ((erectionCompleteTime.HasValue && utcNow < erectionCompleteTime.Value) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override void DeserializeDecorator(Dictionary<string, object> data)
	{
		if (data.ContainsKey("activated_time"))
		{
			ErectionCompleteTime = TFUtils.LoadUlong(data, "activated_time");
			if (ErectionCompleteTime == 0)
			{
				ErectionCompleteTime = TFUtils.LoadUlong(data, "build_finish_time");
			}
		}
		else if (data.ContainsKey("build_finish_time"))
		{
			ErectionCompleteTime = TFUtils.LoadUlong(data, "build_finish_time");
		}
	}

	public override void SerializeDecorator(ref Dictionary<string, object> data)
	{
		data["build_finish_time"] = ErectionCompleteTime;
	}

	public static void Serialize(ref Dictionary<string, object> data, ulong completeTime)
	{
		data["build_finish_time"] = completeTime;
	}
}
