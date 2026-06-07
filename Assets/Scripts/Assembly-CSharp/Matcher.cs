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
			return null;
		}
	}

	protected Matcher()
	{
	}

	protected Matcher(Dictionary<string, MatchableProperty> matchableProperties)
	{
	}

	public virtual uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		return 0u;
	}

	public bool IsRequired(string property)
	{
		return false;
	}

	public bool HasRequirements()
	{
		return false;
	}

	protected MatchableProperty GetProperty(string key)
	{
		return null;
	}

	public object GetTargetObject(string propertyKey)
	{
		return null;
	}

	public string GetTarget(string propertyKey)
	{
		return null;
	}

	public abstract string DescribeSubject(Game game);

	protected bool RegisterProperty(string key, Dictionary<string, object> data)
	{
		return false;
	}

	protected bool RegisterProperty(string key, Dictionary<string, object> data, MatchableProperty.MatchFn matchDelegate)
	{
		return false;
	}

	protected bool AddRequiredProperty(string key, object val)
	{
		return false;
	}

	protected bool AddRequiredProperty(string key, object val, MatchableProperty.MatchFn matchDelegate)
	{
		return false;
	}

	private void AssertNotDuplicate(string key)
	{
	}

	private static uint DefaultMatchFn(MatchableProperty property, Dictionary<string, object> triggerData, Game game)
	{
		return 0u;
	}

	public override string ToString()
	{
		return null;
	}

	protected uint CompareOperandRangesToAmount(object target, int amount)
	{
		return 0u;
	}

	protected uint CompareOperatorAndROperand(Dictionary<string, object> dict, int loperand)
	{
		return 0u;
	}

	protected uint Compare(string operatorString, int loperand, int roperand)
	{
		return 0u;
	}
}
