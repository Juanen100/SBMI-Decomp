using System.Collections.Generic;

public class ResourceCalculatorManager
{
	private Dictionary<int, IResourceProgressCalculator> calculators;

	public ResourceCalculatorManager(LevelingManager levelingManager)
	{
	}

	public IResourceProgressCalculator GetResourceCalculator(int resourceId)
	{
		return null;
	}
}
