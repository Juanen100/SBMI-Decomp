using System.Collections.Generic;

public class DialogPackageManager
{
	private static readonly string DIALOG_PACKAGES_PATH;

	private Dictionary<uint, DialogPackage> dialogPackages;

	private List<DialogInputData> dialogInputs;

	private HashSet<uint> activeDialogs;

	public DialogPackageManager(Dictionary<string, object> gameState)
	{
	}

	private void LoadPersistedDialogs(Dictionary<string, object> gameState)
	{
	}

	public DialogPackage GetDialogPackage(uint packageId)
	{
		return null;
	}

	private void PersistAddingDialogs(Game game, List<DialogInputData> inputs)
	{
	}

	private void PersistRemovingCurrentDialog(Game game, bool removeAll)
	{
	}

	public bool AddDialogInputBatch(Game game, List<DialogInputData> inputs, uint sequenceId = uint.MaxValue)
	{
		return false;
	}

	public DialogInputData PeekCurrentDialogInput()
	{
		return null;
	}

	public void RemoveCurrentDialogInput(Game game)
	{
	}

	public void ClearDialogs(Game game)
	{
	}

	public int GetNumQueuedDialogInputs()
	{
		return 0;
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	private DialogPackage LoadDialogPackageFromFile(string filePath)
	{
		return null;
	}

	private void LoadDialogPackages()
	{
	}

	private void LoadDialogPackagesFromSpread()
	{
	}
}
