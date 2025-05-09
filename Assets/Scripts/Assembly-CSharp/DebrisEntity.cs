public class DebrisEntity : EntityDecorator
{
	public override EntityType Type
	{
		get
		{
			return EntityType.DEBRIS;
		}
	}

	public int? ExpansionId
	{
		get
		{
			object value = null;
			if (Variable.TryGetValue("expansionId", out value))
			{
				return (int?)value;
			}
			return null;
		}
		set
		{
			Variable["expansionId"] = value;
		}
	}

	public DebrisEntity(Entity toDecorate)
		: base(toDecorate)
	{
		new PurchasableDecorator(this);
		new ClearableDecorator(this);
		new StructureDecorator(this);
	}
}
