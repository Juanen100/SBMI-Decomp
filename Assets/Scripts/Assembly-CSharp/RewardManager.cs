using System.Collections.Generic;
using UnityEngine;

public static class RewardManager
{
	public class RewardDropResults
	{
		public Dictionary<string, object> buildingLabels;

		public List<Identity> dropIdentities;

		public RewardDropResults(Dictionary<string, object> buildingLabels, List<Identity> dropIdentities)
		{
		}
	}

	public static void ApplyToGameState(Reward reward, ulong collectionTime, Dictionary<string, object> gameState)
	{
	}

	public static RewardDropResults GenerateRewardDrops(Reward reward, Simulation simulation, Simulated simulated, ulong utcNow, bool bonusReward = false)
	{
		return null;
	}

	public static RewardDropResults GenerateRewardDrops(Reward reward, Simulation simulation, Vector3 dropPosition, ulong utcNow, bool bonusReward = false)
	{
		return null;
	}

	private static void GenerateDividedRewardDrops(Simulation simulation, int resourceDid, List<ItemDropCtor> rewardDrops, ulong utcNow, int amountOfCurrentRewardToDrop, int amountOfNextRewardToDrop)
	{
	}

	public static bool ReleaseDisplayController(Simulation simulation, IDisplayController dc)
	{
		return false;
	}
}
