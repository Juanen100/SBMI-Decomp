using System.Collections.Generic;

public class ResourceCalculatorManager
{
	private Dictionary<int, IResourceProgressCalculator> calculators;

	public ResourceCalculatorManager(LevelingManager levelingManager)
	{
		calculators = new Dictionary<int, IResourceProgressCalculator>();
		calculators[ResourceManager.XP] = levelingManager;
	}

	public IResourceProgressCalculator GetResourceCalculator(int resourceId)
	{
		IResourceProgressCalculator value = null;
		calculators.TryGetValue(resourceId, out value);
		return value;
	}
}
