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
			return 0;
		}
	}

	public int MaxSlots
	{
		get
		{
			return 0;
		}
	}

	public int Did
	{
		get
		{
			return 0;
		}
	}

	public ProductionSlotTable(Dictionary<string, object> data)
	{
	}

	public ProductionSlotTable(int did, int initSlots, List<Cost> costs)
	{
	}

	public Cost GetCostForSlot(int slotId)
	{
		return null;
	}
}
