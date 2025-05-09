public class LandmarkEntity : EntityDecorator
{
	public override EntityType Type
	{
		get
		{
			return EntityType.LANDMARK;
		}
	}

	public LandmarkEntity(Entity toDecorate)
		: base(new PurchasableDecorator(toDecorate))
	{
		new StructureDecorator(this);
		new ActivatableDecorator(this);
	}
}
