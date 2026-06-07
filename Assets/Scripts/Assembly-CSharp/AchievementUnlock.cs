using System.Collections.Generic;

public class AchievementUnlock : SessionActionDefinition
{
	public const string TYPE = "achievement_unlock";

	private string achievementId;

	private const string ACHIEVEMENT_ID = "achievement_id";

	public static AchievementUnlock Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void PreActivate(Game game, SessionActionTracker action)
	{
	}
}
