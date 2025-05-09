#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

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

	private string collectStart;

	private string collectComplete;

	private QuestLineInfo questLine;

	private int autoQuestID;

	private int autoQuestCharacterID;

	private int? microEventDID;

	private string branch;

	public static uint LastRandomQuestId;

	public static uint LastAutoQuestId;

	public static Dictionary<uint, Dictionary<string, object>> StartInputPrompts = new Dictionary<uint, Dictionary<string, object>>();

	public static Dictionary<uint, Dictionary<string, object>> CompleteInputPrompts = new Dictionary<uint, Dictionary<string, object>>();

	public uint Did
	{
		get
		{
			return did;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public bool Chunk
	{
		get
		{
			return chunk;
		}
	}

	public string Tag
	{
		get
		{
			return tag;
		}
	}

	public string Icon
	{
		get
		{
			return icon;
		}
	}

	public string DialogHeading
	{
		get
		{
			return dialogHeading;
		}
	}

	public string StoreTab
	{
		get
		{
			return storeTab;
		}
	}

	public string DialogBody
	{
		get
		{
			return dialogBody;
		}
	}

	public string Portrait
	{
		get
		{
			return portrait;
		}
	}

	public uint DialogPackageDid
	{
		get
		{
			return dialogPackageDid;
		}
	}

	public int AutoQuestID
	{
		get
		{
			return autoQuestID;
		}
	}

	public int AutoQuestCharacterID
	{
		get
		{
			return autoQuestCharacterID;
		}
	}

	public int? MicroEventDID
	{
		get
		{
			return microEventDID;
		}
	}

	public QuestBookendInfo Start
	{
		get
		{
			return start;
		}
	}

	public QuestBookendInfo End
	{
		get
		{
			return end;
		}
	}

	public Reward Reward
	{
		get
		{
			return rewardDefinition.Summary;
		}
	}

	public bool HasFeatureUnlocks
	{
		get
		{
			return featureUnlocks.Count > 0;
		}
	}

	public bool HasBuildingUnlocks
	{
		get
		{
			return buildingUnlocks.Count > 0;
		}
	}

	public bool HasCostumeUnlocks
	{
		get
		{
			return costumeUnlocks.Count > 0;
		}
	}

	public List<string> FeatureUnlocks
	{
		get
		{
			return TFUtils.CloneAndCastList<string, string>(featureUnlocks);
		}
	}

	public List<int> BuildingUnlocks
	{
		get
		{
			return TFUtils.CloneAndCastList<int, int>(buildingUnlocks);
		}
	}

	public List<int> CostumeUnlocks
	{
		get
		{
			return TFUtils.CloneAndCastList<int, int>(costumeUnlocks);
		}
	}

	public string CollectStart
	{
		get
		{
			return collectStart;
		}
		set
		{
			collectStart = value;
		}
	}

	public string CollectComplete
	{
		get
		{
			return collectComplete;
		}
		set
		{
			collectComplete = value;
		}
	}

	public SessionActionDefinition SessionActions
	{
		get
		{
			return sessionActions;
		}
	}

	public SessionActionDefinition PostSessionActions
	{
		get
		{
			return postSessionActions;
		}
	}

	public QuestLineInfo QuestLine
	{
		get
		{
			return questLine;
		}
	}

	public string Branch
	{
		get
		{
			return branch;
		}
	}

	private QuestDefinition()
	{
	}

	public static string GenerateSessionActionId(uint did)
	{
		return "QuestTracker_" + did;
	}

	public Dictionary<string, object> ToDict(bool bForceRandomQuestTrigger)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = did;
		dictionary["name"] = name;
		dictionary["chunk"] = chunk;
		dictionary["icon"] = icon;
		dictionary["dialog_heading"] = dialogHeading;
		dictionary["dialog_body"] = dialogBody;
		dictionary["portrait"] = portrait;
		dictionary["tag"] = tag;
		dictionary["start"] = start.ToDict();
		dictionary["end"] = end.ToDict();
		dictionary["dialog_package_did"] = dialogPackageDid;
		dictionary["branch"] = branch;
		if (!string.IsNullOrEmpty(storeTab))
		{
			dictionary["store_tab"] = storeTab;
		}
		if (microEventDID.HasValue)
		{
			dictionary["micro_event_id"] = microEventDID.Value;
		}
		if (questLine != null)
		{
			dictionary["quest_line"] = questLine.ToDict();
		}
		if (sessionActions != null)
		{
			dictionary["session_actions"] = sessionActions.ToDict();
		}
		if (postSessionActions != null)
		{
			dictionary["post_session_actions"] = postSessionActions.ToDict();
		}
		if (rewardDefinition != null)
		{
			dictionary["reward"] = rewardDefinition.ToDict();
		}
		if (CollectStart != null)
		{
			dictionary["CollectStart"] = CollectStart;
		}
		if (CollectComplete != null)
		{
			dictionary["CollectComplete"] = CollectComplete;
		}
		if (autoQuestID >= 0)
		{
			dictionary["auto_quest_id"] = autoQuestID;
		}
		if (autoQuestCharacterID >= 0)
		{
			dictionary["auto_quest_char_id"] = autoQuestCharacterID;
		}
		return dictionary;
	}

	public static QuestDefinition FromDict(Dictionary<string, object> data)
	{
		string text = TFUtils.TryLoadString(data, "type");
		QuestDefinition questDefinition = new QuestDefinition();
		if (text == null)
		{
			text = "quest";
		}
		uint num = TFUtils.LoadUint(data, "did");
		string text2 = TFUtils.LoadString(data, "name");
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = TFUtils.TryLoadString(data, "tag");
		string text6 = TFUtils.TryLoadString(data, "store_tab");
		if (data.ContainsKey("branch"))
		{
			text4 = TFUtils.LoadString(data, "branch");
		}
		if (text5 == null)
		{
			text5 = "misc_quest";
		}
		if (data.ContainsKey("icon"))
		{
			text3 = TFUtils.LoadString(data, "icon");
		}
		else
		{
			TFUtils.Assert(false, "QuestDid " + num + " does not have an icon.");
		}
		int? num2 = null;
		if (data.ContainsKey("micro_event_id"))
		{
			num2 = TFUtils.TryLoadNullableInt(data, "micro_event_id");
		}
		int num3 = -1;
		if (data.ContainsKey("auto_quest_id"))
		{
			num3 = TFUtils.LoadInt(data, "auto_quest_id");
		}
		int num4 = -1;
		if (data.ContainsKey("auto_quest_char_id"))
		{
			num4 = TFUtils.LoadInt(data, "auto_quest_char_id");
		}
		string text7 = string.Empty;
		string text8 = string.Empty;
		string text9 = string.Empty;
		bool chunkQuest = false;
		if (data.ContainsKey("chunk"))
		{
			chunkQuest = TFUtils.LoadBool(data, "chunk");
			text8 = TFUtils.LoadString(data, "dialog_body");
			text9 = TFUtils.LoadString(data, "portrait");
			if (data.ContainsKey("dialog_heading"))
			{
				text7 = TFUtils.LoadString(data, "dialog_heading");
			}
		}
		questDefinition.CollectStart = TFUtils.TryLoadString(data, "CollectStart");
		questDefinition.CollectComplete = TFUtils.TryLoadString(data, "CollectComplete");
		List<string> list = ((!data.ContainsKey("feature_unlocks")) ? new List<string>() : TFUtils.LoadList<string>(data, "feature_unlocks"));
		List<int> list2 = ((!data.ContainsKey("building_unlocks")) ? new List<int>() : TFUtils.LoadList<int>(data, "building_unlocks"));
		List<int> list3 = ((!data.ContainsKey("costume_unlocks")) ? new List<int>() : TFUtils.LoadList<int>(data, "costume_unlocks"));
		uint dialogPackageId = TFUtils.LoadUint(data, "dialog_package_did");
		bool autoQuest = num >= 500001 && num <= 600000;
		QuestBookendInfo questBookendInfo = QuestBookendInfo.FromDict(TFUtils.LoadDict(data, "start"), false, autoQuest);
		QuestBookendInfo questBookendInfo2 = QuestBookendInfo.FromDict(TFUtils.LoadDict(data, "end"), chunkQuest, autoQuest);
		QuestLineInfo questLineInfo = null;
		if (data.ContainsKey("quest_line"))
		{
			questLineInfo = QuestLineInfo.FromDict(TFUtils.LoadDict(data, "quest_line"));
		}
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(data["reward"]);
		ICondition condition = null;
		condition = ((!questBookendInfo.DialogSequenceId.HasValue) ? ((LoadableCondition)new ConstantCondition(0u, true)) : ((LoadableCondition)new CompleteDialogCondition(0u, questBookendInfo.DialogSequenceId.Value)));
		SessionActionDefinition sessionActionDefinition = null;
		if (data.ContainsKey("session_actions"))
		{
			sessionActionDefinition = SessionActionFactory.Create((Dictionary<string, object>)data["session_actions"], condition, num, 0u);
		}
		SessionActionDefinition sessionActionDefinition2 = null;
		if (data.ContainsKey("post_session_actions"))
		{
			sessionActionDefinition2 = SessionActionFactory.Create((Dictionary<string, object>)data["post_session_actions"], new ConstantCondition(0u, true), num, 0u);
		}
		questDefinition.Initialize(num, text2, chunkQuest, text5, text3, text7, text8, text9, text4, dialogPackageId, num3, num4, num2, questBookendInfo, questBookendInfo2, questLineInfo, sessionActionDefinition, sessionActionDefinition2, rewardDefinition, list, list2, list3, text6);
		TFUtils.DebugLog("Loaded Quest " + questDefinition.did + " (" + questDefinition.name + ")", TFUtils.LogFilter.Quests);
		return questDefinition;
	}

	public static Resource GetRandomRecipe(Game game)
	{
		List<int> list = game.resourceManager.ConsumableProducts(game.craftManager);
		HashSet<int> jellyBasedRecipesCopy = game.craftManager.JellyBasedRecipesCopy;
		foreach (int item in jellyBasedRecipesCopy)
		{
			if (list.Contains(item))
			{
				list.Remove(item);
			}
		}
		HashSet<int> ignoreRandomQuestRecipesCopy = game.craftManager.IgnoreRandomQuestRecipesCopy;
		foreach (int item2 in ignoreRandomQuestRecipesCopy)
		{
			if (list.Contains(item2))
			{
				list.Remove(item2);
			}
		}
		int index = Random.Range(0, list.Count);
		int key = list[index];
		return game.resourceManager.Resources[key];
	}

	public static string ParseResourceFieldString(Resource resource, string field)
	{
		switch (field)
		{
		case "Name":
			return resource.Name;
		case "Texture":
			return resource.GetResourceTexture();
		default:
			TFUtils.ErrorLog("Random resource does not support this field yet");
			return null;
		}
	}

	public static int? ParseResourceFieldInt(Resource resource, string field)
	{
		switch (field)
		{
		case "Did":
			return resource.Did;
		default:
			TFUtils.ErrorLog("Random resource does not support this field yet");
			return null;
		}
	}

	public static QuestDefinition ParseAutoQuest(AutoQuest pAutoQuest, Game pGame)
	{
		if (pAutoQuest == null || pGame == null)
		{
			return null;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "quests");
		dictionary.Add("did", LastAutoQuestId);
		dictionary.Add("name", pAutoQuest.m_sName);
		dictionary.Add("dialog_heading", pAutoQuest.m_sName);
		dictionary.Add("dialog_body", pAutoQuest.m_sDescription);
		dictionary.Add("tag", "auto_quest_" + pAutoQuest.m_nDID);
		dictionary.Add("dialog_package_did", 1);
		dictionary.Add("chunk", true);
		dictionary.Add("auto_quest_id", pAutoQuest.m_nDID);
		dictionary.Add("auto_quest_char_id", pAutoQuest.m_nCharacterDID);
		dictionary.Add("portrait", "mrkrabsportrait_moneygrubbing.png");
		dictionary.Add("CollectStart", "Missing Dialog Text");
		dictionary.Add("CollectComplete", "Missing Dialog Text");
		dictionary.Add("icon", "sb_item_peanutbrittle.png");
		Simulated simulated = pGame.simulation.FindSimulated(pAutoQuest.m_nCharacterDID);
		if (simulated != null && simulated.HasEntity<ResidentEntity>())
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			if (entity.DialogPortrait != null)
			{
				dictionary["portrait"] = entity.DialogPortrait;
			}
			AutoQuestData.DialogData dialogData = pGame.autoQuestDatabase.GetDialogData(pAutoQuest.m_nDID, entity.AutoQuestIntro);
			if (dialogData != null)
			{
				dictionary["CollectStart"] = dialogData.m_sIntroDialog;
			}
			dialogData = pGame.autoQuestDatabase.GetDialogData(pAutoQuest.m_nDID, entity.AutoQuestOutro);
			if (dialogData != null)
			{
				dictionary["CollectComplete"] = dialogData.m_sOutroDialog;
			}
			if (entity.QuestReminderIcon != null)
			{
				dictionary["icon"] = entity.QuestReminderIcon;
			}
		}
		dictionary.Add("start", new Dictionary<string, object>
		{
			{
				"conditions",
				new Dictionary<string, object>
				{
					{ "id", 1 },
					{ "type", "complete_quest" },
					{ "quest_id", 0 }
				}
			},
			{ "postpone", 0 },
			{ "dialog_sequence_id", 10002 }
		});
		List<object> list = new List<object>();
		int num = 1;
		foreach (KeyValuePair<int, int> pRecipe in pAutoQuest.m_pRecipes)
		{
			list.Add(new Dictionary<string, object>
			{
				{
					"conditions",
					new Dictionary<string, object>
					{
						{ "id", num },
						{ "type", "auto_quest_craft_collect" },
						{ "resource_id", pRecipe.Key },
						{ "count", pRecipe.Value }
					}
				},
				{
					"name",
					pGame.resourceManager.Resources[pRecipe.Key].Name
				},
				{
					"icon",
					pGame.resourceManager.Resources[pRecipe.Key].GetResourceTexture()
				}
			});
			num++;
		}
		list.Add(new Dictionary<string, object>
		{
			{
				"conditions",
				new Dictionary<string, object>
				{
					{ "id", num },
					{ "type", "auto_quest_all_done" },
					{ "quest_id", LastAutoQuestId }
				}
			},
			{
				"name",
				string.Empty
			},
			{
				"icon",
				string.Empty
			}
		});
		dictionary.Add("end", new Dictionary<string, object>
		{
			{ "array", list },
			{ "dialog_sequence_id", 10003 }
		});
		dictionary.Add("reward", new Dictionary<string, object>
		{
			{
				"resources",
				new Dictionary<string, object>
				{
					{ "3", pAutoQuest.m_nGoldReward },
					{ "5", pAutoQuest.m_nXPReward }
				}
			},
			{ "thought_icon", null }
		});
		return FromDict(dictionary);
	}

	public static QuestDefinition ParseRandomTemplate(QuestTemplate template, Game game)
	{
		Dictionary<string, object> templateData = template.TemplateData;
		string text = (string)templateData["icon"];
		Dictionary<string, object> dictionary = (Dictionary<string, object>)templateData["variables"];
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)templateData["end"];
		Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dictionary2["conditions"];
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		Dictionary<string, object> o = (Dictionary<string, object>)templateData["reward"];
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		foreach (string key2 in dictionary.Keys)
		{
			Dictionary<string, object> dictionary6 = (Dictionary<string, object>)dictionary[key2];
			switch ((string)dictionary6["type"])
			{
			case "RandomRecipe":
				dictionary5.Add(key2, GetRandomRecipe(game));
				break;
			case "RandomInt":
			{
				int min = int.Parse(dictionary6["min"].ToString());
				int max = 0;
				string text2 = (string)dictionary6["max"];
				if (text2 == "$Level")
				{
					max = game.resourceManager.Resources[ResourceManager.LEVEL].Amount;
				}
				else
				{
					TFUtils.ErrorLog("RandomInt only supports playerlevel for max right now");
				}
				dictionary5.Add(key2, Random.Range(min, max));
				break;
			}
			case "string":
				dictionary5.Add(key2, dictionary6["value"] as string);
				break;
			case "int":
				dictionary5.Add(key2, int.Parse(dictionary6["value"].ToString()));
				break;
			case "playerLevel":
				dictionary5.Add(key2, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				break;
			default:
				TFUtils.ErrorLog("This random template variable type is not implemented yet!");
				break;
			}
		}
		string value = string.Empty;
		bool chunkQuest = false;
		if (templateData.ContainsKey("chunk"))
		{
			chunkQuest = TFUtils.LoadBool(templateData, "chunk");
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string value2 = (string)text.Clone();
		string empty4 = string.Empty;
		string empty5 = string.Empty;
		string empty6 = string.Empty;
		string text3 = null;
		if (text.StartsWith("$"))
		{
			empty4 = text.Substring(0, text.IndexOf('.'));
			empty5 = text.Substring(text.IndexOf('.') + 1);
			if (dictionary5.ContainsKey(empty4))
			{
				value2 = ParseResourceFieldString((Resource)dictionary5[empty4], empty5);
			}
		}
		foreach (string key3 in dictionary3.Keys)
		{
			dictionary4.Add(key3, dictionary3[key3]);
		}
		string text4 = dictionary3["resource_id"] as string;
		empty4 = text4.Substring(0, text4.IndexOf('.'));
		empty5 = text4.Substring(text4.IndexOf('.') + 1);
		if (dictionary5.ContainsKey(empty4))
		{
			dictionary4["resource_id"] = ParseResourceFieldInt((Resource)dictionary5[empty4], empty5);
		}
		text4 = (string)dictionary3["count"];
		if (dictionary5.ContainsKey(text4))
		{
			dictionary4["count"] = int.Parse(dictionary5[text4].ToString());
		}
		QuestDefinition questDefinition = new QuestDefinition();
		questDefinition.CollectStart = dictionary5["$CollectStart"] as string;
		questDefinition.CollectComplete = dictionary5["$CollectComplete"] as string;
		uint lastRandomQuestId = LastRandomQuestId;
		string text5 = "misc_quest";
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		uint dialogPackageId = 1u;
		Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
		dictionary7["id"] = 1;
		dictionary7["type"] = "complete_quest";
		dictionary7["quest_id"] = 0;
		Dictionary<string, object> dictionary8 = new Dictionary<string, object>();
		dictionary8["conditions"] = dictionary7;
		dictionary8["dialog_sequence_id"] = 10000;
		StartInputPrompts[lastRandomQuestId] = new Dictionary<string, object>();
		CompleteInputPrompts[lastRandomQuestId] = new Dictionary<string, object>();
		StartInputPrompts[lastRandomQuestId]["type"] = "quest_start";
		CompleteInputPrompts[lastRandomQuestId]["type"] = "quest_complete";
		if ((string)dictionary4["type"] == "craft_collect")
		{
			int key = int.Parse(dictionary4["resource_id"].ToString());
			int num = int.Parse(dictionary4["count"].ToString());
			string name_Plural = game.resourceManager.Resources[key].Name;
			if (num > 1)
			{
				name_Plural = game.resourceManager.Resources[key].Name_Plural;
			}
			value = string.Format(Language.Get((string)dictionary5["$CollectStart"]), num.ToString(), name_Plural);
			StartInputPrompts[lastRandomQuestId]["title"] = value;
			StartInputPrompts[lastRandomQuestId]["icon"] = value2;
			CompleteInputPrompts[lastRandomQuestId]["title"] = string.Format(Language.Get((string)dictionary5["$CollectComplete"]), num.ToString(), name_Plural);
			CompleteInputPrompts[lastRandomQuestId]["icon"] = value2;
		}
		else
		{
			TFUtils.ErrorLog("Random Quest does not support this endCondition type yet");
		}
		Dictionary<string, object> dictionary9 = new Dictionary<string, object>();
		dictionary9["conditions"] = dictionary4;
		dictionary9["dialog_sequence_id"] = 10001;
		QuestBookendInfo questBookendInfo = QuestBookendInfo.FromDict(dictionary8, false, false);
		QuestBookendInfo questBookendInfo2 = QuestBookendInfo.FromDict(dictionary9, chunkQuest, false);
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(o);
		SessionActionDefinition sessionActionDefinition = null;
		Dictionary<string, object> dictionary10 = new Dictionary<string, object>();
		dictionary10["type"] = "trigger_random_quest";
		List<object> list4 = new List<object>();
		list4.Add(dictionary10);
		Dictionary<string, object> dictionary11 = new Dictionary<string, object>();
		dictionary11["type"] = "array";
		dictionary11["actions"] = list4;
		SessionActionDefinition sessionActionDefinition2 = null;
		questDefinition.Initialize(lastRandomQuestId, value, chunkQuest, text5, value2, empty, empty2, empty3, empty6, dialogPackageId, -1, -1, null, questBookendInfo, questBookendInfo2, null, sessionActionDefinition, sessionActionDefinition2, rewardDefinition, list, list2, list3, text3);
		return questDefinition;
	}

	public static QuestDefinition CreateRandom(QuestManager questManager, Game game)
	{
		QuestTemplate randomQuestTemplate = questManager.GetRandomQuestTemplate();
		return ParseRandomTemplate(randomQuestTemplate, game);
	}

	public static QuestDefinition CreateAuto(Game pGame)
	{
		AutoQuest autoQuest = pGame.autoQuestDatabase.GenerateNextAutoQuest(pGame);
		if (autoQuest == null)
		{
			return null;
		}
		return ParseAutoQuest(autoQuest, pGame);
	}

	public static QuestDialogInputData RecreateRandomQuestStartInputData(Game game, uint target)
	{
		QuestDefinition questDefinition = game.questManager.GetQuestDefinition(target);
		Dictionary<string, object> dictionary = questDefinition.end.Chunks[0].Condition.ToDict();
		StartInputPrompts[target] = new Dictionary<string, object>();
		StartInputPrompts[target]["type"] = "quest_start";
		if ((string)dictionary["type"] == "craft_collect")
		{
			int key = TFUtils.LoadInt(dictionary, "resource_id");
			int num = TFUtils.LoadInt(dictionary, "count");
			string resourceTexture = game.resourceManager.Resources[key].GetResourceTexture();
			StartInputPrompts[target]["title"] = string.Format(Language.Get(questDefinition.CollectStart), num.ToString(), game.resourceManager.Resources[key].Name);
			StartInputPrompts[target]["icon"] = resourceTexture;
		}
		List<object> list = new List<object>();
		list.Add(questDefinition.Reward.ToDict());
		List<object> value = list;
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2.Add("rewards", value);
		Dictionary<string, object> contextData = dictionary2;
		return new QuestStartDialogInputData(10000u, StartInputPrompts[target], contextData, target);
	}

	public static QuestDialogInputData RecreateRandomQuestCompleteInputData(Game game, uint target)
	{
		QuestDefinition questDefinition = game.questManager.GetQuestDefinition(target);
		Dictionary<string, object> dictionary = questDefinition.end.Chunks[0].Condition.ToDict();
		CompleteInputPrompts[target] = new Dictionary<string, object>();
		CompleteInputPrompts[target]["type"] = "quest_complete";
		if ((string)dictionary["type"] == "craft_collect")
		{
			int key = TFUtils.LoadInt(dictionary, "resource_id");
			int num = TFUtils.LoadInt(dictionary, "count");
			string resourceTexture = game.resourceManager.Resources[key].GetResourceTexture();
			CompleteInputPrompts[target]["title"] = string.Format(Language.Get(questDefinition.CollectComplete), num.ToString(), game.resourceManager.Resources[key].Name);
			CompleteInputPrompts[target]["icon"] = resourceTexture;
		}
		List<object> list = null;
		if (questDefinition.Reward != null)
		{
			List<object> list2 = new List<object>();
			list2.Add(questDefinition.Reward.ToDict());
			list = list2;
		}
		else
		{
			list = new List<object>();
		}
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2.Add("rewards", list);
		Dictionary<string, object> contextData = dictionary2;
		return new QuestCompleteDialogInputData(10000u, CompleteInputPrompts[target], contextData, target);
	}

	public static CharacterDialogInputData RecreateAutoQuestIntroInputData(Game pGame, uint uTarget)
	{
		QuestDefinition questDefinition = pGame.questManager.GetQuestDefinition(uTarget);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "character");
		dictionary.Add("character_icon", questDefinition.Portrait);
		dictionary.Add("text", questDefinition.CollectStart);
		if (StartInputPrompts.ContainsKey(uTarget))
		{
			StartInputPrompts[uTarget] = dictionary;
		}
		else
		{
			StartInputPrompts.Add(uTarget, dictionary);
		}
		return new CharacterDialogInputData(10002u, dictionary);
	}

	public static CharacterDialogInputData RecreateAutoQuestOutroInputData(Game pGame, uint uTarget)
	{
		QuestDefinition questDefinition = pGame.questManager.GetQuestDefinition(uTarget);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "character");
		dictionary.Add("character_icon", questDefinition.Portrait);
		dictionary.Add("text", questDefinition.CollectComplete);
		if (CompleteInputPrompts.ContainsKey(uTarget))
		{
			CompleteInputPrompts[uTarget] = dictionary;
		}
		else
		{
			CompleteInputPrompts.Add(uTarget, dictionary);
		}
		return new CharacterDialogInputData(10003u, dictionary);
	}

	public void Initialize(uint id, string name, bool chunk, string tag, string icon, string dialogHeading, string dialogBody, string portrait, string branch, uint dialogPackageId, int autoQuestID, int autoQuestCharacterID, int? microEventDID, QuestBookendInfo start, QuestBookendInfo end, QuestLineInfo questLine, SessionActionDefinition sessionActions, SessionActionDefinition postSessionActions, RewardDefinition rewardDefinition, List<string> featureUnlocks, List<int> buildingUnlocks, List<int> costumeUnlocks, string storeTab)
	{
		did = id;
		this.start = start;
		this.end = end;
		this.name = name;
		this.chunk = chunk;
		this.tag = tag;
		this.icon = icon;
		this.dialogHeading = dialogHeading;
		this.dialogBody = dialogBody;
		this.portrait = portrait;
		this.branch = branch;
		dialogPackageDid = dialogPackageId;
		this.sessionActions = sessionActions;
		this.postSessionActions = postSessionActions;
		this.rewardDefinition = rewardDefinition;
		this.featureUnlocks = featureUnlocks;
		this.buildingUnlocks = buildingUnlocks;
		this.costumeUnlocks = costumeUnlocks;
		this.questLine = questLine;
		this.autoQuestID = autoQuestID;
		this.autoQuestCharacterID = autoQuestCharacterID;
		this.microEventDID = microEventDID;
		this.storeTab = storeTab;
	}

	public override string ToString()
	{
		string text = "QuestDefinition(";
		text = text + "did=" + did;
		text = text + ", name=" + name;
		text = text + ", tag=" + tag;
		text = text + ", branch=" + branch;
		text += ", startingConditions=[";
		foreach (QuestBookendInfo.ChunkConditions chunk in start.Chunks)
		{
			string text2 = text;
			text = text2 + "( " + chunk.Condition.Description(null) + ", " + chunk.Name + ", " + chunk.Icon + " ), ";
		}
		text += "], endingConditions=[";
		foreach (QuestBookendInfo.ChunkConditions chunk2 in end.Chunks)
		{
			string text2 = text;
			text = text2 + "( " + chunk2.Condition.Description(null) + ", " + chunk2.Name + ", " + chunk2.Icon + " ), ";
		}
		text = text + "], sessionActions=" + ((sessionActions != null) ? sessionActions.ToString() : "null");
		text = text + ", postSessionActions=" + ((postSessionActions != null) ? postSessionActions.ToString() : "null");
		return text + ")";
	}

	public Reward GenerateReward(Simulation simulation)
	{
		return rewardDefinition.GenerateReward(simulation, true);
	}
}
