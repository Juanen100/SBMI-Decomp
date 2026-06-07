using System.Collections.Generic;

public class GamestateMigrator
{
	public enum MigrationTypes
	{
		INITIAL = 0,
		RESOURCE_AMOUNTS_MIGRATION = 1,
		SOARING_SAVE_MIGRATION = 2
	}

	public const int STATUS_NO_MIGRATION_PERFORMED = 1;

	public const int STATUS_MIGRATION_PERFORMED = 2;

	public const int STATUS_CANNOT_MIGRATE_NEWER_PROTOCOL = 3;

	public static int CURRENT_VERSION;

	private static Dictionary<MigrationTypes, Migration> migrationTypeToMigration;

	static GamestateMigrator()
	{
	}

	public int GetProtocolVersion(Dictionary<string, object> gamestate)
	{
		return 0;
	}

	public void Migrate(Dictionary<string, object> gamestate, List<Dictionary<string, object>> actionList, StaticContentLoader contentLoader, Player p, out int performedMigration)
	{
		performedMigration = default(int);
	}

	public static void RegisterMigration(MigrationTypes migrationType, Migration migration)
	{
	}
}
