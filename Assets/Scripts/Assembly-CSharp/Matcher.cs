#define ASSERTS_ON
using System.Collections.Generic;

public abstract class Matcher : IMatcher
{
	public const string OPERATOR = "operator";

	public const string LOPERAND = "loperand";

	public const string ROPERAND = "roperand";

	private Dictionary<string, MatchableProperty> matchableProperties;

	private bool hasRequirements;

	public ICollection<string> Keys
	{
		get
		{
			return matchableProperties.Keys;
		}
	}

	protected Matcher()
		: this(new Dictionary<string, MatchableProperty>())
	{
	}

	protected Matcher(Dictionary<string, MatchableProperty> matchableProperties)
	{
		if (matchableProperties.Count > 0)
		{
			hasRequirements = true;
		}
		this.matchableProperties = matchableProperties;
	}

	public virtual uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		uint num = 1u;
		foreach (string key in matchableProperties.Keys)
		{
			MatchableProperty matchableProperty = matchableProperties[key];
			if (matchableProperty.IsRequired)
			{
				num *= matchableProperty.Evaluate(data, game);
			}
		}
		return num;
	}

	public bool IsRequired(string property)
	{
		TFUtils.Assert(matchableProperties.ContainsKey(property), string.Format("Can't query a property({0}) that has not been added!", property));
		return matchableProperties[property].IsRequired;
	}

	public bool HasRequirements()
	{
		return hasRequirements;
	}

	protected MatchableProperty GetProperty(string key)
	{
		TFUtils.Assert(matchableProperties.ContainsKey(key), "Cannot get property for key=" + key + " since it does not have a registered matchable property.");
		return matchableProperties[key];
	}

	public object GetTargetObject(string propertyKey)
	{
		return matchableProperties[propertyKey].Target;
	}

	public string GetTarget(string propertyKey)
	{
		return matchableProperties[propertyKey].Target.ToString();
	}

	public abstract string DescribeSubject(Game game);

	protected bool RegisterProperty(string key, Dictionary<string, object> data)
	{
		return RegisterProperty(key, data, DefaultMatchFn);
	}

	protected bool RegisterProperty(string key, Dictionary<string, object> data, MatchableProperty.MatchFn matchDelegate)
	{
		if (data.ContainsKey(key))
		{
			hasRequirements = true;
			Dictionary<string, object> dictionary = data[key] as Dictionary<string, object>;
			if (dictionary != null)
			{
				return AddRequiredProperty(key, dictionary, matchDelegate);
			}
			return AddRequiredProperty(key, data[key].ToString(), matchDelegate);
		}
		matchableProperties[key] = new MatchableProperty(false, key, null, matchDelegate);
		return false;
	}

	protected bool AddRequiredProperty(string key, object val)
	{
		return AddRequiredProperty(key, val, DefaultMatchFn);
	}

	protected bool AddRequiredProperty(string key, object val, MatchableProperty.MatchFn matchDelegate)
	{
		AssertNotDuplicate(key);
		matchableProperties[key] = new MatchableProperty(true, key, val, matchDelegate);
		return true;
	}

	private void AssertNotDuplicate(string key)
	{
		TFUtils.Assert(!matchableProperties.ContainsKey(key), "Already have value for this key!");
	}

	private static uint DefaultMatchFn(MatchableProperty property, Dictionary<string, object> triggerData, Game game)
	{
		TFUtils.Assert(property.IsRequired, "Should not be trying to match against an optional parameter!");
		if (!triggerData.ContainsKey(property.Key))
		{
			return 0u;
		}
		string text = triggerData[property.Key].ToString();
		if (text.Equals(property.Target))
		{
			return 1u;
		}
		return 0u;
	}

	public override string ToString()
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, MatchableProperty> matchableProperty in matchableProperties)
		{
			text += matchableProperty.Value.ToString();
			text += ", ";
		}
		return "Matcher:(properties=[" + text + "])";
	}

	protected uint CompareOperandRangesToAmount(object target, int amount)
	{
		int result;
		if (int.TryParse(target.ToString(), out result))
		{
			if (amount == result)
			{
				return 1u;
			}
			return 0u;
		}
		Dictionary<string, object> dict = target as Dictionary<string, object>;
		return CompareOperatorAndROperand(dict, amount);
	}

	protected uint CompareOperatorAndROperand(Dictionary<string, object> dict, int loperand)
	{
		if (dict != null)
		{
			TFUtils.Assert(!dict.ContainsKey("loperand"), "Do not specify a loperand for a range that compares against a derived amount!");
			int roperand = int.Parse(dict["roperand"].ToString());
			string operatorString = dict["operator"].ToString();
			return Compare(operatorString, loperand, roperand);
		}
		return 0u;
	}

	protected uint Compare(string operatorString, int loperand, int roperand)
	{
		if (operatorString == ">" && loperand > roperand)
		{
			return 1u;
		}
		if (operatorString == ">=" && loperand >= roperand)
		{
			return 1u;
		}
		if (operatorString == "<" && loperand < roperand)
		{
			return 1u;
		}
		if (operatorString == "<=" && loperand <= roperand)
		{
			return 1u;
		}
		return 0u;
	}
}
