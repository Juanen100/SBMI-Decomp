using System.Collections.Generic;

public class VendorDefinition
{
	public const string TYPE = "vendor";

	public const int COUNT = 12;

	public List<int> generalStock;

	public List<int> specialStock;

	public int did;

	public string sessionActionId;

	public string cancelButtonTexture;

	public string titleTexture;

	public string titleIconTexture;

	public List<int> backgroundColor;

	public string buttonLabel;

	public string openSound;

	public string closeSound;

	public string music;

	private Cost rushCost;

	private int count;

	public Cost RushCost
	{
		get
		{
			return null;
		}
	}

	public int InstanceCount
	{
		get
		{
			return 0;
		}
	}

	public VendorDefinition(Dictionary<string, object> data)
	{
	}
}
