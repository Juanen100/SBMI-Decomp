using System.Collections.Generic;

public abstract class LoadableCondition : BaseCondition
{
	private string loadTokenType;

	public bool hasCountField;

	protected string LoadTokenType
	{
		get
		{
			return null;
		}
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICollection<string> relevantTypes)
	{
	}

	protected void Initialize(uint id, uint count, string loadToken, ICollection<string> relevantTypes, IList<uint> prerequisiteConditions)
	{
	}

	public virtual Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
