using System.Collections.Generic;

public class PaveCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "pave";

	public static PaveCondition FromDict(Dictionary<string, object> dict)
	{
		PaveCondition paveCondition = new PaveCondition();
		paveCondition.Parse(dict, "pave", new List<string> { typeof(PaveAction).ToString() }, new List<IMatcher> { PaveMatcher.FromDict(dict) });
		return paveCondition;
	}

	public override string Description(Game game)
	{
		return Language.Get("!!COND_PLACE_PATH");
	}
}
