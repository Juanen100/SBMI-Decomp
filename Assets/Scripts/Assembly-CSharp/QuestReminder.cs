using System.Collections.Generic;

public class QuestReminder : UiTargetingSessionActionDefinition
{
	public const string TYPE = "quest_reminder";

	private QuestReminderBanner banner;

	private uint questID;

	private string barTexture;

	private string circleTexture;

	public static QuestReminder Create(Dictionary<string, object> data, uint id, ICondition startingConditions, uint originatedFromQuest)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
	}
}
