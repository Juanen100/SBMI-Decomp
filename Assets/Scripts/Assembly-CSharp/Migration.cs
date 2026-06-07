using System.Collections.Generic;

public abstract class Migration
{
	public delegate bool ActionMigrationDelegate(Dictionary<string, object> actionDict, StaticContentLoader contentLoader);

	private Dictionary<string, ActionMigrationDelegate> actionToMigrationDelegate;

	public Migration()
	{
	}

	protected void RegisterActionMigrationDelegate(string actionType, ActionMigrationDelegate migrationDelegate)
	{
	}

	public abstract bool MigrateGamestate(Dictionary<string, object> gamestate, StaticContentLoader contentLoader);

	public void MigrateActions(List<Dictionary<string, object>> actionList, StaticContentLoader contentLoader)
	{
	}
}
