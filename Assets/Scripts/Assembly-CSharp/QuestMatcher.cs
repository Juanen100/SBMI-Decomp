using System.Collections.Generic;

public class QuestMatcher : Matcher
{
	public const string DEFINITION_ID = "quest_id";

	public static QuestMatcher FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher questMatcher = new QuestMatcher();
		questMatcher.RegisterProperty("quest_id", dict);
		return questMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + GetTarget("quest_id");
		}
		uint did = uint.Parse(GetTarget("quest_id"));
		return game.questManager.GetQuestDefinition(did).Name;
	}
}
