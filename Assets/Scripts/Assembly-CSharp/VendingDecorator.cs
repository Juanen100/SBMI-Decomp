public class VendingDecorator : EntityDecorator
{
	public int VendorId
	{
		get
		{
			return 0;
		}
	}

	public ulong RestockPeriod
	{
		get
		{
			return 0uL;
		}
	}

	public ulong RestockTime
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public ulong SpecialRestockPeriod
	{
		get
		{
			return 0uL;
		}
	}

	public ulong SpecialRestockTime
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public VendingDecorator(Entity toDecorate)
		: base(null)
	{
	}
}
