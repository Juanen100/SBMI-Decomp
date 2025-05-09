#define ASSERTS_ON
using System;
using System.Collections.Generic;

public class QuestReminder : UiTargetingSessionActionDefinition
{
	public const string TYPE = "quest_reminder";

	private QuestReminderBanner banner = new QuestReminderBanner();

	private uint questID;

	private string barTexture;

	private string circleTexture;

	public static QuestReminder Create(Dictionary<string, object> data, uint id, ICondition startingConditions, uint originatedFromQuest)
	{
		TFUtils.Assert(!data.ContainsKey("target"), "QuestReminders cannot specify a target!");
		TFUtils.Assert(originatedFromQuest != 0, "QuestReminders can only be originated from quests! This one wasn't given a quest Did.");
		QuestReminder questReminder = new QuestReminder();
		questReminder.Parse(data, id, startingConditions, originatedFromQuest, QuestDefinition.GenerateSessionActionId(originatedFromQuest));
		questReminder.questID = originatedFromQuest;
		if (data.ContainsKey("bar_texture"))
		{
			questReminder.barTexture = TFUtils.LoadString(data, "bar_texture");
		}
		if (data.ContainsKey("circle_texture"))
		{
			questReminder.circleTexture = TFUtils.LoadString(data, "circle_texture");
		}
		return questReminder;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dict = base.ToDict();
		banner.AddToDict(ref dict);
		return dict;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		Quest quest = session.TheGame.questManager.GetQuest(questID);
		if (quest != null)
		{
			SBGUIStandardScreen screen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
			session.AddAsyncResponse("standard_screen", screen);
			Action clickHandler = delegate
			{
				screen.TryFireQuestStatusEvent(session, (int)questID);
			};
			if (!quest.TriggeredReminder)
			{
				quest.TriggeredReminder = true;
				banner.Spawn(session.TheGame, action, target, containingScreen, clickHandler, barTexture, circleTexture);
			}
		}
	}
}
