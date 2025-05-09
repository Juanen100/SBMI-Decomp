#define ASSERTS_ON
using System.Collections.Generic;

public class ResourceCategory
{
	public string name;

	public List<ResourceProductGroup> productGroups = new List<ResourceProductGroup>();

	public static ResourceCategory FromDict(Dictionary<string, object> data)
	{
		ResourceCategory resourceCategory = new ResourceCategory();
		TFUtils.Assert(data.Count == 1, "Invalid category_to_productgroups array. Each category should only contain a single list of product groups.");
		foreach (string key in data.Keys)
		{
			resourceCategory.name = key;
			List<string> list = TFUtils.LoadList<string>(data, resourceCategory.name);
			foreach (string item in list)
			{
				ResourceProductGroup resourceProductGroup = new ResourceProductGroup();
				resourceProductGroup.name = item;
				resourceCategory.productGroups.Add(resourceProductGroup);
			}
		}
		return resourceCategory;
	}

	public ResourceProductGroup GetProductGroupByName(string name)
	{
		foreach (ResourceProductGroup productGroup in productGroups)
		{
			if (productGroup.name.Equals(name))
			{
				return productGroup;
			}
		}
		return null;
	}
}
