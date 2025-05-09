#define ASSERTS_ON
using System;
using System.Collections.Generic;
using MiniJSON;

public class DialogPackageManager
{
	private static readonly string DIALOG_PACKAGES_PATH = "Dialogs";

	private Dictionary<uint, DialogPackage> dialogPackages;

	private List<DialogInputData> dialogInputs;

	private HashSet<uint> activeDialogs;

	public DialogPackageManager(Dictionary<string, object> gameState)
	{
		dialogPackages = new Dictionary<uint, DialogPackage>();
		dialogInputs = new List<DialogInputData>();
		activeDialogs = new HashSet<uint>();
		LoadDialogPackagesFromSpread();
		LoadPersistedDialogs(gameState);
	}

	private void LoadPersistedDialogs(Dictionary<string, object> gameState)
	{
		List<object> list = new List<object>();
		if (gameState.ContainsKey("dialogs"))
		{
			list = (List<object>)gameState["dialogs"];
		}
		foreach (Dictionary<string, object> item in list)
		{
			PersistedDialogInputData persistedDialogInputData = PersistedDialogInputData.FromPersistenceDict(item);
			if (!activeDialogs.Contains(persistedDialogInputData.SequenceId))
			{
				activeDialogs.Add(persistedDialogInputData.SequenceId);
			}
			dialogInputs.Add(persistedDialogInputData);
		}
	}

	public DialogPackage GetDialogPackage(uint packageId)
	{
		TFUtils.Assert(dialogPackages.ContainsKey(packageId), "No dialog package registered with the packageId=" + packageId);
		return dialogPackages[packageId];
	}

	private void PersistAddingDialogs(Game game, List<DialogInputData> inputs)
	{
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			List<object> list = new List<object>();
			if (gameState.ContainsKey("dialogs"))
			{
				list = (List<object>)gameState["dialogs"];
			}
			foreach (DialogInputData input in inputs)
			{
				PersistedDialogInputData persistedDialogInputData = input as PersistedDialogInputData;
				if (persistedDialogInputData != null)
				{
					list.Add(persistedDialogInputData.ToPersistenceDict());
				}
			}
		};
		game.LockedGameStateChange(writer);
	}

	private void PersistRemovingCurrentDialog(Game game, bool removeAll)
	{
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			if (gameState.ContainsKey("dialogs"))
			{
				List<object> list = (List<object>)gameState["dialogs"];
				if (removeAll)
				{
					list.Clear();
				}
				else
				{
					list.RemoveAt(0);
				}
			}
		};
		game.LockedGameStateChange(writer);
	}

	public bool AddDialogInputBatch(Game game, List<DialogInputData> inputs, uint sequenceId = uint.MaxValue)
	{
		List<object> list = null;
		List<DialogInputData> list2 = new List<DialogInputData>();
		Type type = null;
		if (sequenceId != uint.MaxValue && activeDialogs.Contains(sequenceId))
		{
			TFUtils.ErrorLog("Adding a sequence that is already active!? Is this ok?");
			return false;
		}
		activeDialogs.Add(sequenceId);
		foreach (DialogInputData input in inputs)
		{
			TFUtils.Assert(sequenceId == input.SequenceId, "Got a DialogInputBatch where individual DialogInputData don't have the same sequenceId.");
			if (input is CharacterDialogInputData)
			{
				CharacterDialogInputData characterDialogInputData = (CharacterDialogInputData)input;
				if (type == null || !type.Equals(typeof(CharacterDialogInputData)))
				{
					list = new List<object>();
				}
				list.AddRange(characterDialogInputData.PromptsData);
			}
			else
			{
				if (list != null)
				{
					list2.Add(new CharacterDialogInputData(sequenceId, list));
					list = null;
				}
				list2.Add(input);
			}
			type = input.GetType();
		}
		if (list != null)
		{
			list2.Add(new CharacterDialogInputData(sequenceId, list));
		}
		dialogInputs.AddRange(list2);
		PersistAddingDialogs(game, list2);
		return true;
	}

	public DialogInputData PeekCurrentDialogInput()
	{
		if (dialogInputs.Count > 0)
		{
			return dialogInputs[0];
		}
		return null;
	}

	public void RemoveCurrentDialogInput(Game game)
	{
		if (dialogInputs.Count > 0)
		{
			DialogInputData dialogInputData = dialogInputs[0];
			dialogInputs.RemoveAt(0);
			PersistRemovingCurrentDialog(game, false);
			if (dialogInputs.Count == 0 || dialogInputData.SequenceId != dialogInputs[0].SequenceId)
			{
				game.triggerRouter.RouteTrigger(dialogInputData.CreateTrigger(TFUtils.EpochTime()), game);
				activeDialogs.Remove(dialogInputData.SequenceId);
			}
		}
	}

	public void ClearDialogs(Game game)
	{
		PersistRemovingCurrentDialog(game, true);
		activeDialogs.Clear();
	}

	public int GetNumQueuedDialogInputs()
	{
		return dialogInputs.Count;
	}

	private string[] GetFilesToLoad()
	{
		return Config.DIALOG_PACKAGES_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private DialogPackage LoadDialogPackageFromFile(string filePath)
	{
		string json = TFUtils.ReadAllText(filePath);
		Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(json);
		return new DialogPackage(data);
	}

	private void LoadDialogPackages()
	{
		string[] filesToLoad = GetFilesToLoad();
		string[] array = filesToLoad;
		foreach (string filePath in array)
		{
			string filePathFromString = GetFilePathFromString(filePath);
			DialogPackage dialogPackage = LoadDialogPackageFromFile(filePathFromString);
			dialogPackages[dialogPackage.Did] = dialogPackage;
		}
	}

	private void LoadDialogPackagesFromSpread()
	{
		string sheetName = "Dialogs";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_SHEET(sheetName);
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			TFError.DM_LOG_ERROR_NO_ROWS(sheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		Dictionary<int, Dictionary<string, object>> dictionary2 = new Dictionary<int, Dictionary<string, object>>();
		Dictionary<string, object> dictionary3 = null;
		dictionary.Add("type", "dialogs");
		dictionary.Add("did", 1);
		dictionary.Add("sequences", list);
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "special rewards");
		if (columnIndexInSheet < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("special rewards");
		}
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		if (columnIndexInSheet2 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("type");
		}
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "id");
		if (columnIndexInSheet3 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("id");
		}
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		if (columnIndexInSheet4 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("did");
		}
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "character icon");
		if (columnIndexInSheet5 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("character icon");
		}
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "text");
		if (columnIndexInSheet6 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("text");
		}
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "voiceover");
		if (columnIndexInSheet7 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("voiceover");
		}
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "title");
		if (columnIndexInSheet8 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("title");
		}
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "icon");
		if (columnIndexInSheet9 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("icon");
		}
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "portrait");
		if (columnIndexInSheet10 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("portrait");
		}
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "heading");
		if (columnIndexInSheet11 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("heading");
		}
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "body");
		if (columnIndexInSheet12 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("body");
		}
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "effect");
		if (columnIndexInSheet13 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("effect");
		}
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "reward xp");
		if (columnIndexInSheet14 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("reward xp");
		}
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "reward gold");
		if (columnIndexInSheet15 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("reward gold");
		}
		string text = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, columnIndexInSheet3).ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet);
			}
			bool flag = true;
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4);
			if (dictionary2.ContainsKey(intCell))
			{
				dictionary3 = dictionary2[intCell];
				flag = false;
			}
			Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
			dictionary4.Add("type", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet2));
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet5);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("character_icon", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet6);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("text", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet7);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("voiceover", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet8);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("title", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet9);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("icon", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet10);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("portrait", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet11);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("heading", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet12);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("body", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet13);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text)
			{
				dictionary4.Add("effect", stringCell);
			}
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14);
			if (intCell2 > 0)
			{
				dictionary4.Add("reward", new Dictionary<string, object> { 
				{
					"resources",
					new Dictionary<string, object> { { "5", intCell2 } }
				} });
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15);
			if (intCell2 > 0)
			{
				if (!dictionary4.ContainsKey("reward"))
				{
					dictionary4.Add("reward", new Dictionary<string, object> { 
					{
						"resources",
						new Dictionary<string, object> { { "3", intCell2 } }
					} });
				}
				else
				{
					((Dictionary<string, object>)((Dictionary<string, object>)dictionary4["reward"])["resources"]).Add("3", intCell2);
				}
			}
			for (int j = 1; j <= num2; j++)
			{
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, "special reward type " + j);
				if (!string.IsNullOrEmpty(stringCell) && !(stringCell == text))
				{
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "special reward did " + j);
					int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "special reward amount " + j);
					if (!dictionary4.ContainsKey("reward"))
					{
						dictionary4.Add("reward", new Dictionary<string, object> { 
						{
							stringCell,
							new Dictionary<string, object> { 
							{
								intCell2.ToString(),
								intCell3
							} }
						} });
					}
					else if (!((Dictionary<string, object>)dictionary4["reward"]).ContainsKey(stringCell))
					{
						((Dictionary<string, object>)dictionary4["reward"]).Add(stringCell, new Dictionary<string, object> { 
						{
							intCell2.ToString(),
							intCell3
						} });
					}
					else
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary4["reward"])[stringCell]).Add(intCell2.ToString(), intCell3);
					}
				}
			}
			if (flag)
			{
				dictionary3 = new Dictionary<string, object>();
				dictionary3.Add("id", intCell);
				dictionary3.Add("prompts", new List<object> { dictionary4 });
				list.Add(dictionary3);
				dictionary2.Add(intCell, dictionary3);
			}
			else
			{
				((List<object>)dictionary3["prompts"]).Add(dictionary4);
			}
		}
		DialogPackage dialogPackage = new DialogPackage(dictionary);
		dialogPackages[dialogPackage.Did] = dialogPackage;
	}
}
