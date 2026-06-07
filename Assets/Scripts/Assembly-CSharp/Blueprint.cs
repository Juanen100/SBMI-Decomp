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
			return null;
		}
	}

	public ReadWriteIndexer Variable
	{
		get
		{
			return null;
		}
	}

	public EntityType PrimaryType
	{
		get
		{
			return default(EntityType);
		}
	}

	public bool Disabled { get; set; }

	public int? GetInstanceLimitByLevel(int level)
	{
		return null;
	}

	public Dictionary<string, object> InvariableProperties()
	{
		return null;
	}

	public Dictionary<string, object> VariableProperties()
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
