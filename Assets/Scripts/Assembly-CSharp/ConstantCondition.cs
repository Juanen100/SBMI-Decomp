using System.Collections.Generic;

public class ConstantCondition : LoadableCondition
{
	public const string LOAD_TOKEN = "constant";

	private bool val;

	public bool Value
	{
		get
		{
			return false;
		}
	}

	private ConstantCondition()
	{
	}

	public ConstantCondition(uint id, bool val)
	{
	}

	public static ConstantCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICollection<string> relevantTypes, bool val)
	{
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
	}
}
