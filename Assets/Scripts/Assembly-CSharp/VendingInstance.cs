using System.Collections.Generic;

public class VendingInstance
{
	public int remaining;

	private int stockId;

	private int slotId;

	private Cost cost;

	private bool special;

	public int StockId
	{
		get
		{
			return 0;
		}
	}

	public int SlotId
	{
		get
		{
			return 0;
		}
	}

	public Cost Cost
	{
		get
		{
			return null;
		}
	}

	public bool Special
	{
		get
		{
			return false;
		}
	}

	public VendingInstance(int slotId, int stockId, int remaining, Cost cost, bool special)
	{
	}

	public static VendingInstance FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
