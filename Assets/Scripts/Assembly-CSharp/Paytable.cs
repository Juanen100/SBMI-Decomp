using System.Collections.Generic;

public class Paytable
{
	private const string DID = "did";

	private const string WAGERS = "wagers";

	private const uint DYNAMIC_ID = 0u;

	public static Reward CONSOLATION_REWARD = new Reward(new Dictionary<int, int> { 
	{
		ResourceManager.SOFT_CURRENCY,
		5
	} }, null, null, null, null, null, null, null, false, null);

	public static Reward CONSOLATION_REWARD_HALLOWEEN = new Reward(new Dictionary<int, int> { 
	{
		ResourceManager.HALLOWEEN_CURRENCY,
		1
	} }, null, null, null, null, null, null, null, false, null);

	private uint did;

	private Dictionary<uint, RewardDefinition> wagers;

	public uint Did
	{
		get
		{
			return did;
		}
	}

	public static Paytable FromDict(Dictionary<string, object> data)
	{
		Paytable paytable = new Paytable();
		paytable.did = TFUtils.LoadUint(data, "did");
		paytable.wagers = new Dictionary<uint, RewardDefinition>();
		Dictionary<string, object> dictionary = TFUtils.LoadDict(data, "wagers");
		foreach (string key in dictionary.Keys)
		{
			paytable.wagers[uint.Parse(key)] = RewardDefinition.FromObject(dictionary[key]);
		}
		return paytable;
	}

	public Paytable Join(Paytable that)
	{
		Paytable paytable = new Paytable();
		paytable.wagers = new Dictionary<uint, RewardDefinition>(wagers);
		if (that != null)
		{
			foreach (uint key in that.wagers.Keys)
			{
				if (paytable.wagers.ContainsKey(key))
				{
					paytable.wagers[key] = paytable.wagers[key].Join(that.wagers[key]);
				}
				else
				{
					paytable.wagers.Add(key, that.wagers[key]);
				}
			}
		}
		return paytable;
	}

	public void Normalize()
	{
		foreach (RewardDefinition value in wagers.Values)
		{
			value.Normalize();
		}
	}

	public bool CanWager(uint wager)
	{
		return wagers.ContainsKey(wager);
	}

	public void ValidateProbabilities()
	{
		foreach (RewardDefinition value in wagers.Values)
		{
			value.Validate(true);
		}
	}

	public Reward Spin(uint wager, Simulation simulation, Reward consolationReward)
	{
		if (wagers.ContainsKey(wager))
		{
			return wagers[wager].GenerateReward(simulation, consolationReward, false, false);
		}
		return null;
	}

	public Reward Spin(int wager, Simulation simulation, Reward consolationReward)
	{
		if (wagers.ContainsKey((uint)wager))
		{
			return wagers[(uint)wager].GenerateReward(simulation, consolationReward, false, false);
		}
		return null;
	}

	public override string ToString()
	{
		string text = "[Paytable (wagers=[\n";
		foreach (KeyValuePair<uint, RewardDefinition> wager in wagers)
		{
			string text2 = text;
			text = text2 + "  wager(" + wager.Key + "): ";
			text = string.Concat(text, wager.Value, "\n");
		}
		return text + "] )]";
	}
}
