using System.Collections.Generic;

public class QuerySimulatedCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "query_simulated";

	public const int QUERIER = 0;

	public override bool IsExpensiveToCalculate
	{
		get
		{
			return true;
		}
	}

	public static QuerySimulatedCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedQuerier item = SimulatedQuerier.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		QuerySimulatedCondition querySimulatedCondition = new QuerySimulatedCondition();
		querySimulatedCondition.Parse(dict, "query_simulated", null, list);
		return querySimulatedCondition;
	}

	public override string Description(Game game)
	{
		return string.Empty;
	}
}
