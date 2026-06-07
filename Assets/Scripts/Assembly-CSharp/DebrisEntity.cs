public class DebrisEntity : EntityDecorator
{
	public override EntityType Type
	{
		get
		{
			return default(EntityType);
		}
	}

	public int? ExpansionId
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public DebrisEntity(Entity toDecorate)
		: base(null)
	{
	}
}
