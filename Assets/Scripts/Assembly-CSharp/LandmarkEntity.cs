public class LandmarkEntity : EntityDecorator
{
	public override EntityType Type
	{
		get
		{
			return default(EntityType);
		}
	}

	public LandmarkEntity(Entity toDecorate)
		: base(null)
	{
	}
}
