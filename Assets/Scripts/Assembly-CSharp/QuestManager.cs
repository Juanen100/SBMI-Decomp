using System;
using System.Collections.Generic;
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

	public ulong m_uQuestTimeGap;

	public ulong? m_uQuestCompletionTimestamp;

	public ulong? m_uAutoQuestStartTime;

	public ulong m_uTimeTillResetQuest;

	public ulong m_uCurrentTime;

	public int m_autoQuestCount;

	private bool activated;

	private static readonly string QUESTS_PATH;

	private bool showDialogs;

	private bool isActive;

	private DialogPackageManager dialogPackageManager;

	private List<ITrigger> deferredTriggers;

	private Dictionary<uint, Quest> questList;

	private Dictionary<uint, QuestDefinition> questDefinitionList;

	private Dictionary<uint, QuestTemplate> randomQuestTemplateList;

	private OrderedSet<uint> activatedDids;

	private OrderedSet<uint> completedDids;

	private OrderedSet<uint> deactivatedCompletedDids;

	private Action onShowDialogCallback;

	private Dictionary<string, Vector2> questLineProgress;

	private Queue<PostponedDialogParams> postponed;

	public bool IsActive
	{
		get
		{
			return false;
		}
	}

	public Action OnShowDialogCallback
	{
		set
		{
		}
	}

	public OrderedSet<uint> ActiveQuestDids
	{
		get
		{
			return null;
		}
	}

	public OrderedSet<uint> ActiveQuestDidsNotInPostponed
	{
		get
		{
			return null;
		}
	}

	public OrderedSet<uint> CompletedQuestDids
	{
		get
		{
			return null;
		}
	}

	public Dictionary<uint, QuestDefinition> QuestDefinitionList
	{
		get
		{
			return null;
		}
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	private void LoadAndInitializeQuestPrototypes()
	{
	}

	public void AddRandomQuestTemplate(QuestTemplate questDef)
	{
	}

	public Quest AddQuestDefinition(QuestDefinition questDef)
	{
		return null;
	}

	public QuestTemplate GetRandomQuestTemplate()
	{
		return null;
	}

	public Quest CreateNewQuestInfo(uint did)
	{
		return null;
	}

	public void SetDialogManager(DialogPackageManager dialogPackageMgr)
	{
	}

	public void Activate(Game game)
	{
	}

	public Quest GetQuest(uint did)
	{
		return null;
	}

	public QuestDefinition GetQuestDefinition(uint did)
	{
		return null;
	}

	public List<int> GetTasksCompleting()
	{
		return null;
	}

	public void RegisterQuest(Game pGame, Quest quest)
	{
	}

	public void ActivateQuest(Quest quest, Game game)
	{
	}

	public void DeactivateQuest(Game game, uint did)
	{
	}

	public void CompleteQuest(Quest quest, Game game)
	{
	}

	public float? GetQuestLineProgress(QuestDefinition questDef)
	{
		return null;
	}

	public float? GetQuestLineLastProgress(QuestDefinition questDef)
	{
		return null;
	}

	private void ProgressTowardsStartConditions(Quest quest, Game game, List<uint> conditionIds)
	{
	}

	private void ProgressTowardsEndConditions(Quest quest, Game game, List<uint> conditionIds)
	{
	}

	private void FailQuest(Quest quest)
	{
	}

	public void OnUpdate(Game game)
	{
	}

	public bool IsQuestActivated(uint did)
	{
		return false;
	}

	public bool IsQuestCompleted(uint did)
	{
		return false;
	}

	public void CreateAndTriggerRandomQuest(Game game)
	{
	}

	public void CreateAndTriggerAutoQuest(Game pGame)
	{
	}

	private void QueueDialogSequences(uint packageId, uint sequenceId, List<Reward> rewards, float postpone, uint questId)
	{
	}

	public void AddDialogSequences(Game game, uint packageId, uint sequenceId, List<Reward> rewards, uint questId, bool bShowDialogs = true)
	{
	}

	public void ProcessTrigger(ITrigger trigger, Game game)
	{
	}

	public void HandleMicroEventClosedStatusChange(Game pGame, MicroEvent pMicroEvent)
	{
	}

	public bool IsQuestActive(uint did)
	{
		return false;
	}

	public bool QuestContainsPostponedDialog(int nQuestDID)
	{
		return false;
	}

	public int GetNumberOfActiveQuests()
	{
		return 0;
	}

	public void DebugCompleteAllQuests(Game game)
	{
	}

	private List<string> HandleFeatureUnlocks(Game game, QuestDefinition questDef)
	{
		return null;
	}

	private List<int> HandleBuildingUnlocks(Game game, QuestDefinition questDef)
	{
		return null;
	}

	private List<int> HandleCostumeUnlocks(Game game, QuestDefinition questDef)
	{
		return null;
	}

	private List<int> HandleResidentUnlocks(Game game, QuestDefinition questDef)
	{
		return null;
	}

	private void UpdateQuestLineProgress(Quest quest)
	{
	}

	private void EnqueueDeferredAction(Game game, PersistedTriggerableAction action)
	{
	}

	public string GetStoreTabValue()
	{
		return null;
	}
}
