using System;
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
		CURRENT_VERSION = -1;
		migrationTypeToMigration = new Dictionary<MigrationTypes, Migration>();
		RegisterMigration(MigrationTypes.RESOURCE_AMOUNTS_MIGRATION, new ResourceAmountsMigration());
		RegisterMigration(MigrationTypes.SOARING_SAVE_MIGRATION, new SoaringSaveMigration());
		MigrationTypes[] array = (MigrationTypes[])Enum.GetValues(typeof(MigrationTypes));
		CURRENT_VERSION = (int)array[array.Length - 1];
		TFUtils.DebugLog(string.Format("Current protocol version is {0} ({1})", CURRENT_VERSION, Enum.GetName(typeof(MigrationTypes), array[array.Length - 1])));
	}

	public int GetProtocolVersion(Dictionary<string, object> gamestate)
	{
		int result = 0;
		if (gamestate.ContainsKey("protocol_version"))
		{
			result = TFUtils.LoadInt(gamestate, "protocol_version");
		}
		return result;
	}

	public void Migrate(Dictionary<string, object> gamestate, List<Dictionary<string, object>> actionList, StaticContentLoader contentLoader, Player p, out int performedMigration)
	{
		int protocolVersion = GetProtocolVersion(gamestate);
		performedMigration = 1;
		TFUtils.DebugLog("Gamestate is currently running protocol version " + Enum.GetName(typeof(MigrationTypes), protocolVersion));
		if (protocolVersion > CURRENT_VERSION)
		{
			performedMigration = 3;
		}
		else if (protocolVersion < CURRENT_VERSION)
		{
			MigrationTypes[] array = (MigrationTypes[])Enum.GetValues(typeof(MigrationTypes));
			for (int i = protocolVersion + 1; i < array.Length; i++)
			{
				TFUtils.DebugLog("Migrating gamestate to protocol version " + Enum.GetName(typeof(MigrationTypes), array[i]));
				Migration migration = migrationTypeToMigration[array[i]];
				migration.MigrateGamestate(gamestate, contentLoader);
				migration.MigrateActions(actionList, contentLoader);
				gamestate["protocol_version"] = (int)array[i];
			}
			int protocolVersion2 = GetProtocolVersion(gamestate);
			if (protocolVersion2 != protocolVersion)
			{
				performedMigration = 2;
			}
		}
	}

	public static void RegisterMigration(MigrationTypes migrationType, Migration migration)
	{
		migrationTypeToMigration.Add(migrationType, migration);
	}
}
