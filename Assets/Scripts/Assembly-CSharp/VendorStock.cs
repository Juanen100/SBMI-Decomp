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
			return did;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
	}

	public int MinimumLevel
	{
		get
		{
			return minLevel;
		}
	}

	public string Icon
	{
		get
		{
			return icon;
		}
	}

	public VendorStock(int did, string name, string description, string icon, int minLevel, RewardDefinition rewardDefinition, CdfDictionary<Cost> costs, ResultGenerator instances)
	{
		this.icon = icon;
		this.did = did;
		this.name = name;
		this.description = description;
		this.minLevel = minLevel;
		this.rewardDefinition = rewardDefinition;
		this.costs = costs;
		this.instances = instances;
	}

	public static VendorStock FromDict(Dictionary<string, object> data)
	{
		if (data == null)
		{
			return null;
		}
		int num = TFUtils.LoadInt(data, "did");
		string arg = TFUtils.LoadString(data, "name");
		string text = TFUtils.LoadString(data, "description");
		int num2 = TFUtils.LoadInt(data, "minimum_level");
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(data["reward"]);
		CdfDictionary<Cost>.ParseT parser = (object val) => Cost.FromObject(val);
		List<object> data2 = TFUtils.LoadList<object>(data, "costs");
		CdfDictionary<Cost> cdfDictionary = CdfDictionary<Cost>.FromList(data2, parser);
		cdfDictionary.Validate(true, string.Format("VendorStock id {0} name {1}", num, arg));
		ResultGenerator resultGenerator = null;
		resultGenerator = ((data["instances"] is Dictionary<string, object>) ? new ProbabilityTable((Dictionary<string, object>)data["instances"]) : ((!(data["instances"] is List<object>)) ? ((ResultGenerator)new ConstantGenerator(data["instances"].ToString())) : ((ResultGenerator)new UniformGenerator((List<object>)data["instances"]))));
		string text2 = (string)data["icon"];
		return new VendorStock(num, arg, text, text2, num2, rewardDefinition, cdfDictionary, resultGenerator);
	}

	public VendingInstance GenerateVendingInstance(int slotId, bool special)
	{
		string result = instances.GetResult();
		int remaining = ((result == null) ? 1 : int.Parse(result));
		return new VendingInstance(slotId, did, remaining, costs.Spin(), special);
	}

	public Reward GenerateReward(Simulation simulation)
	{
		return rewardDefinition.GenerateReward(simulation, true);
	}
}
