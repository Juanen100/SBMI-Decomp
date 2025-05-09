using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

public class PersistedActionBuffer
{
	public abstract class PersistedAction
	{
		public delegate PersistedAction ConstructFromDict(Dictionary<string, object> dict);

		public static Dictionary<string, ConstructFromDict> TypeRegistry;

		private static int counter;

		public string type;

		public Identity target;

		private ulong time;

		public string tag;

		public PersistedAction(string type, Identity target)
		{
			this.type = type;
			this.target = target;
		}

		static PersistedAction()
		{
			TypeRegistry = new Dictionary<string, ConstructFromDict>();
			counter = 0;
			TypeRegistry.Add("nb", NewBuildingAction.FromDict);
			TypeRegistry.Add("rb", RushBuildAction.FromDict);
			TypeRegistry.Add("rh", RushHungerAction.FromDict);
			TypeRegistry.Add("cb", CompleteBuildingAction.FromDict);
			TypeRegistry.Add("cr", CollectRentAction.FromDict);
			TypeRegistry.Add("rr", RushRentAction.FromDict);
			TypeRegistry.Add("m", MoveAction.FromDict);
			TypeRegistry.Add("np", PaveAction.FromDict);
			TypeRegistry.Add("s", SellAction.FromDict);
			TypeRegistry.Add("i", InitializeAction.FromDict);
			TypeRegistry.Add("fu", FeedUnitAction.FromDict);
			TypeRegistry.Add("emb", EarnMatchBonusAction.FromDict);
			TypeRegistry.Add("cmb", CollectMatchBonusAction.FromDict);
			TypeRegistry.Add("cs", CraftStartAction.FromDict);
			TypeRegistry.Add("cf", CraftCompleteAction.FromDict);
			TypeRegistry.Add("cc", CraftCollectAction.FromDict);
			TypeRegistry.Add("aqcc", AutoQuestCraftCollectAction.FromDict);
			TypeRegistry.Add("rc", RushCraftAction.FromDict);
			TypeRegistry.Add("qp", QuestProgressAction.FromDict);
			TypeRegistry.Add("qs", QuestStartAction.FromDict);
			TypeRegistry.Add("qc", QuestCompleteAction.FromDict);
			TypeRegistry.Add("aqad", FromDict);
			TypeRegistry.Add("rq", RandomQuestCreateAction.FromDict);
			TypeRegistry.Add("ru", RandomQuestCleanupAction.FromDict);
			TypeRegistry.Add("aq", AutoQuestCreateAction.FromDict);
			TypeRegistry.Add("au", AutoQuestCleanupAction.FromDict);
			TypeRegistry.Add("lu", LevelUpAction.FromDict);
			TypeRegistry.Add("ne", NewExpansionAction.FromDict);
			TypeRegistry.Add("ds", DebrisStartAction.FromDict);
			TypeRegistry.Add("dc", DebrisCompleteAction.FromDict);
			TypeRegistry.Add("rd", RushDebrisAction.FromDict);
			TypeRegistry.Add("nw", NewWishAction.FromDict);
			TypeRegistry.Add("fw", FailWishAction.FromDict);
			TypeRegistry.Add("pd", PickupDropAction.FromDict);
			TypeRegistry.Add("pr", PurchaseResourcesAction.FromDict);
			TypeRegistry.Add("uf", FeatureUnlocksAction.FromDict);
			TypeRegistry.Add("ub", BuildingUnlocksAction.FromDict);
			TypeRegistry.Add("pcs", PurchaseCraftingSlotAction.FromDict);
			TypeRegistry.Add("cap", RewardCapAction.FromDict);
			TypeRegistry.Add("tc", TreasureCollectAction.FromDict);
			TypeRegistry.Add("ts", TreasureSpawnAction.FromDict);
			TypeRegistry.Add("tu", TreasureUncoverAction.FromDict);
			TypeRegistry.Add("tt", TreasureCooldownAction.FromDict);
			TypeRegistry.Add("rra", ReceiveRewardAction.FromDict);
			TypeRegistry.Add("vr", RestockVendorAction.FromDict);
			TypeRegistry.Add("va", VendingAction.FromDict);
			TypeRegistry.Add("rrs", RushRestockAction.FromDict);
			TypeRegistry.Add("sw", SpawnWandererAction.FromDict);
			TypeRegistry.Add("tw", TapWandererAction.FromDict);
			TypeRegistry.Add("hw", HideWandererAction.FromDict);
			TypeRegistry.Add("df", DisableFleeAction.FromDict);
			TypeRegistry.Add("lr", LockRecipeAction.FromDict);
			TypeRegistry.Add("cca", ChangeCostumeAction.FromDict);
			TypeRegistry.Add("uc", UnlockCostumeAction.FromDict);
			TypeRegistry.Add("tsa", TaskStartAction.FromDict);
			TypeRegistry.Add("tca", TaskCompleteAction.FromDict);
			TypeRegistry.Add("tua", TaskUpdateAction.FromDict);
			TypeRegistry.Add("msa", MicroEventStartAction.FromDict);
			TypeRegistry.Add("mca", MicroEventCompleteAction.FromDict);
			TypeRegistry.Add("moa", MicroEventOpenAction.FromDict);
			TypeRegistry.Add("mcla", MicroEventCloseAction.FromDict);
			TypeRegistry.Add("sr", SpawnResidentAction.FromDict);
		}

		protected static string NextTag(ulong timestamp)
		{
			return string.Format("{0}_{1}", timestamp, counter++);
		}

		public ulong GetTime()
		{
			if (SBSettings.UseActionFile)
			{
				return time;
			}
			return TFUtils.EpochTime();
		}

		public static PersistedAction FromDict(Dictionary<string, object> data)
		{
			if (!data.ContainsKey("type"))
			{
				TFUtils.DebugLog("Attempting to create an action from malformed data! This should not have occurred, locate the source and fix it.");
				return null;
			}
			string text = (string)data["type"];
			if (TypeRegistry.ContainsKey(text))
			{
				PersistedAction persistedAction = TypeRegistry[text](data);
				persistedAction.AddEnvelope(TFUtils.LoadUlong(data, "ts"), TFUtils.LoadString(data, "tag"));
				return persistedAction;
			}
			throw new InvalidOperationException("Unknown type: " + text);
		}

		public virtual void AddEnvelope(ulong time)
		{
			AddEnvelope(time, NextTag(time));
		}

		public virtual void AddEnvelope(ulong time, string tag)
		{
			this.time = time;
			this.tag = tag;
		}

		public virtual Dictionary<string, object> ToDict()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["ts"] = time;
			dictionary["type"] = type;
			dictionary["target"] = target.Describe();
			dictionary["tag"] = tag;
			return dictionary;
		}

		public string DebugToString()
		{
			return TFUtils.DebugDictToString(ToDict());
		}

		public abstract void Apply(Game game, ulong utcNow);

		public virtual void Confirm(Dictionary<string, object> gameState)
		{
			((Dictionary<string, object>)gameState["farm"])["last_action"] = tag;
		}

		public abstract void Process(Game game);
	}

	private const int BUFFER_SOFT_LIMIT = 1;

	private List<PersistedAction> unconfirmed = new List<PersistedAction>();

	public static string ACTION_LIST_FILE = "actions.json";

	private object unconfirmedLock = new object();

	private string unconfirmedFile;

	public PersistedActionBuffer(Player p, List<Dictionary<string, object>> actionList)
	{
		unconfirmedFile = p.CacheFile(ACTION_LIST_FILE);
		LoadActionsIntoList(actionList, unconfirmed);
	}

	public static List<Dictionary<string, object>> LoadActionList(Player p)
	{
		string text = p.CacheFile(ACTION_LIST_FILE);
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		try
		{
			if (TFUtils.FileIsExists(text))
			{
				string text2 = TFUtils.ReadAllText(text);
				string[] array = text2.Split('\n');
				foreach (string text3 in array)
				{
					if (!string.IsNullOrEmpty(text3))
					{
						TFUtils.DebugLog("Loading json: " + text3);
						Dictionary<string, object> item = (Dictionary<string, object>)Json.Deserialize(text3);
						list.Add(item);
					}
				}
			}
			else
			{
				using (FileStream fileStream = File.Create(text))
				{
					fileStream.Close();
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("No Action File Found: " + ex.Message, LogType.Warning);
		}
		return list;
	}

	public void Record(PersistedAction action)
	{
		if (!SBSettings.UseActionFile)
		{
			return;
		}
		action.AddEnvelope(TFUtils.EpochTime());
		lock (unconfirmedLock)
		{
			unconfirmed.Add(action);
			RecordActionToFile(action, unconfirmedFile);
		}
	}

	public List<PersistedAction> GetAllUnackedActions()
	{
		List<PersistedAction> list = new List<PersistedAction>();
		lock (unconfirmedLock)
		{
			list.AddRange(unconfirmed);
			return list;
		}
	}

	public void DestroyCache()
	{
		lock (unconfirmedLock)
		{
			TFUtils.DeleteFile(unconfirmedFile);
			unconfirmed.Clear();
		}
	}

	public void Flush()
	{
		TFUtils.DebugLog("Flushing action buffer");
		lock (unconfirmedLock)
		{
			unconfirmed = new List<PersistedAction>();
			TFUtils.TruncateFile(unconfirmedFile);
		}
	}

	private void LoadFileToList(string fileName, List<PersistedAction> list)
	{
		string text = TFUtils.ReadAllText(fileName);
		string[] array = text.Split('\n');
		foreach (string text2 in array)
		{
			if (!string.IsNullOrEmpty(text2))
			{
				TFUtils.DebugLog("Loading json: " + text2);
				Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(text2);
				PersistedAction persistedAction = PersistedAction.FromDict(data);
				if (persistedAction != null)
				{
					list.Add(persistedAction);
				}
			}
		}
	}

	private void LoadActionsIntoList(List<Dictionary<string, object>> src, List<PersistedAction> dst)
	{
		foreach (Dictionary<string, object> item in src)
		{
			PersistedAction persistedAction = PersistedAction.FromDict(item);
			if (persistedAction != null)
			{
				dst.Add(persistedAction);
			}
		}
	}

	private void RecordActionToFile(PersistedAction action, string fileName)
	{
		string text = Json.Serialize(action.ToDict());
		TFUtils.DebugLog("Writing json to ActionBuffer: " + text);
		File.AppendAllText(fileName, text + "\n");
	}
}
