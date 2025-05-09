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
			return stockId;
		}
	}

	public int SlotId
	{
		get
		{
			return slotId;
		}
	}

	public Cost Cost
	{
		get
		{
			return cost;
		}
	}

	public bool Special
	{
		get
		{
			return special;
		}
	}

	public VendingInstance(int slotId, int stockId, int remaining, Cost cost, bool special)
	{
		this.slotId = slotId;
		this.stockId = stockId;
		this.remaining = remaining;
		this.cost = cost;
		this.special = special;
	}

	public static VendingInstance FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "slot_id");
		int num2 = TFUtils.LoadInt(data, "stock_id");
		int num3 = TFUtils.LoadInt(data, "remaining");
		Cost cost = Cost.FromObject(data["cost"]);
		bool flag = TFUtils.LoadBool(data, "special");
		return new VendingInstance(num, num2, num3, cost, flag);
	}

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["slot_id"] = slotId;
		dictionary["stock_id"] = stockId;
		dictionary["remaining"] = remaining;
		dictionary["cost"] = cost.ToDict();
		dictionary["special"] = special;
		return dictionary;
	}

	public override string ToString()
	{
		return string.Concat("[VendingInstance (vendorStockId= ", stockId, ", cost= ", cost, ", slotId= ", slotId, ", remaining= ", remaining, ", special= ", special, ")]");
	}
}
