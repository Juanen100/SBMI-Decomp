using System.Collections.Generic;

public class QuerySimulatedCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "query_simulated";

	public const int QUERIER = 0;

	public override bool IsExpensiveToCalculate
	{
		get
		{
			return false;
		}
	}

	public static QuerySimulatedCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
