using System.Collections.Generic;

public abstract class Migration
{
	public delegate bool ActionMigrationDelegate(Dictionary<string, object> actionDict, StaticContentLoader contentLoader);

	private Dictionary<string, ActionMigrationDelegate> actionToMigrationDelegate = new Dictionary<string, ActionMigrationDelegate>();

	public Migration()
	{
	}

	protected void RegisterActionMigrationDelegate(string actionType, ActionMigrationDelegate migrationDelegate)
	{
		actionToMigrationDelegate.Add(actionType, migrationDelegate);
	}

	public abstract bool MigrateGamestate(Dictionary<string, object> gamestate, StaticContentLoader contentLoader);

	public void MigrateActions(List<Dictionary<string, object>> actionList, StaticContentLoader contentLoader)
	{
		foreach (Dictionary<string, object> action in actionList)
		{
			if (!action.ContainsKey("type"))
			{
				TFUtils.DebugLog("Attempting to migration an action from malformed data! This should not have occurred, locate the source and fix it.");
			}
			string key = (string)action["type"];
			if (actionToMigrationDelegate.ContainsKey(key))
			{
				ActionMigrationDelegate actionMigrationDelegate = actionToMigrationDelegate[key];
				actionMigrationDelegate(action, contentLoader);
			}
		}
	}
}
