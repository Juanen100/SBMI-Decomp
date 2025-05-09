using System.Collections.Generic;

public abstract class LoadableCondition : BaseCondition
{
	private string loadTokenType;

	public bool hasCountField = true;

	protected string LoadTokenType
	{
		get
		{
			return loadTokenType;
		}
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICollection<string> relevantTypes)
	{
		uint? num = TFUtils.TryLoadUint(loadedData, "count");
		if (!num.HasValue)
		{
			num = 1u;
			hasCountField = false;
		}
		List<uint> list = new List<uint>();
		if (loadedData.ContainsKey("prerequisite_conditions"))
		{
			list = TFUtils.LoadList<uint>(loadedData, "prerequisite_conditions");
		}
		Initialize(TFUtils.LoadUint(loadedData, "id"), num.Value, loadToken, relevantTypes, list);
	}

	protected void Initialize(uint id, uint count, string loadToken, ICollection<string> relevantTypes, IList<uint> prerequisiteConditions)
	{
		Initialize(id, count, relevantTypes, prerequisiteConditions);
		loadTokenType = loadToken;
	}

	public virtual Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = loadTokenType;
		dictionary["id"] = Id;
		dictionary["count"] = Count;
		return dictionary;
	}

	public override string ToString()
	{
		return "LoadableCondition:(loadTokenType=" + loadTokenType + ", " + base.ToString() + ")";
	}
}
