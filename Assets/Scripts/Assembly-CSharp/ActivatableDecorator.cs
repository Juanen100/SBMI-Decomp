using System.Collections.Generic;

public class ActivatableDecorator : EntityDecorator
{
	public ulong Activated
	{
		get
		{
			object value;
			if (Variable.TryGetValue("activatedTime", out value))
			{
				return (ulong)value;
			}
			return 0uL;
		}
		set
		{
			Variable["activatedTime"] = value;
		}
	}

	public ActivatableDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}

	public override void DeserializeDecorator(Dictionary<string, object> data)
	{
		if (data.ContainsKey("activated_time"))
		{
			Activated = TFUtils.LoadUlong(data, "activated_time");
		}
	}

	public override void SerializeDecorator(ref Dictionary<string, object> data)
	{
		data["activated_time"] = Activated;
	}

	public static void Serialize(ref Dictionary<string, object> data, ulong startTime)
	{
		data["activated_time"] = startTime;
	}
}
