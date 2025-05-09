using System.Collections.Generic;

public class CostumeMatcher : Matcher
{
	public const string DEFINITION_ID = "costume_id";

	public static CostumeMatcher FromDict(Dictionary<string, object> dict)
	{
		CostumeMatcher costumeMatcher = new CostumeMatcher();
		costumeMatcher.RegisterProperty("costume_id", dict);
		return costumeMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + GetTarget("costume_id");
		}
		uint nCostumeDID = uint.Parse(GetTarget("costume_id"));
		return game.costumeManager.GetCostume((int)nCostumeDID).m_sName;
	}
}
