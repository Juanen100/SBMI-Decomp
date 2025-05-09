using System.Collections.Generic;

public interface IResourceProgressCalculator
{
	void GetRewardsForIncreasingResource(Simulation simulation, Dictionary<int, Resource> currentResources, int amountToIncrease, out List<Reward> rewards);

	float ComputeProgressPercentage(Dictionary<int, Resource> currentResources);

	string ComputeProgressFraction(Dictionary<int, Resource> currentResources);

	int GetResourceType();
}
