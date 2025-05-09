using System.Collections.Generic;

public class AutoQuestCreateAction : PersistedTriggerableAction
{
	public const string QUEST_CREATE = "aq";

	private QuestDefinition m_pQuestDef;

	private Quest m_pQuest;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public AutoQuestCreateAction(QuestDefinition pQuestDef)
		: base("aq", Identity.Null())
	{
		m_pQuestDef = pQuestDef;
	}

	public new static AutoQuestCreateAction FromDict(Dictionary<string, object> pData)
	{
		QuestDefinition pQuestDef = QuestDefinition.FromDict((Dictionary<string, object>)pData["questdef"]);
		return new AutoQuestCreateAction(pQuestDef);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["questdef"] = m_pQuestDef.ToDict(false);
		return dictionary;
	}

	public override void Process(Game pGame)
	{
		m_pQuest = pGame.questManager.AddQuestDefinition(m_pQuestDef);
		m_pQuest.StartConditions = new ConditionState(m_pQuestDef.Start.Chunks[0].Condition);
		m_pQuest.StartConditions.Hydrate(m_pQuest.StartProgress, pGame);
		int count = m_pQuestDef.End.Chunks.Count;
		for (int i = 0; i < count; i++)
		{
			ConditionState conditionState = new ConditionState(m_pQuestDef.End.Chunks[i].Condition);
			m_pQuest.EndConditions.Add(conditionState);
			conditionState.Hydrate(m_pQuest.EndProgress, pGame);
		}
		pGame.autoQuestDatabase.AddPreviousAutoQuests(m_pQuestDef.AutoQuestID, m_pQuestDef.AutoQuestCharacterID);
	}

	public override void Apply(Game pGame, ulong nUtcNow)
	{
		base.Apply(pGame, nUtcNow);
		m_pQuest = pGame.questManager.AddQuestDefinition(m_pQuestDef);
		if (QuestDefinition.LastAutoQuestId < m_pQuestDef.Did)
		{
			QuestDefinition.LastAutoQuestId = m_pQuestDef.Did;
		}
		m_pQuest.StartConditions = new ConditionState(m_pQuestDef.Start.Chunks[0].Condition);
		m_pQuest.StartConditions.Hydrate(m_pQuest.StartProgress, pGame);
		int count = m_pQuestDef.End.Chunks.Count;
		for (int i = 0; i < count; i++)
		{
			ConditionState conditionState = new ConditionState(m_pQuestDef.End.Chunks[0].Condition);
			m_pQuest.EndConditions.Add(conditionState);
			conditionState.Hydrate(m_pQuest.EndProgress, pGame);
		}
		pGame.questManager.RegisterQuest(pGame, m_pQuest);
		pGame.autoQuestDatabase.AddPreviousAutoQuests(m_pQuestDef.AutoQuestID, m_pQuestDef.AutoQuestCharacterID);
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		List<object> list = (List<object>)dictionary["generated_quest_definition"];
		list.Add(m_pQuestDef.ToDict(true));
		List<object> list2 = (List<object>)dictionary["quests"];
		list2.Add(m_pQuest.ToDict());
		((Dictionary<string, object>)pGameState["farm"])["auto_quest_id"] = QuestDefinition.LastAutoQuestId;
		AutoQuestDatabase.WritePreviousAutoQuestDataToGameState(pGameState);
		base.Confirm(pGameState);
	}
}
