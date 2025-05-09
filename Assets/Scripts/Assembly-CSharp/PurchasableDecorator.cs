public class PurchasableDecorator : EntityDecorator
{
	public bool Purchased
	{
		get
		{
			if (!Variable.ContainsKey("purchased"))
			{
				return true;
			}
			return (bool)Variable["purchased"];
		}
		set
		{
			Variable["purchased"] = value;
		}
	}

	public override string SoundOnTouch
	{
		get
		{
			if (Purchased)
			{
				return base.SoundOnTouch;
			}
			return (string)Invariable["sound_on_touch_error"];
		}
	}

	public PurchasableDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}
}
