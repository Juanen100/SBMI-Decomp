#define ASSERTS_ON
using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class QuestManager : ITriggerObserver
{
	private class PostponedDialogParams
	{
		public uint packageId;

		public uint sequenceId;

		public List<Reward> rewards;

		public DateTime complete;

		public uint questId;
	}

	public ulong m_uQuestTimeGap = 600uL;

	public ulong? m_uQuestCompletionTimestamp;

	public ulong? m_uAutoQuestStartTime;

	public ulong m_uTimeTillResetQuest = 86400uL;

	public ulong m_uCurrentTime;

	public int m_autoQuestCount;

	private static readonly string QUESTS_PATH = "Quests";

	private bool showDialogs;

	private bool isActive;

	private DialogPackageManager dialogPackageManager;

	private List<ITrigger> deferredTriggers = new List<ITrigger>();

	private Dictionary<uint, Quest> questList;

	private Dictionary<uint, QuestDefinition> questDefinitionList;

	private Dictionary<uint, QuestTemplate> randomQuestTemplateList;

	private OrderedSet<uint> activatedDids;

	private OrderedSet<uint> completedDids;

	private OrderedSet<uint> deactivatedCompletedDids;

	private Action onShowDialogCallback;

	private Dictionary<string, Vector2> questLineProgress;

	private Queue<PostponedDialogParams> postponed = new Queue<PostponedDialogParams>();

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}

	public Action OnShowDialogCallback
	{
		set
		{
			onShowDialogCallback = value;
		}
	}

	public OrderedSet<uint> ActiveQuestDids
	{
		get
		{
			return activatedDids;
		}
	}

	public OrderedSet<uint> ActiveQuestDidsNotInPostponed
	{
		get
		{
			PostponedDialogParams[] array = postponed.ToArray();
			int num = array.Length;
			OrderedSet<uint> orderedSet = new OrderedSet<uint>();
			foreach (uint activeQuestDid in ActiveQuestDids)
			{
				bool flag = false;
				for (int i = 0; i < num; i++)
				{
					PostponedDialogParams postponedDialogParams = array[i];
					if (activeQuestDid == postponedDialogParams.questId)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					orderedSet.Add(activeQuestDid);
				}
			}
			return orderedSet;
		}
	}

	public OrderedSet<uint> CompletedQuestDids
	{
		get
		{
			return completedDids;
		}
	}

	public Dictionary<uint, QuestDefinition> QuestDefinitionList
	{
		get
		{
			return questDefinitionList;
		}
	}

	public QuestManager()
	{
		questList = new Dictionary<uint, Quest>();
		questDefinitionList = new Dictionary<uint, QuestDefinition>();
		randomQuestTemplateList = new Dictionary<uint, QuestTemplate>();
		questLineProgress = new Dictionary<string, Vector2>();
		activatedDids = new OrderedSet<uint>();
		completedDids = new OrderedSet<uint>();
		deactivatedCompletedDids = new OrderedSet<uint>();
		LoadAndInitializeQuestPrototypes();
	}

	private string[] GetFilesToLoad()
	{
		return Config.QUESTS_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private void LoadAndInitializeQuestPrototypes()
	{
		string[] filesToLoad = GetFilesToLoad();
		string[] array = filesToLoad;
		foreach (string text in array)
		{
			string filePathFromString = GetFilePathFromString(text);
			string json = TFUtils.ReadAllText(filePathFromString);
			List<object> list = (List<object>)Json.Deserialize(json);
			foreach (Dictionary<string, object> item in list)
			{
				if (!item.ContainsKey("type"))
				{
					TFUtils.ErrorLog("Quest Manager cannot process file: " + text);
					continue;
				}
				switch ((string)item["type"])
				{
				case "quest":
				{
					QuestDefinition questDefinition = QuestDefinition.FromDict(item);
					TFUtils.Assert(questDefinition.Did < 400000 || questDefinition.Did > 500000, "Invalid Quest Id (" + questDefinition.Did + ") in file " + text + ".  Quest ids from " + 400000u + " to " + 500000u + " are reserved for Randomly Generated Quests.");
					AddQuestDefinition(questDefinition);
					break;
				}
				case "quest_template":
				{
					QuestTemplate questDef = QuestTemplate.FromDict(item);
					AddRandomQuestTemplate(questDef);
					break;
				}
				default:
					TFUtils.ErrorLog(string.Concat("Quest Manager found unexpected file type in Quests: ", item["type"], " in ", text));
					break;
				}
			}
		}
	}

	public void AddRandomQuestTemplate(QuestTemplate questDef)
	{
		randomQuestTemplateList[questDef.Did] = questDef;
	}

	public Quest AddQuestDefinition(QuestDefinition questDef)
	{
		if (!SBSettings.EnableRandomQuests && questDef.Did >= 400000 && questDef.Did <= 500000)
		{
			return null;
		}
		if (!SBSettings.EnableAutoQuests && questDef.Did >= 500001 && questDef.Did <= 600000)
		{
			return null;
		}
		TFUtils.Assert(!questDefinitionList.ContainsKey(questDef.Did), "Duplicate Quest ID found for quest: " + questDef.Did);
		Quest result = CreateNewQuestInfo(questDef.Did);
		questDefinitionList[questDef.Did] = questDef;
		if (questDef.QuestLine != null && questDef.QuestLine.HasProgress)
		{
			string name = questDef.QuestLine.Name;
			Vector2 up = Vector2.up;
			if (questLineProgress.ContainsKey(name))
			{
				up += questLineProgress[name];
			}
			questLineProgress[name] = up;
		}
		return result;
	}

	public QuestTemplate GetRandomQuestTemplate()
	{
		if (randomQuestTemplateList.Count == 0)
		{
			TFUtils.ErrorLog("Failed to find any random quest templates");
		}
		uint[] array = new uint[randomQuestTemplateList.Keys.Count];
		randomQuestTemplateList.Keys.CopyTo(array, 0);
		return randomQuestTemplateList[array[UnityEngine.Random.Range(0, randomQuestTemplateList.Count)]];
	}

	public Quest CreateNewQuestInfo(uint did)
	{
		ConditionalProgress conditionalProgress = new ConditionalProgress();
		Quest quest = new Quest(did, conditionalProgress, conditionalProgress, null, null, false);
		questList[did] = quest;
		return quest;
	}

	public void SetDialogManager(DialogPackageManager dialogPackageMgr)
	{
		dialogPackageManager = dialogPackageMgr;
	}

	public void Activate(Game game)
	{
		foreach (Quest value in questList.Values)
		{
			value.StartConditions = new ConditionState(questDefinitionList[value.Did].Start.Chunks[0].Condition);
			value.StartConditions.Hydrate(value.StartProgress, game);
			foreach (QuestBookendInfo.ChunkConditions chunk in questDefinitionList[value.Did].End.Chunks)
			{
				ConditionState conditionState = new ConditionState(chunk.Condition);
				conditionState.Hydrate(value.EndProgress, game);
				value.EndConditions.Add(conditionState);
			}
		}
		foreach (Quest value2 in questList.Values)
		{
			QuestDefinition questDefinition = GetQuestDefinition(value2.Did);
			if (questDefinition != null && questDefinition.MicroEventDID.HasValue && !game.microEventManager.IsMicroEventActive(questDefinition.MicroEventDID.Value))
			{
				continue;
			}
			if (!value2.StartTime.HasValue)
			{
				if (value2.StartConditions.Examine() == ConditionResult.PASS)
				{
					ProgressTowardsStartConditions(value2, game, value2.StartConditions.Dehydrate().MetIds);
					ActivateQuest(value2, game);
					game.ModifyGameState(new QuestStartAction(value2));
				}
			}
			else if (!value2.CompletionTime.HasValue)
			{
				if (questDefinition.SessionActions != null)
				{
					SessionActionTracker sessionAction = value2.InstantiateSessionAction(questDefinition.SessionActions);
					game.sessionActionManager.Request(sessionAction, game, value2.TrackerTag);
				}
				if (questDefinition.Start.DialogSequenceId.HasValue)
				{
					game.triggerRouter.RouteTrigger(new QuestCompleteDialogInputData(questDefinition.Start.DialogSequenceId.Value, null, null, questDefinition.Did).CreateTrigger(TFUtils.EpochTime()), game);
				}
			}
		}
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		isActive = true;
		List<ITrigger> list4 = new List<ITrigger>(completedDids.Count);
		foreach (uint completedDid in completedDids)
		{
			Quest quest = questList[completedDid];
			QuestCompleteAction questCompleteAction = new QuestCompleteAction(quest, null, null);
			list4.Add(questCompleteAction.CreateTrigger(new Dictionary<string, object>()));
			list.AddRange(HandleFeatureUnlocks(game, GetQuestDefinition(completedDid)));
			list2.AddRange(HandleBuildingUnlocks(game, GetQuestDefinition(completedDid)));
			list3.AddRange(HandleCostumeUnlocks(game, GetQuestDefinition(completedDid)));
			UpdateQuestLineProgress(quest);
		}
		foreach (ITrigger item in list4)
		{
			game.triggerRouter.RouteTrigger(item, game);
		}
		foreach (uint activatedDid in activatedDids)
		{
			list.AddRange(HandleFeatureUnlocks(game, GetQuestDefinition(activatedDid)));
			list2.AddRange(HandleBuildingUnlocks(game, GetQuestDefinition(activatedDid)));
			list3.AddRange(HandleCostumeUnlocks(game, GetQuestDefinition(activatedDid)));
		}
		if (list.Count > 0)
		{
			game.simulation.ModifyGameState(new FeatureUnlocksAction(list));
		}
		int count = list2.Count;
		if (count > 0)
		{
			game.simulation.ModifyGameState(new BuildingUnlocksAction(list2));
			string sName = string.Empty;
			string sType = string.Empty;
			for (int i = 0; i < count; i++)
			{
				game.catalog.GetNameAndTypeForDID(list2[i], out sName, out sType);
				sType = Catalog.ConvertTypeToDeltaDNAType(sType);
				AnalyticsWrapper.LogFeatureUnlocked(game, sName, sType);
			}
		}
		foreach (int item2 in list3)
		{
			CostumeManager.Costume costume = game.costumeManager.GetCostume(item2);
			if (costume != null)
			{
				game.analytics.LogCostumeUnlocked(costume.m_nDID);
				AnalyticsWrapper.LogCostumeUnlocked(game, costume);
			}
			game.simulation.ModifyGameState(new UnlockCostumeAction(item2));
		}
		Quest quest2 = GetQuest(QuestDefinition.LastAutoQuestId);
		if (quest2 != null && !quest2.CompletionTime.HasValue)
		{
			QuestDefinition questDefinition2 = GetQuestDefinition(quest2.Did);
			if (questDefinition2 != null && !game.autoQuestDatabase.IsQuestValid(game, questDefinition2))
			{
				game.ModifyGameState(new AutoQuestCleanupAction(quest2));
				m_uQuestCompletionTimestamp = (ulong)SoaringTime.AdjustedServerTime;
				game.ModifyGameState(new UpdateVariableAction<ulong?>("auto_quest_completion_time", m_uQuestCompletionTimestamp));
				if (m_autoQuestCount == 1)
				{
					m_uAutoQuestStartTime = m_uCurrentTime;
					m_uAutoQuestStartTime = (ulong)SoaringTime.AdjustedServerTime;
					game.ModifyGameState(new UpdateVariableAction<ulong?>("first_auto_quest_completion_timestamp", m_uAutoQuestStartTime));
				}
				game.ModifyGameState(new UpdateVariableAction<int>("current_auto_quest_completion_count", m_autoQuestCount));
				m_autoQuestCount++;
			}
		}
		game.triggerRouter.RouteTrigger(Trigger.Null, game);
		game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), game);
	}

	public Quest GetQuest(uint did)
	{
		if (questList.ContainsKey(did))
		{
			return questList[did];
		}
		return null;
	}

	public QuestDefinition GetQuestDefinition(uint did)
	{
		if (questDefinitionList.ContainsKey(did))
		{
			return questDefinitionList[did];
		}
		return null;
	}

	public List<int> GetTasksCompleting()
	{
		List<int> list = new List<int>();
		List<uint> list2 = new List<uint>(activatedDids);
		int count = list2.Count;
		QuestDefinition questDefinition = null;
		for (int i = 0; i < count; i++)
		{
			questDefinition = GetQuestDefinition(list2[i]);
			if (questDefinition == null)
			{
				continue;
			}
			List<QuestBookendInfo.ChunkConditions> chunks = questDefinition.End.Chunks;
			int count2 = chunks.Count;
			for (int j = 0; j < count2; j++)
			{
				if (chunks[j].Condition is TaskCompleteCondition)
				{
					Dictionary<string, object> dictionary = chunks[j].Condition.ToDict();
					if (dictionary.ContainsKey("task_id"))
					{
						list.Add(TFUtils.LoadInt(dictionary, "task_id"));
					}
				}
			}
		}
		return list;
	}

	public void RegisterQuest(Game pGame, Quest quest)
	{
		uint did = quest.Did;
		QuestDefinition questDefinition = GetQuestDefinition(did);
		if (questDefinition == null)
		{
			if (quest.CompletionTime.HasValue && !deactivatedCompletedDids.Contains(did))
			{
				deactivatedCompletedDids.Add(did);
			}
		}
		else
		{
			if (questDefinition.MicroEventDID.HasValue && !pGame.microEventManager.IsMicroEventActive(questDefinition.MicroEventDID.Value))
			{
				return;
			}
			questList[did] = quest;
			if (quest.CompletionTime.HasValue)
			{
				if (completedDids.Contains(did))
				{
				}
				activatedDids.Remove(did);
				completedDids.Add(did);
			}
			else if (quest.StartTime.HasValue)
			{
				activatedDids.Add(did);
				TFUtils.Assert(!completedDids.Contains(did), "An active quest shouldn't have been on the completed list");
			}
		}
	}

	public void ActivateQuest(Quest quest, Game game)
	{
		uint did = quest.Did;
		TFUtils.Assert(!activatedDids.Contains(did) && !completedDids.Contains(did), string.Format("Tried to Re-Activate Quest {0}. It a to have already been activated or completed before.", quest.ToString()));
		TFUtils.Assert(quest.TrackerTag != null && quest.TrackerTag != string.Empty, "Quest is missing its tracker tag. Ensure that it has one before calling ActivateQuest");
		quest.Start(TFUtils.EpochTime());
		activatedDids.Add(quest.Did);
		QuestDefinition questDefinition = questDefinitionList[quest.Did];
		if (questDefinition.HasFeatureUnlocks)
		{
			HandleFeatureUnlocks(game, questDefinition);
			game.Record(new FeatureUnlocksAction(questDefinition.FeatureUnlocks));
		}
		if (questDefinition.HasBuildingUnlocks)
		{
			game.Record(new BuildingUnlocksAction(questDefinition.BuildingUnlocks));
			HandleBuildingUnlocks(game, questDefinition);
			string sName = string.Empty;
			string sType = string.Empty;
			int count = questDefinition.BuildingUnlocks.Count;
			for (int i = 0; i < count; i++)
			{
				game.catalog.GetNameAndTypeForDID(questDefinition.BuildingUnlocks[i], out sName, out sType);
				sType = Catalog.ConvertTypeToDeltaDNAType(sType);
				AnalyticsWrapper.LogFeatureUnlocked(game, sName, sType);
			}
		}
		if (quest.Did >= 500001 && quest.Did <= 600000)
		{
			List<QuestBookendInfo.ChunkConditions> chunks = questDefinition.End.Chunks;
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			int num = chunks.Count - 1;
			for (int j = 0; j < num; j++)
			{
				Dictionary<string, object> dictionary = chunks[j].Condition.ToDict();
				if (dictionary.ContainsKey("count") && dictionary.ContainsKey("resource_id"))
				{
					soaringDictionary.addValue(TFUtils.LoadInt(dictionary, "count"), TFUtils.LoadInt(dictionary, "resource_id").ToString());
				}
			}
			if (soaringDictionary.count() <= 0)
			{
				soaringDictionary = null;
			}
			game.analytics.LogAutoQuestStarted(questDefinition.Tag);
			AnalyticsWrapper.LogAutoQuestStarted(game, questDefinition, soaringDictionary);
		}
		game.analytics.LogQuestStart(questDefinition.Tag, questDefinition.Name, questDefinition.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
		AnalyticsWrapper.LogQuestStarted(game, questDefinition);
		if (questDefinition.Start.DialogSequenceId.HasValue)
		{
			QueueDialogSequences(questDefinition.DialogPackageDid, questDefinition.Start.DialogSequenceId.Value, new List<Reward> { questDefinition.Reward }, questDefinition.Start.Postpone, questDefinition.Did);
		}
		if (questDefinition.HasCostumeUnlocks)
		{
			List<int> list = HandleCostumeUnlocks(game, questDefinition);
			foreach (int item in list)
			{
				CostumeManager.Costume costume = game.costumeManager.GetCostume(item);
				if (costume != null)
				{
					game.analytics.LogCostumeUnlocked(costume.m_nDID);
					AnalyticsWrapper.LogCostumeUnlocked(game, costume);
				}
				game.simulation.ModifyGameState(new UnlockCostumeAction(item));
				QueueDialogSequences((uint)item, 999999995u, new List<Reward>(), 0f, 999999995u);
			}
		}
		if (questDefinition.SessionActions != null)
		{
			SessionActionTracker sessionAction = quest.InstantiateSessionAction(questDefinition.SessionActions);
			game.sessionActionManager.Request(sessionAction, game, quest.TrackerTag);
		}
		game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), game);
	}

	public void DeactivateQuest(Game game, uint did)
	{
		if (activatedDids.Contains(did))
		{
			activatedDids.Remove(did);
		}
		if (completedDids.Contains(did))
		{
			completedDids.Remove(did);
			deactivatedCompletedDids.Add(did);
		}
	}

	public void CompleteQuest(Quest quest, Game game)
	{
		QuestDefinition questDefinition = questDefinitionList[quest.Did];
		ulong num = TFUtils.EpochTime();
		quest.Complete(num);
		activatedDids.Remove(quest.Did);
		completedDids.Add(quest.Did);
		UpdateQuestLineProgress(quest);
		Reward reward = questDefinition.GenerateReward(game.simulation);
		if (quest.Did >= 500001 && quest.Did <= 600000)
		{
			List<QuestBookendInfo.ChunkConditions> chunks = questDefinition.End.Chunks;
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			int num2 = chunks.Count - 1;
			for (int i = 0; i < num2; i++)
			{
				Dictionary<string, object> dictionary = chunks[i].Condition.ToDict();
				if (dictionary.ContainsKey("count") && dictionary.ContainsKey("resource_id"))
				{
					soaringDictionary.addValue(TFUtils.LoadInt(dictionary, "count"), TFUtils.LoadInt(dictionary, "resource_id").ToString());
				}
			}
			if (soaringDictionary.count() <= 0)
			{
				soaringDictionary = null;
			}
			game.analytics.LogAutoQuestCompleted(questDefinition.Tag);
			AnalyticsWrapper.LogAutoQuestCompleted(game, questDefinition, soaringDictionary, reward);
		}
		else
		{
			game.analytics.LogQuestCompleteSoaring(questDefinition.Tag);
			AnalyticsWrapper.LogQuestCompleted(game, questDefinition, reward);
		}
		game.analytics.LogQuestComplete(questDefinition.Tag, questDefinition.Name, quest.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
		game.analytics.LogQuestCompleteJJAMT(questDefinition.Tag, questDefinition.Name, quest.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount, game.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount);
		game.analytics.LogQuestCompleteGoldAMT(questDefinition.Tag, questDefinition.Name, quest.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount, game.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount);
		if (questDefinition.End.DialogSequenceId.HasValue)
		{
			QueueDialogSequences(questDefinition.DialogPackageDid, questDefinition.End.DialogSequenceId.Value, new List<Reward> { questDefinition.Reward }, questDefinition.End.Postpone, questDefinition.Did);
		}
		Dictionary<string, object> buildingLabels = reward.BuildingLabels;
		List<Reward> list = new List<Reward>();
		foreach (KeyValuePair<int, int> resourceAmount in reward.ResourceAmounts)
		{
			int key = resourceAmount.Key;
			int value = resourceAmount.Value;
			IResourceProgressCalculator resourceCalculator = game.resourceCalculatorManager.GetResourceCalculator(key);
			if (resourceCalculator == null)
			{
				continue;
			}
			List<Reward> rewards;
			resourceCalculator.GetRewardsForIncreasingResource(game.simulation, game.simulation.resourceManager.Resources, value, out rewards);
			if (rewards == null)
			{
				continue;
			}
			for (int j = 0; j < rewards.Count; j++)
			{
				Reward reward2 = rewards[j];
				if (reward2 != null)
				{
					game.ApplyReward(reward2, num, false);
				}
				game.resourceManager.Add(ResourceManager.LEVEL, 1, game);
				int level = game.resourceManager.Query(ResourceManager.LEVEL);
				game.ModifyGameState(new LevelUpAction(level, reward2, TFUtils.EpochTime()));
			}
			list.AddRange(rewards);
		}
		game.ApplyReward(reward, num, false);
		if (list.Count > 0)
		{
			int packageId = game.resourceManager.Query(ResourceManager.LEVEL);
			QueueDialogSequences((uint)packageId, 999999999u, list, questDefinition.End.Postpone, 999999999u);
		}
		QuestCompleteAction action = new QuestCompleteAction(quest, reward, buildingLabels);
		EnqueueDeferredAction(game, action);
		if (quest.Did >= 400000 && quest.Did <= 500000)
		{
			game.ModifyGameState(new RandomQuestCleanupAction(quest));
		}
		if (quest.Did >= 500001 && quest.Did <= 600000)
		{
			game.ModifyGameState(new AutoQuestCleanupAction(quest));
			m_uQuestCompletionTimestamp = (ulong)SoaringTime.AdjustedServerTime;
			game.ModifyGameState(new UpdateVariableAction<ulong?>("auto_quest_completion_time", m_uQuestCompletionTimestamp));
			if (m_autoQuestCount == 1)
			{
				m_uAutoQuestStartTime = m_uCurrentTime;
				m_uAutoQuestStartTime = (ulong)SoaringTime.AdjustedServerTime;
				game.ModifyGameState(new UpdateVariableAction<ulong?>("first_auto_quest_completion_timestamp", m_uAutoQuestStartTime));
			}
			game.ModifyGameState(new UpdateVariableAction<int>("current_auto_quest_completion_count", m_autoQuestCount));
			m_autoQuestCount++;
		}
		game.sessionActionManager.ObliterateAnyTagged(quest.TrackerTag, game);
		if (questDefinition.PostSessionActions != null)
		{
			SessionActionTracker sessionAction = quest.InstantiateSessionAction(questDefinition.PostSessionActions);
			game.sessionActionManager.Request(sessionAction, game, quest.TrackerTag);
		}
		foreach (int costumeUnlock in reward.CostumeUnlocks)
		{
			QueueDialogSequences((uint)costumeUnlock, 999999995u, new List<Reward>(), questDefinition.End.Postpone, 999999995u);
		}
	}

	public float? GetQuestLineProgress(QuestDefinition questDef)
	{
		Vector2 value;
		if (questDef.QuestLine == null || !questLineProgress.TryGetValue(questDef.QuestLine.Name, out value))
		{
			return null;
		}
		return value.x / value.y;
	}

	public float? GetQuestLineLastProgress(QuestDefinition questDef)
	{
		Vector2 value;
		if (questDef.QuestLine == null || !questLineProgress.TryGetValue(questDef.QuestLine.Name, out value))
		{
			return null;
		}
		value -= Vector2.right;
		return value.x / value.y;
	}

	private void ProgressTowardsStartConditions(Quest quest, Game game, List<uint> conditionIds)
	{
		TFUtils.Assert(!completedDids.Contains(quest.Did), "Trying to make start progress on a quest that is already complete");
		EnqueueDeferredAction(game, new QuestProgressAction(quest, QuestProgressAction.ConditionType.START, conditionIds));
	}

	private void ProgressTowardsEndConditions(Quest quest, Game game, List<uint> conditionIds)
	{
		TFUtils.Assert(activatedDids.Contains(quest.Did), "Trying to make ending progress on a quest that isn't active");
		EnqueueDeferredAction(game, new QuestProgressAction(quest, QuestProgressAction.ConditionType.END, conditionIds));
	}

	private void FailQuest(Quest quest)
	{
		TFUtils.Assert(false, "Quest failures not implemented");
		activatedDids.Remove(quest.Did);
	}

	public void OnUpdate(Game game)
	{
		if (postponed.Count > 0)
		{
			PostponedDialogParams postponedDialogParams = postponed.Peek();
			if (postponedDialogParams.complete.CompareTo(DateTime.Now) <= 0)
			{
				postponedDialogParams = postponed.Dequeue();
				AddDialogSequences(game, postponedDialogParams.packageId, postponedDialogParams.sequenceId, postponedDialogParams.rewards, postponedDialogParams.questId);
			}
		}
		if (isActive)
		{
			if (SBSettings.EnableRandomQuests && game.featureManager.CheckFeature("allow_random_quests") && !game.questManager.IsQuestActivated(QuestDefinition.LastRandomQuestId))
			{
				CreateAndTriggerRandomQuest(game);
			}
			if (SBSettings.EnableAutoQuests && game.featureManager.CheckFeature("allow_auto_quests") && !game.questManager.IsQuestActivated(QuestDefinition.LastAutoQuestId))
			{
				if (m_uQuestCompletionTimestamp.HasValue)
				{
					ulong? uQuestCompletionTimestamp = m_uQuestCompletionTimestamp;
					ulong? num = ((!uQuestCompletionTimestamp.HasValue) ? ((ulong?)null) : new ulong?((ulong)SoaringTime.AdjustedServerTime - uQuestCompletionTimestamp.Value));
					if (!num.HasValue || num.Value <= m_uQuestTimeGap)
					{
						goto IL_0160;
					}
				}
				if (m_autoQuestCount < 3)
				{
					m_autoQuestCount++;
					CreateAndTriggerAutoQuest(game);
				}
			}
		}
		goto IL_0160;
		IL_0160:
		if (showDialogs)
		{
			if (onShowDialogCallback != null)
			{
				onShowDialogCallback();
			}
			showDialogs = false;
		}
		uint num2 = 40000u;
		if (game.resourceManager.PlayerLevelAmount > 1 && game.questManager.IsQuestActivated(num2))
		{
			activatedDids.Remove(num2);
			completedDids.Add(num2);
			game.sessionActionManager.ObliterateAnyTagged(questList[num2].TrackerTag, game);
		}
		uint num3 = 2720u;
		if (game.questManager.IsQuestActivated(num3) || game.questManager.IsQuestActive(num3))
		{
			activatedDids.Remove(num3);
			completedDids.Add(num3);
			game.sessionActionManager.ObliterateAnyTagged(questList[num3].TrackerTag, game);
		}
		m_uCurrentTime = (ulong)SoaringTime.AdjustedServerTime;
		ulong? uAutoQuestStartTime = m_uAutoQuestStartTime;
		if (!uAutoQuestStartTime.HasValue)
		{
			m_uAutoQuestStartTime = m_uCurrentTime;
			m_autoQuestCount = 0;
		}
		ulong? uAutoQuestStartTime2 = m_uAutoQuestStartTime;
		if (TFUtils.EpochToDateTime(uAutoQuestStartTime2.Value + m_uTimeTillResetQuest) < TFUtils.EpochToDateTime(m_uCurrentTime))
		{
			m_autoQuestCount = 0;
			m_uAutoQuestStartTime = 0uL;
		}
	}

	public bool IsQuestActivated(uint did)
	{
		return activatedDids.Contains(did);
	}

	public bool IsQuestCompleted(uint did)
	{
		return completedDids.Contains(did) || deactivatedCompletedDids.Contains(did);
	}

	public void CreateAndTriggerRandomQuest(Game game)
	{
		if (QuestDefinition.LastRandomQuestId == 0 || ++QuestDefinition.LastRandomQuestId > 500000)
		{
			QuestDefinition.LastRandomQuestId = 400000u;
		}
		QuestDefinition questDefinition = QuestDefinition.CreateRandom(this, game);
		game.ModifyGameState(new RandomQuestCreateAction(questDefinition));
		Quest quest = GetQuest(questDefinition.Did);
		ActivateQuest(quest, game);
		game.ModifyGameState(new QuestStartAction(quest));
	}

	public void CreateAndTriggerAutoQuest(Game pGame)
	{
		if (QuestDefinition.LastAutoQuestId == 0 || ++QuestDefinition.LastAutoQuestId > 600000)
		{
			QuestDefinition.LastAutoQuestId = 500001u;
		}
		QuestDefinition questDefinition = QuestDefinition.CreateAuto(pGame);
		if (questDefinition != null)
		{
			pGame.ModifyGameState(new AutoQuestCreateAction(questDefinition));
			Quest quest = GetQuest(questDefinition.Did);
			ActivateQuest(quest, pGame);
			pGame.ModifyGameState(new QuestStartAction(quest));
		}
	}

	private void QueueDialogSequences(uint packageId, uint sequenceId, List<Reward> rewards, float postpone, uint questId)
	{
		PostponedDialogParams postponedDialogParams = new PostponedDialogParams();
		postponedDialogParams.packageId = packageId;
		postponedDialogParams.sequenceId = sequenceId;
		postponedDialogParams.rewards = rewards;
		postponedDialogParams.complete = DateTime.Now.AddSeconds(postpone);
		postponedDialogParams.questId = questId;
		postponed.Enqueue(postponedDialogParams);
	}

	public void AddDialogSequences(Game game, uint packageId, uint sequenceId, List<Reward> rewards, uint questId, bool bShowDialogs = true)
	{
		int count = rewards.Count;
		List<object> list = new List<object>(count);
		for (int i = 0; i < count; i++)
		{
			list.Add(rewards[i].ToDict());
		}
		if (sequenceId == 0)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("rewards", list);
		Dictionary<string, object> contextData = dictionary;
		switch (sequenceId)
		{
		case 10000u:
			if (!QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				QuestDefinition.RecreateRandomQuestStartInputData(game, QuestDefinition.LastRandomQuestId);
			}
			if (QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				List<DialogInputData> list6 = new List<DialogInputData>();
				QuestStartDialogInputData item5 = new QuestStartDialogInputData(10000u, QuestDefinition.StartInputPrompts[QuestDefinition.LastRandomQuestId], contextData, QuestDefinition.LastRandomQuestId);
				list6.Add(item5);
				dialogPackageManager.AddDialogInputBatch(game, list6, 10000u);
			}
			break;
		case 10001u:
			if (!QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				QuestDefinition.RecreateRandomQuestCompleteInputData(game, QuestDefinition.LastRandomQuestId);
			}
			if (QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				List<DialogInputData> list5 = new List<DialogInputData>();
				QuestCompleteDialogInputData item4 = new QuestCompleteDialogInputData(10001u, QuestDefinition.CompleteInputPrompts[QuestDefinition.LastRandomQuestId], contextData, QuestDefinition.LastRandomQuestId);
				list5.Add(item4);
				dialogPackageManager.AddDialogInputBatch(game, list5, 10001u);
			}
			break;
		case 10002u:
		{
			if (!QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				QuestDefinition.RecreateAutoQuestIntroInputData(game, QuestDefinition.LastAutoQuestId);
			}
			List<DialogInputData> list4 = new List<DialogInputData>();
			if (QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				CharacterDialogInputData item3 = new CharacterDialogInputData(10002u, QuestDefinition.StartInputPrompts[QuestDefinition.LastAutoQuestId]);
				list4.Add(item3);
			}
			list4.Add(new QuestStartDialogInputData(10002u, new Dictionary<string, object> { { "type", "quest_start" } }, contextData, QuestDefinition.LastAutoQuestId));
			dialogPackageManager.AddDialogInputBatch(game, list4, 10002u);
			break;
		}
		case 10003u:
		{
			if (!QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				QuestDefinition.RecreateAutoQuestOutroInputData(game, QuestDefinition.LastAutoQuestId);
			}
			List<DialogInputData> list3 = new List<DialogInputData>();
			if (QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				CharacterDialogInputData item2 = new CharacterDialogInputData(10003u, QuestDefinition.CompleteInputPrompts[QuestDefinition.LastAutoQuestId]);
				list3.Add(item2);
			}
			list3.Add(new QuestCompleteDialogInputData(10003u, new Dictionary<string, object> { { "type", "quest_complete" } }, contextData, QuestDefinition.LastAutoQuestId));
			dialogPackageManager.AddDialogInputBatch(game, list3, 10003u);
			break;
		}
		case 999999999u:
		{
			List<DialogInputData> list2 = new List<DialogInputData>();
			list2.Add(new LevelUpDialogInputData((int)packageId, rewards));
			dialogPackageManager.AddDialogInputBatch(game, list2);
			break;
		}
		case 999999995u:
		{
			CostumeManager.Costume costume = game.costumeManager.GetCostume((int)packageId);
			FoundItemDialogInputData item = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get(costume.m_sName)), costume.m_sPortrait, "Beat_FoundRecipe");
			dialogPackageManager.AddDialogInputBatch(game, new List<DialogInputData> { item });
			break;
		}
		default:
		{
			DialogPackage dialogPackage = dialogPackageManager.GetDialogPackage(packageId);
			List<DialogInputData> dialogInputsInSequence = dialogPackage.GetDialogInputsInSequence(sequenceId, contextData, questId);
			dialogPackageManager.AddDialogInputBatch(game, dialogInputsInSequence, sequenceId);
			break;
		}
		}
		if (bShowDialogs)
		{
			showDialogs = true;
		}
	}

	public void ProcessTrigger(ITrigger trigger, Game game)
	{
		if (!isActive)
		{
			return;
		}
		foreach (Quest value in questList.Values)
		{
			if (activatedDids.Contains(value.Did) || completedDids.Contains(value.Did))
			{
				continue;
			}
			if (trigger != null && value.StartConditions.Recalculate(game, trigger))
			{
				ProgressTowardsStartConditions(value, game, value.StartConditions.Dehydrate().MetIds);
			}
			ConditionResult conditionResult = value.StartConditions.Examine();
			QuestDefinition questDefinition = GetQuestDefinition(value.Did);
			if (questDefinition == null || !questDefinition.MicroEventDID.HasValue || game.microEventManager.IsMicroEventActive(questDefinition.MicroEventDID.Value))
			{
				switch (conditionResult)
				{
				case ConditionResult.PASS:
					ActivateQuest(value, game);
					EnqueueDeferredAction(game, new QuestStartAction(value));
					break;
				case ConditionResult.FAIL:
					FailQuest(value);
					break;
				}
			}
		}
		List<uint> list = new List<uint>(activatedDids);
		foreach (uint item in list)
		{
			if (!activatedDids.Contains(item))
			{
				continue;
			}
			Quest quest = questList[item];
			ConditionResult conditionResult2 = ConditionResult.PASS;
			bool flag = false;
			foreach (ConditionState endCondition in quest.EndConditions)
			{
				if (trigger != null && endCondition.Recalculate(game, trigger))
				{
					flag = true;
				}
				ConditionResult conditionResult3 = endCondition.Examine();
				if (conditionResult3 != ConditionResult.PASS && conditionResult2 != ConditionResult.FAIL)
				{
					conditionResult2 = conditionResult3;
				}
			}
			if (flag || conditionResult2 == ConditionResult.PASS)
			{
				ConditionalProgress conditionalProgress = ConditionState.DehydrateChunks(quest.EndConditions);
				ProgressTowardsEndConditions(quest, game, conditionalProgress.MetIds);
			}
			switch (conditionResult2)
			{
			case ConditionResult.PASS:
				CompleteQuest(quest, game);
				continue;
			case ConditionResult.FAIL:
				FailQuest(quest);
				continue;
			}
			if (trigger == null || quest.Did != QuestDefinition.LastAutoQuestId || !(trigger.Type == "UpdateResource"))
			{
				continue;
			}
			Session session = game.communityEventManager.GetSession();
			SBGUIStandardScreen sBGUIStandardScreen = null;
			if (session != null)
			{
				sBGUIStandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
				if (sBGUIStandardScreen != null)
				{
					session.AddAsyncResponse("standard_screen", sBGUIStandardScreen);
				}
			}
			if (!(sBGUIStandardScreen != null))
			{
				continue;
			}
			QuestDefinition questDefinition2 = GetQuestDefinition(quest.Did);
			if (questDefinition2 == null)
			{
				continue;
			}
			List<QuestBookendInfo.ChunkConditions> list2 = new List<QuestBookendInfo.ChunkConditions>(questDefinition2.End.Chunks);
			list2.RemoveAt(list2.Count - 1);
			List<ConditionState> endConditions = quest.EndConditions;
			int count = endConditions.Count;
			int num = 0;
			for (int i = 0; i < count && i - num < list2.Count; i++)
			{
				if (endConditions[i].Examine() == ConditionResult.PASS)
				{
					list2.RemoveAt(i - num);
					num++;
				}
			}
			int count2 = list2.Count;
			if (count2 <= 0)
			{
				continue;
			}
			Dictionary<string, object> data = trigger.Data;
			if (!data.ContainsKey("resource_amounts"))
			{
				continue;
			}
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["resource_amounts"];
			bool flag2 = true;
			bool flag3 = false;
			for (int j = 0; j < count2; j++)
			{
				QuestBookendInfo.ChunkConditions chunkConditions = list2[j];
				Dictionary<string, object> dictionary2 = chunkConditions.Condition.ToDict();
				if (dictionary2.ContainsKey("count") && dictionary2.ContainsKey("resource_id"))
				{
					int num2 = TFUtils.LoadInt(dictionary2, "count");
					int key = TFUtils.LoadInt(dictionary2, "resource_id");
					int amount = game.resourceManager.Resources[key].Amount;
					if (dictionary.ContainsKey(key.ToString()))
					{
						flag3 = true;
					}
					if (amount < num2)
					{
						flag2 = false;
						break;
					}
				}
			}
			if (flag2 && !flag3)
			{
			}
		}
		if (deferredTriggers.Count <= 0)
		{
			return;
		}
		List<ITrigger> list3 = deferredTriggers;
		deferredTriggers = new List<ITrigger>();
		foreach (ITrigger item2 in list3)
		{
			game.triggerRouter.RouteTrigger(item2, game);
		}
	}

	public void HandleMicroEventClosedStatusChange(Game pGame, MicroEvent pMicroEvent)
	{
		if (pMicroEvent == null)
		{
			return;
		}
		int nDID = pMicroEvent.m_pMicroEventData.m_nDID;
		List<int> list = new List<int>();
		foreach (Quest value in questList.Values)
		{
			if (IsQuestCompleted(value.Did))
			{
				continue;
			}
			QuestDefinition questDefinition = GetQuestDefinition(value.Did);
			if (questDefinition == null || !questDefinition.MicroEventDID.HasValue || questDefinition.MicroEventDID.Value != nDID)
			{
				continue;
			}
			if (pMicroEvent.m_bIsClosed)
			{
				DeactivateQuest(pGame, value.Did);
				if (!pMicroEvent.IsCompleted() && !list.Contains(questDefinition.MicroEventDID.Value))
				{
					QueueDialogSequences(1u, (uint)pMicroEvent.m_pMicroEventData.m_nCloseDialogSequenceDID, new List<Reward>(), 0f, 0u);
					list.Add(questDefinition.MicroEventDID.Value);
				}
			}
			else
			{
				activatedDids.Add(value.Did);
				ProcessTrigger(null, pGame);
			}
		}
	}

	public bool IsQuestActive(uint did)
	{
		return activatedDids.Contains(did);
	}

	public bool QuestContainsPostponedDialog(int nQuestDID)
	{
		PostponedDialogParams[] array = postponed.ToArray();
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			PostponedDialogParams postponedDialogParams = array[i];
			if (postponedDialogParams.questId == nQuestDID)
			{
				return true;
			}
		}
		return false;
	}

	public int GetNumberOfActiveQuests()
	{
		return activatedDids.Count;
	}

	public void DebugCompleteAllQuests(Game game)
	{
		foreach (Quest value in questList.Values)
		{
			if (!completedDids.Contains(value.Did))
			{
				if (!IsQuestActive(value.Did))
				{
					ProgressTowardsStartConditions(value, game, value.StartConditions.Dehydrate().MetIds);
					ActivateQuest(value, game);
					game.Record(new QuestStartAction(value));
				}
				CompleteQuest(value, game);
			}
		}
		game.dialogPackageManager.ClearDialogs(game);
		postponed.Clear();
	}

	private List<string> HandleFeatureUnlocks(Game game, QuestDefinition questDef)
	{
		List<string> list = new List<string>();
		foreach (string featureUnlock in questDef.FeatureUnlocks)
		{
			if (game.featureManager.CheckFeature(featureUnlock))
			{
				continue;
			}
			list.Add(featureUnlock);
			game.featureManager.UnlockFeature(featureUnlock);
			game.featureManager.ActivateFeatureActions(game, featureUnlock);
			if (featureUnlock == "unrestrict_clicks" && game.tutorialLocked)
			{
				game.tutorialLocked = false;
				RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
				RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
				if (game.simulation.FindSimulated(PlayHavenController.PAYMIUM_ITEM_DID) == null && game.inventory.HasItem(PlayHavenController.PAYMIUM_ITEM_DID))
				{
					game.playHavenController.RequestContent("end_tutorial_paymium_item_in_inventory");
				}
			}
			else if (featureUnlock == "purchase_expansions" || featureUnlock == "purchase_expansions_boardwalk")
			{
				game.terrain.UpdateRealtySigns(game.entities.DisplayControllerManager, SBCamera.BillboardDefinition, game);
			}
		}
		return list;
	}

	private List<int> HandleBuildingUnlocks(Game game, QuestDefinition questDef)
	{
		List<int> list = new List<int>();
		foreach (int buildingUnlock in questDef.BuildingUnlocks)
		{
			if (!game.buildingUnlockManager.CheckBuildingUnlock(buildingUnlock))
			{
				list.Add(buildingUnlock);
				game.buildingUnlockManager.UnlockBuilding(buildingUnlock);
			}
		}
		return list;
	}

	private List<int> HandleCostumeUnlocks(Game game, QuestDefinition questDef)
	{
		List<int> list = new List<int>();
		foreach (int costumeUnlock in questDef.CostumeUnlocks)
		{
			if (!game.costumeManager.IsCostumeUnlocked(costumeUnlock))
			{
				list.Add(costumeUnlock);
				game.costumeManager.UnlockCostume(costumeUnlock);
			}
		}
		return list;
	}

	private void UpdateQuestLineProgress(Quest quest)
	{
		QuestDefinition questDefinition = questDefinitionList[quest.Did];
		if (questDefinition.QuestLine != null && questDefinition.QuestLine.HasProgress)
		{
			Vector2 value = questLineProgress[questDefinition.QuestLine.Name];
			value += Vector2.right;
			questLineProgress[questDefinition.QuestLine.Name] = value;
		}
	}

	private void EnqueueDeferredAction(Game game, PersistedTriggerableAction action)
	{
		action.Process(game);
		game.Record(action);
		deferredTriggers.Add(action.CreateTrigger(new Dictionary<string, object>()));
	}

	public string GetStoreTabValue()
	{
		List<uint> list = new List<uint>(activatedDids);
		foreach (uint item in list)
		{
			if (activatedDids.Contains(item))
			{
				QuestDefinition questDefinition = questDefinitionList[item];
				if (!string.IsNullOrEmpty(questDefinition.StoreTab))
				{
					return questDefinition.StoreTab;
				}
			}
		}
		return null;
	}
}
