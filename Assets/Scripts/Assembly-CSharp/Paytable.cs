using System.Collections.Generic;

public class Paytable
{
	private const string DID = "did";

	private const string WAGERS = "wagers";

	public static Reward CONSOLATION_REWARD;

	public static Reward CONSOLATION_REWARD_HALLOWEEN;

	private const uint DYNAMIC_ID = 0u;

	private uint did;

	private Dictionary<uint, RewardDefinition> wagers;

	public uint Did
	{
		get
		{
			return 0u;
		}
	}

	public static Paytable FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public Paytable Join(Paytable that)
	{
		return null;
	}

	public void Normalize()
	{
	}

	public bool CanWager(uint wager)
	{
		return false;
	}

	public void ValidateProbabilities()
	{
	}

	public Reward Spin(uint wager, Simulation simulation, Reward consolationReward)
	{
		return null;
	}

	public Reward Spin(int wager, Simulation simulation, Reward consolationReward)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
