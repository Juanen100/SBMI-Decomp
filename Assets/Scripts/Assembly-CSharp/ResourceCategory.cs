using System.Collections.Generic;

public class ResourceCategory
{
	public string name;

	public List<ResourceProductGroup> productGroups;

	public static ResourceCategory FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public ResourceProductGroup GetProductGroupByName(string name)
	{
		return null;
	}
}
