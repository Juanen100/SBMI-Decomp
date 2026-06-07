using System.Collections.Generic;

public class ActivatableDecorator : EntityDecorator
{
	public ulong Activated
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public ActivatableDecorator(Entity toDecorate)
		: base(null)
	{
	}

	public override void DeserializeDecorator(Dictionary<string, object> data)
	{
	}

	public override void SerializeDecorator(ref Dictionary<string, object> data)
	{
	}

	public static void Serialize(ref Dictionary<string, object> data, ulong startTime)
	{
	}
}
