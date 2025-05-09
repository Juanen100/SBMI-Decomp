#define ASSERTS_ON
using System.Collections.Generic;

public class ProductionSlotTable
{
	public const string TYPE = "slot_costs";

	private int did;

	private int initSlots;

	private List<Cost> costs;

	public int MinSlots
	{
		get
		{
			return initSlots;
		}
	}

	public int MaxSlots
	{
		get
		{
			return initSlots + costs.Count;
		}
	}

	public int Did
	{
		get
		{
			return did;
		}
	}

	public ProductionSlotTable(Dictionary<string, object> data)
	{
		did = TFUtils.LoadInt(data, "did");
		initSlots = TFUtils.LoadInt(data, "init_slots");
		costs = new List<Cost>();
		List<object> list = TFUtils.LoadList<object>(data, "costs");
		foreach (object item in list)
		{
			costs.Add(Cost.FromObject(item));
		}
	}

	public ProductionSlotTable(int did, int initSlots, List<Cost> costs)
	{
		this.did = did;
		this.initSlots = initSlots;
		this.costs = costs;
		TFUtils.Assert(costs.Count > 0, "Should not be defining a Production Slot Table with no costs: did " + did);
	}

	public Cost GetCostForSlot(int slotId)
	{
		if (slotId >= MaxSlots)
		{
			return null;
		}
		if (slotId < initSlots)
		{
			return costs[0];
		}
		return costs[slotId - initSlots];
	}
}
