using System.Collections.Generic;

public class VendorStock
{
	public const string TYPE = "vendor_stock";

	private int did;

	private string name;

	private string description;

	private int minLevel;

	private string icon;

	private RewardDefinition rewardDefinition;

	private CdfDictionary<Cost> costs;

	private ResultGenerator instances;

	public int Did
	{
		get
		{
			return 0;
		}
	}

	public string Name
	{
		get
		{
			return null;
		}
	}

	public string Description
	{
		get
		{
			return null;
		}
	}

	public int MinimumLevel
	{
		get
		{
			return 0;
		}
	}

	public string Icon
	{
		get
		{
			return null;
		}
	}

	public VendorStock(int did, string name, string description, string icon, int minLevel, RewardDefinition rewardDefinition, CdfDictionary<Cost> costs, ResultGenerator instances)
	{
	}

	public static VendorStock FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public VendingInstance GenerateVendingInstance(int slotId, bool special)
	{
		return null;
	}

	public Reward GenerateReward(Simulation simulation)
	{
		return null;
	}
}
