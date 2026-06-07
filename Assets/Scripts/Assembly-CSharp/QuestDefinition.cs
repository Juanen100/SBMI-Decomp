using System.Collections.Generic;

public class QuestDefinition
{
	private const string DEFAULT_QUEST_TAG = "misc_quest";

	public const uint RANDOM_QUEST_ID_START = 400000u;

	public const uint RANDOM_QUEST_ID_END = 500000u;

	public const int RANDOM_QUEST_START_DIALOG = 10000;

	public const int RANDOM_QUEST_END_DIALOG = 10001;

	public const uint AUTO_QUEST_ID_START = 500001u;

	public const uint AUTO_QUEST_ID_END = 600000u;

	public const int AUTO_QUEST_START_DIALOG = 10002;

	public const int AUTO_QUEST_END_DIALOG = 10003;

	public const uint COMMUNITY_EVENT_FAKE_QUEST_ID = 600001u;

	private const string DID = "did";

	private const string NAME = "name";

	private const string CHUNK = "chunk";

	private const string TAG = "tag";

	private const string ICON = "icon";

	private const string DIALOG_HEADING = "dialog_heading";

	private const string DIALOG_BODY = "dialog_body";

	private const string PORTRAIT = "portrait";

	private const string FEATURE_UNLOCKS = "feature_unlocks";

	private const string BUILDING_UNLOCKS = "building_unlocks";

	private const string COSTUME_UNLOCKS = "costume_unlocks";

	private const string RESIDENT_UNLOCKS = "resident_unlocks";

	private const string DIALOG_PACKAGE_DID = "dialog_package_did";

	private const string QUEST_LINE = "quest_line";

	private const string SESSION_ACTIONS = "session_actions";

	private const string POST_SESSION_ACTIONS = "post_session_actions";

	private const string REWARD = "reward";

	private const string START = "start";

	private const string END = "end";

	private const string AUTO_QUEST_ID = "auto_quest_id";

	private const string AUTO_QUEST_CHAR_ID = "auto_quest_char_id";

	private const string MICRO_EVENT_DID = "micro_event_id";

	private const string BRANCH = "branch";

	private const string STORE_TAB = "store_tab";

	private string storeTab;

	private uint did;

	private string name;

	private bool chunk;

	private string tag;

	private string icon;

	private uint dialogPackageDid;

	private string dialogHeading;

	private string dialogBody;

	private string portrait;

	private QuestBookendInfo start;

	private QuestBookendInfo end;

	private SessionActionDefinition sessionActions;

	private SessionActionDefinition postSessionActions;

	private RewardDefinition rewardDefinition;

	private List<string> featureUnlocks;

	private List<int> buildingUnlocks;

	private List<int> costumeUnlocks;

	private List<int> residentUnlocks;

	private string collectStart;

	private string collectComplete;

	private QuestLineInfo questLine;

	private int autoQuestID;

	private int autoQuestCharacterID;

	private int? microEventDID;

	private string branch;

	public static uint LastRandomQuestId;

	public static uint LastAutoQuestId;

	public static Dictionary<uint, Dictionary<string, object>> StartInputPrompts;

	public static Dictionary<uint, Dictionary<string, object>> CompleteInputPrompts;

	public uint Did
	{
		get
		{
			return 0u;
		}
	}

	public string Name
	{
		get
		{
			return null;
		}
	}

	public bool Chunk
	{
		get
		{
			return false;
		}
	}

	public string Tag
	{
		get
		{
			return null;
		}
	}

	public string Icon
	{
		get
		{
			return null;
		}
	}

	public string DialogHeading
	{
		get
		{
			return null;
		}
	}

	public string StoreTab
	{
		get
		{
			return null;
		}
	}

	public string DialogBody
	{
		get
		{
			return null;
		}
	}

	public string Portrait
	{
		get
		{
			return null;
		}
	}

	public uint DialogPackageDid
	{
		get
		{
			return 0u;
		}
	}

	public int AutoQuestID
	{
		get
		{
			return 0;
		}
	}

	public int AutoQuestCharacterID
	{
		get
		{
			return 0;
		}
	}

	public int? MicroEventDID
	{
		get
		{
			return null;
		}
	}

	public QuestBookendInfo Start
	{
		get
		{
			return null;
		}
	}

	public QuestBookendInfo End
	{
		get
		{
			return null;
		}
	}

	public Reward Reward
	{
		get
		{
			return null;
		}
	}

	public bool HasFeatureUnlocks
	{
		get
		{
			return false;
		}
	}

	public bool HasBuildingUnlocks
	{
		get
		{
			return false;
		}
	}

	public bool HasCostumeUnlocks
	{
		get
		{
			return false;
		}
	}

	public bool HasResidentUnlocks
	{
		get
		{
			return false;
		}
	}

	public List<string> FeatureUnlocks
	{
		get
		{
			return null;
		}
	}

	public List<int> BuildingUnlocks
	{
		get
		{
			return null;
		}
	}

	public List<int> CostumeUnlocks
	{
		get
		{
			return null;
		}
	}

	public List<int> ResidentUnlocks
	{
		get
		{
			return null;
		}
	}

	public string CollectStart
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string CollectComplete
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SessionActionDefinition SessionActions
	{
		get
		{
			return null;
		}
	}

	public SessionActionDefinition PostSessionActions
	{
		get
		{
			return null;
		}
	}

	public QuestLineInfo QuestLine
	{
		get
		{
			return null;
		}
	}

	public string Branch
	{
		get
		{
			return null;
		}
	}

	private QuestDefinition()
	{
	}

	public static string GenerateSessionActionId(uint did)
	{
		return null;
	}

	public Dictionary<string, object> ToDict(bool bForceRandomQuestTrigger)
	{
		return null;
	}

	public static QuestDefinition FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public static Resource GetRandomRecipe(Game game)
	{
		return null;
	}

	public static string ParseResourceFieldString(Resource resource, string field)
	{
		return null;
	}

	public static int? ParseResourceFieldInt(Resource resource, string field)
	{
		return null;
	}

	public static QuestDefinition ParseAutoQuest(AutoQuest pAutoQuest, Game pGame)
	{
		return null;
	}

	public static QuestDefinition ParseRandomTemplate(QuestTemplate template, Game game)
	{
		return null;
	}

	public static QuestDefinition CreateRandom(QuestManager questManager, Game game)
	{
		return null;
	}

	public static QuestDefinition CreateAuto(Game pGame)
	{
		return null;
	}

	public static QuestDialogInputData RecreateRandomQuestStartInputData(Game game, uint target)
	{
		return null;
	}

	public static QuestDialogInputData RecreateRandomQuestCompleteInputData(Game game, uint target)
	{
		return null;
	}

	public static CharacterDialogInputData RecreateAutoQuestIntroInputData(Game pGame, uint uTarget)
	{
		return null;
	}

	public static CharacterDialogInputData RecreateAutoQuestOutroInputData(Game pGame, uint uTarget)
	{
		return null;
	}

	public void Initialize(uint id, string name, bool chunk, string tag, string icon, string dialogHeading, string dialogBody, string portrait, string branch, uint dialogPackageId, int autoQuestID, int autoQuestCharacterID, int? microEventDID, QuestBookendInfo start, QuestBookendInfo end, QuestLineInfo questLine, SessionActionDefinition sessionActions, SessionActionDefinition postSessionActions, RewardDefinition rewardDefinition, List<string> featureUnlocks, List<int> buildingUnlocks, List<int> costumeUnlocks, string storeTab, List<int> residentUnlocks = null)
	{
	}

	public override string ToString()
	{
		return null;
	}

	public Reward GenerateReward(Simulation simulation)
	{
		return null;
	}
}
