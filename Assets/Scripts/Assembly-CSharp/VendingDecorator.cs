public class VendingDecorator : EntityDecorator
{
	public int VendorId
	{
		get
		{
			return (int)Invariable["vendor_id"];
		}
	}

	public ulong RestockPeriod
	{
		get
		{
			return (ulong)Invariable["restock_time"];
		}
	}

	public ulong RestockTime
	{
		get
		{
			if (!Variable.ContainsKey("restock_time"))
			{
				Variable["restock_time"] = TFUtils.EpochTime();
			}
			return (ulong)Variable["restock_time"];
		}
		set
		{
			Variable["restock_time"] = value;
		}
	}

	public ulong SpecialRestockPeriod
	{
		get
		{
			return (ulong)Invariable["special_time"];
		}
	}

	public ulong SpecialRestockTime
	{
		get
		{
			if (!Variable.ContainsKey("special_time"))
			{
				Variable["special_time"] = TFUtils.EpochTime();
			}
			return (ulong)Variable["special_time"];
		}
		set
		{
			Variable["special_time"] = value;
		}
	}

	public VendingDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}
}
