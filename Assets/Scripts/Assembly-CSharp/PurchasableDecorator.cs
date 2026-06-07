public class PurchasableDecorator : EntityDecorator
{
	public bool Purchased
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public override string SoundOnTouch
	{
		get
		{
			return null;
		}
	}

	public PurchasableDecorator(Entity toDecorate)
		: base(null)
	{
	}
}
