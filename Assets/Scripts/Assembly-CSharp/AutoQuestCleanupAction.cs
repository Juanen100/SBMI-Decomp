using System;
using System.Collections.Generic;

public class AutoQuestCleanupAction : PersistedTriggerableAction
{
	public const string QUEST_CLEANUP = "au";

	private uint m_uQuestId = 500001u;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	private AutoQuestCleanupAction(uint uQuestId)
		: base("au", Identity.Null())
	{
		m_uQuestId = uQuestId;
	}

	public AutoQuestCleanupAction(Quest pQuest)
		: this(pQuest.Did)
	{
	}

	public new static AutoQuestCleanupAction FromDict(Dictionary<string, object> pData)
	{
		uint uQuestId = TFUtils.LoadUint(pData, "did");
		return new AutoQuestCleanupAction(uQuestId);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = m_uQuestId;
		return dictionary;
	}

	public override void Process(Game pGame)
	{
		pGame.questManager.DeactivateQuest(pGame, QuestDefinition.LastAutoQuestId);
		base.Process(pGame);
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["quests"];
		Predicate<object> match = delegate(object pObj)
		{
			uint num4 = uint.Parse(((Dictionary<string, object>)pObj)["did"].ToString());
			return (num4 == m_uQuestId) ? true : false;
		};
		list.Remove(list.Find(match));
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		List<object> list2 = (List<object>)dictionary["generated_quest_definition"];
		int num = -1;
		for (int num2 = 0; num2 < list2.Count; num2++)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)list2[num2];
			uint num3 = TFUtils.LoadUint(data, "did");
			if (m_uQuestId == num3)
			{
				num = num2;
			}
		}
		if (num != -1)
		{
			list2.RemoveAt(num);
		}
		base.Confirm(pGameState);
	}
}
