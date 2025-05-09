using System.Collections.Generic;

public class ChangeCostumeCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "change_costume";

	public static ChangeCostumeCondition FromDict(Dictionary<string, object> dict)
	{
		ChangeCostumeCondition changeCostumeCondition = new ChangeCostumeCondition();
		changeCostumeCondition.Parse(dict, "change_costume", new List<string> { typeof(ChangeCostumeAction).ToString() }, new List<IMatcher> { CostumeMatcher.FromDict(dict) });
		return changeCostumeCondition;
	}

	public override string Description(Game game)
	{
		return "change_costume";
	}
}
