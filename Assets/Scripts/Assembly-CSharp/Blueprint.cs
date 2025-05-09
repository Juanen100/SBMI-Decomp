using System.Collections.Generic;

public class Blueprint
{
	private Dictionary<string, object> iproperties;

	private ReadWriteIndexer iindexer;

	private Dictionary<string, object> vproperties;

	private ReadWriteIndexer vindexer;

	public ReadWriteIndexer Invariable
	{
		get
		{
			return iindexer;
		}
	}

	public ReadWriteIndexer Variable
	{
		get
		{
			return vindexer;
		}
	}

	public EntityType PrimaryType
	{
		get
		{
			return (EntityType)(int)Invariable["type"];
		}
	}

	public bool Disabled { get; set; }

	public Blueprint()
	{
		Disabled = false;
		iproperties = new Dictionary<string, object>();
		iindexer = new ReadWriteIndexer(iproperties);
		vproperties = new Dictionary<string, object>();
		vindexer = new ReadWriteIndexer(vproperties);
	}

	public int? GetInstanceLimitByLevel(int level)
	{
		if (Invariable["instance_limit"] == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = (Dictionary<int, int>)Invariable["instance_limit"];
		for (int num = level; num >= 0; num--)
		{
			if (dictionary.ContainsKey(num))
			{
				return dictionary[num];
			}
		}
		return null;
	}

	public Dictionary<string, object> InvariableProperties()
	{
		return iproperties;
	}

	public Dictionary<string, object> VariableProperties()
	{
		return TFUtils.CloneDictionary(vproperties);
	}

	public override string ToString()
	{
		if (iproperties.ContainsKey("name") && iproperties.ContainsKey("type"))
		{
			return iproperties["type"].ToString() + " blueprint: " + (string)iproperties["name"];
		}
		return base.ToString();
	}
}
