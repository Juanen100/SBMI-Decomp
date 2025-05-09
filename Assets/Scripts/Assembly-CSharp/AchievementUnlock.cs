using System.Collections.Generic;
using UnityEngine;

public class AchievementUnlock : SessionActionDefinition
{
	public const string TYPE = "achievement_unlock";

	private const string ACHIEVEMENT_ID = "achievement_id";

	private string achievementId;

	public static AchievementUnlock Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		AchievementUnlock achievementUnlock = new AchievementUnlock();
		achievementUnlock.Parse(data, id, startConditions, originatedFromQuest);
		return achievementUnlock;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		achievementId = TFUtils.LoadString(data, "achievement_id");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["achievement_id"] = achievementId;
		return dictionary;
	}

	public override void PreActivate(Game game, SessionActionTracker action)
	{
		GameObject gameObject = GameObject.Find("SBGameCenterManager");
		SBGameCenterManager component = gameObject.GetComponent<SBGameCenterManager>();
		component.ReportAchievement(achievementId, 100f);
	}
}
